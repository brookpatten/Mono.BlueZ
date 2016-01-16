using System;
using System.Collections.Generic;
using DBus;

namespace Mono.BlueZ.DBus
{
	// on /org/bluez
	[Interface("org.bluez.Alert1")]
	public interface Alert1
	{
		void NewAlert(string category,UInt16 count,string description);
		void RegisterAlert(string category,ObjectPath agent);
		void UnreadAlert(string category,UInt16 count);
	}
}
