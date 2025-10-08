using UnityEngine;

using System.Collections.Generic;
using Sirenix.OdinInspector;

public class WorldWaypoint : WorldElement
{
    [SerializeField] private int waypointID; public int WaypointId { get => waypointID; set => waypointID = value; }

    [Header("Elements")]
    [SerializeField] private WorldRoad closestRoad; public WorldRoad ClosestRoad => closestRoad;
    [SerializeField] private WorldCharacter[] characterArray;

    [Header("Evolution")]
    [SerializeField] private GameObject activeOnlyFirstLayer;
    [SerializeField] private GameObject[] activiateForEachLevel;

    [Header("Stats")]
    [SerializeField] private float size;

    [Header("Storage")]
    [SerializeField] private int corruptionLevel; // How much the demon has taken here

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

    private void OnEnable()
    {
        RefreshVisual(0);
    }

    public void RefreshVisual(int state)
    {
        corruptionLevel = state;

        if (activeOnlyFirstLayer)
        {
            activeOnlyFirstLayer.SetActive(state == 0);
        }

        int length = activiateForEachLevel.Length;

        // If State == 1 -> 1 kill -> Display layer 0 == i
        for (int i = 0; i < length; i++) // First element of array is for first kill
        {
            if (activiateForEachLevel[i] == null) continue;

            activiateForEachLevel[i].SetActive(i < state);
        }
    }

    [Button, ButtonGroup("Corruption")]
    public void RiseCorruption()
    {
        RefreshVisual(corruptionLevel + 1);
    }

    [Button, ButtonGroup("Corruption")]
    public void LowerCorruption() // Only for debug
    {
        RefreshVisual(corruptionLevel - 1);
    }

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
