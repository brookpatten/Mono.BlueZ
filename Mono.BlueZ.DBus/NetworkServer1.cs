using System;
using System.Collections.Generic;
using DBus;

namespace Mono.BlueZ.DBus
{
	// on /org/bluez/hciX
	[Interface("org.bluez.NetworkServer1")]
	public interface NetworkServer1
	{
		void Register (string uuid,string bridge);
		void Unregister(string uuid);
	}
}
