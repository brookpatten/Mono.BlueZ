using System;
using System.Collections.Generic;
using DBus;

namespace Mono.BlueZ.DBus
{
	// on /org/bluez/hciX/dev_XX_XX_XX_XX_XX_XX/serviceXX/charYYYY/descriptorZZZ
	[Interface("org.bluez.GattDescriptor1")]
	public interface GattDescriptor1
	{
        byte[] ReadValue(IDictionary<string, object> flags);
        void WriteValue(byte[] value, IDictionary<string, object> flags);

        string UUID { get; }
        ObjectPath Characteristic { get; }
        string[] Flags { get; }
	}
}
