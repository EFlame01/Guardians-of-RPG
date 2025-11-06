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

    public Transform Cursor;
    public ButtonField[] ButtonFields;

    // private bool doneWithStart;
    // private Vector2 currentPos = new Vector2(0,0);

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

        foreach (ButtonField buttonField in ButtonFields)
        {
            if (sceneName.Contains(buttonField.name))
                Cursor.position = new Vector3(buttonField.button.position.x - 13f, buttonField.button.position.y + 50.6f, 0f);

        }
    }
}
