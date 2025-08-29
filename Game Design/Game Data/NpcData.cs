using System;
using UnityEngine;

[Serializable]
public class NpcData
{
    public string ID;
    public Vector3 Position;

    public bool foughtPlayer;

    //flag and flag values are conditions that need to be met for NPC to stay active
    public string[] Flags;
    public bool[] FlagValues;

    public NpcData(string id, Vector3 position, string[] flags, bool[] flagValues)
    {
        ID = id;
        Position = position;
        Flags = flags;
        FlagValues = flagValues;

        NpcDataContainer.NpcDataList.Add(this);
    }
}