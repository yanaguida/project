using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class EnterText : MonoBehaviour
{

    public TMPro.TMP_InputField inputField;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // get enable Components
        inputField = GameObject.Find("Input1").GetComponent<TMPro.TMP_InputField>();
    }

    void Start()
    {
        if(inputField == null)
        {
            Debug.Log("//// inputField : NULL ////\nPLEASE FIX IT");
        }
    }
}
