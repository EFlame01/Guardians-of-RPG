using UnityEngine;
using UnityEngine.UI;

public class MainCameraCanvasGroup : MonoBehaviour
{
    [SerializeField] CanvasGroup MainCameraCG;

    public void Update()
    {
        MainCameraCG.blocksRaycasts = GameManager.Instance.EnableButtons;
    }
}