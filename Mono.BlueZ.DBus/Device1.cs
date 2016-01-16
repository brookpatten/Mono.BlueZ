using System;
using System.Collections.Generic;
using DBus;

namespace Mono.BlueZ.DBus
{
	// on /org/bluez/hciX/dev_XX_XX_XX_XX_XX_XX
	[Interface("org.bluez.Device1")]
	public interface Device1
	{
		void CancelPairing();
		void Connect();
		void ConnectProfile(string UUID);
		void Disconnect();
		void DisconnectProfile(string UUID);
		void Pair ();

		string[] UUIDs{get;}
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
}
