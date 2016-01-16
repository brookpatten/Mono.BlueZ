using System;
using System.Collections.Generic;
using DBus;

namespace Mono.BlueZ.DBus
{
	// on /org/bluez
	[Interface("org.bluez.ProfileManager1")]
	public interface ProfileManager1
	{
		void RegisterProfile(ObjectPath profile,string UUID,IDictionary<string,object> options);
		void UnregisterProfile(ObjectPath profile);
	}
}
