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
			System.Console.WriteLine ("Release");
		}
		public string RequestPinCode(ObjectPath device)
		{
			return "1";
		}
		public void DisplayPinCode(ObjectPath device,string pinCode)
		{
			System.Console.WriteLine ("DisplayPinCode");
		}
		public uint RequestPasskey(ObjectPath device)
		{
			return 1;
		}
		public void DisplayPasskey (ObjectPath device, uint passkey, ushort entered)
		{
			System.Console.WriteLine ("DisplayPasskey");
		}
		public void RequestConfirmation(ObjectPath device,uint passkey)
		{
			System.Console.WriteLine ("RequestConfirmation");
		}
		public void RequestAuthorization(ObjectPath device)
		{
			System.Console.WriteLine ("RequestAuthorization");
		}
		public void AuthorizeService(ObjectPath device,string uuid)
		{
			System.Console.WriteLine ("AuthorizeService");
		}
		public void Cancel()
		{
		}
	}
}

