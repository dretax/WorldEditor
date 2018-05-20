using System;
using uLink;
using UnityEngine;
using MonoBehaviour = UnityEngine.MonoBehaviour;

namespace WorldEditor
{
    public class Editor : MonoBehaviour
    {
        public bool ShowList = false;
        public GameObject TempGameObject;
        public LoadingHandler.LoadObjectFromBundle SpawnedObject;
        public int Grid = 0;
        public static GUIStyle style1 = new GUIStyle();
        public static GUIStyle style2 = new GUIStyle();
        public Texture2D texture;
        
        GUIContent[] comboBoxList;
        private ComboBox comboBoxControl;// = new ComboBox();
        private GUIStyle listStyle = new GUIStyle();

        public void Start()
        {
            const string img =
                "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAIBAQIBAQICAgICAgICAwUDAwMDAwYEBAMFBwYHBwcGBwcICQsJCAgKCAcHCg0KCgsMDAwMBwkODw0MDgsMDAz/2wBDAQICAgMDAwYDAwYMCAcIDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAz/wAARCABkAGQDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD+f+iiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigD/2Q==";
            byte[] bytes = Convert.FromBase64String(img);
            texture = new Texture2D(200, 200, TextureFormat.RGBA32, false);
            texture.LoadImage(bytes);
            
            style1 = new GUIStyle();

            style1.fontSize = Screen.height * 2 / 130;
            style1.richText = true;
            style1.alignment = TextAnchor.MiddleCenter;

            style1.normal.background = texture;
            style1.normal.textColor = Color.yellow;

            style1.hover.background = texture;
            style1.hover.textColor = Color.green;

            style1.active.background = texture;
            style1.active.textColor = Color.red;
            
            style2 = new GUIStyle();

            style2.fontSize = Screen.height * 2 / 110;
            style2.richText = true;
            style2.alignment = TextAnchor.MiddleCenter;
            style2.normal.textColor = Color.yellow;
            
            comboBoxList = new GUIContent[WorldEditor.Instance.Prefabs.Count];
            for (int i = 0; i < comboBoxList.Length; i++)
            {
                comboBoxList[i] = new GUIContent(WorldEditor.Instance.Prefabs[i]);
            }
 
            listStyle.normal.textColor = Color.white; 
            listStyle.onHover.background =
            listStyle.hover.background = new Texture2D(2, 2);
            listStyle.padding.left =
            listStyle.padding.right =
            listStyle.padding.top =
            listStyle.padding.bottom = 4;
            
            comboBoxControl = new ComboBox(new Rect(Screen.width / 2, Screen.height - Screen.height + 25, 100, 20), comboBoxList[0], comboBoxList, "button", "box", listStyle);
        }

        public void Update()
        {
            if (!WorldEditor.Instance.Enabled)
            {
                return;
            }
            
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.LeftAlt))
            {
                if (ShowList)
                {
                    Screen.lockCursor = true;
                    ShowList = false;
                }
                else
                {
                    Screen.lockCursor = false;
                    ShowList = true;
                }
            }

