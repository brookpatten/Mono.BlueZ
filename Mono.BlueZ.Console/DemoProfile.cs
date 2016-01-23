using System;
using System.Collections.Generic;
using DBus;
using Mono.BlueZ.DBus;
using org.freedesktop.DBus;
using System.IO;

namespace Mono.BlueZ.Console
{
	public class DemoProfile:Profile1
	{
		private FileDescriptor _fileDescriptor;

		public Action<ObjectPath,FileDescriptor,IDictionary<string,object>> NewConnectionAction{get;set;}
		public Action<ObjectPath,FileDescriptor> RequestDisconnectionAction{ get; set; }
		public Action<FileDescriptor> ReleaseAction{ get; set; }

		public DemoProfile ()
		{
		}

		public void Release ()
		{
			System.Console.WriteLine ("Release");
			if (ReleaseAction != null) {
				ReleaseAction (_fileDescriptor);
			}
		}
		public void NewConnection (ObjectPath device, FileDescriptor fileDescriptor, IDictionary<string,object> properties)
		{
			System.Console.WriteLine ("NewConnection");
			_fileDescriptor = fileDescriptor;
			if (NewConnectionAction != null) {
				NewConnectionAction (device, _fileDescriptor, properties);
			}
		}
		public void RequestDisconnection (ObjectPath device)
		{
			System.Console.WriteLine ("RequestDisconnection");
			if (RequestDisconnectionAction != null) {
				RequestDisconnectionAction (device, _fileDescriptor);
			} else {
				if (_fileDescriptor != null) {
					_fileDescriptor.Close ();
				}
			}
		}

	}
}

