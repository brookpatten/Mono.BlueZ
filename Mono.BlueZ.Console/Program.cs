using System;
using Mono.BlueZ;

namespace Mono.BlueZ.Console
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var bootstrap = new Bootstrap ();
			System.Console.WriteLine ("dbus up, bootstrap run");
			bootstrap.Run ();
			System.Console.WriteLine ("bootstrap done");
			System.Console.ReadLine ();
		}
	}
}
