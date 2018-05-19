using System;
using System.IO;
using System.Security.Cryptography;
using RustBuster2016;
using RustBuster2016.API;
using RustBuster2016.API.Events;
using Hooks = RustBuster2016.API.Hooks;

namespace WorldEditor
{
    public class WorldEditor : RustBuster2016.API.RustBusterPlugin
    {
        private bool IsAdmin = false;
        private bool Enabled = false;
        public IniParser SavedData;
        
        public override string Name
        {
            get { return "WorldEditor"; }
        }

        public override string Author
        {
            get { return "DreTaX"; }
        }

        public override Version Version
        {
            get { return new Version("1.0"); }
        }

        public override void DeInitialize()
        {
            IsAdmin = false;
            Enabled = false;
            RustBuster2016.API.Hooks.OnRustBusterClientPluginsLoaded -= Loaded;
            RustBuster2016.API.Hooks.OnRustBusterClientChat -= OnRustBusterClientChat;
        }

        public override void Initialize()
        {
            if (!Directory.Exists(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\WorldEditor"))
            {
                Directory.CreateDirectory(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\WorldEditor");
            }
            if (!File.Exists(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\WorldEditor\\Objects.ini"))
            {
                File.Create(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\WorldEditor\\Objects.ini").Dispose();
            }
            SavedData = new IniParser(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\WorldEditor\\Objects.ini");
            RustBuster2016.API.Hooks.OnRustBusterClientPluginsLoaded += Loaded;
            RustBuster2016.API.Hooks.OnRustBusterClientChat += OnRustBusterClientChat;
            
            //todo: load each section to the world, add editable, and moving functions.
        }

        private void OnRustBusterClientChat(ChatEvent ce)
        {
            if (ce.ChatUI.textInput.Text.Contains("/worldedit"))
            {
                if (ce.ChatUI.textInput.Text == "/worldedit")
                {
                    Enabled = !Enabled;
                    Rust.Notice.Inventory("", "WorldEdit Toggled: " + Enabled);
                }
                else
                {
                    string[] args = ce.ChatUI.textInput.Text.Split(new string[] { "/worldedit"}, StringSplitOptions.None)[1].Split(' ');
                    if (args.Length > 0)
                    {
                        string arg = args[0];
                        switch (arg)
                        {
                            case "editsection":
                                //todo
                                break;
                            case "idunnowhat":
                                break;
                        }
                    }
                }
            }
        }

        private void Loaded()
        {
            IsAdmin = this.SendMessageToServer("worldeditadmin").Contains("yes");
        }
    }
}