using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EffectMaker is a class that parses through
/// the data to create <c>Effect</c> objects for
/// the <c>Character</c> class.
/// </summary>
public class EffectMaker : Singleton<EffectMaker>
{
    private const int EFFECT_INDEX = 3;
    private const int HEALTH_EFFECT_INDEX = 4;
    private const int IMMUNITY_EFFECT_INDEX = 5;
    private const int RECHARGE_EFFECT_INDEX = 0; //TODO: Create document and thus index for recharge effect
    private const int RECOIL_EFFECT_INDEX = 13;
    private const int NEGATION_EFFECT_INDEX = 9;
    private const int STAT_CHANGE_EFFECT_INDEX = 14;
    private const int STATUS_CONDITION_EFFECT_INDEX = 17;
    private const int EFFECT_DATA_LENGTH = 6;

    /// <summary>
    /// Gets and returns the effects based on the <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name of the effect</param>
    /// <returns>an array of <c>Effect</c> objects or <c>null</c> if the effects could not be found.</returns>
    public Effect[] GetEffectsBasedOnName(string name)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
        {
            Debug.LogWarning($"WARNING: The effect [ {name} ] is either null or empty");
            return null;
        }

        List<Effect> listOfEffects = new();
        Effect effect;
        string[] mainAttributes;
        string[] additionalAttributes;

        string[] effects = DataRetriever.Instance.SplitDataBasedOnName(DataRetriever.Instance.Database[EFFECT_INDEX], name);

        foreach (string effectAttributes in effects)
        {
            effect = null;
            if (string.IsNullOrEmpty(effectAttributes) || effectAttributes.Split(',').Length != EFFECT_DATA_LENGTH)
            {
                Debug.LogWarning($"WARNING: Some error occured trying to retrieve the effect {name} from the database. Please report bug.");
                continue;
            }
            else
                mainAttributes = effectAttributes.Split(',');

            string id = mainAttributes[0];

            switch (mainAttributes[3])
            {
                case "ANNOUNCE":
                    effect = new AnnounceEffect
                    (
                        mainAttributes[0],
                        mainAttributes[1],
                        Effect.GetEffectOrigin(mainAttributes[2]),
                        Effect.GetEffectType(mainAttributes[3]),
                        Move.ConvertToMoveTarget(mainAttributes[4]),
                        double.TryParse(mainAttributes[5], out double announceEffectAccuracy) ? announceEffectAccuracy : 1.0
                    );
                    break;
                case "HEALTH_BOOST":
                    additionalAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[HEALTH_EFFECT_INDEX], id).Split(',');

                    effect = new HealthBoostEffect
                    (
                        mainAttributes[0],
                        mainAttributes[1],
                        Effect.GetEffectOrigin(mainAttributes[2]),
                        Effect.GetEffectType(mainAttributes[3]),
                        Move.ConvertToMoveTarget(mainAttributes[4]),
                        double.TryParse(mainAttributes[5], out double healthBoostEffectAccuracy) ? healthBoostEffectAccuracy : 1.0,
                        double.TryParse(additionalAttributes[2], out double healthBoostEffectAmount) ? healthBoostEffectAmount : 0.10
                    );
                    break;
                case "IMMUNITY":
                    additionalAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[IMMUNITY_EFFECT_INDEX], id).Split(',');

