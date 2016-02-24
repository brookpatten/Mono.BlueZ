using System;
using System.Collections.Generic;

using DBus;

namespace Mono.BlueZ.DBus
{
	// on /org/bluez
	[Interface("org.bluez.AgentManager1")]
	public interface AgentManager1
	{
		void RegisterAgent(ObjectPath agent,string capability);
		void RequestDefaultAgent(ObjectPath agent);
		void UnregisterAgent(ObjectPath agent);
	}
}
