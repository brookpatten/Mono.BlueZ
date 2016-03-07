using System;
using System.Collections.Generic;
using DBus;

namespace Mono.BlueZ.DBus
{
	// on /org/bluez/hciX/dev_XX_XX_XX_XX_XX_XX/serviceXX/charYYYY/descriptorZZZ
	[Interface("org.bluez.GattDescriptor1")]
	public interface GattDescriptor1
	{
		byte[] ReadValue ();
		void WriteValue(byte[] value);

		string UUID{ get; }
		ObjectPath Characteristic{ get; }
		byte[] Value{ get; }
		string[] Flags{get;}
	}
}
