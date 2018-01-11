using System;

using DBus;

namespace Mono.BlueZ.Console
{
    public class TestService : Service
    {
        private const bool primary = true;
        private const string testServiceUUID = "12345678-1234-5678-1234-56789abcdef0";

        public TestService(Bus bus, int index) : base(bus, index, testServiceUUID, primary)
        {
            AddCharacteristic(new TestCharacteristic(bus, 0, GetPath()));
        }
    }
}
