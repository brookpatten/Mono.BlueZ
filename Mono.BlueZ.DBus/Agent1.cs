using System;
using System.Collections.Generic;

using DBus;

namespace Mono.BlueZ.DBus
{
	[Interface("org.bluez.Agent1")]
	public interface Agent1
	{
		void Release();
		string RequestPinCode(ObjectPath device);
		void DisplayPinCode(ObjectPath device,string pinCode);
		uint RequestPasskey(ObjectPath device);
		void DisplayPasskey (ObjectPath device, uint passkey, ushort entered);
		void RequestConfirmation(ObjectPath device,uint passkey);
		void RequestAuthorization(ObjectPath device);
		void AuthorizeService(ObjectPath device,string uuid);
		void Cancel();
	}
}

