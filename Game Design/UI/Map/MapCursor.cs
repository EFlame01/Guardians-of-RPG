using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using TMPro;
using TMPro.Examples;
using System;
using UnityEngine.SceneManagement;

public class MapCursor : MonoBehaviour
{
    // [SerializeField] Button EquitumCity;
    // [SerializeField] Button NoMansLand;
    // [SerializeField] Button GormaniaA;

    [SerializeField] public Transform Cursor;
    [SerializeField] public ButtonField[] ButtonFields;

    [Serializable]
    public struct ButtonField
    {
        public Transform button;
        public string name;
    }

    // Start is called before the first frame update
    void Start()
    {
        string sceneName = MapLocation.GetCurrentMapLocation();

        foreach(ButtonField buttonField in ButtonFields)
        {
            if(sceneName.Contains(buttonField.name))
                Cursor.position = new Vector3(buttonField.button.position.x - 10f, buttonField.button.position.y + 50f, 0f);
            
        }
    }
}
