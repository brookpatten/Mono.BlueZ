using System;

using DBus;
using org.freedesktop.DBus;

namespace Mono.BlueZ.DBus
{
	/// <summary>
	/// constants and utility functions to quickly locate bluez paths
	/// inside dbus
	/// </summary>
	public static class BlueZPath
	{
		public const string Service = "org.bluez";
		public const string RootString = "/org/bluez";
		public static readonly ObjectPath Root = new ObjectPath(RootString);

		public static string AdapterString (string name)
		{
			return string.Format ("{0}/{1}", RootString, name);
		}

		public static ObjectPath Adapter (string name)
		{
			if (string.IsNullOrWhiteSpace (name)) 
			{
				throw new ArgumentNullException ("name");
			}
			name = name.ToLower ();
			if (!name.StartsWith ("hci")) 
			{
				throw new ArgumentException ("Adapter name must start with hci");
			}
			return new ObjectPath (AdapterString(name));
		}

		/// <summary>
		/// Takes a device addess of format XX:XX:XX:XX:XX:XX
		/// returns component of device path eg dev_XX_XX_XX_XX_XX_XX
		/// </summary>
		/// <returns>The component.</returns>
		/// <param name="deviceAddress">Device address.</param>
		public static string DeviceComponent (string deviceAddress)
		{
			//there's obviously more we could do to validate this, but we're just going to 
			//unwisely assume some level of competance on the caller
			if (string.IsNullOrWhiteSpace (deviceAddress) 
			    || deviceAddress.Length != 17
			    || deviceAddress[2]!=':'
			    || deviceAddress[5]!=':'
			    || deviceAddress[8]!=':'
			    || deviceAddress[11]!=':'
			    || deviceAddress[14]!=':') 
			{
				throw new FormatException ("device address is of an invalid format");
			}
			return string.Format("dev_{0}",deviceAddress.ToUpper ().Replace (":", "_"));
		}

		public static string DeviceString (string adapterName, string deviceAddress)
		{
			return string.Format ("{0}/{1}", AdapterString (adapterName), DeviceComponent (deviceAddress));
		}

		public static ObjectPath Device (string adapterName, string deviceAddress)
		{
			return new ObjectPath (DeviceString (adapterName, deviceAddress));
		}
	}
}