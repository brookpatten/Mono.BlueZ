using System;
using System.Collections.Generic;
using DBus;

namespace Mono.BlueZ.DBus
{
	// on /org/bluez/hciX
	//appears on all adapters, even ones that don't do LE.... which is kindof odd
	[Interface("org.bluez.GattManager1")]
	public interface GattManager1
	{
        void RegisterApplication(ObjectPath application, IDictionary<string, object> options);
        void UnregisterApplication(ObjectPath application);
	}
}
