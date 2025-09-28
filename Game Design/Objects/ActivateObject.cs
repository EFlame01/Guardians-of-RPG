using System;
using UnityEngine;

/// <summary>
/// ActivateObject is a class that 
/// determines if an object should be active
/// or not based on certain criteria.
/// </summary>
public class ActivateObject : MonoBehaviour
{
    //Serialized variables
    [SerializeField] private CutSceneCriteria[] ActivateCriteria;
    [SerializeField] private CutSceneCriteria[] DeactivateCriteria;
    [SerializeField] private bool DetermineOnAwake = true;

    //public variable
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

    /// <summary>
    /// Determines if the gameObject should
    /// be active based on the ActivateCriteria.
    /// If the AcitvateCriteria is null or empty,
    /// it will automatically return true.
    /// </summary>
    /// <returns><c>TRUE</c> if all the critera exists in the flag dictionary and is marked accurately. Otherwise it will return <c>FALSE</c>.</returns>
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

    /// <summary>
    /// Determines if the gameObject should
    /// be active based on the DeactivateCriteria.
    /// If the DeacitvateCriteria is null or empty,
    /// it will automatically return false.
    /// </summary>
    /// <returns><c>TRUE</c> if all the critera exists in the flag dictionary and is NOT marked accurately. Otherwise it will return <c>FALSE</c>.</returns>
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

    /// <summary>
    /// Determines if the object should be deactive
    /// based on the criteria. If it should not be active,
    /// or it should be deactivated, it will set the
    /// gameobject active state to false.
    /// </summary>
    private void DetermineCriteria()
    {
        bool activate = DetermineActivateCriteria();
        bool deactivate = DetermineDeactivateCriteria();

        if (!activate || deactivate)
            gameObject.SetActive(false);
    }
}