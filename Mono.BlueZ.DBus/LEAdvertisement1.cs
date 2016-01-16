using System;
using System.Collections.Generic;
using DBus;

namespace Mono.BlueZ.DBus
{
	// exposed by client application to advertise
	[Interface("org.bluez.LEAdvertisement1")]
	public interface LEAdvertisement1
	{
		void Release();

		string Type{get;set;}
		IList<string> UUIDs{get;set;}
		IDictionary<string,object> ManufacturerData{get;set;}
		IList<string> SolicitUUIDs{get;set;}
		IDictionary<string,object> ServiceData{get;set;}
		bool IncludeTxPower{get;set;}
	}
}



