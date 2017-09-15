using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FloreniconEmulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Lua lua;
        private WoW_API api;
        private WoWEvents events;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void registerWoWApiFunctions()
        {
            lua.RegisterFunction("UnitClass", api, api.GetType().GetMethod("UnitClass"));
            lua.RegisterFunction("UnitName", api, api.GetType().GetMethod("UnitName"));
            lua.RegisterFunction("UnitAffectingCombat", api, api.GetType().GetMethod("UnitAffectingCombat"));
            lua.RegisterFunction("GetTime", api, api.GetType().GetMethod("GetTime"));
            lua.RegisterFunction("max", api, api.GetType().GetMethod("max"));
            lua.RegisterFunction("floor", api, api.GetType().GetMethod("floor"));

            lua.RegisterFunction("GetWoWEventsObject", events, events.GetType().GetMethod("Instance"));

            lua.RegisterFunction("GetDefaultChatFrame", this, this.GetType().GetMethod("GetDefaultChatFrame"));

            lua.DoString(
                "function wipe(arr)\n" +
                "  arr = {};\n" +
                "  return arr;\n" +
                "end\n"
                );

            lua.DoString(
                "function tinsert(arr, element)\n" +
                "  table.insert(arr, element);\n" +
                "  return arr;\n" +
                "end\n"
            );

            lua.DoString(
                "function tremove(arr, index)\n" +
                "  table.remove(arr, index);\n" +
                "  return arr;\n" +
                "end\n"
            );

            lua.DoString("DEFAULT_CHAT_FRAME = GetDefaultChatFrame();");
        }

        public MainWindow GetDefaultChatFrame()
        {
            return this;
        }

        public void AddMessage(string message, float red, float green, float blue)
        {
            textBox.Text = textBox.Text + "\n" + message;
        }

        private void GenerateSpellEvent(string type, string target, int spellid, int amount, int overheal, bool crit)
        {
            lua.DoString("Florenicon_OnEvent(false, false, false, \"" + type + "\", false, false, \"Merinoura\", false, false, false, \"" + target + "\", false, false, " + spellid.ToString() + ", false, false, " + amount.ToString() + ", " + overheal.ToString() + ", absorb, " + (crit ? "1" : "false") + ")");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            api = new WoW_API();
            events = new WoWEvents(api);

            lua = new Lua();
            lua.DebugHook += (s, ev) => {
                textBox.Text = textBox.Text + "\n" + ev.LuaDebug.ToString();
            };

            registerWoWApiFunctions();

            var objs = lua.DoFile("C:\\Program Files (x86)\\World of Warcraft\\Interface\\AddOns\\Florenicon\\Florenicon.lua");
            if (objs != null)
            {
                foreach (var obj in objs)
                {
                    textBox.Text = textBox.Text + "\n" + obj.ToString();
                }
            }

            lua.DoString("Florenicon_OnLoad(GetWoWEventsObject());");

            GenerateSpellEvent("SPELL_AURA_APPLIED", "Player1", 81269, 100, 10, false);
            GenerateSpellEvent("SPELL_CAST_SUCCESS", "Player1", 81269, 100, 10, false);

            Thread.Sleep(1000);
            api.PlayerInCombat = false;
            GenerateSpellEvent("SPELL_HEAL", "Player1", 81269, 100, 10, false);
            GenerateSpellEvent("SPELL_HEAL", "Player2", 81269, 100, 10, false);

            Thread.Sleep(1000);
            api.PlayerInCombat = true;
            GenerateSpellEvent("SPELL_HEAL", "Player3", 81269, 100, 10, false);
            GenerateSpellEvent("SPELL_HEAL", "Player4", 81269, 100, 10, false);
            Thread.Sleep(1000);
            GenerateSpellEvent("SPELL_HEAL", "Player3", 81269, 150, 100, true);
            GenerateSpellEvent("SPELL_HEAL", "Player4", 81269, 100, 10, false);
            api.PlayerInCombat = false;

            Thread.Sleep(1000);
            GenerateSpellEvent("SPELL_HEAL", "Player5", 81269, 100, 10, false);
            GenerateSpellEvent("SPELL_HEAL", "Player6", 81269, 100, 10, false);

            Thread.Sleep(1000);
            Thread.Sleep(1000);
            Thread.Sleep(1000);
            Thread.Sleep(1000);
            Thread.Sleep(1000);
        }
    }
}
