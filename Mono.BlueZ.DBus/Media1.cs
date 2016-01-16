using System;
using System.Collections.Generic;
using DBus;

namespace Mono.BlueZ.DBus
{
	// on /org/bluez/hciX
	[Interface("org.bluez.Media1")]
	public interface Media1
	{
		void RegisterEndpoint (ObjectPath endpoint,IDictionary<string,object> properties);
		void RegisterPlayer (ObjectPath player,IDictionary<string,object> properties);
		void UnregisterEndpoint(ObjectPath endpoint);
		void UnregisterPlayer(ObjectPath player);
	}
}
