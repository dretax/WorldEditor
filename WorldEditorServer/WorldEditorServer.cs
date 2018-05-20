using System;
using Fougerite;
using RustBuster2016Server;
using UnityEngine;

namespace WorldEditorServer
{
    public class WorldEditorServer : Fougerite.Module
    {
        private bool _FoundRB;
        public static string AssetPath = "file:///";
        
        public override string Name
        {
            get { return "WorldEditorServer"; }
        }

        public override string Author
        {
            get { return "DreTaX & Salva"; }
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
            Caching.expirationDelay = 1;
            Caching.CleanCache();
            Hooks.OnModulesLoaded += OnModulesLoaded;
            AssetPath = AssetPath + @Util.GetRootFolder() + "\\Save\\WorldEditorServer\\myasset.unity3d";
            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("WorldEditor\\", Util.GetRootFolder() + "\\Save\\WorldEditorServer\\myasset.unity3d"));
        }

        public override void DeInitialize()
        {
            Hooks.OnModulesLoaded -= OnModulesLoaded;
            if (_FoundRB)
            {
                RustBuster2016Server.API.OnRustBusterUserMessage -= OnRustBusterUserMessage;
            }

            Caching.expirationDelay = 1;
            Caching.CleanCache();
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