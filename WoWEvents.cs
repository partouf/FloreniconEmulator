using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloreniconEmulator
{
    public class WoWEvents
    {
        private List<string> RegisteredEvents = null;

        public WoWEvents(WoW_API api)
        {
            RegisteredEvents = new List<string>();
        }

        public void RegisterEvent(string eventname)
        {
            RegisteredEvents.Add(eventname);
        }

        public WoWEvents Instance()
        {
            return this;
        }
    }
}
