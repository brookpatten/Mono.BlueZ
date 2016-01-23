using System;
using System.Collections.Generic;
using DBus;
using System.Net;
using System.IO;

namespace Mono.BlueZ.DBus
{
	// exposed via application and registered by profilemanager
	[Interface("org.bluez.Profile1")]
	public interface Profile1
	{
		void Release ();
		void NewConnection (ObjectPath device, FileDescriptor fd, IDictionary<string,object> properties);
		void RequestDisconnection (ObjectPath device);
	}
}
