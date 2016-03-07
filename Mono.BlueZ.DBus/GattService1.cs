using System;
using System.Collections.Generic;
using DBus;

namespace Mono.BlueZ.DBus
{
	// on /org/bluez/hciX/dev_XX_XX_XX_XX_XX_XX/serviceXX
	[Interface("org.bluez.GattService1")]
	public interface GattService1
	{
		string UUID{ get; }
		bool Primary{get;}
		ObjectPath Device{ get; }
		ObjectPath[] Characteristics{get;}
		ObjectPath[] Includes{get;}
	}
}
