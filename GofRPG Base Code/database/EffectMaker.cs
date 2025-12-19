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
    private readonly string _effectPath = "/database/effects.csv";
    private readonly string _healthBoostEffectPath = "/database/health_boost_effects.csv";
    private readonly string _immunityEffectPath = "/database/immunity_effects.csv";
    private readonly string _rechargeEffectPath = "/database/recharge_effects.csv";
    private readonly string _recoilEffectPath = "/database/recoil_effects.csv";
    private readonly string _negationEffectPath = "/database/negation_effects.csv";
    private readonly string _statChangeEffectPath = "/database/stat_change_effects.csv";
    private readonly string _statusConditionEffectPath = "/database/status_condition_effects.csv";

    /// <summary>
    /// Gets and returns the effects based on the <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name of the effect</param>
    /// <returns>an array of <c>Effect</c> objects or <c>null</c> if the effects could not be found.</returns>
    public Effect[] GetEffectsBasedOnName(string name)
    {
        if (name == null)
            return null;

        List<Effect> listOfEffects = new List<Effect>();
        Effect effect;
        string[] foundEffects;
        string[] mainAttributes;
        string[] additionalAttributes;

        // DataEncoder.Instance.DecodePersistentDataFile(_effectPath);
        DataEncoder.Instance.GetStreamingAssetsFile(_effectPath);
        foundEffects = DataEncoder.Instance.GetRowsOfData(name);
        DataEncoder.ClearData();

        foreach (string foundEffect in foundEffects)
        {
            effect = null;
            mainAttributes = foundEffect.Split(',');

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
                    // DataEncoder.Instance.DecodePersistentDataFile(_healthBoostEffectPath);
                    DataEncoder.Instance.GetStreamingAssetsFile(_healthBoostEffectPath);
                    additionalAttributes = DataEncoder.Instance.GetRowOfData(name).Split(',');

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
                    // DataEncoder.Instance.DecodePersistentDataFile(_immunityEffectPath);
                    DataEncoder.Instance.GetStreamingAssetsFile(_immunityEffectPath);
                    additionalAttributes = DataEncoder.Instance.GetRowOfData(name).Split(',');

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
                    // DataEncoder.Instance.DecodePersistentDataFile(_negationEffectPath);
                    DataEncoder.Instance.GetStreamingAssetsFile(_negationEffectPath);
                    additionalAttributes = DataEncoder.Instance.GetRowOfData(name).Split(','); ;

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
                    // DataEncoder.Instance.DecodePersistentDataFile(_rechargeEffectPath);
                    DataEncoder.Instance.GetStreamingAssetsFile(_rechargeEffectPath);
                    additionalAttributes = DataEncoder.Instance.GetRowOfData(name).Split(',');

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
                    // DataEncoder.Instance.DecodePersistentDataFile(_recoilEffectPath);
                    DataEncoder.Instance.GetStreamingAssetsFile(_recoilEffectPath);
                    additionalAttributes = DataEncoder.Instance.GetRowOfData(name).Split(',');

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
                    // DataEncoder.Instance.DecodePersistentDataFile(_statChangeEffectPath);
                    DataEncoder.Instance.GetStreamingAssetsFile(_statChangeEffectPath);
                    additionalAttributes = DataEncoder.Instance.GetRowOfData(name).Split(',');

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
                    // DataEncoder.Instance.DecodePersistentDataFile(_statusConditionEffectPath);
                    DataEncoder.Instance.GetStreamingAssetsFile(_statusConditionEffectPath);
                    additionalAttributes = DataEncoder.Instance.GetRowOfData(name).Split(',');

                    effect = new StatusConditionEffect
                    (
                        foundEffect.Split(',')[0],
                        foundEffect.Split(',')[1],
                        Effect.GetEffectOrigin(foundEffect.Split(',')[2]),
                        Effect.GetEffectType(foundEffect.Split(',')[3]),
                        Move.ConvertToMoveTarget(foundEffect.Split(',')[4]),
                        double.Parse(foundEffect.Split(',')[5]),
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