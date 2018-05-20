using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using RustBuster2016;
using RustBuster2016.API;
using RustBuster2016.API.Events;
using UnityEngine;
using Hooks = RustBuster2016.API.Hooks;

namespace WorldEditor
{
    public class WorldEditor : RustBuster2016.API.RustBusterPlugin
    {
        private bool IsAdmin = false;
        private static WorldEditor _inst;
        internal bool Enabled = false;
        
        public static string AssetPath = "file://";
        public GameObject MainHolder;
        public LoadingHandler Handler;
        public Editor Editor;
        public List<string> Prefabs = new List<string>();
        public List<LoadingHandler.LoadObjectFromBundle> AllSpawnedObjects = new List<LoadingHandler.LoadObjectFromBundle>();
        public string SavedObjectsPath;
        
        public override string Name
        {
            get { return "WorldEditor"; }
        }

        public override string Author
        {
            get { return "DreTaX & Salva"; }
        }

        public override Version Version
        {
            get { return new Version("1.0"); }
        }
        
        public static WorldEditor Instance
        {
            get { return _inst; }
        }

        public override void DeInitialize()
        {
            IsAdmin = false;
            Enabled = false;
            RustBuster2016.API.Hooks.OnRustBusterClientPluginsLoaded -= Loaded;
            RustBuster2016.API.Hooks.OnRustBusterClientChat -= OnRustBusterClientChat;
            foreach (var x in AllSpawnedObjects)
            {
                if (x.ObjectInstantiate != null)
                {
                    UnityEngine.Object.Destroy(x.ObjectInstantiate);
                }
            }
            UnityEngine.Object.Destroy(MainHolder);
            AllSpawnedObjects.Clear();
            Caching.expirationDelay = 1;
            Caching.CleanCache();
        }
        
        public override void Initialize()
        {
            Caching.expirationDelay = 1;
            Caching.CleanCache();
            _inst = this;
            if (!Directory.Exists(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\WorldEditor"))
            {
                Directory.CreateDirectory(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\WorldEditor");
            }

            SavedObjectsPath = RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\WorldEditor\\Objects.txt";
            if (!File.Exists(SavedObjectsPath))
            {
                File.Create(SavedObjectsPath).Dispose();
            }
            
            RustBuster2016.API.Hooks.OnRustBusterClientPluginsLoaded += Loaded;
            RustBuster2016.API.Hooks.OnRustBusterClientChat += OnRustBusterClientChat;
            
            AssetPath = AssetPath + @RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\WorldEditor\\myasset.unity3d";
            MainHolder = new GameObject();
            Handler = MainHolder.AddComponent<LoadingHandler>();
            UnityEngine.Object.DontDestroyOnLoad(Handler);
            try
            {
                Handler.StartCoroutine(Handler.LoadAsset());
            }
            catch (Exception ex)
            {
                RustBuster2016.API.Hooks.LogData("WorldEditor", "Exception: " + ex);
            }
        }
        
        private void OnRustBusterClientChat(ChatEvent ce)
        {
            if (ce.ChatUI.textInput.Text.Contains("/worldedit"))
            {
                //ce.Cancel(true);
                if (!IsAdmin)
                {
                    return;
                }
                if (ce.ChatUI.textInput.Text == "/worldedit")
                {
                    Enabled = !Enabled;
                    Rust.Notice.Inventory("", "WorldEdit Toggled: " + Enabled);
                }
                /*else
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
                }*/
            }
        }

        private void Loaded()
        {
            IsAdmin = this.SendMessageToServer("worldeditadmin").Contains("yes");
        }
    }
}