using UnityEngine;


using Sirenix.OdinInspector;

public class DemonDen : WorldCharacter
{
    public override bool IsEvil => true;

    [Header("Elements")]
    [SerializeField] private SphereCollider selfCollider;

    [Header("Dialog")]
    [SerializeField] private QuestDialogStructure dialogStructure;

    private string startString = "Welcome my devoted, times are very hard. Come closer, I need you to seek goods for me.\n";
    private string locString = "You will find what I need in Ukankh Village, by following the North Path.";
    private string charaString = "Take what <color=red>Ullianah</color> has and bring it to me.";
    private string endString = "Go my child, go, don't loose a minute.";

    //[Header("Storage")]


    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    #region Quest

    public void StartTalking()
    {
        if (!world)
        {
            Debug.LogError($"{transform.name} No world!");
            return;
        }

        if(world.QuestStarted())
        {
            if(world.Player && world.Player.HasGiftFromChara)
            {
                // QUEST IS DONE!
                EndQuest();
                return;
            }

            QuestAlreadyStartedText();
            return;
        }

        // Create a new quest
        StartNewQuest();
    }

    private void QuestAlreadyStartedText()
    {
        DialogBubble newBubble = new(0, "Hurry up my child!");
        DialogData data = new(new DialogBubble[] { newBubble });

        world.StartDialog(this, data);
    }

    [Button]
    private void StartNewQuest()
    {
        QuestData currQuest = new();
        world.GetNextQuest(ref currQuest);

        if(!currQuest.goalChara)
        {
            // WIN GAME
            world.WinGame();
            return;
        }

        string textStart = dialogStructure.GetRandomStarter();
        string textLoc = dialogStructure.GetRandomLocation();
        textLoc = textLoc.Replace("@Road", $"<color=blue>{currQuest.RoadName}</color>");
        textLoc = textLoc.Replace("@Loc", $"<color=green>{currQuest.WaypointName}</color>");

        string textChara = dialogStructure.GetRandomCharacter();
        textChara = textChara.Replace("@Chara", $"<color=red>{currQuest.CharaName}</color>");

        string textEnd = dialogStructure.GetRandomEnd();

        string text = $"{textStart} {textLoc} {textChara} {textEnd}";

        DialogBubble newBubble = new(0, text);
        DialogData data = new(new DialogBubble[] { newBubble });
        world.StartDialog(this, data);
    }

    #endregion Quest

    #region QuestEnd

    public override void DialogEnded()
    {
        // If quest is not started ->
        if (!world.QuestStarted())
        {
            StartNewQuest();
        }
    }

    private void EndQuest()
    {
        world.ClearQuest();

        // Give reward

        // Talk
        DialogBubble newBubble = new(0, "OooOoOh Congratulations! You brought what I asked you! Continue like this, this task is of great importance.");
        DialogData data = new(new DialogBubble[] { newBubble });

        world.Player.EndQuest();

        world.StartDialog(this, data);

        // -> New quest when dialog end
    }

    #endregion QuestEnd

    #region UnityFunctions

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        StartTalking();
    }

    private void OnDrawGizmos()
    {
        if (!selfCollider) return;

        CoreUtility.DrawCircle(Color.red, transform.position + selfCollider.center, selfCollider.radius);
    }

    #endregion UnityFunctions
}

[System.Serializable]
public struct QuestData
{
    public WorldRoad goalRoad;
    public WorldWaypoint goalWaypoint;
    public WorldCharacter goalChara;
    public float timeToDoTheTask;
    public float timeLeft;

    public float reward;

    public string RoadName => goalRoad ? goalRoad.PublicName : "Nowhere";
    public string WaypointName => goalWaypoint ? goalWaypoint.PublicName : "Nowhere";
    public string CharaName => goalChara ? goalChara.PublicName : "Noone";
}

[System.Serializable]
public struct QuestDialogStructure
{
    public int storyState; // Early / Mid / Late... ETC
    [TextArea] public string[] starter;
    [TextArea, GUIColor("cyan")] public string[] location;
    [TextArea] public string[] character;
    [TextArea, GUIColor("green")] public string[] end;

    public string GetRandomStarter() => GetRandomElement(starter);
    public string GetRandomLocation() => GetRandomElement(location);
    public string GetRandomCharacter() => GetRandomElement(character);
    public string GetRandomEnd() => GetRandomElement(end);

    public string GetRandomElement(string[] array)
    {
        if (array == null || array.Length == 0) return string.Empty;
        return array[Random.Range(0, array.Length)];
    }
}