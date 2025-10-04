using UnityEngine;

using System.Collections.Generic;
using Sirenix.OdinInspector;

public class WorldWaypoint : WorldElement
{

    [Header("Elements")]
    [SerializeField] private WorldRoad closestRoad; public WorldRoad ClosestRoad => closestRoad;
    [SerializeField] private WorldCharacter[] characterArray;

    [Header("Stats")]
    [SerializeField] private float size;

    [Header("Storage")]
    [SerializeField] private float corruptionLevel; // How much the demon has taken here

    [Button]
    private void GetAllChara()
    {
        characterArray = GetComponentsInChildren<WorldCharacter>();

        int length = characterArray.Length;
        for (int i = 0; i < length; i++)
        {
            characterArray[i].Waypoint = this;
            if(world != null) characterArray[i].World = world;
        }
    }

    #region Quest

    public WorldCharacter GetCharaForQuest()
    {
        List<WorldCharacter> charaList = new();
        charaList.AddRange(characterArray);
        CoreUtility.Shuffle(charaList);

        int length = charaList.Count;
        for (int i = 0; i < length; i++)
        {
            if(charaList[i] && charaList[i].CanBeQuestGoal)
            {
                return charaList[i];
            }
        }

        return null;
    }

    #endregion

    // Editor
    private void OnDrawGizmos()
    {
        CoreUtility.DrawCircle(Color.green, transform.position + Vector3.up * 0.1f, size);

        if(ClosestRoad)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, ClosestRoad.transform.position);
        }
    }
}
