using System;
using Mono.BlueZ.DBus;
using DBus;

namespace Mono.BlueZ.Console
{
	public class DemoAgent:Agent1
	{
		public DemoAgent ()
		{
		}
		public void Release()
		{
		}
		public string RequestPinCode(ObjectPath device)
		{
			return "1";
		}
		public void DisplayPinCode(ObjectPath device,string pinCode)
		{
		}
		public uint RequestPasskey(ObjectPath device)
		{
			return 1;
		}
		public void DisplayPasskey (ObjectPath device, uint passkey, ushort entered)
		{
		}
		public void RequestConfirmation(ObjectPath device,uint passkey)
		{
		}
		public void RequestAuthorization(ObjectPath device)
		{
		}
		public void AuthorizeService(ObjectPath device,string uuid)
		{
		}
		public void Cancel()
		{
		}
	}
}

