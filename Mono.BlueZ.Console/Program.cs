using System;
using Mono.BlueZ;

namespace Mono.BlueZ.Console
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler (GlobalHandler);
			var bootstrap = new Bootstrap ();
			bootstrap.Run (true,"hci1");
		}

		static void GlobalHandler(object sender, UnhandledExceptionEventArgs args) 
		{
			Exception e = (Exception) args.ExceptionObject;
			System.Console.WriteLine(e.Message);
		}
	}
}
