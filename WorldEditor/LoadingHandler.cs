using System;
using System.Collections;
using UnityEngine;

namespace WorldEditor
{
    public class LoadingHandler : MonoBehaviour
    {
        public static AssetBundle bundle;

        public GameObject ourobject;
        public LoadObjectFromBundle Spawner;

        public IEnumerator LoadAsset()
        {
            WWW www = WWW.LoadFromCacheOrDownload(WorldEditor.AssetPath, 1);
            yield return www;
            if (www.error != null)
            {
                RustBuster2016.API.Hooks.LogData("WorldEditor", "www: " + www.error);
                //return;
            }
            bundle = www.assetBundle;
            www.Dispose();
            ourobject = new GameObject();
            Spawner = ourobject.AddComponent<LoadObjectFromBundle>();
            UnityEngine.Object.DontDestroyOnLoad(ourobject);
            
            foreach (UnityEngine.Object item in bundle.LoadAll())
            {
                if (item.name.ToLower().Contains("mat") || item.name.ToLower().Contains("avatar") || item.name.ToLower().Contains("img"))
                {
                    continue;
                }

                if (!WorldEditor.Instance.Prefabs.Contains(item.name))
                {
                    WorldEditor.Instance.Prefabs.Add(item.name);
                }
            }
        }
        
        public class LoadObjectFromBundle : MonoBehaviour
        {
            private GameObject _ObjectInstantiate;
            private string _name;
            private Vector3 _pos;
            private Quaternion _rot;
            private Vector3 _siz;

            public void Create(string name, Vector3 pos, Quaternion rot, Vector3 siz)
            {
                _name = name;
                _pos = pos;
                _rot = rot;
                _siz = siz;
                try
                {
                    _ObjectInstantiate = bundle.Load(_name, typeof(GameObject)) as GameObject;
                    if (_ObjectInstantiate != null)
                    {
                        _ObjectInstantiate.transform.localScale = siz;
                        _ObjectInstantiate = (GameObject) Instantiate(_ObjectInstantiate, _pos, _rot);
                    }
                }
                catch (Exception ex)
                {
                    RustBuster2016.API.Hooks.LogData("CustomObjects", "Exception: " + ex);
                }
            }
            
            public GameObject ObjectInstantiate
            {
                get { return _ObjectInstantiate; }
            }

            public string Name
            {
                get { return _name; }
            }

            public Vector3 Position
            {
                get { return _pos; }
            }

            public Quaternion Rotation
            {
                get { return _rot; }
            }

            public Vector3 Size
            {
                get { return _siz; }
            }
        }
    }
}