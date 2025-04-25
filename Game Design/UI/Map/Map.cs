using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using TMPro;
using TMPro.Examples;
using System;
using UnityEngine.SceneManagement;

public class Map : ButtonUI
{
    [SerializeField] Button mapButton;
    [SerializeField] Camera mapCamera;
    [SerializeField] CanvasGroup buttonGroup;
    [SerializeField] CanvasGroup informationWidget;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI countryText;
    [SerializeField] TextMeshProUGUI continentText;
    [SerializeField] TextMeshProUGUI descriptionText;

    private bool mapReset;
    private static string sceneName = "Tiro Town";


    public override void Start()
    {
        base.Start();
        ResetMapAtStart();
    }

    public static string GetSceneName()
    {
        return sceneName;
    }

    public void OnMapButtonPressed()
    {
        mapButton.interactable = false;
        sceneName = SceneManager.GetActiveScene().name;
        SceneLoader.walkInAnimation = false;
        SceneLoader.Instance.LoadScene("Map Scene", TransitionType.FADE_TO_BLACK);
    }

    public void OnLocationPressed(string id)
    {
        mapReset = false;
        LocationInformation locationInformation = MapDescMaker.Instance.GetLocationInformationTEST(id);

        nameText.text = locationInformation.Name;
        countryText.text = locationInformation.Territory.Equals("none") ? "" : locationInformation.Territory;
        continentText.text = locationInformation.Continent;
        descriptionText.text = locationInformation.Description;

        StartCoroutine(FadeCanvasGroup(informationWidget, 1f));
        StartCoroutine(FadeCanvasGroup(buttonGroup, 0f));
        StartCoroutine(ChangeCameraLocation(locationInformation));
    }

    public void OnBackButtonPressed()
    {
        if(!mapReset)
            ResetMap();
        else
            ExitMap();
    }

    private void ResetMapAtStart()
    {
        mapReset = true;
        if(mapCamera == null)
            return;
        if(buttonGroup == null)
            return;
        if(informationWidget == null)
            return;

        informationWidget.alpha = 0f;
        informationWidget.interactable = false;
        buttonGroup.alpha = 1f;
        buttonGroup.interactable = true;
        mapCamera.orthographicSize = 5;
        mapCamera.transform.position = new Vector3(0, 0, -10);
    }

    private void ResetMap()
    {
        mapReset = true;
        if(mapCamera == null)
            return;
        if(buttonGroup == null)
            return;
        if(informationWidget == null)
            return;
        StartCoroutine(FadeCanvasGroup(informationWidget, 0f));
        StartCoroutine(FadeCanvasGroup(buttonGroup, 1f));
        StartCoroutine(ChangeCameraLocation(null));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float value)
    {
        float currValue = value == 0f ? 1f : 0f;
        float increment = value == 0f ? 0.1f : -0.1f;

        cg.alpha = currValue;
        for(int i = 0; i < 10; i++)
        {
            cg.alpha = Mathf.Lerp(cg.alpha, value, (float)(i+1)/10f);
            yield return new WaitForSeconds(0.05f);
        }
        cg.alpha = value;

        cg.interactable = value == 1f;
    }

    private IEnumerator ChangeCameraLocation(LocationInformation locationInformation)
    {
        Vector3 coordinates = new Vector3(0, 0, -10);
        int size = 5;
        
        if(locationInformation != null)
        {
            coordinates = new Vector3(locationInformation.Coordinates.x, locationInformation.Coordinates.y, -10);
            size = 1;
        }

        for(int i = 0; i < 10; i++)
        {
            mapCamera.orthographicSize = Mathf.Lerp(mapCamera.orthographicSize, size, (float)(i+1)/10f);
            mapCamera.transform.position = Vector3.Lerp(mapCamera.transform.position, coordinates, (float)(i+1)/10f);
            yield return new WaitForSeconds(0.05f);
        }

        mapCamera.orthographicSize = size;
        mapCamera.transform.position = coordinates;
    }

    private void ExitMap()
    {
        GameManager.Instance.PlayerState = PlayerState.TRANSITION;
        SceneLoader.walkInAnimation = false;
        SceneLoader.Instance.LoadScene(sceneName, TransitionType.FADE_TO_BLACK);
    }

}