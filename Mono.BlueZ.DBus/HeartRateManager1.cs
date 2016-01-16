using System;
using System.Collections.Generic;
using DBus;

namespace Mono.BlueZ.DBus
{
	// on /org/bluez/hciX
	[Interface("org.bluez.HeartRateManager1")]
	public interface HeartRateManager1
	{
		void RegisterWatcher (ObjectPath agent);
		void UnregisterWatcher(ObjectPath agent);
	}
}
