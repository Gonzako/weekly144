using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;


public class LocalCSVLoader 
{
    private const string localizerFilePath = "Assets/Resources/Localization.csv";
    private const string localizerFileName = "Localization";

    private TextAsset csvFile;
    private char lineSeparator = '\n';
    private char surround = '"';
    private string[] fieldSeparator = { "\",\"" };

    public void loadCSV()
    {
        csvFile = Resources.Load<TextAsset>(localizerFileName);
    }

    public Dictionary<string, string> getLanguageDic(string langId)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();

        string[] lines = csvFile.text.Split(lineSeparator);

        int attributeIndx = -1;

        string[] headers = lines[0].Split(fieldSeparator, System.StringSplitOptions.None);

        for (int i = 0; i < headers.Length; i++)
        {
            if(headers[i] == langId)
            {
                attributeIndx = i;
                break;
            }
        }

        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            string[] fields = CSVParser.Split(line);

            for (int j = 0; j < fields.Length; j++)
            {
                fields[j] = fields[j].TrimStart(' ', surround);
                fields[j] = fields[j].TrimEnd(surround);
            }

            if(fields.Length > attributeIndx && attributeIndx >= 0)
            {
                string key = fields[0];

                if (dic.ContainsKey(key))
                {
                    continue;
                }

                var value = fields[attributeIndx];

                dic.Add(key, value);
            }
        }

        return dic;
    }

#if UNITY_EDITOR
    public void Add(string key, string value)
    {
        string appended = string.Format("\n\"{0}\",\"{1}\"", key, value);
        File.AppendAllText(localizerFilePath, appended);

        UnityEditor.AssetDatabase.Refresh();
    }

    public void Remove(string key)
    {
        var lines = csvFile.text.Split(lineSeparator);
        var keys = new string[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            keys[i] = line.Split(fieldSeparator, StringSplitOptions.None)[0];
        }

        int index = -1;

        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].Contains(key))
            {
                index = i;
                break;
            }
        }

        if (index > 1)
        {
            string[] newLines;
            newLines = lines.Where(w => w != lines[index]).ToArray();

            string replaced = string.Join(lineSeparator.ToString(), newLines);
            File.WriteAllText(localizerFilePath, replaced);
        }
    }

    public void Edit(string key, string value)
    {
        Remove(key);
        Add(key, value);
    } 
#endif

}
