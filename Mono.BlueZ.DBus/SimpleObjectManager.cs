using System;
using System.Collections.Generic;
using org.freedesktop.DBus;
using DBus;
using System.Linq;

namespace Mono.BlueZ.DBus
{
	/// <summary>
	/// Simple object manager.
	/// </summary>
	[Interface ("org.freedesktop.DBus.ObjectManager")]
	public class SimpleObjectManager:ObjectManager
	{
		private IDictionary<ObjectPath,IDictionary<string,IDictionary<string,object>>> _managedObjects;

		public SimpleObjectManager ()
		{
			_managedObjects = new Dictionary<ObjectPath,IDictionary<string,IDictionary<string,object>>> ();
		}

		public IDictionary<ObjectPath,IDictionary<string,IDictionary<string,object>>> GetManagedObjects()
		{
			return _managedObjects;
		}

		public event InterfacesAddedHandler InterfacesAdded;
		public event InterfacesRemovedHandler InterfacesRemoved;

		public void Update(IDictionary<ObjectPath,IDictionary<string,IDictionary<string,object>>> update)
		{
			var added = new Dictionary<ObjectPath, IDictionary<string,IDictionary<string,object>>> ();
			var removed = new Dictionary<ObjectPath,IList<string>> ();

			foreach (var path in update.Keys) {
				if (_managedObjects.ContainsKey (path)) {
					var pathAdd = new Dictionary<string,IDictionary<string,object>> ();
					foreach (var iface in update[path].Keys) {
						if (!_managedObjects [path].ContainsKey (iface)) {
							pathAdd.Add (iface, update [path] [iface]);
						}
					}
					added.Add (path, pathAdd);
				} else {
					added.Add (path, update [path]);
				}
			}

			foreach (var path in _managedObjects.Keys) {
				if (update.ContainsKey (path)) {
					var pathRemove = new List<string> ();
					foreach (var iface in _managedObjects[path].Keys) {
						if (!update[path].ContainsKey (iface)) {
							pathRemove.Add(iface);
						}
					}
					removed.Add (path, pathRemove);
				} else {
					removed.Add (path, _managedObjects [path].Keys.ToList ());
				}
			}

			if (added.Any () && InterfacesAdded!=null) {
				foreach (var key in added.Keys) {
					InterfacesAdded (key, added [key]);
				}
			}

			if (removed.Any() && InterfacesRemoved != null) {
				foreach (var key in removed.Keys) {
					InterfacesRemoved (key, removed[key].ToArray());
				}
			}
			
			_managedObjects = update;
		}
	}
}

