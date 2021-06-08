#if (UNITY_EDITOR) 
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Utility;
using System;
using TMPro;
using UnityEngine.UIElements;

public class UnfinishedGamesTools : EditorWindow
    {

    [ExecuteInEditMode]
    [MenuItem("Unfinished Games/Tools/Create Folder-System")]
    static public void GenerateFolder()
        {
        Directory.CreateDirectory(Application.dataPath + "/00_Downloads");
        Directory.CreateDirectory(Application.dataPath + "/00_Downloads/00_Design");
        Directory.CreateDirectory(Application.dataPath + "/00_Downloads/01_Tools");
        Directory.CreateDirectory(Application.dataPath + "/01_Scenes");
        Directory.CreateDirectory(Application.dataPath + "/02_Scripts");
        Directory.CreateDirectory(Application.dataPath + "/03_Prefabs");
        Directory.CreateDirectory(Application.dataPath + "/04_SFX");
        Directory.CreateDirectory(Application.dataPath + "/05_Materials");
        Directory.CreateDirectory(Application.dataPath + "/06_Textures");
        Directory.CreateDirectory(Application.dataPath + "/07_Presets");
        Directory.CreateDirectory(Application.dataPath + "/08_Settings");
        Directory.CreateDirectory(Application.dataPath + "/09_Animations");
        Directory.CreateDirectory(Application.dataPath + "/10_Tools");
        Directory.CreateDirectory(Application.dataPath + "/11_Language_Files");
        Directory.CreateDirectory(Application.dataPath + "/12_Fonts");
        Directory.CreateDirectory(Application.dataPath + "/13_Minimap");
        Directory.CreateDirectory(Application.dataPath + "/14_Shared");
        Directory.CreateDirectory(Application.dataPath + "/Resources");
        }

    [MenuItem("Unfinished Games/Tools/Folder Junction")]
    static public void FolderJunktion()
        {
        if (EditorUtility.DisplayDialog("Folder Junction", "Paste data in console  mklink / J \"PATH 1\" \"PATH 2\"", "CMD", "CANCEL"))
            {
            GUIUtility.systemCopyBuffer = "mklink /J \"Unity Path\" \"Dropbox\"";
            Process.Start("cmd.exe", "");
            }
        }
    [MenuItem("Unfinished Games/Tools/Change Font in Scene")]
    static public void ChangeFontInScene()
        {
        if (EditorUtility.DisplayDialog("Change Font in Scene", "Is a Font in Resource Folder called \"Font\"?", "YES", "CANCEL"))
            {
            Text[] AllObjects = GameObject.FindObjectsOfType<Text>();
            foreach (Text TextLabels in AllObjects)
                {
                TextLabels.font = (Font)Resources.Load("Font");
                }
            }
        }
    [MenuItem("Unfinished Games/Tools/Add Highlight to GameObject")]
    static public void AddHighLight()
        {
        Selection.activeGameObject.AddComponent<HierarchyHighlighter>();
        }


    public static T CreateAsset<T>(string path) where T : ScriptableObject
        {
        var temp = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(temp, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return temp;
        }
    public static T LoadAsset<T>(string path) where T : ScriptableObject
        {
        return AssetDatabase.LoadAssetAtPath<T>(path);
        }
    public static T LoadOrCreateAsset<T>(string path) where T : ScriptableObject
        {
        var temp = LoadAsset<T>(path);
        return temp ?? CreateAsset<T>(path);
        }




    [MenuItem("Unfinished Games/Workflow/Brainstorm")]
    static void CreateProjectCreationWindow()
        {
        UnfinishedGamesTools window = new UnfinishedGamesTools();

        window.titleContent.text = Application.companyName + " - " + Application.productName + " Brainstorm";
        window.Show();
        }

    public UnfinishedGamesBrainstormData _data;
    private string _dataPath = @"Assets/10_Tools/UnfinishedGamesTools/Brainstorm.asset";

    public string editorWindowTitleLabel = "Brainstorm Title: ";
    string BrainstormTitle, BrainstormText;
    string SearchString = "";
    int BrainstormTag = 0;
    Vector2 ScrollPosition;
    int ShowTags;

    string[] _options = new string[4] { "IDEAS", "MAYBE", "IN WORK", "DONE" };
    string[] FilterOptions = new string[5] { "All", "IDEAS", "MAYBE", "IN WORK", "DONE" };

    private void OnEnable()
        {
        _data = LoadOrCreateAsset<UnfinishedGamesBrainstormData>(_dataPath);
        }
    void OnGUI()
        {
        GUIStyle MyStyle = new GUIStyle();
        MyStyle.fixedWidth = 50;
        MyStyle.alignment = TextAnchor.UpperRight;
        MyStyle.padding.top = 2;
        MyStyle.clipping = TextClipping.Overflow;
        MyStyle.fontStyle = FontStyle.Bold;
        MyStyle.normal.textColor = EditorStyles.boldLabel.normal.textColor;
        
        EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.MaxWidth(800));
            {
            BrainstormTitle = EditorGUILayout.TextField(editorWindowTitleLabel, BrainstormTitle, GUILayout.MaxWidth(800));
            BrainstormText = EditorGUILayout.TextArea(BrainstormText, GUILayout.Height(100), GUILayout.MaxWidth(800));
            EditorGUILayout.BeginHorizontal();
                {
                GUILayout.Label("Tag:", MyStyle);
                BrainstormTag = EditorGUILayout.Popup(BrainstormTag, _options, GUILayout.MaxWidth(100));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                }
            if (GUILayout.Button("Add Brainstorm Idea"))
                {
                _data.Entries.Add(new BrainStormEntry(BrainstormTitle, BrainstormText, BrainstormTag));
                BrainstormTitle = "";
                BrainstormText = "";
                SaveBrainStromToFile(_data);
                }
            EditorGUILayout.EndVertical();
            }       

        GUILayout.Space(50f);
        EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.MaxWidth(800));
            {
            EditorGUILayout.BeginHorizontal();
                {
                GUILayout.Label("Filter:", MyStyle);
                ShowTags = EditorGUILayout.Popup(ShowTags, FilterOptions, GUILayout.MaxWidth(100));
                GUILayout.FlexibleSpace();
                SearchString = SearchField(SearchString, GUILayout.Width(250));
                EditorGUILayout.EndHorizontal();
                }
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition);
                { 
            foreach (BrainStormEntry item in _data.Entries)
                {
                    if(item == null || SearchString == null)
                        {
                        return;
                        }
                if (item.Tag == ShowTags - 1)
                    {
                    if (item.Text.ToUpper().Contains(SearchString.ToUpper()) || item.Title.ToUpper().Contains(SearchString.ToUpper()))
                        {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.MaxWidth(800));
                            {
                            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(760));
                                {
                                EditorGUILayout.LabelField(item.Title, EditorStyles.boldLabel);
                                GUILayout.FlexibleSpace();
                                if (GUILayout.Button("x", EditorStyles.miniButton))
                                    {
                                    _data.Entries.Remove(item);
                                        SaveBrainStromToFile(_data);
                                    return;
                                    }
                                EditorGUILayout.EndHorizontal();
                                }
                            EditorGUILayout.LabelField(item.Text, EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(750));
                            EditorGUILayout.BeginHorizontal();
                                {
                                GUILayout.FlexibleSpace();
                                GUILayout.Label("Change Tag:", MyStyle);
                                item.Tag = EditorGUILayout.Popup(item.Tag, _options, GUILayout.MaxWidth(100));
                                EditorGUILayout.EndHorizontal();
                                }
                            EditorGUILayout.EndVertical();
                            }
                        }
                    }
                else if (ShowTags == 0)
                    {
                    if (item.Text.ToUpper().Contains(SearchString.ToUpper()) || item.Title.ToUpper().Contains(SearchString.ToUpper()))
                        {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.MaxWidth(800));
                            {
                            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(760));
                                {
                                EditorGUILayout.LabelField(item.Title, EditorStyles.boldLabel);
                                GUILayout.FlexibleSpace();
                                if (GUILayout.Button("x", EditorStyles.miniButton))
                                    {
                                    _data.Entries.Remove(item);
                                        SaveBrainStromToFile(_data);
                                    return;
                                    }
                                EditorGUILayout.EndHorizontal();
                                }
                            EditorGUILayout.LabelField(item.Text, EditorStyles.wordWrappedLabel, GUILayout.MaxWidth(750));
                            EditorGUILayout.BeginHorizontal();
                                {
                                GUILayout.FlexibleSpace();
                                GUILayout.Label("Change Tag:", MyStyle);
                                item.Tag = EditorGUILayout.Popup(item.Tag, _options, GUILayout.MaxWidth(100));
                                EditorGUILayout.EndHorizontal();
                                }
                            EditorGUILayout.EndVertical();
                            }
                        }
                    }
                }
            GUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
            }
        GUILayout.FlexibleSpace();
        }    
private string SearchField(string searchStr, params GUILayoutOption[] options)
    {
    searchStr = GUILayout.TextField(searchStr, "ToolbarSeachTextField", options);
    if (GUILayout.Button("", "ToolbarSeachCancelButton"))
        {
        searchStr = "";
        GUI.FocusControl(null);
        }
    return searchStr;
    }

    private void SaveBrainStromToFile(UnfinishedGamesBrainstormData data)
        {
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();       
        }
    }
#endif

