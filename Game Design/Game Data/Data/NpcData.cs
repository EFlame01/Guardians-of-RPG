using System;
using UnityEngine;

///<summary>
/// NpcData is the class that
/// compresses the meta data
/// for <c>NPCObject</c> based
/// on the id, position, if the
/// npc fought the player, and
/// what direction they're facing.
/// </summary>
[Serializable]
public class NpcData
{
    //public variables
    public string ID;
    public Vector3 Position;
    public bool foughtPlayer;
    public bool wonAgainstPlayer;
    public string direction;

    //flag and flag values are conditions that need to be met for NPC to stay active
    public string[] Flags;
    public bool[] FlagValues;

    //Constructor
    public NpcData(string id, Vector3 position, string[] flags, bool[] flagValues)
    {
        ID = id;
        Position = position;
        Flags = flags;
        FlagValues = flagValues;

        NpcDataContainer.NpcDataList.Add(this);
    }
}