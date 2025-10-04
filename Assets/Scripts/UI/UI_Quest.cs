using UnityEngine;

using TMPro;
using Sirenix.OdinInspector;

public class UI_Quest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questText;

    [Header("Storage")]
    [SerializeField, ReadOnly] private bool receivedQuest;
     
    public void Start()
    {
        if (receivedQuest) return;

        ClearQuestText();
    }
    public void SetQuestText(QuestData data)
    {
        receivedQuest = true;

        if (questText)
        {
            questText.text = $"{data.RoadName}\n\\/\n{data.WaypointName}\n\\/\n{data.CharaName}";
        }
    }

    public void SetQuestReturnText()
    {
        receivedQuest = true;

        if (questText)
        {
            questText.text = $"Return to me";
        }
    }

    public void ClearQuestText()
    {
        if (questText)
        {
            questText.text = "No current mission.";
        }
    }
}
