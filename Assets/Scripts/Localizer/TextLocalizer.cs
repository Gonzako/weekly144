using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLocalizer : MonoBehaviour
{

    TextMeshProUGUI textField;

    public LocalizedString key;

    private void OnEnable()
    {
        textField = GetComponent<TextMeshProUGUI>();
        textField.text = key.value;
        LocalizationSystem.onLangChanged -= handleLangChange;
        LocalizationSystem.onLangChanged += handleLangChange;
    }

    private void OnDisable()
    {
        LocalizationSystem.onLangChanged -= handleLangChange;
    }

    private void handleLangChange(IReadOnlyDictionary<string, string> obj)
    {
        var value = key.key;
        obj.TryGetValue(key.key, out value);
        textField.text = value;
    }
}
