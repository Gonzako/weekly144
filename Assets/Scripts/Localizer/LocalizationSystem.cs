using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationSystem
{

    
    public enum Languages
    {
        English, French, Spanish
    }

    public static Languages currentLang { get => lang;
        set { lang = value; onLangChange();   } }

    public static Action<IReadOnlyDictionary<string, string>> onLangChanged;

    private static Languages lang = Languages.English;
    private static Dictionary<string, string> localEn;
    private static Dictionary<string, string> localFr;
    private static Dictionary<string, string> localSpa;

    public static bool isInit = false;
    public static LocalCSVLoader cSVLoader;

    private static void onLangChange()
    {
        switch (currentLang)
        {
            case Languages.English:
                onLangChanged?.Invoke(localEn);
                break;
            case Languages.French:
                onLangChanged?.Invoke(localFr);
                break;
            case Languages.Spanish:
                onLangChanged?.Invoke(localSpa);
                break;
        }
    }
    public static void Init()
    {
        cSVLoader = new LocalCSVLoader();

        cSVLoader.loadCSV();

        localEn = cSVLoader.getLanguageDic("en");
        localFr = cSVLoader.getLanguageDic("fr");
        localSpa = cSVLoader.getLanguageDic("spa");

        isInit = true;
    }


    public static string getLocalizedValue(string key)
    {
        if (!isInit)
            Init();

        string val = key;

        switch (currentLang)
        {
            case Languages.English:
                localEn.TryGetValue(key, out val);
                break;
            case Languages.French:
                localFr.TryGetValue(key, out val);
                break;
            case Languages.Spanish:
                localSpa.TryGetValue(key, out val);
                break;
        }

        return val;
    }

    public static void UpdateDictionaries()
    {

        localEn = cSVLoader.getLanguageDic("en");
        localFr = cSVLoader.getLanguageDic("fr");
        localSpa = cSVLoader.getLanguageDic("spa");

    }

    public static void Add(string key, string value)
    {
        if (value.Contains("\""))
            value.Replace('"', '\"');

        if (cSVLoader == null)
            cSVLoader = new LocalCSVLoader();

        cSVLoader.loadCSV();
        cSVLoader.Add(key, value);
        cSVLoader.loadCSV();

        UpdateDictionaries();

    }
    
    public static void Replace(string key, string value)
    {
        if (value.Contains("\""))
            value.Replace('"', '\"');

        if (cSVLoader == null)
            cSVLoader = new LocalCSVLoader();

        cSVLoader.loadCSV();
        cSVLoader.Edit(key, value);
        cSVLoader.loadCSV();

        UpdateDictionaries();

    }
  
    public static IReadOnlyDictionary<string, string> getEditorDic()
    {
        if (!isInit)
            Init();
        return localEn;
    }

    public static void Remove(string key)
    {

        if (cSVLoader == null)
            cSVLoader = new LocalCSVLoader();

        cSVLoader.loadCSV();
        cSVLoader.Remove(key);
        cSVLoader.loadCSV();

        UpdateDictionaries();

    }
}