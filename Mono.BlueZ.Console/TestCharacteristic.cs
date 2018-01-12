using System;
using DBus;

namespace Mono.BlueZ.Console
{
    public class TestCharacteristic : Characteristic
    {
        private const string testCharacteristicUUID = "12345678-1234-5678-1234-56789abcdef1";
        private static readonly string[] flags = { "read", "write", "writable-auxiliaries" };
        private byte[] data;


        public TestCharacteristic(Bus bus, int index, ObjectPath service) : base(bus, index, testCharacteristicUUID, flags, service)
        {
            AddDescriptor(new TestDescriptor(bus, 0, GetPath()));
            data = new byte[0];
        }

        public override byte[] ReadValue(System.Collections.Generic.IDictionary<string, object> options)
        {
            System.Console.WriteLine("Characteristic ReadValue: " + BitConverter.ToString(data));
            return data;
        }

        public override void WriteValue(byte[] value, System.Collections.Generic.IDictionary<string, object> options)
        {
            System.Console.WriteLine("Characteristic WriteValue: " + BitConverter.ToString(value));
            data = value;
        }
    }
}
