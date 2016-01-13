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
		void RemoveDevice(ObjectPath device);
		void SetDiscoveryFilter (IDictionary<string,object> properties);
		void StartDiscovery();
		void StopDiscovery();

		IList<string> UUIDs{ get; }
		bool Discoverable{get;set;}
		bool Discovering{get;}
		bool Pairable{get;set;}
		bool Powered{get;set;}
		string Address{ get; }
		string Alias{get;set;}
		string Modalias{ get; }
		string Name{get;}
		uint Class{ get; }
		uint DiscoverableTimeout{get;set;}
		uint PairableTimeout{ get; set; }
	}

	[Interface("org.bluez.CyclingSpeedManager1")]
	public interface CyclingSpeedManager1
	{
		void RegisterWatcher (ObjectPath agent);
		void UnregisterWatcher(ObjectPath agent);
	}

	[Interface("org.bluez.HeartRateManager1")]
	public interface HeartRateManager1
	{
		void RegisterWatcher (ObjectPath agent);
		void UnregisterWatcher(ObjectPath agent);
	}

	[Interface("org.bluez.Media1")]
	public interface Media1
	{
		void RegisterEndpoint (ObjectPath endpoint,IDictionary<string,object> properties);
		void RegisterPlayer (ObjectPath player,IDictionary<string,object> properties);
		void UnregisterEndpoint(ObjectPath endpoint);
		void UnregisterPlayer(ObjectPath player);
	}

	[Interface("org.bluez.NetworkServer1")]
	public interface NetworkServer1
	{
		void Register (string uuid,string bridge);
		void Unregister(string uuid);
	}

	[Interface("org.bluez.ThermometerManager1")]
	public interface ThermometerManager1
	{
		void DisableIntermediateMeasurement (ObjectPath endpoint);
		void EnableIntermediateMeasurement (ObjectPath player);
		void RegisterWatcher(ObjectPath endpoint);
		void UnregisterWatcher(ObjectPath player);
	}

	[Interface("org.bluez.Device1")]
	public interface Device1
	{
		void CancelPairing();
		void Connect();
		void ConnectProfile(string UUID);
		void Disconnect();
		void DisconnectProfile(string UUID);
		void Pair ();

		IList<string> UUIDs{get;}
		bool Blocked{get;set;}
		bool Connected{ get; }
		bool LegacyPairing{ get; }
		bool Paired{get;}
		bool Trusted{get;set;}
		short RSSI{ get; }
		ObjectPath Adapter{ get; }
		string Address{get;}
		string Alias{get;}
		string Icon{ get; }
		string Modalias{get;}
		string Name{get;}
		ushort Appearance{get;}
		uint Class{get;}
	}

	[Interface("org.bluez.MediaControl1")]
	public interface MediaControl1
	{
		void FastForward();
		void Next();
		void Pause();
		void Play();
		void Previous();
		void Rewind();
		void Stop();
		void VolumeDown();
		void VolumeUp();

		bool Connected{get;}
	}
}



