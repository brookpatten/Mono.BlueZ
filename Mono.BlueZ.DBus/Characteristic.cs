using System;
using System.Collections.Generic;

using DBus;
using Mono.BlueZ.DBus;
using org.freedesktop.DBus;

namespace Mono.BlueZ
{
    public abstract class Characteristic : GattCharacteristic1, Properties
    {
        private const string gattCharacteristic = "org.bluez.GattCharacteristic1";
        private readonly Bus bus;
        private readonly string path;
        private List<Descriptor> descriptors;

        public event PropertiesChangedHandler PropertiesChanged;

        public Characteristic(Bus bus, int index, string UUID, string[] flags, ObjectPath service)
        {
            this.bus = bus;
            this.UUID = UUID;
            Flags = flags;
            Service = service;
            descriptors = new List<Descriptor>();
            path = service + "/char" + index;

            bus.Register(new ObjectPath(path), this);
        }

        public string UUID { get; private set; }

        public ObjectPath Service { get; private set; }

        public byte[] Value { get; protected set; }

        public bool WriteAcquired 
        {
            get
            {
                Console.WriteLine("Characteristic Get WriteAcquired called, returning error");
                throw new NotSupportedException();
            }
        }

        public bool NotifyAcquired
        {
            get
            {
                Console.WriteLine("Characteristic Get NotifyAcquired called, returning error");
                throw new NotSupportedException();
            }
        }

        public bool Notifying
        {
            get
            {
                Console.WriteLine("Characteristic Get Notifying called, returning error");
                throw new NotSupportedException();
            }
        }

        public string[] Flags { get; private set; }

        public ObjectPath GetPath()
        {
            return new ObjectPath(path);
        }

        public void Confirm()
        {
            Console.WriteLine("Characteristic Confirm called, returning error");
            throw new NotSupportedException();
        }

        public virtual byte[] ReadValue(IDictionary<string, object> options)
        {
            Console.WriteLine("Characteristic ReadValue called, returning error");
            throw new NotSupportedException();
        }

        public virtual void WriteValue(byte[] value, IDictionary<string, object> options)
        {
            Console.WriteLine("Characteristic WriteValue called, returning error");
            throw new NotSupportedException();
        }

        public void StartNotify()
        {
            Console.WriteLine("Characteristic StartNotify called, returning error");
            throw new NotSupportedException();
        }

        public void StopNotify()
        {
            Console.WriteLine("Characteristic StopNotify called, returning error");
            throw new NotSupportedException();
        }

        [return: Argument("value")]
        public object Get(string @interface, string propname)
        {
            Console.WriteLine("Characteristic Get called, returning error");
            throw new NotSupportedException();
        }

        public void Set(string @interface, string propname, object value)
        {
            Console.WriteLine("Characteristic Set called, returning error");
            throw new NotSupportedException();
        }

        public void AddDescriptor(Descriptor descriptor)
        {
            descriptors.Add(descriptor);
        }

        public IEnumerable<Descriptor> GetDescriptors()
        {
            return descriptors;
        }

        public IDictionary<string, IDictionary<string, object>> GetProperties()
        {
            var dict = new Dictionary<string, IDictionary<string, object>>();
            var properties = GetAll(gattCharacteristic);

            dict.Add(gattCharacteristic, properties);

            return dict;
        }

        [return: Argument("props")]
        public IDictionary<string, object> GetAll(string @interface)
        {
            if (@interface != gattCharacteristic)
            {
                Console.WriteLine("GattCharacteristic: Not the good interfance for org.bluez.GattCharacteristic1. " + @interface);
                throw new ArgumentException();
            }

            var dict = new Dictionary<string, object>
            {
                { nameof(Service), Service },
                { nameof(UUID), UUID },
                { nameof(Flags), Flags}
            };

            return dict;
        }
    }
}
