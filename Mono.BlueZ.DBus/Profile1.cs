using System;
using System.Collections.Generic;
using DBus;

namespace Mono.BlueZ.DBus
{
	// exposed via application and registered by profilemanager
	[Interface("org.bluez.Profile1")]
	public interface Profile1
	{
		void Release ();
		void NewConnection (ObjectPath device, string fileDescriptor, IDictionary<string,object> properties);
		void RequestDisconnection (ObjectPath device);
	}
}
