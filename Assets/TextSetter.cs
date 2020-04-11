using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class TextSetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        char letter = transform.parent.name[transform.parent.name.Length-1];
        GetComponent<TextMeshProUGUI>().text = letter.ToString();
    }

    

    // Update is called once per frame
    void Update()
    {
        char letter = transform.parent.name[transform.parent.name.Length - 1];
        GetComponent<TextMeshProUGUI>().text = letter.ToString();
    }
}
