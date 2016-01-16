using System;
using System.Collections.Generic;
using DBus;

namespace Mono.BlueZ.DBus
{
	// on /org/bluez
	[Interface("org.bluez.HealthManager1")]
	public interface HealthManager1
	{
		ObjectPath CreateApplication(IDictionary<string,object> config);
		void DestroyApplication(ObjectPath application);
	}
}
