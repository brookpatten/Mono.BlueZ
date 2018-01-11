using System;
using System.Collections.Generic;

using DBus;
using Mono.BlueZ.DBus;
using org.freedesktop.DBus;

namespace Mono.BlueZ
{
    public abstract class Descriptor : GattDescriptor1, Properties
    {
        private const string gattDescriptor1 = "org.bluez.GattDescriptor1";
        private readonly Bus bus;
        private readonly string path;

        public event PropertiesChangedHandler PropertiesChanged;

        public Descriptor(Bus bus, int index, string UUID, string[] flags, ObjectPath characteristic)
        {
            this.bus = bus;
            this.UUID = UUID;
            Flags = flags;
            Characteristic = characteristic;
            path = characteristic + "/desc" + index;

            bus.Register(new ObjectPath(path), this);
        }

        public string UUID { get; private set; }

        public ObjectPath Characteristic { get; private set; }

        public ObjectPath GetPath()
        {
            return new ObjectPath(path);
        }

        public string[] Flags { get; private set; }

        public virtual byte[] ReadValue(IDictionary<string, object> flags)
        {
            Console.WriteLine("Descriptor ReadValue called, returning error");
            throw new NotSupportedException();
        }

        public virtual void WriteValue(byte[] value, IDictionary<string, object> flags)
        {
            Console.WriteLine("Descriptor ReadValue called, returning error");
            throw new NotSupportedException();
        }

        public IDictionary<string, IDictionary<string, object>> GetProperties()
        {
            var dict = new Dictionary<string, IDictionary<string, object>>();
            var properties = GetAll(gattDescriptor1);

            dict.Add(gattDescriptor1, properties);

            return dict;
        }

        [return: Argument("value")]
        public object Get(string @interface, string propname)
        {
            Console.WriteLine("Descriptor Get called, returning error");
            throw new NotSupportedException();
        }

        public void Set(string @interface, string propname, object value)
        {
            Console.WriteLine("Descriptor Set called, returning error");
            throw new NotSupportedException();
        }

        [return: Argument("props")]
        public IDictionary<string, object> GetAll(string @interface)
        {
            if (@interface != gattDescriptor1)
            {
                throw new ArgumentException();
            }

            var dict = new Dictionary<string, object>
            {
                { nameof(UUID), UUID },
                { nameof(Flags), Flags },
                { nameof(Characteristic), Characteristic }
            };

            return dict;
        }
    }
}
