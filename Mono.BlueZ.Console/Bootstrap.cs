using System;
using System.Threading;
using System.Globalization;
using System.Linq;
using DBus;
using Mono.BlueZ.DBus;
using org.freedesktop.DBus;
using System.Collections.Generic;

namespace Mono.BlueZ.Console
{
	public class Bootstrap
	{
		private Bus _system;
		//private Bus _session;
		public Exception _startupException{ get; private set; }
		private ManualResetEvent _started = new ManualResetEvent(false);
	
		public Bootstrap()
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
				System.Console.WriteLine ("Bus connected at " + _system.UniqueName);
			}
		}

		public void Run()
		{
			//hack to test some stuff out
			string Service = "org.bluez";

			var manager = GetObject<org.freedesktop.DBus.ObjectManager> (Service, ObjectPath.Root);
			manager.InterfacesAdded += (p, i) => {
				System.Console.WriteLine (p + " Added");
			};
			manager.InterfacesRemoved += (p, i) => {
				System.Console.WriteLine (p + " Removed");
			};

			var managedObjects = manager.GetManagedObjects();
			var profileManager = GetObject<ProfileManager1> (Service, new ObjectPath ("/org/bluez"));

			//this is the value that was used by inthehand.bluetooth
			string serialUUID = "00001101-0000-1000-8000-00805F9B34FB";

			string adapterName = "hci1";
			var profile = new Profile1 ();
			//var appService = "io.mrgibbs";
			//var appPath = "/io/mrgibbs";

			ObjectPath adapterPath = null;

			foreach (var obj in managedObjects.Keys) {
				if (managedObjects [obj].ContainsKey (typeof(Adapter1).DBusInterfaceName ())) {
					if (obj.ToString ().EndsWith (adapterName)) {
						System.Console.WriteLine ("Adapter found" + obj);
						adapterPath = obj;
					}
				}
			}

			if (adapterPath == null) {
				System.Console.WriteLine ("Couldn't find adapter " + adapterName);
				return;
			}

			var devices = new List<Device1> ();

			foreach (var obj in managedObjects.Keys) {
				if (obj.ToString ().StartsWith (adapterPath.ToString ())) {
					if (managedObjects [obj].ContainsKey (typeof(Device1).DBusInterfaceName ())) {

						var managedObject = managedObjects [obj];
						var name = (string)managedObject[typeof(Device1).DBusInterfaceName()]["Name"];

						if (name.StartsWith ("Pebble")) {
							System.Console.WriteLine ("Device " + name + " at " + obj);
							var device = _system.GetObject<Device1> (Service, obj);
							devices.Add (device);

							if (!device.Paired) {
								System.Console.WriteLine (name + " not paired, attempting to pair");
								device.Pair ();
								System.Console.WriteLine ("Paired");
							}
							if (!device.Trusted) {
								System.Console.WriteLine (name + " not trust, attempting to trust");
								device.Trusted=true;
								System.Console.WriteLine ("Trusted");
							}
						}
					}
				}
			}

			_system.Register (ObjectPath.Root, profile);

			System.Console.WriteLine ("Registered profile on DBus");

			//var sessionBus = Bus.Session;

			//if (sessionBus.RequestName (appService) == RequestNameReply.PrimaryOwner) {
				//create a new instance of the object to be exported
			//	sessionBus.Register (new ObjectPath(appPath),profile);

			//} else {
				//import a remote to a local proxy
			//	profile = sessionBus.GetObject<DemoProfile> (appService,new ObjectPath(appPath));
			//}

			var properties = new Dictionary<string,object> ();
			//properties ["AutoConnect"] = true;
			//properties ["Name"] = "MrGibbs";
			//properties ["Role"] = "server";
			//properties ["RequireAuthentication"] = false;
			//properties ["RequireAuthorization"] = false;
			//properties ["Channel"] = (ushort)1;

			profileManager.RegisterProfile (ObjectPath.Root
				, serialUUID
				, properties);
			System.Console.WriteLine ("Registered profile with BlueZ");

			foreach (var device in devices) {
				try{
					System.Console.WriteLine("Attempting Connection to "+device.Name);
					System.Console.WriteLine("Trusted:"+device.Trusted.ToString());
					System.Console.WriteLine("UUIDs:");
					foreach(var uuid in device.UUIDs)
					{
						System.Console.WriteLine(uuid);
					}





					device.ConnectProfile(serialUUID);
					//device.Connect();
					System.Console.Write("Connected");
				}
				catch(Exception ex){
					System.Console.WriteLine (ex.Message);
				}
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
				// Open our own copy of the System bus, as a consumer
				// application could have opened it already and we would
				// then be relying on their threading.
				/*var tA = typeof(NDesk.DBus.Bus).Assembly.GetType("NDesk.DBus.Address", true);
				string name = (string)tA.InvokeMember("System",
					System.Reflection.BindingFlags.Public
					| System.Reflection.BindingFlags.Static
					| System.Reflection.BindingFlags.GetProperty,
					null, null, null,
					CultureInfo.InvariantCulture);
				_bus = new Bus(name); */ // Bus.System

				_system=Bus.System;
				//_session = Bus.Session;
			} catch (Exception ex) {
				_startupException = ex;
				return;
			} finally {
				_started.Set();
			}
			//Console.WriteLine("Dbus loop running");
			//
			while (true) {
				_system.Iterate();
				//_session.Iterate ();
			}
		}
	}

}

