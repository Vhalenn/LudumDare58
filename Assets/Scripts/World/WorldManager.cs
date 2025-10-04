using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private GameManager game; public GameManager Game => game;
    [SerializeField] private float currentTime;
    [SerializeField] private bool accelerateTimeForDebug;

    [Header("Main Elements")]
    [SerializeField] private Player player; public Player Player => player;
    [SerializeField] private DemonDen demonDen;

    [Header("Content")]
    [SerializeField] private WorldWaypoint[] waypointArray;
    [SerializeField] private WorldRoad[] roadArray;

    [Header("Storage")]
    [SerializeField, ReadOnly()] private int currentVillageID = -1;
    [SerializeField, ReadOnly()] private int lastVillageID = -1;
    [SerializeField] private QuestData currentQuest;

    private void Start()
    {

    }

    private void Update()
    {
        if(QuestStarted() && game && game.CanvasManager.Timer) // Current quest in progress
        {
            if (!player.CurrentlyInDialog) // If not in dialog
            {
                if (accelerateTimeForDebug)
                {
                    currentQuest.timeLeft -= Time.deltaTime * 5;
                }
                else
                {
                    currentQuest.timeLeft -= Time.deltaTime;
                }
            }

            game.CanvasManager.Timer.SetTimer(currentQuest.timeLeft, currentQuest.timeToDoTheTask);

            if(currentQuest.timeLeft < 0f)
            {
                LooseGame(); // No more time
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
        Debug.Log("You Lost the game");

        game.CanvasManager.endScreen.ShowLooseScreen();
        ClearQuest();
    }

    public bool QuestStarted()
    {
        return currentQuest.goalChara != null;
    }

    public void ClearQuest()
    {
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

        if(!quest.goalChara)
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

        quest.timeToDoTheTask = 30;
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
        if(currentVillageID < 0)
        {
            // Need to pick a first ID
        }

        int length = waypointArray.Length;
        for (int i = 0; i < length; i++) // For each waypoint
        {
            if (lastVillageID == i) continue; // Skip village if it was the last one already

            var chara =  waypointArray[i].GetCharaForQuest();
            if (chara)
            {
                lastVillageID = i;
                return chara;
            }
        }

        // Found no character -> Check in the last village
        if(lastVillageID >= 0)
        {
            return waypointArray[lastVillageID].GetCharaForQuest();
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
