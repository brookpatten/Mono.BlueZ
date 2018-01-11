using System;
using System.Collections.Generic;

using DBus;
using Mono.BlueZ.DBus;
using org.freedesktop.DBus;

namespace Mono.BlueZ
{
    public abstract class Service : GattService1, Properties
    {
        private const string gattService = "org.bluez.GattService1";
        private const string pathBase = "/org/bluez/example/service";

        private readonly string path;
        private readonly Bus bus;
        private List<Characteristic> characteristics;

        public string UUID { get; private set; }

        public bool Primary { get; private set; }

        public event PropertiesChangedHandler PropertiesChanged;

        public Service(Bus bus, int index, string UUID, bool primary)
        {
            this.bus = bus;
            this.UUID = UUID;
            Primary = primary;
            characteristics = new List<Characteristic>();
            path = pathBase + index;

            bus.Register(new ObjectPath(path), this);
        }

        public ObjectPath GetPath()
        {
            return new ObjectPath(path);
        }

        public void AddCharacteristic(Characteristic characteristic)
        {
            characteristics.Add(characteristic);
        }

        public List<ObjectPath> GetCharacteristicPaths()
        {
            var characteristicPaths = new List<ObjectPath>();

            foreach (var characteristic in characteristics)
            {
               characteristicPaths.Add(characteristic.GetPath());
            }

            return characteristicPaths;
        }

        public IEnumerable<Characteristic> GetCharacteristics()
        {
            return characteristics;
        }

        [return: Argument("value")]
        public object Get(string @interface, string propname)
        {
            Console.WriteLine("Get called, returning error");
            throw new NotSupportedException();
        }

        public void Set(string @interface, string propname, object value)
        {
            Console.WriteLine("Set called, returning error");
            throw new NotSupportedException();
        }

        public IDictionary<string, IDictionary<string, object>> GetProperties()
        {
            var dict = new Dictionary<string, IDictionary<string, object>>();
            var properties = GetAll(gattService);

            dict.Add(gattService, properties);

            return dict;
        }

        [return: Argument("props")]
        public IDictionary<string, object> GetAll(string @interface)
        {
            if (@interface != gattService)
            {
                Console.WriteLine("GattService: hmm not the good interfance for org.bluez.GattService1. " + @interface);
                throw new ArgumentException();
            }

            var dict = new Dictionary<string, object>
            {
                { nameof(UUID), UUID },
                { nameof(Primary), Primary }
            };

            return dict;
        }
    }
}
