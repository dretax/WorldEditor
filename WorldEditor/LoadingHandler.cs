using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace WorldEditor
{
    public class LoadingHandler : MonoBehaviour
    {
        public static AssetBundle bundle;

        public GameObject ourobject;
        public LoadObjectFromBundle Spawner;
        
        private void LoadAllSetObjects()
        {
            if (!File.Exists(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\WorldEditor\\ClientSideAssets.txt"))
            {
                return;
            }
            foreach (string line in File.ReadAllLines(RustBuster2016.API.Hooks.GameDirectory + "\\RB_Data\\WorldEditor\\ClientSideAssets.txt"))
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                try
                {
                    string[] pares = line.Split(':');

                    var nombre = pares[0];
                    var loc = pares[1];
                    var qua = pares[2];
                    var siz = pares[3];

                    // Position
                    string[] locsplit = loc.ToString().Split(',');
                    float posx = float.Parse(locsplit[0]);
                    float posy = float.Parse(locsplit[1]);
                    float posz = float.Parse(locsplit[2]);

                    // Quaternion
                    string[] quasplit = qua.ToString().Split(',');
                    float quax = float.Parse(quasplit[0]);
                    float quay = float.Parse(quasplit[1]);
                    float quaz = float.Parse(quasplit[2]);
                    float quaw = float.Parse(quasplit[3]);

                    // Size
                    string[] sizsplit = siz.ToString().Split(',');
                    float sizx = float.Parse(sizsplit[0]);
                    float sizy = float.Parse(sizsplit[1]);
                    float sizz = float.Parse(sizsplit[2]);


                    GameObject TempGameObject = new GameObject();
                    LoadObjectFromBundle SpawnedObject =
                        TempGameObject.AddComponent<LoadingHandler.LoadObjectFromBundle>();
                    SpawnedObject.Create(nombre, new Vector3(posx, posy, posz), new Quaternion(quax, quay, quaz, quaw),
                        new Vector3(sizx, sizy, sizz));
                    UnityEngine.Object.DontDestroyOnLoad(TempGameObject);
                }
                catch
                {
                    RustBuster2016.API.Hooks.LogData("Error", "Failure to load: " + line);
                }
            }
        }

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
                GameObject go = new GameObject();
                LoadObjectFromBundle lo = go.AddComponent<LoadObjectFromBundle>();
                // Test if we can create the object. If not, it's probably not a prefab so we don't need it.
                bool b = lo.Create(item.name, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), new Vector3(1, 1, 1));
                if (!b)
                {
                    UnityEngine.Object.Destroy(go);
                    continue;
                }
                UnityEngine.Object.Destroy(lo.ObjectInstantiate);
                UnityEngine.Object.Destroy(go);

                if (!WorldEditor.Instance.Prefabs.Contains(item.name))
                {
                    WorldEditor.Instance.Prefabs.Add(item.name);
                }
            }

            LoadAllSetObjects();
            WorldEditor.Instance.Editor = WorldEditor.Instance.MainHolder.AddComponent<Editor>();
        }
        
        public class LoadObjectFromBundle : MonoBehaviour
        {
            private GameObject _ObjectInstantiate;
            private string _name;
            private Vector3 _pos;
            private Quaternion _rot;
            private Vector3 _siz;

            public bool Create(string name, Vector3 pos, Quaternion rot, Vector3 siz)
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
                        WorldEditor.Instance.AllSpawnedObjects.Add(this);
                        //CustomObjectIdentifier id = _ObjectInstantiate.collider.gameObject.AddComponent<CustomObjectIdentifier>();
                        //id.BundleClass = this;
                        return true;
                    }
                }
                catch
                {
                    // ignore
                }

                return false;
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