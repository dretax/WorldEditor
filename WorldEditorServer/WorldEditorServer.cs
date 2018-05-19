using System;
using Fougerite;
using RustBuster2016Server;

namespace WorldEditorServer
{
    public class WorldEditorServer : Fougerite.Module
    {
        private bool _FoundRB;
        
        public override string Name
        {
            get { return "WorldEditorServer"; }
        }

        public override string Author
        {
            get { return "DreTaX"; }
        }

        public override string Description
        {
            get { return "WorldEditorServer"; }
        }

        public override Version Version
        {
            get { return new Version("1.0"); }
        }

        public override void Initialize()
        {
            Hooks.OnModulesLoaded += OnModulesLoaded;
        }

        public override void DeInitialize()
        {
            Hooks.OnModulesLoaded -= OnModulesLoaded;
            if (_FoundRB)
            {
                RustBuster2016Server.API.OnRustBusterUserMessage -= OnRustBusterUserMessage;
            }
        }

        private void OnModulesLoaded()
        {
            foreach (var x in Fougerite.ModuleManager.Modules)
            {
                if (x.Plugin.Name.ToLower().Contains("rustbuster"))
                {
                    _FoundRB = true;
                    RustBuster2016Server.API.OnRustBusterUserMessage += OnRustBusterUserMessage;
                    break;
                }
            }
        }

        private void OnRustBusterUserMessage(API.RustBusterUserAPI user, Message msgc)
        {
            if (msgc.PluginSender == "WorldEditor")
            {
                if (msgc.MessageByClient == "worldeditadmin")
                {
                    msgc.ReturnMessage = user.Player.Admin ? "yes" : "no";
                }
            }
        }
    }
}