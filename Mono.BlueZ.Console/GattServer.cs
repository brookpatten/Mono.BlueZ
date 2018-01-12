using System;
using System.Collections.Generic;
using System.Threading;

using DBus;
using Mono.BlueZ.DBus;
using org.freedesktop.DBus;

namespace Mono.BlueZ.Console
{
    public class GattServer
    {
        private Bus system;
        private ManualResetEvent _started = new ManualResetEvent(false);
        public Exception _startupException { get; private set; }
        private const string SERVICE = "org.bluez";

        public void Run()
        {
            StartMessageLoopDBus();

            GattManager1 gattManager = null;
            ObjectPath appObjectPath = null;

            try
            {
                System.Console.WriteLine("Fetching objects");

                // 2. Find adapter
                var adapterFound = FindAdapter();

                if (adapterFound == null)
                {
                    System.Console.WriteLine("Couldn't find adapter that supports LE");
                    return;
                }

                gattManager = GetObject<GattManager1>(SERVICE, adapterFound);

                if (gattManager == null)
                {
                    System.Console.WriteLine("Couldn't find Gatt manager.");
                    return;
                }
                else
                {
                    System.Console.WriteLine("Found Gatt manager.");
                }

                var application = new Application(system);
                application.AddService(new TestService(system, 0));

                appObjectPath = application.GetPath();
                var options = new Dictionary<string, object>();
                gattManager.RegisterApplication(appObjectPath, options);

                while(true)
                {
                    // Gatt server is running. Do nothing here.
                }

                //Thread.Sleep(30000);
            }
            catch (Exception exception)
            {
                System.Console.WriteLine(exception);
            }
            finally
            {
                gattManager.UnregisterApplication(appObjectPath);
            }
        }

        private void StartMessageLoopDBus()
        {
            // Run a message loop for DBus on a new thread.
            var t = new Thread(DBusLoop)
            {
                IsBackground = true
            };
            t.Start();
            _started.WaitOne(15 * 1000);
            _started.Close();
            if (_startupException != null)
            {
                throw _startupException;
            }
            else
            {
                System.Console.WriteLine("Bus connected at " + system.UniqueName);
            }
        }

        private void DBusLoop()
        {
            try
            {
                system = Bus.System;
            }
            catch (Exception ex)
            {
                _startupException = ex;
                return;
            }
            finally
            {
                _started.Set();
            }

            while (true)
            {
                system.Iterate();
            }
        }

        private ObjectManager GetCopyObjectManager()
        {
            //get a copy of the object manager so we can browse the "tree" of bluetooth items
            ObjectManager manager = null;

            manager = GetObject<ObjectManager>(SERVICE, ObjectPath.Root);

            if (manager != null)
            {
                System.Console.WriteLine("Registering interface events");

                //register these events so we can tell when things are added/removed (eg: discovery)
                manager.InterfacesAdded += (p, i) =>
                {
                    System.Console.WriteLine(p + " Discovered");
                };
                manager.InterfacesRemoved += (p, i) =>
                {
                    System.Console.WriteLine(p + " Lost");
                };

                System.Console.WriteLine("Done");
            }

            return manager;
        }

        private ObjectPath FindAdapter()
        {
            var manager = GetCopyObjectManager();
            //get the bluetooth object tree
            var managedObjects = manager.GetManagedObjects();
            //find our adapter
            ObjectPath adapterPath = null;
            foreach (var obj in managedObjects.Keys)
            {
                System.Console.WriteLine("Checking " + obj);
                if (managedObjects[obj].ContainsKey(typeof(LEAdvertisingManager1).DBusInterfaceName()))
                {
                    System.Console.WriteLine("Adapter found at" + obj + " that supports LE");
                    adapterPath = obj;
                    break;
                }
            }

            return adapterPath;
        }

        private T GetObject<T>(string busName, ObjectPath path)
        {
            if (system != null)
            {
                var obj = system.GetObject<T>(busName, path);

                if (obj == null)
                {
                    System.Console.WriteLine("Object is NULL");
                }

                return obj;
            }

            System.Console.WriteLine("System is NULL");
            return default(T);
        }
    }
}
