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
				//System.Console.WriteLine ("Bus connected at " + _system.UniqueName);
			}
		}

		public void Run(bool doDiscovery,string adapterName)
		{
			string Service = "org.bluez";
			//Important!: there is a flaw in dbus-sharp such that you can only register one interface
			//at each path, so we have to put these at 2 seperate paths, otherwise I'd probably just put them 
			//both at root
			var agentPath = new ObjectPath ("/agent");
			var profilePath = new ObjectPath ("/profiles");
			var blueZPath = new ObjectPath ("/org/bluez");

			//still don't really understand this.  The pebble advertises this uuid,
			//and this is what works, but all my reading tells me that the uuid for the 
			//serial service should be the 2nd one
			string pebbleSerialUUID = "00000000-deca-fade-deca-deafdecacaff";
			string serialServiceUUID = "00001101-0000-1000-8000-00805F9B34FB";

			//these properties are defined by bluez in /doc/profile-api.txt
			//but it turns out the defaults work just fine
			var properties = new Dictionary<string,object> ();
			//properties ["AutoConnect"] = true;
			//properties ["Name"] = "Serial Port";
			//properties ["Service"] = pebbleSerialUUID;
			//properties ["Role"] = "client";
			//properties ["PSM"] = (ushort)1;
			//properties ["RequireAuthentication"] = false;
			//properties ["RequireAuthorization"] = false;
			//properties ["Channel"] = (ushort)1;

			//get a proxy for the profile manager so we can register our profile
			var profileManager = GetObject<ProfileManager1> (Service, blueZPath);
			//create and register our profile
			var profile = new DemoProfile ();
			_system.Register (profilePath, profile);
			profileManager.RegisterProfile (profilePath
				, pebbleSerialUUID
				, properties);
			System.Console.WriteLine ("Registered profile with BlueZ");

			//get a copy of the object manager so we can browse the "tree" of bluetooth items
			var manager = GetObject<org.freedesktop.DBus.ObjectManager> (Service, ObjectPath.Root);
			//register these events so we can tell when things are added/removed (eg: discovery)
			manager.InterfacesAdded += (p, i) => {
				System.Console.WriteLine (p + " Discovered");
			};
			manager.InterfacesRemoved += (p, i) => {
				System.Console.WriteLine (p + " Lost");
			};

			//get the agent manager so we can register our agent
			var agentManager = GetObject<AgentManager1> (Service, blueZPath);
			var agent = new DemoAgent ();
			//register our agent and make it the default
			_system.Register (agentPath, agent);
			agentManager.RegisterAgent (agentPath, "KeyboardDisplay");
			agentManager.RequestDefaultAgent (agentPath);

			try
			{
				//get the bluetooth object tree
				var managedObjects = manager.GetManagedObjects();
				//find our adapter
				ObjectPath adapterPath = null;
				foreach (var obj in managedObjects.Keys) {
					if (managedObjects [obj].ContainsKey (typeof(Adapter1).DBusInterfaceName ())) {
						if (string.IsNullOrEmpty(adapterName) || obj.ToString ().EndsWith (adapterName)) {
							System.Console.WriteLine ("Adapter found at" + obj);
							adapterPath = obj;
							break;
						}
					}
				}

				if (adapterPath == null) {
					System.Console.WriteLine ("Couldn't find adapter " + adapterName);
					return;
				}

				//get a dbus proxy to the adapter
				var adapter = GetObject<Adapter1> (Service, adapterPath);

				if(doDiscovery)
				{
					//scan for any new devices
					System.Console.WriteLine("Starting Discovery...");
					adapter.StartDiscovery ();
					Thread.Sleep(5000);//totally arbitrary constant, the best kind
					//Thread.Sleep ((int)adapter.DiscoverableTimeout * 1000);

					//refresh the object graph to get any devices that were discovered
					//arguably we should do this in the objectmanager added/removed events and skip the full
					//refresh, but I'm lazy.
					managedObjects = manager.GetManagedObjects();
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

				foreach (var device in devices) {
					try{
						System.Console.WriteLine("Attempting Connection to "+device.Name);
						System.Console.WriteLine("Paired:"+device.Paired.ToString());
						System.Console.WriteLine("Trusted:"+device.Trusted.ToString());
						System.Console.WriteLine("UUIDs:");
						foreach(var uuid in device.UUIDs)
						{
							System.Console.WriteLine(uuid);
						}

						Stream pebbleStream=null;

						var bytes = System.Text.Encoding.ASCII.GetBytes("Hello world!");
						profile.NewConnectionAction=(path,stream,props)=>{

							System.Console.WriteLine("Connected to fd "+stream);
							pebbleStream=stream;
							System.Console.WriteLine(props.Count+" properties");
							foreach(var k in props.Keys)
							{
								System.Console.WriteLine(k+":"+props[k]);
							}
//
//							try{
//								System.Console.WriteLine("Can Read:"+stream.CanRead+", Can Write:"+stream.CanWrite);
//
//								var pebbleHello = PebbleHello();
//
//								stream.Write(bytes,0,bytes.Length);
//								stream.Flush();
//
//								StreamReader reader = new StreamReader(stream);
//								while(!reader.EndOfStream)
//								{
//									System.Console.WriteLine(reader.ReadLine());
//								}
//							}
//							catch(Exception ex){
//								System.Console.WriteLine("Exception writing to stream "+ex.Message);
//								throw;
//							}
						};

						System.Console.WriteLine("Connecting...");
						//device.Connect();
						device.ConnectProfile(pebbleSerialUUID);
						//System.Threading.Thread.Sleep(1000);
						System.Console.WriteLine("ConnectProfile returned");

						var pebbleHello = PebbleHello();

						pebbleStream.Write(pebbleHello,0,pebbleHello.Length);
						pebbleStream.Flush();

						bool success=false;
						while(true){
							try{
								byte[] buffer = new byte[72];

								int count = pebbleStream.Read(buffer,0,72);
								string read = System.Text.ASCIIEncoding.ASCII.GetString(buffer);
								System.Console.WriteLine(read);
								success=true;
							//System.Console.WriteLine(reader.ReadLine());
							}
							catch(Exception ex)
							{
								if(success)
								{
									System.Console.WriteLine(ex.Message);
									success=false;
								}
							}
						}
						System.Threading.Thread.Sleep(10000);
					}
					catch(Exception ex){
						System.Console.WriteLine("Bootstrap handler");
						System.Console.WriteLine (ex.Message);
						System.Console.WriteLine(ex.StackTrace);
					}
				}

				managedObjects = manager.GetManagedObjects();
			}
			finally 
			{
				agentManager.UnregisterAgent (agentPath);
				profileManager.UnregisterProfile (profilePath);
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

		public const byte PEBBLE_CLIENT_VERSION = 2;

		private static byte[] PebbleHello()
		{
			byte[] prefix = { PEBBLE_CLIENT_VERSION, 0xFF, 0xFF, 0xFF, 0xFF };
			byte[] session = GetBytes( (uint)SessionCaps.GAMMA_RAY );
			byte[] remote = GetBytes( (uint)( RemoteCaps.Telephony | RemoteCaps.SMS | RemoteCaps.Android ) );

			byte[] msg = CombineArrays( prefix, session, remote );
			return msg;
		}

		public static byte[] GetBytes( uint value )
		{
			byte[] bytes = BitConverter.GetBytes( value );
			if ( BitConverter.IsLittleEndian )
				Array.Reverse( bytes );
			return bytes;
		}

		public static byte[] CombineArrays( params byte[][] array )
		{
			var rv = new byte[array.Select( x => x.Length ).Sum()];

			for ( int i = 0, insertionPoint = 0; i < array.Length; insertionPoint += array[i].Length, i++ )
				Array.Copy( array[i], 0, rv, insertionPoint, array[i].Length );
			return rv;
		}
	}

	[Flags]
	public enum RemoteCaps : uint
	{
		Unknown = 0,
		IOS = 1,
		Android = 2,
		OSX = 3,
		Linux = 4,
		Windows = 5,
		Telephony = 16,
		SMS = 32,
		GPS = 64,
		BTLE = 128,
		// 240? No, that doesn't make sense.  But it's apparently true.
		CameraFront = 240,
		CameraRear = 256,
		Accelerometer = 512,
		Gyro = 1024,
		Compass = 2048
	}

	public enum SessionCaps : uint
	{
		GAMMA_RAY = 0x80000000
	}
}

