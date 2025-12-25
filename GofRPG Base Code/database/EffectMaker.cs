using System;
using System.Collections.Generic;

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

    /// <summary>
    /// Gets and returns the effects based on the <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name of the effect</param>
    /// <returns>an array of <c>Effect</c> objects or <c>null</c> if the effects could not be found.</returns>
    public Effect[] GetEffectsBasedOnName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        List<Effect> listOfEffects = new();
        Effect effect;
        string[] mainAttributes;
        string[] additionalAttributes;

        string[] effects = DataRetriever.Instance.SplitDataBasedOnID(DataRetriever.Instance.Database[EFFECT_INDEX], name);

        foreach (string effectAttributes in effects)
        {
            effect = null;
            mainAttributes = effectAttributes.Split(',');

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
                        double.Parse(mainAttributes[5])
                    );
                    break;
                case "HEALTH_BOOST":
                    additionalAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[HEALTH_EFFECT_INDEX], name).Split(',');

                    effect = new HealthBoostEffect
                    (
                        mainAttributes[0],
                        mainAttributes[1],
                        Effect.GetEffectOrigin(mainAttributes[2]),
                        Effect.GetEffectType(mainAttributes[3]),
                        Move.ConvertToMoveTarget(mainAttributes[4]),
                        double.Parse(mainAttributes[5]),
                        double.Parse(additionalAttributes[2])
                    );
                    break;
                case "IMMUNITY":
                    additionalAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[IMMUNITY_EFFECT_INDEX], name).Split(',');

                    effect = new ImmunityEffect
                    (
                        mainAttributes[0],
                        mainAttributes[1],
                        Effect.GetEffectOrigin(mainAttributes[2]),
                        Effect.GetEffectType(mainAttributes[3]),
                        Move.ConvertToMoveTarget(mainAttributes[4]),
                        double.Parse(mainAttributes[5]),
                        additionalAttributes[2]
                    );
                    break;
                case "NEGATION":
                    additionalAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[NEGATION_EFFECT_INDEX], name).Split(','); ;

                    effect = new NegationEffect
                    (
                        mainAttributes[0],
                        mainAttributes[1],
                        Effect.GetEffectOrigin(mainAttributes[2]),
                        Effect.GetEffectType(mainAttributes[3]),
                        Move.ConvertToMoveTarget(mainAttributes[4]),
                        double.Parse(mainAttributes[5]),
                        additionalAttributes[2]
                    );
                    break;
                case "RECHARGE":
                    additionalAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[RECHARGE_EFFECT_INDEX], name).Split(',');

                    effect = new RechargeEffect
                    (
                        mainAttributes[0],
                        mainAttributes[1],
                        Effect.GetEffectOrigin(mainAttributes[2]),
                        Effect.GetEffectType(mainAttributes[3]),
                        Move.ConvertToMoveTarget(mainAttributes[4]),
                        double.Parse(mainAttributes[5]),
                        int.Parse(additionalAttributes[2])
                    );
                    break;
                case "RECOIL":
                    additionalAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[RECOIL_EFFECT_INDEX], name).Split(',');

                    effect = new RecoilEffect
                    (
                        mainAttributes[0],
                        mainAttributes[1],
                        Effect.GetEffectOrigin(mainAttributes[2]),
                        Effect.GetEffectType(mainAttributes[3]),
                        Move.ConvertToMoveTarget(mainAttributes[4]),
                        double.Parse(mainAttributes[5]),
                        double.Parse(additionalAttributes[2])
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
                        double.Parse(mainAttributes[5])
                    );
                    break;
                case "STAT_CHANGE":
                    additionalAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[STAT_CHANGE_EFFECT_INDEX], name).Split(',');

                    effect = new StatChangeEffect
                    (
                        mainAttributes[0],
                        mainAttributes[1],
                        Effect.GetEffectOrigin(mainAttributes[2]),
                        Effect.GetEffectType(mainAttributes[3]),
                        Move.ConvertToMoveTarget(mainAttributes[4]),
                        double.Parse(mainAttributes[5]),
                        additionalAttributes[2].Split('~'),
                        Array.ConvertAll(additionalAttributes[3].Split('~'), int.Parse)
                    );
                    break;
                case "STATUS_CONDITION":
                    additionalAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[STATUS_CONDITION_EFFECT_INDEX], name).Split(',');

                    effect = new StatusConditionEffect
                    (
                        mainAttributes[0],
                        mainAttributes[1],
                        Effect.GetEffectOrigin(mainAttributes[2]),
                        Effect.GetEffectType(mainAttributes[3]),
                        Move.ConvertToMoveTarget(mainAttributes[4]),
                        double.Parse(mainAttributes[5]),
                        StatusCondition.GenerateStatusCondition
                        (
                            additionalAttributes[2],
                            int.Parse(additionalAttributes[3]),
                            int.Parse(additionalAttributes[4]),
                            int.Parse(additionalAttributes[5]),
                            int.Parse(additionalAttributes[6]),
                            int.Parse(additionalAttributes[7]),
                            int.Parse(additionalAttributes[8]),
                            int.Parse(additionalAttributes[9]),
                            int.Parse(additionalAttributes[10])
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