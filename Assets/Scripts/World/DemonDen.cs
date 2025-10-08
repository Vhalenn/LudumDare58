using UnityEngine;


using Sirenix.OdinInspector;

public class DemonDen : WorldCharacter
{
    public override bool IsEvil => true;

    [Header("Elements")]
    [SerializeField] private SphereCollider selfCollider;
    [SerializeField] private anim_FaceElement animFace;

    [Header("DialogColor")]
    [SerializeField] private Color colorRoad;
    [SerializeField] private Color colorLoc;
    [SerializeField] private Color colorChara;

    [Header("Dialog")]
    [SerializeField] private QuestDialogStructure firstDialogStructure;
    [SerializeField] private QuestDialogStructure dialogStructure;

    [Header("Audio")]
    [SerializeField] private AudioClip audioQuestStart;
    [SerializeField] private AudioClip audioQuestDone;

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

        if(world.QuestStarted()) // Finish quest
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

        if(!currQuest.goalChara || world.QuestCount >= WorldManager.QUEST_TO_WIN) // Quest to win == 6
        {
            // WIN GAME
            world.WinGame();
            return;
        }

        // Get dialog possibilities
        QuestDialogStructure dialStruct;
        if(world.QuestCount <= 0 && world.ErrorCount <= 0)
        {
            dialStruct = firstDialogStructure;
        }
        else
        {
            dialStruct = dialogStructure;
        }

        // Create the sentence
        string text = GetDialogString(dialStruct, currQuest);

        DialogBubble newBubble = new(0, text);
        DialogData data = new(new DialogBubble[] { newBubble });
        world.StartDialog(this, data);

        if (audioSource)
        {
            audioSource.PlayOneShot(audioQuestStart);
        }
    }

    private string GetDialogString(QuestDialogStructure dialStruct, QuestData currQuest)
    {
        string textStart = dialStruct.GetRandomStarter();
        string textLoc = dialStruct.GetRandomLocation();
        textLoc = textLoc.Replace("@Road", $"<color=#{ColorUtility.ToHtmlStringRGB(colorRoad)}>{currQuest.RoadName}</color>");
        textLoc = textLoc.Replace("@Loc", $"<color=#{ColorUtility.ToHtmlStringRGB(colorLoc)}>{currQuest.WaypointName}</color>");

        string textChara = dialStruct.GetRandomCharacter();
        textChara = textChara.Replace("@Chara", $"<color=#{ColorUtility.ToHtmlStringRGB(colorChara)}>{currQuest.CharaName}</color>");

        string textEnd = dialStruct.GetRandomEnd();

        return $"{textStart} {textLoc} {textChara} {textEnd}";
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

    public void EndQuestBecauseFailed()
    {
        world.ClearQuest(validated: false);

        // Give reward

        // Talk
        DialogBubble newBubble = new(0, "HhhhHhHhhHHh, I am not AT ALL proud of you! You won't have another chance!!");
        DialogData data = new(new DialogBubble[] { newBubble });

        world.Player.EndQuest();

        world.StartDialog(this, data);

        // -> New quest when dialog end
    }

    private void EndQuest()
    {
        world.ClearQuest(validated:true);

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