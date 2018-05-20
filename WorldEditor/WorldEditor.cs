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
        
        public IniParser SavedData;
        public static string AssetPath = "file://";
        public GameObject MainHolder;
        public LoadingHandler Handler;
        public Editor Editor;
        public List<string> Prefabs = new List<string>();
        public List<LoadingHandler.LoadObjectFromBundle> AllSpawnedObjects = new List<LoadingHandler.LoadObjectFromBundle>();
        
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
            Caching.CleanCache();
        }
        
        public override void Initialize()
        {
            _inst = this;
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
                //todo admin check
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