using UnityEngine;
using System;

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
                Cursor.localPosition = new Vector3(buttonField.button.localPosition.x - 10f, buttonField.button.localPosition.y + 50f, 0f);

        }
    }
}
