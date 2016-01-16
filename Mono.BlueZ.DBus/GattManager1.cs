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
		void RegisterProfile(ObjectPath profile, string[] UUIDs,IDictionary<string,object> options);
		void RegisterService(ObjectPath service,IDictionary<string,object> options);
		void UnregisterProfile(ObjectPath profile);
		void UnregisterService(ObjectPath service);
	}
}
