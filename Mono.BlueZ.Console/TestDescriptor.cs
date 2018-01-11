using System;
using DBus;

namespace Mono.BlueZ.Console
{
    public class TestDescriptor : Descriptor
    {
        private const string testDescriptorUUID = "12345678-1234-5678-1234-56789abcdef2";
        private static readonly string[] flags = { "read", "write" };
        private byte[] data;

        public TestDescriptor(Bus bus, int index, ObjectPath characteristic) : base(bus, index, testDescriptorUUID, flags, characteristic)
        {
            data = new byte[] { 0x61, 0x62, 0x63, 0x64 };
        }

        public override byte[] ReadValue(System.Collections.Generic.IDictionary<string, object> flags)
        {
            System.Console.WriteLine("Descriptor ReadValue: " + BitConverter.ToString(data));
            return data;
        }

        public override void WriteValue(byte[] value, System.Collections.Generic.IDictionary<string, object> flags)
        {
            System.Console.WriteLine("Descriptor ReadValue: " + BitConverter.ToString(value));
            data = value;
        }
    }
}
