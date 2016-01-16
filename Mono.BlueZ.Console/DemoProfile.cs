using System;
using System.Collections.Generic;
using DBus;
using Mono.BlueZ.DBus;
using org.freedesktop.DBus;

namespace Mono.BlueZ.Console
{
	[Interface("org.bluez.Profile1")]
	public class Profile1//:Profile1
	{

		private string _fileDescriptor;

		public Profile1 ()
		{

		}

		public void Release ()
		{
		}
		public void NewConnection (ObjectPath device, string fileDescriptor, IDictionary<string,object> properties)
		{
			_fileDescriptor = fileDescriptor;
			System.Console.WriteLine ("Received connection for " + fileDescriptor);
		}
		public void RequestDisconnection (ObjectPath device)
		{
		}

	}
}

