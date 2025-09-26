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

    public Transform Cursor;
    public ButtonField[] ButtonFields;
    public float x_offset = 1f;
    public float y_offset = 15f;

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
                Cursor.position = new Vector3(buttonField.button.position.x - x_offset, buttonField.button.position.y + y_offset, 0f);

        }

        // currentPos = new Vector2(Cursor.position.x, Cursor.position.y);
        // doneWithStart = true;
    }

    public void Update()
    {
        // if(doneWithStart)
        //     Cursor.position = new Vector3(currentPos.x - x_offset, currentPos.y + y_offset, 0f);
    }
}
