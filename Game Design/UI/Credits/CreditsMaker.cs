using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsMaker : MonoBehaviour
{
    public GameObject logo;
    public GameObject playAgainButton;
    public float ScrollSpeed;
    public Transform CreditsLayout;
    public TitleCredit TitleCreditPrefab;
    public RoleCredit RoleCreditPrefab;
    public CreditInformation[] Credits;

    private bool stopScroll;

    // Start is called before the first frame update
    void Start()
    {
        GenerateCredits();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: This is the only way I could get the layout to work.
        //      Need to fix in the future.
        CreditsLayout.GetComponent<VerticalLayoutGroup>().spacing = 10f;
        CreditsLayout.GetComponent<VerticalLayoutGroup>().spacing = 0f;
        //-----------------------------------------------------------

        if (stopScroll)
            return;

        if (!StopScroll())
            CreditsLayout.position += new Vector3(0, ScrollSpeed * Time.fixedDeltaTime, 0);
        else
        {
            stopScroll = true;
            StartCoroutine(FinishCreditsAnimation());
        }
    }

    IEnumerator FinishCreditsAnimation()
    {
        logo.SetActive(true);
        logo.GetComponent<Animator>().Play("fade_to_ui");
        yield return new WaitForSeconds(3f);
        playAgainButton.SetActive(true);
        playAgainButton.GetComponent<Animator>().Play("fade_to_ui");
    }

    public void OnPlayAgainButtonPressed()
    {
        GameManager.Instance.SaveGameData();
        SceneLoader.Instance.LoadScene("Start Scene", TransitionType.FADE_TO_BLACK);
    }

    void GenerateCredits()
    {
        foreach (CreditInformation creditInfo in Credits)
        {
            Credit credit;
            if (creditInfo.Type.Equals(CreditType.ROLE))
                credit = Instantiate(RoleCreditPrefab, CreditsLayout);
            else
                credit = Instantiate(TitleCreditPrefab, CreditsLayout);

            credit.UpdateText(creditInfo);
        }
    }

    bool StopScroll()
    {
        //TODO: It stops at a preset number. Will have to make more dynamic in the future
        return CreditsLayout.GetComponent<RectTransform>().anchoredPosition.y >= 11700f;
    }
}