                    effect = new ImmunityEffect
                    (
                        mainAttributes[0],
                        mainAttributes[1],
                        Effect.GetEffectOrigin(mainAttributes[2]),
                        Effect.GetEffectType(mainAttributes[3]),
                        Move.ConvertToMoveTarget(mainAttributes[4]),
                        double.TryParse(mainAttributes[5], out double immunityEffectAccuracy) ? immunityEffectAccuracy : 1.0,
                        additionalAttributes[2]
                    );
                    break;
                case "NEGATION":
                    additionalAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[NEGATION_EFFECT_INDEX], id).Split(','); ;

                    effect = new NegationEffect
                    (
                        mainAttributes[0],
                        mainAttributes[1],
                        Effect.GetEffectOrigin(mainAttributes[2]),
                        Effect.GetEffectType(mainAttributes[3]),
                        Move.ConvertToMoveTarget(mainAttributes[4]),
                        double.TryParse(mainAttributes[5], out double negationEffectAccuracy) ? negationEffectAccuracy : 1.0,
                        additionalAttributes[2]
                    );
                    break;
                case "RECHARGE":
                    additionalAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[RECHARGE_EFFECT_INDEX], id).Split(',');

                    effect = new RechargeEffect
                    (
                        mainAttributes[0],
                        mainAttributes[1],
                        Effect.GetEffectOrigin(mainAttributes[2]),
                        Effect.GetEffectType(mainAttributes[3]),
                        Move.ConvertToMoveTarget(mainAttributes[4]),
                        double.TryParse(mainAttributes[5], out double rechargingEffectAccuracy) ? rechargingEffectAccuracy : 1.0,
                        int.TryParse(additionalAttributes[2], out int rechargeTurns) ? rechargeTurns : 1
                    );
                    break;
                case "RECOIL":
                    additionalAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[RECOIL_EFFECT_INDEX], id).Split(',');

                    effect = new RecoilEffect
                    (
                        mainAttributes[0],
                        mainAttributes[1],
                        Effect.GetEffectOrigin(mainAttributes[2]),
                        Effect.GetEffectType(mainAttributes[3]),
                        Move.ConvertToMoveTarget(mainAttributes[4]),
                        double.TryParse(mainAttributes[5], out double recoilEffectAccuracy) ? recoilEffectAccuracy : 1.0,
                        double.TryParse(additionalAttributes[2], out double recoilDamagePercentage) ? recoilDamagePercentage : 0.10
                    );
                    break;
                case "STAB_BOOST":
                    effect = new StabBoostEffect
                    (
                        mainAttributes[0],
                        mainAttributes[1],
                        Effect.GetEffectOrigin(mainAttributes[2]),
                        Effect.GetEffectType(mainAttributes[3]),
                        Move.ConvertToMoveTarget(mainAttributes[4]),
                        double.TryParse(mainAttributes[5], out double stabBoostEffectAccuracy) ? stabBoostEffectAccuracy : 1.0
                    );
                    break;
                case "STAT_CHANGE":
                    additionalAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[STAT_CHANGE_EFFECT_INDEX], id).Split(',');

                    effect = new StatChangeEffect
                    (
                        mainAttributes[0],
                        mainAttributes[1],
                        Effect.GetEffectOrigin(mainAttributes[2]),
                        Effect.GetEffectType(mainAttributes[3]),
                        Move.ConvertToMoveTarget(mainAttributes[4]),
                        double.TryParse(mainAttributes[5], out double statChangeEffectAccuracy) ? statChangeEffectAccuracy : 1.0,
                        additionalAttributes[2].Split('~'),
                        Array.ConvertAll(additionalAttributes[3].Split('~'), int.Parse)
                    );
                    break;
                case "STATUS_CONDITION":
                    additionalAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[STATUS_CONDITION_EFFECT_INDEX], id).Split(',');

                    effect = new StatusConditionEffect
                    (
                        mainAttributes[0],
                        mainAttributes[1],
                        Effect.GetEffectOrigin(mainAttributes[2]),
                        Effect.GetEffectType(mainAttributes[3]),
                        Move.ConvertToMoveTarget(mainAttributes[4]),
                        double.TryParse(mainAttributes[5], out double statusConditionEffectAccuracy) ? statusConditionEffectAccuracy : 1.0,
                        StatusCondition.GenerateStatusCondition
                        (
                            additionalAttributes[2],
                            int.TryParse(additionalAttributes[2], out int charmDuration) ? charmDuration : 0,
                            int.TryParse(additionalAttributes[3], out int confuseDuration) ? confuseDuration : 0,
                            int.TryParse(additionalAttributes[4], out int rollAdvantage) ? rollAdvantage : 0,
                            int.TryParse(additionalAttributes[5], out int freezeRate) ? freezeRate : 0,
                            int.TryParse(additionalAttributes[6], out int poisonRate) ? poisonRate : 0,
                            int.TryParse(additionalAttributes[7], out int restrainRate) ? restrainRate : 0,
                            int.TryParse(additionalAttributes[8], out int sleepRate) ? sleepRate : 0,
                            int.TryParse(additionalAttributes[9], out int stunRate) ? stunRate : 0
                        )
                    );
                    break;
            }

            if (effect != null)
                listOfEffects.Add(effect);
        }

        return listOfEffects.ToArray();
    }
}