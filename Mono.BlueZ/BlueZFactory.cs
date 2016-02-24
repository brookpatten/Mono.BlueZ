using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DBus;
using Mono.BlueZ.DBus;
using org.freedesktop.DBus;

namespace Mono.BlueZ
{
    public class BlueZFactory
    {
        private Bus _system;
        public Exception _startupException { get; private set; }
        private ManualResetEvent _started = new ManualResetEvent(false);

        private const string BlueZRootPath = "/org/bluez";
        private const string BlueZService = "org.bluez";

        private ObjectManager _objectManager;
        private AgentManager1 _agentManager;
        private ProfileManager1 _profileManager;

        public BlueZFactory()
		{
			// Run a message loop for DBus on a new thread.
			var t = new Thread(DBusLoop);
			t.IsBackground = true;
			t.Start();
			_started.WaitOne(60 * 1000);
			_started.Close();
            if (_startupException != null)
            {
                throw _startupException;
            }
            else
            {
                _objectManager = _system.GetObject<org.freedesktop.DBus.ObjectManager>(BlueZService, ObjectPath.Root);
            }
		}

        private void DBusLoop()
        {
            try
            {
                _system = Bus.System;
            }
            catch (Exception ex)
            {
                _startupException = ex;
                return;
            }
            finally
            {
                _started.Set();
            }

            while (true)
            {
                _system.Iterate();
            }
        }

        public void RegisterAgent(Agent1 agent)
        {
            var agentPath = GetObjectPathForLocalObject(agent);
            _system.Register(agentPath, agent);
            AgentManager.RegisterAgent(agentPath, "KeyboardDisplay");
        }

        public void SetDefaultAgent(Agent1 agent)
        {
            var agentPath = GetObjectPathForLocalObject(agent);
            AgentManager.RequestDefaultAgent(agentPath);
        }

        private AgentManager1 AgentManager
        {
            get
            {
                if (_agentManager == null)
                {
                    _agentManager = _system.GetObject<AgentManager1>(BlueZService, new ObjectPath(BlueZRootPath));
                }
                return _agentManager;
            }
        }

        private ProfileManager1 profileManager1
        {
            get
            {
                if (_profileManager == null)
                {
                    _profileManager = _system.GetObject<ProfileManager1>(BlueZService, new ObjectPath(BlueZRootPath));
                }
                return _profileManager;
            }
        }

        private ObjectPath GetObjectPathForLocalObject(object obj)
        {
            return new ObjectPath("/" + obj.GetHashCode());
        }
    }
}
