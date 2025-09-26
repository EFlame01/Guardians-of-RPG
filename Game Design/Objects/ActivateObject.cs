using System;
using UnityEngine;

public class ActivateObject : MonoBehaviour
{
    public CutSceneCriteria[] ActivateCriteria;
    public CutSceneCriteria[] DeactivateCriteria;
    public bool DetermineOnAwake = true;

    [Serializable]
    public struct CutSceneCriteria
    {
        public string storyFlagID;
        public bool storyFlagValue;
    }

    public void Awake()
    {
        if (DetermineOnAwake)
            DetermineCriteria();
    }

    public void DetermineCriteria()
    {
        bool activate = DetermineActivateCriteria();
        bool deactivate = DetermineDeactivateCriteria();

        if (!activate || deactivate)
            gameObject.SetActive(false);
    }

    public bool DetermineActivateCriteria()
    {
        if (ActivateCriteria == null || ActivateCriteria.Length <= 0)
            return true;
        try
        {
            foreach (CutSceneCriteria criteria in ActivateCriteria)
            {
                bool criteriaExists = StoryFlagManager.FlagDictionary.ContainsKey(criteria.storyFlagID);
                bool criteriaTheSame = false;

                if (criteriaExists)
                    criteriaTheSame = StoryFlagManager.FlagDictionary[criteria.storyFlagID].Value == criteria.storyFlagValue;

                if (!criteriaExists || !criteriaTheSame)
                    return false;

                Debug.Log(criteria.storyFlagID + ": " + StoryFlagManager.FlagDictionary[criteria.storyFlagID].Value);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.ToString());
        }

        return true;
    }

    public bool DetermineDeactivateCriteria()
    {
        if (DeactivateCriteria == null || DeactivateCriteria.Length <= 0)
            return false;
        try
        {
            bool deactivate = true;

            foreach (CutSceneCriteria criteria in DeactivateCriteria)
            {
                bool criteriaExists = StoryFlagManager.FlagDictionary.ContainsKey(criteria.storyFlagID);
                bool criteriaTheSame = false;

                if (criteriaExists)
                    criteriaTheSame = StoryFlagManager.FlagDictionary[criteria.storyFlagID].Value == criteria.storyFlagValue;

                if (!criteriaExists || !criteriaTheSame)
                    deactivate = false;

                Debug.Log(criteria.storyFlagID + ": " + StoryFlagManager.FlagDictionary[criteria.storyFlagID].Value);
            }

            return deactivate;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.ToString());
        }

        return false;
    }
}