using System;
using System.Collections.Generic;
using DBus;

namespace Mono.BlueZ.DBus
{
	// on /org/bluez/hciX/dev_XX_XX_XX_XX_XX_XX/serviceXX/charYYYY
	[Interface("org.bluez.GattCharacteristic1")]
	public interface GattCharacteristic1
	{
        byte[] ReadValue(IDictionary<string, object> options);
        void WriteValue(byte[] value, IDictionary<string, object> options);
        void StartNotify();
        void StopNotify();
        void Confirm();

        string UUID { get; }
        ObjectPath Service { get; }
        string[] Flags { get; }
	}
}
