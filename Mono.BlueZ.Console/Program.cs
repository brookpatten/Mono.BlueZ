using System;
using Mono.BlueZ;

namespace Mono.BlueZ.Console
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler (GlobalHandler);
			var bootstrap = new BlendMicroBootstrap ();
			bootstrap.Run ();

			//var bootstrap = new PebbleBootstrap ();
			//bootstrap.Run (true, null);
		}

		static void GlobalHandler(object sender, UnhandledExceptionEventArgs args) 
		{
			Exception e = (Exception) args.ExceptionObject;
			System.Console.WriteLine("AppDomain.UnhandledException: "+e.Message+" "+e.StackTrace);
		}
	}
}
