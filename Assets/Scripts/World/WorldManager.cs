using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private GameManager game; public GameManager Game => game;
    [SerializeField] private float currentTime;
#if UNITY_EDITOR
    [SerializeField] private bool accelerateTimeForDebug;
#endif
    [Header("Main Elements")]
    [SerializeField] private Player player; public Player Player => player;
    [SerializeField] private DemonDen demonDen;

    [Header("Quest")]
    [SerializeField] private float questTimeDistanceRatio = 0.5f; // The lower the shorter the time to do the quest
    [SerializeField] private int questCount; public int QuestCount => questCount;
    [SerializeField] private int errorCount; public int ErrorCount => errorCount;

    public const int QUEST_TO_WIN = 6;
    [SerializeField] private QuestData currentQuest;

    [Header("Content")]
    [SerializeField] private WorldWaypoint[] waypointArray;
    [SerializeField] private WorldRoad[] roadArray;


    [Header("Storage")]
    //[SerializeField, ReadOnly()] private int currentVillageID = -1;
    [SerializeField, ReadOnly()] private int lastVillageID = -1;
    [SerializeField, ReadOnly()] private bool shuffledWaypoints;
    [SerializeField] private WorldWaypoint[] shuffledWaypointarray;

    private void Start()
    {

    }

    private void Update()
    {
        if(QuestStarted() && game && game.CanvasManager.Timer) // Current quest in progress
        {
            if (!player.CurrentlyInDialog) // If not in dialog
            {

#if UNITY_EDITOR
                if (accelerateTimeForDebug)
                {
                    currentQuest.timeLeft -= Time.deltaTime * 10;
                }
                else
#endif
                {
                    currentQuest.timeLeft -= Time.deltaTime;
                }
            }

            game.CanvasManager.Timer.SetTimer(currentQuest.timeLeft, currentQuest.timeToDoTheTask);

            if(currentQuest.timeLeft < 0f)
            {
                errorCount++;

                if(errorCount > 1)
                {
                    LooseGame(); // No more time
                }
                else // Not lost yet -> Display error dialog
                {
                    player.Teleport(Vector3.up * 0.25f);
                    demonDen.EndQuestBecauseFailed();
                }
            }
        }
    }

    public void WinGame() // VICTORY !!!
    {
        Debug.Log("YOU WIN THE GAME!!!");

        game.CanvasManager.endScreen.ShowWinScreen();
    }

    public void LooseGame() // Loose
    {
        Debug.Log($"You Lost the game - errorCount: {errorCount}");

        game.CanvasManager.endScreen.ShowLooseScreen();
        ClearQuest(validated: false);
    }

    public bool QuestStarted()
    {
        return currentQuest.goalChara != null;
    }

    public void ClearQuest(bool validated) 
    {
        if(QuestStarted() && validated)
        {
            questCount++;
        }

        currentQuest = new();

        if ( game && game.CanvasManager.Timer) // Current quest in progress
        {
            game.CanvasManager.Timer.ClearTimer();
        }
    }

    public bool IsQuestGoal(WorldCharacter chara)
    {
        return chara != null && currentQuest.goalChara == chara;
    }

    public void StartDialog(WorldCharacter chara, DialogData dialogData)
    {
        if (Game && Game.CanvasManager)
        {
            Game.CanvasManager.dialogManager.StartDialog(chara, dialogData);
        }

        if (player)
        {
            player.SetInDialog(chara);
        }
    }

    public void DialogEnded()
    {
        if (player)
        {
            player.SetInDialog(null);
        }
    }

    #region Quest

    public void GetNextQuest(ref QuestData quest)
    {
        quest.goalChara = GetCharaForQuest();

        if (!quest.goalChara)
        {
            Debug.LogError("Did not found ANY character for quest!");
            // -> Game success ? If all goal done
            return;
        }

        quest.goalWaypoint = quest.goalChara.Waypoint;

        if(quest.goalWaypoint)
        {
            quest.goalRoad = quest.goalWaypoint.ClosestRoad; // Found the logic road to go to this location
        }
        else
        {
            // Character is not in a village
        }

        float distance = Vector3.Distance( quest.goalChara.transform.position, player.transform.position);
        quest.timeToDoTheTask = distance * questTimeDistanceRatio;
        quest.timeLeft = quest.timeToDoTheTask;


        // Display quest data
        if (Game && Game.CanvasManager && Game.CanvasManager.uiQuest)
        {
            Game.CanvasManager.uiQuest.SetQuestText(quest);
        }

        currentQuest = quest;
    }

    private WorldCharacter GetCharaForQuest()
    {
        // At start get a random village id then always a different one
        //if(!shuffledWaypoints)
        {
            List<WorldWaypoint> shuffled = new();
            shuffled.AddRange(waypointArray);
            CoreUtility.Shuffle(shuffled);

            shuffledWaypointarray = shuffled.ToArray();
            shuffledWaypoints = true;
            //lastVillageID = -1; // Init
        }

        // Look for all waypoints except last one
        int length = shuffledWaypointarray.Length;
        for (int i = 0; i < length; i++) // For each waypoint
        {
            if (shuffledWaypointarray[i].WaypointId == lastVillageID) continue; // Skip village if it was the last one already

            var chara = shuffledWaypointarray[i].GetCharaForQuest();
            if (chara)
            {
                lastVillageID = shuffledWaypointarray[i].WaypointId;
                return chara;
            }
        }

        if (lastVillageID < 0) return null; // No last-id

        // Found no character -> Check in the last village
        for (int i = 0; i < length; i++) // For each waypoint
        {
            if (shuffledWaypointarray[i].WaypointId != lastVillageID) continue; // Opposite condition

            var chara = shuffledWaypointarray[i].GetCharaForQuest();
            if (chara)
            {
                lastVillageID = shuffledWaypointarray[i].WaypointId;
                return chara;
            }
        }

        return null;
    }

    #endregion Quest

    #region PlayerQuest

    public void ReceiveGiftFromChara(WorldCharacter chara, GameObject giftObject)
    {
        if(player)
        {
            player.ReceiveGiftFromChara(chara, giftObject);
        }

        if(game.CanvasManager.uiQuest)
        {
            game.CanvasManager.uiQuest.SetQuestReturnText();
        }
    }

    #endregion

    // Editor
    [Button]
    private void GetAllWaypoints()
    {
        waypointArray = GetComponentsInChildren<WorldWaypoint>();

        int length = waypointArray.Length;
        for (int i = 0; i < length; i++)
        {
            waypointArray[i].World = this;
            waypointArray[i].WaypointId = i;
        }

        // Road
        roadArray = GetComponentsInChildren<WorldRoad>();

        length = roadArray.Length;
        for (int i = 0; i < length; i++)
        {
            roadArray[i].World = this;
        }
    }


}
