using System;
using System.Collections.Generic;

using DBus;

namespace Mono.BlueZ.DBus
{
	// on /org/bluez/hciX
	[Interface("org.bluez.ThermometerManager1")]
	public interface ThermometerManager1
	{
		void DisableIntermediateMeasurement (ObjectPath endpoint);
		void EnableIntermediateMeasurement (ObjectPath player);
		void RegisterWatcher(ObjectPath endpoint);
		void UnregisterWatcher(ObjectPath player);
	}
}
