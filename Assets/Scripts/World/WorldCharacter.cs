using UnityEngine;

using Sirenix.OdinInspector;

public class WorldCharacter : WorldElement
{
    [SerializeField] protected WorldWaypoint waypoint; public WorldWaypoint Waypoint { get => waypoint; set => waypoint = value; }

    [Header("Stats")]
    [SerializeField] protected bool hasSomethingToGive;
    public virtual bool IsEvil => false;
    [SerializeField, ShowIf(nameof(hasSomethingToGive))] protected float giftValue;
    [SerializeField, ShowIf(nameof(hasSomethingToGive))] protected DialogData dialogData;
    [SerializeField, ShowIf(nameof(hasSomethingToGive))] protected GameObject gift;

    [Header("Storage")]
    [SerializeField] protected bool done;
    public bool CanBeQuestGoal => hasSomethingToGive && !done;

    public virtual void DialogEnded()
    {

    }

}

[System.Serializable]
public struct DialogData
{
    public DialogBubble[] bubbleArray;

    public bool IsValid()
    {
        return bubbleArray != null && bubbleArray.Length > 0;
    }

    public DialogData(DialogBubble[] bubbleArray)
    {
        this.bubbleArray = bubbleArray;
    }
}

[System.Serializable]
public struct DialogBubble
{
    [Title("$Title"), GUIColor("green")]
    public int speaker; // 0 == self - 1 == player
    [TextArea, HideLabel] public string text;

    // For inspector
    public string Title => speaker == 0 ? "Self" : "Player";
    public string Color => speaker == 0 ? "green" : "red";

    public DialogBubble(int speaker, string text)
    {
        this.speaker = speaker;
        this.text = text;
    }
}
