using System;
using System.Threading;
using System.Globalization;
using System.Linq;
using DBus;
using Mono.BlueZ.DBus;
using org.freedesktop.DBus;
using System.Collections.Generic;
using System.IO;

namespace Mono.BlueZ.Console
{
	public class BlendMicroBootstrap
	{
		private Bus _system;
		//private Bus _session;
		public Exception _startupException{ get; private set; }
		private ManualResetEvent _started = new ManualResetEvent(false);
	
		public BlendMicroBootstrap()
		{
			// Run a message loop for DBus on a new thread.
			var t = new Thread(DBusLoop);
			t.IsBackground = true;
			t.Start();
			_started.WaitOne(60 * 1000);
			_started.Close();
			if (_startupException != null) 
			{
				throw _startupException;
			}
			else 
			{
				//System.Console.WriteLine ("Bus connected at " + _system.UniqueName);
			}
		}

		public void Run()
		{
			string serviceUUID="713d0000-503e-4c75-ba94-3148f18d941e";
			string charVendorName = "713D0001-503E-4C75-BA94-3148F18D941E";
			string charRead = "713D0002-503E-4C75-BA94-3148F18D941E";
			string charWrite = "713D0003-503E-4C75-BA94-3148F18D941E";
			string charAck = "713D0004-503E-4C75-BA94-3148F18D941E";
			string charVersion = "713D0005-503E-4C75-BA94-3148F18D941E";

			System.Console.WriteLine ("Starting Blend Micro Bootstrap");
			string Service = "org.bluez";
			//Important!: there is a flaw in dbus-sharp such that you can only register one interface
			//at each path, so we have to put these at 2 seperate paths, otherwise I'd probably just put them 
			//both at root
			var agentPath = new ObjectPath ("/agent");
			var gattProfilePath = new ObjectPath ("/gattprofiles");
			var blueZPath = new ObjectPath ("/org/bluez");

			//get a copy of the object manager so we can browse the "tree" of bluetooth items
			var manager = GetObject<org.freedesktop.DBus.ObjectManager> (Service, ObjectPath.Root);
			//register these events so we can tell when things are added/removed (eg: discovery)
			manager.InterfacesAdded += (p, i) => {
				System.Console.WriteLine (p + " Discovered");
			};
			manager.InterfacesRemoved += (p, i) => {
				System.Console.WriteLine (p + " Lost");
			};

			System.Console.WriteLine ("Registring agent");
			//get the agent manager so we can register our agent
			var agentManager = GetObject<AgentManager1> (Service, blueZPath);
			var agent = new DemoAgent ();
			GattManager1 gattManager=null;
			//register our agent and make it the default
			_system.Register (agentPath, agent);
			agentManager.RegisterAgent (agentPath, "KeyboardDisplay");
			agentManager.RequestDefaultAgent (agentPath);

			try
			{
				System.Console.WriteLine("Fetching objects");
				//get the bluetooth object tree
				var managedObjects = manager.GetManagedObjects();
				//find our adapter
				ObjectPath adapterPath = null;
				foreach (var obj in managedObjects.Keys) {
					System.Console.WriteLine("Checking "+obj);
					if (managedObjects [obj].ContainsKey (typeof(LEAdvertisingManager1).DBusInterfaceName ())) {
						System.Console.WriteLine ("Adapter found at" + obj+" that supports LE");
						adapterPath = obj;
					}
				}

				if (adapterPath == null) {
					System.Console.WriteLine ("Couldn't find adapter that supports LE");
					return;
				}

				//get a dbus proxy to the adapter
				var adapter = GetObject<Adapter1> (Service, adapterPath);
				gattManager = GetObject<GattManager1>(Service,adapterPath);
				var gattProfile = new BlendGattProfile();
				_system.Register(gattProfilePath,gattProfile); 
				gattManager.RegisterProfile(gattProfilePath,new string[]{charRead},new Dictionary<string,object>());
				System.Console.WriteLine("Registered gatt profile");

				//assume discovery for ble
				//scan for any new devices
				System.Console.WriteLine("Starting LE Discovery...");
				var discoveryProperties = new Dictionary<string,object>();
				discoveryProperties["Transport"]="le";
				adapter.SetDiscoveryFilter(discoveryProperties);
				adapter.StartDiscovery ();
				Thread.Sleep(5000);//totally arbitrary constant, the best kind
				//Thread.Sleep ((int)adapter.DiscoverableTimeout * 1000);

				//refresh the object graph to get any devices that were discovered
				//arguably we should do this in the objectmanager added/removed events and skip the full
				//refresh, but I'm lazy.
				managedObjects = manager.GetManagedObjects();

				var devices = new List<Device1> ();
				foreach (var obj in managedObjects.Keys) {
					if (obj.ToString ().StartsWith (adapterPath.ToString ())) {
						if (managedObjects [obj].ContainsKey (typeof(Device1).DBusInterfaceName ())) {

							var managedObject = managedObjects [obj];
							var name = (string)managedObject[typeof(Device1).DBusInterfaceName()]["Name"];

							if (name.StartsWith ("MrGibbs")) {
								System.Console.WriteLine ("Device " + name + " at " + obj);
								var device = _system.GetObject<Device1> (Service, obj);

								var uuids = device.UUIDs;
								foreach(var uuid in device.UUIDs)
								{
									System.Console.WriteLine("\tUUID: "+uuid);
								}

								devices.Add(device);

							}
						}
					}
				}

				foreach(var device in devices)
				{
					System.Console.WriteLine("Connecting to "+device.Name);
					device.Connect();
					System.Console.WriteLine("\tConnected");
				}

				//var c = GetObject<GattService1>(Service,new ObjectPath("/org/bluez/hci1/dev_F6_58_7F_09_5D_E6/service000c"));
				var c = GetObject<GattCharacteristic1>(Service,new ObjectPath("/org/bluez/hci1/dev_F6_58_7F_09_5D_E6/service000f/char000d"));
				//var c = GetObject<GattDescriptor1>(Service,new ObjectPath("/org/bluez/hci1/dev_F6_58_7F_09_5D_E6/service000c/char000f/desc0011"));
				for(int i=0;i<100;i++)
				{
					try
					{
						System.Console.WriteLine("Reading data....");
						//var bytes = c.Value;
						var bytes = c.ReadValue();

						foreach(var b in bytes)
						{
							System.Console.Write(b+",");
							System.Console.WriteLine("Received");
						}
					}
					catch{}
					System.Threading.Thread.Sleep(1000);
				}

			}
			finally 
			{
				agentManager.UnregisterAgent (agentPath);
				gattManager.UnregisterProfile (gattProfilePath);
			}
		}

		private T GetObject<T>(string busName, ObjectPath path)
		{
			var obj = _system.GetObject<T>(busName, path);
			return obj;
		}
	
		private void DBusLoop()
		{
			try 
			{
				_system=Bus.System;
			} catch (Exception ex) {
				_startupException = ex;
				return;
			} finally {
				_started.Set();
			}

			while (true) {
				_system.Iterate();
			}
		}

		public static byte[] CombineArrays( params byte[][] array )
		{
			var rv = new byte[array.Select( x => x.Length ).Sum()];

			for ( int i = 0, insertionPoint = 0; i < array.Length; insertionPoint += array[i].Length, i++ )
				Array.Copy( array[i], 0, rv, insertionPoint, array[i].Length );
			return rv;
		}
	}

	public class BlendGattProfile:GattProfile1
	{
		public void Release()
		{
			System.Console.WriteLine ("GattProfile1.Release");
		}
		public void NewClientConfiguration(ObjectPath characteristic,string device_address,byte[] value)
		{
			System.Console.WriteLine ("NewClientConfiguration");
		}
	}

}

