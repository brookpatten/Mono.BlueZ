using System.Collections.Generic;

using DBus;

namespace Mono.BlueZ.DBus
{
	[Interface("org.bluez.Profile1")]
	public interface Profile1
	{
		void Release ();
		void NewConnection (ObjectPath device, FileDescriptor fd, IDictionary<string,object> properties);
		void RequestDisconnection (ObjectPath device);
	}
}
