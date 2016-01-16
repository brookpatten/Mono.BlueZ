using System;
using System.Collections.Generic;
using DBus;

namespace Mono.BlueZ.DBus
{
	// on /org/bluez/hciX/dev_XX_XX_XX_XX_XX_XX/serviceXX/charYYYY
	[Interface("org.bluez.GattCharacteristic1")]
	public interface GattCharacteristic1
	{
		byte[] ReadValue ();
		void WriteValue(byte[] value);
		void StartNotify();
		void StopNotify ();

		string UUID{ get; }
		ObjectPath Service{ get; }
		byte[] Value{ get; }
		bool Notifying{get;}
		IList<string> Flags{get;}
		IList<object> Descriptors{get;}
	}
}