            if (SpawnedObject != null && SpawnedObject.ObjectInstantiate != null)
            {
                if (Input.GetKey(KeyCode.Keypad1))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        SpawnedObject.ObjectInstantiate.transform.position += new Vector3(0.1f, 0, 0); //POSX
                    }
                    else
                    {
                        SpawnedObject.ObjectInstantiate.transform.position += new Vector3(0.01f, 0, 0); //POSX
                    }
                }

                if (Input.GetKey(KeyCode.Keypad2))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        SpawnedObject.ObjectInstantiate.transform.position -= new Vector3(0.1f, 0, 0); //POSX
                    }
                    else
                    {
                        SpawnedObject.ObjectInstantiate.transform.position -= new Vector3(0.01f, 0, 0); //POSX
                    }
                }

                if (Input.GetKey(KeyCode.Keypad4))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        SpawnedObject.ObjectInstantiate.transform.position += new Vector3(0, 0, 0.1f); //POSZ
                    }
                    else
                    {
                        SpawnedObject.ObjectInstantiate.transform.position += new Vector3(0, 0, 0.01f); //POSZ
                    }
                }

                if (Input.GetKey(KeyCode.Keypad5))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        SpawnedObject.ObjectInstantiate.transform.position -= new Vector3(0, 0, 0.1f); //POSZ
                    }
                    else
                    {
                        SpawnedObject.ObjectInstantiate.transform.position -= new Vector3(0, 0, 0.01f); //POSZ
                    }
                }

                if (Input.GetKey(KeyCode.Keypad7))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        SpawnedObject.ObjectInstantiate.transform.position += new Vector3(0, 0.1f, 0); //POSY
                    }
                    else
                    {
                        SpawnedObject.ObjectInstantiate.transform.position += new Vector3(0, 0.01f, 0); //POSY
                    }
                }

                if (Input.GetKey(KeyCode.Keypad8))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        SpawnedObject.ObjectInstantiate.transform.position -= new Vector3(0, 0.1f, 0); //POSY
                    }
                    else
                    {
                        SpawnedObject.ObjectInstantiate.transform.position -= new Vector3(0, 0.01f, 0); //POSY
                    }
                }

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.x = rot.x + 0.1f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                    else
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.x = rot.x + 0.01f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                }

                if (Input.GetKey(KeyCode.DownArrow))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.x = rot.x - 0.1f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                    else
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.x = rot.x - 0.01f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                }

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.y = rot.y + 0.1f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                    else
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.y = rot.y + 0.01f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                }

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.y = rot.y - 0.1f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                    else
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.y = rot.y - 0.01f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                }

                if (Input.GetKey(KeyCode.Keypad3))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.z = rot.z + 0.1f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                    else
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.z = rot.z + 0.01f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                }

                if (Input.GetKey(KeyCode.KeypadEnter))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.z = rot.z - 0.1f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                    else
                    {
                        var rot = SpawnedObject.ObjectInstantiate.transform.rotation;
                        rot.z = rot.z - 0.01f;
                        SpawnedObject.ObjectInstantiate.transform.rotation = rot;
                    }
                }


                if (Input.GetKey(KeyCode.KeypadMultiply))
                {
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        SpawnedObject.ObjectInstantiate.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f); //SIZE +
                    }
                    else
                    {
                        SpawnedObject.ObjectInstantiate.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f); //SIZE +
                    }
                }

                if (Input.GetKey(KeyCode.KeypadMinus))
                {
                    if (SpawnedObject.ObjectInstantiate.transform.localScale == Vector3.zero)
                    {
                        return;
                    }
                    if (Input.GetKey(KeyCode.RightControl))
                    {
                        SpawnedObject.ObjectInstantiate.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f); //SIZE -
                    }
                    else
                    {
                        SpawnedObject.ObjectInstantiate.transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f); //SIZE -
                    }
                }
            }
        }
        
        public void OnGUI()
        {
            if (!WorldEditor.Instance.Enabled)
            {
                return;
            }

            try
            {
                Vector3 playerloc = Camera.main.ViewportToWorldPoint(transform.localPosition);


                if (SpawnedObject != null && SpawnedObject.ObjectInstantiate != null)
                {
                    Collider collider = SpawnedObject.ObjectInstantiate.collider;
                    bool hascollider = false;
                    if (collider != null)
                    {
                        hascollider = SpawnedObject.ObjectInstantiate
                            .collider.enabled;
                    }
                    GUI.Label(new Rect(0, Screen.height - 100, Screen.width, 20), "POS:"
                                                                                  + SpawnedObject.ObjectInstantiate
                                                                                      .transform.position.ToString()
                                                                                  + " ROT:" + SpawnedObject
                                                                                      .ObjectInstantiate.transform
                                                                                      .rotation.ToString()
                                                                                  + " Size:" +
                                                                                  SpawnedObject.ObjectInstantiate
                                                                                      .transform.localScale.ToString()
                                                                                  + " Col: " +
                                                                                  hascollider, style2);
                }

                string prefab = "";
                if (ShowList)
                {
                    int selectedItemIndex = comboBoxControl.Show();
                    if (comboBoxList[selectedItemIndex] != null)
                    {
                        prefab = comboBoxList[selectedItemIndex].text;
                    }

                    //GUI.Label(new Rect(130, 10, Screen.width - 150, Screen.height - 150), "dfdsfYou picked " + comboBoxList[selectedItemIndex].text + "!" );
                    //Grid = GUI.SelectionGrid(new Rect(130, 10, Screen.width - 150, Screen.height - 150), Grid, WorldEditor.Instance.Prefabs.ToArray(), 10, style1);
                }

                GUI.Box(new Rect(0, 120, 140, 90), "Object Spawn");
                GUI.Label(new Rect(10, 140, 120, 20), string.Format("<b><color=#298A08>" + prefab + "</color></b> "));


                if (GUI.Button(new Rect(10, 160, 120, 20), "Spawn"))
                {
                    try
                    {
                        TempGameObject = new GameObject();
                        SpawnedObject = TempGameObject.AddComponent<LoadingHandler.LoadObjectFromBundle>();
                        SpawnedObject.Create(WorldEditor.Instance.Prefabs.ToArray()[Grid],
                            new Vector3(playerloc.x + 10f, playerloc.y, playerloc.z + 10f), new Quaternion(0, 0, 0, 0),
                            new Vector3(1, 1, 1));
                        UnityEngine.Object.DontDestroyOnLoad(TempGameObject);
                        Screen.lockCursor = true;
                    }
                    catch (Exception ex)
                    {
                    }
                }

                if (GUI.Button(new Rect(10, 180, 120, 20), "Destroy"))
                {
                    if (SpawnedObject != null && SpawnedObject.ObjectInstantiate != null)
                    {
                        UnityEngine.Object.Destroy(SpawnedObject.ObjectInstantiate);
                        SpawnedObject = null;
                        UnityEngine.Object.Destroy(TempGameObject);
                        TempGameObject = null;
                    }
                }

                GUI.Box(new Rect(0, 310, 140, 120), "Object Control");

                if (GUI.Button(new Rect(10, 360, 120, 20), "To Me"))
                {
                    if (SpawnedObject != null && SpawnedObject.ObjectInstantiate != null)
                    {
                        SpawnedObject.ObjectInstantiate.transform.position = playerloc;
                    }
                }

                if (GUI.Button(new Rect(10, 380, 120, 20), "Reset Rot"))
                {
                    if (SpawnedObject != null && SpawnedObject.ObjectInstantiate != null)
                    {
                        SpawnedObject.ObjectInstantiate.transform.rotation = new Quaternion(0, 0, 0, 1);
                    }
                }

                if (GUI.Button(new Rect(10, 400, 120, 20), "Collider"))
                {
                    if (SpawnedObject != null && SpawnedObject.ObjectInstantiate != null)
                    {
                        Collider collider = SpawnedObject.ObjectInstantiate.collider;
                        if (collider != null)
                        {
                            SpawnedObject.ObjectInstantiate.collider.enabled =
                                !SpawnedObject.ObjectInstantiate.collider.enabled;
                        }
                    }
                }

                const string a = "LIST (LControl + LAlt)" + "\n \n" +
                                 "POSx + (Key 1)" + "\n" +
                                 "POSx - (Key 2)" + "\n" +
                                 "POSz + (Key 4)" + "\n" +
                                 "POSz - (Key 5)" + "\n" +
                                 "POSy + (Key 7)" + "\n" +
                                 "POSy - (Key 8)" + "\n \n" +

                                 "ROTx + (UP)" + "\n" +
                                 "ROTx - (DOWN)" + "\n" +
                                 "ROTy + (LEFT)" + "\n" +
                                 "ROTy - (RIGTH)" + "\n \n" +
                                 "ROTz + (Key 3)" + "\n" +
                                 "ROTz - (Key Intro)" + "\n \n" +

                                 "SIZE + (Key *)" + "\n" +
                                 "SIZE - (Key -)";
                GUI.Label(new Rect(10, 430, 600, 600), a);
            }
            catch (Exception ex)
            {
                RustBuster2016.API.Hooks.LogData("Error", "Something is wrong with the gui: " + ex);
            }
        }
    }
}