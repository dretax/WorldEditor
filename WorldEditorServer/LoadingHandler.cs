using System.Collections;
using Fougerite;
using UnityEngine;

namespace WorldEditorServer
{
    public class LoadingHandler : MonoBehaviour
    {
        public static AssetBundle bundle;

        public GameObject ourobject;
        public LoadObjectFromBundle Spawner;
        
        /// <summary>
        /// Put here all your saved objects from the file like the example is.
        /// Please don't forget to remove all of the objects that are here by default.
        /// </summary>
        internal readonly string[] MyDefinedObjects = new string[]
        {
            "personal_transport_helicopter:6308.106,360.2357,-4514.635:0,0,0,1:1,1,1",
            "personal_transport_helicopter:6317.337,360.2363,-4523.314:0,0,0,1:1,1,1",
            "personal_transport_helicopter:6323.524,360.2387,-4528.035:0,0,0,1:1,1,1",
        };
        
        private void LoadAllSetObjects()
        {
            foreach (string line in MyDefinedObjects)
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
                    Logger.LogError("[WorldEditorServer] Failure to load: " + line);
                }
            }
        }

        public IEnumerator LoadAsset()
        {
            WWW www = WWW.LoadFromCacheOrDownload(WorldEditorServer.AssetPath, 1);
            yield return www;
            if (www.error != null)
            {
                Logger.LogError("[WorldEditorServer] Failure to load www: " + www.error);
            }
            bundle = www.assetBundle;
            www.Dispose();
            ourobject = new GameObject();
            Spawner = ourobject.AddComponent<LoadObjectFromBundle>();
            UnityEngine.Object.DontDestroyOnLoad(ourobject);
            
            foreach (UnityEngine.Object item in bundle.LoadAll())
            {
                
            }
            bundle.Unload(false);
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