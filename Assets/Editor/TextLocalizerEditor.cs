using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class TextLocalizerED : EditorWindow
{

    public static void Open(string key)
    {
        TextLocalizerED window = new TextLocalizerED();
        window.titleContent = new GUIContent("Localizer window");
        window.ShowUtility();
        window.key = key;
    }

    public string key;
    public string value;

    private void OnGUI()
    {
        key = EditorGUILayout.TextField("Key :", key);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Value", GUILayout.MaxWidth(50));
        EditorStyles.textArea.wordWrap = true;

        value = EditorGUILayout.TextArea(value, EditorStyles.textArea,
            GUILayout.Height(100), GUILayout.Width(400));
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Add"))
        {
            if(LocalizationSystem.getLocalizedValue(key) != string.Empty)
            {
                LocalizationSystem.Replace(key, value);
            }
            else
            {
                LocalizationSystem.Add(key, value);
            }
        }

        minSize = new Vector2(460, 250);
        maxSize = minSize;
    }
}

public class LocaliserSearchWindow : EditorWindow
{
    public static void Open(string value = null)
    {
        LocaliserSearchWindow window = new LocaliserSearchWindow();
        window.titleContent = new GUIContent("Localization Search");
        if(value != null)
            window.value = value;

        Vector2 mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        Rect rect = new Rect(mouse.x - 450, mouse.y, 10, 10);
        window.dictionary = LocalizationSystem.getEditorDic();
        window.ShowAsDropDown(rect, new Vector2(500, 300));

    }


    public string value;
    public Vector2 scroll;
    public IReadOnlyDictionary<string, string> dictionary;

    private void OnEnable()
    {
        
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal("Box");
        EditorGUILayout.LabelField("Search: ", EditorStyles.boldLabel);
        value = EditorGUILayout.TextField(value);
        EditorGUILayout.EndHorizontal();
        GetSearchResults();
    }

    private void GetSearchResults()
    {
        if(value == null)
        {
            return;
        }

        EditorGUILayout.BeginVertical();
        scroll = EditorGUILayout.BeginScrollView(scroll);
        foreach(KeyValuePair<string, string> n in dictionary)
        {
            if (n.Key.ToLower().Equals("key"))
                continue;
            if (n.Key.ToLower().Contains(value.ToLower()) || n.Value.ToLower().Contains(value.ToLower()))
            {
                EditorGUILayout.BeginHorizontal("Box");
                Texture closeIcon = (Texture)Resources.Load("closeIcon");

                GUIContent closeCont = new GUIContent(closeIcon);

                //Remove Button
                if(GUILayout.Button(closeCont, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                {
                    if (EditorUtility.DisplayDialog("Remove Key" + n.Key +"?", 
                        "This will remove the element from localization, are you sure?", "Do it"))
                    {
                        LocalizationSystem.Remove(n.Key);
                        AssetDatabase.Refresh();
                        LocalizationSystem.Init();
                        dictionary = LocalizationSystem.getEditorDic();
                    }
                }

                EditorGUILayout.TextField(n.Key);
                EditorGUILayout.LabelField(n.Value);
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

}