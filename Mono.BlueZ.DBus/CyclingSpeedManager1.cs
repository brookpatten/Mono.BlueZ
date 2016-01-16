using System;
using System.Collections.Generic;
using DBus;

namespace Mono.BlueZ.DBus
{
	// on /org/bluez/hciX
	[Interface("org.bluez.CyclingSpeedManager1")]
	public interface CyclingSpeedManager1
	{
		void RegisterWatcher (ObjectPath agent);
		void UnregisterWatcher(ObjectPath agent);
	}
}
