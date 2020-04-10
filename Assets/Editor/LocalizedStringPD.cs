using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LocalizedString))]
public class LocalizedStringPD : PropertyDrawer
{

    bool dropdown;
    float height;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (dropdown)
            return height + 25;
        return 20;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position,
            GUIUtility.GetControlID(FocusType.Passive), label);
        position.width -= 34f;//???
        position.height = 18;

        Rect valRect = new Rect(position);
        valRect.x += 15;
        valRect.width -= 15;

        Rect foldButtonRect = new Rect(position);
        //foldButtonRect.x += 10;
        foldButtonRect.width = 25;

        dropdown = EditorGUI.Foldout(foldButtonRect, dropdown, "");

        position.x += 15;
        position.width -= 15;

        SerializedProperty key = property.FindPropertyRelative("key");
        key.stringValue = EditorGUI.TextField(position, key.stringValue);

        position.x += position.width + 2;
        position.width = 17;
        position.height = 17;

        Texture searchIcon = (Texture)Resources.Load("searchIcon");
        GUIContent searchCont = new GUIContent(searchIcon);

        if (GUI.Button(position, searchCont))
        {

            LocaliserSearchWindow.Open(key.stringValue);

        }

        position.x += position.width + 2;

        Texture storeIcon = (Texture)Resources.Load("storeIcon");
        GUIContent storeCont = new GUIContent(storeIcon);


        if (GUI.Button(position, storeCont))
        {
            TextLocalizerED.Open(key.stringValue);
        }

        if (dropdown)
        {
            var value = LocalizationSystem.getLocalizedValue(key.stringValue);
            GUIStyle style = GUI.skin.box;
            height = style.CalcHeight(new GUIContent(value), valRect.width);

            valRect.height = height;
            valRect.y += 21;
            EditorGUI.LabelField(valRect, value, EditorStyles.wordWrappedLabel);
        }

        EditorGUI.EndProperty();
    }
}
