using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloreniconEmulator
{
    public class WoW_API
    {
        public bool PlayerInCombat = false;

        public WoW_API()
        {

        }

        public string UnitClass(string hint)
        {
            if (hint.Equals("player", StringComparison.OrdinalIgnoreCase))
            {
                return "Druid";
            }
            else
            {
                return "Unknown";
            }
        }

        public string UnitName(string hint)
        {
            if (hint.Equals("player", StringComparison.OrdinalIgnoreCase))
            {
                return "Merinoura";
            }
            else
            {
                return "Unknown";
            }
        }

        public bool UnitAffectingCombat(string hint)
        {
            if (hint.Equals("player", StringComparison.OrdinalIgnoreCase))
            {
                return PlayerInCombat;
            }

            return false;
        }

        public long GetTime()
        {
            var t = DateTime.Now;
            return t.Ticks / 10000000;
        }

        public int max(int a, int b)
        {
            return a > b ? b : a;
        }

        public int floor(double a)
        {
            return (int)Math.Truncate(a);
        }
    }
}
