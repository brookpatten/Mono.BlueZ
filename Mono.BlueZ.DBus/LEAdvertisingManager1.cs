using System;
using System.Collections.Generic;
using DBus;

namespace Mono.BlueZ.DBus
{
	// on /org/bluez/hciX
	//only appears on adapters that support LE
	[Interface("org.bluez.LEAdvertisingManager1")]
	public interface LEAdvertisingManager1
	{
		void RegisterAdvertisement(ObjectPath advertisement,IDictionary<string,object> options);
		void UnregisterAdvertisement(ObjectPath service);
	}
}
