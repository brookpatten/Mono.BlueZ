using System;
using System.Collections.Generic;
using NDesk.DBus;

namespace Mono.BlueZ.DBus
{
	[Interface("org.bluez.AgentManager1")]
	public interface AgentManager1
	{
		void RegisterAgent(ObjectPath agent,string capability);
		void RequestDefaultAgent(ObjectPath agent);
		void UnregisterAgent(ObjectPath agent);
	}

	[Interface("org.bluez.Alert1")]
	public interface Alert1
	{
		void NewAlert(string category,UInt16 count,string description);
		void RegisterAlert(string category,ObjectPath agent);
		void UnreadAlert(string category,UInt16 count);
	}

	[Interface("org.bluez.HealthManager1")]
	public interface HealthManager1
	{
		ObjectPath CreateApplication(IDictionary<string,object> config);
		void DestroyApplication(ObjectPath application);
	}

	[Interface("org.bluez.ProfileManager1")]
	public interface ProfileManager1
	{
		void RegisterProfile(ObjectPath profile,string UUID,IDictionary<string,object> options);
		void UnregisterProfile(ObjectPath profile);
	}

	[Interface("org.bluez.Adapter1")]
	public interface Adapter1
	{
	}

	[Interface("org.bluez.CyclingSpeedManager1")]
	public interface CyclingSpeedManager1
	{
	}
}

