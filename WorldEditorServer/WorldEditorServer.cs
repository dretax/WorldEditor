using System;
using System.IO;
using Fougerite;
using RustBuster2016Server;
using UnityEngine;

namespace WorldEditorServer
{
    public class WorldEditorServer : Fougerite.Module
    {
        private bool _FoundRB;
        public static string AssetPath;
        public GameObject MainHolder;
        public LoadingHandler Handler;
        
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
            get { return new Version("1.1.2"); }
        }

        public override void Initialize()
        {
            AssetPath = "file:///";
            Caching.expirationDelay = 1;
            Caching.CleanCache();
            Hooks.OnModulesLoaded += OnModulesLoaded;
            if (!File.Exists(Util.GetRootFolder() + "\\Save\\WorldEditorServer\\ClientSideAssets.txt"))
            {
                File.Create(Util.GetRootFolder() + "\\Save\\WorldEditorServer\\ClientSideAssets.txt").Dispose();
            }
            AssetPath = AssetPath + @Util.GetRootFolder() + "\\Save\\WorldEditorServer\\myasset.unity3d";
            
            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("WorldEditor\\", Util.GetRootFolder() + "\\Save\\WorldEditorServer\\myasset.unity3d"));
            RustBuster2016Server.API.AddFileToDownload(new RBDownloadable("WorldEditor\\", Util.GetRootFolder() + "\\Save\\WorldEditorServer\\ClientSideAssets.txt"));
            
            MainHolder = new GameObject();
            Handler = MainHolder.AddComponent<LoadingHandler>();
            UnityEngine.Object.DontDestroyOnLoad(Handler);
            try
            {
                Handler.StartCoroutine(Handler.LoadAsset());
            }
            catch (Exception ex)
            {
                Logger.LogError("Couroutine failed. " + ex);
            }

            if (_FoundRB)
            {
                RustBuster2016Server.API.OnRustBusterUserMessage += OnRustBusterUserMessage;
            }
        }

        public override void DeInitialize()
        {
            Hooks.OnModulesLoaded -= OnModulesLoaded;
            if (_FoundRB)
            {
                RustBuster2016Server.API.OnRustBusterUserMessage -= OnRustBusterUserMessage;
            }
            UnityEngine.Object.Destroy(MainHolder);
            if (LoadingHandler.bundle != null) 
            {
                LoadingHandler.bundle.Unload(true); // Unload all objects from the bundle.
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
