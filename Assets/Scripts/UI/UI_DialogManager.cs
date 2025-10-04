using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Febucci.UI;
using Febucci.UI.Core.Parsing;

public class UI_DialogManager : MonoBehaviour
{
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private bool debug;

    [Header("Elements")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI buttonText;

    [Header("Animation")]
    [SerializeField] private TextAnimator_TMP textAnimator;
    [SerializeField] private TypewriterByCharacter dialogTypewritter;
    [SerializeField] private string demonAnim;
    [SerializeField] private string peopleAnim;

    [Header("Storage")]
    [SerializeField] private WorldCharacter currentChara;
    [SerializeField] private DialogData currDial;
    [SerializeField] private int currBubbleId;
    [SerializeField] private bool init;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if(init) return;

        HideDialog();

        init = true;
    }

    public void PlayerActionPressed() // Pressed E
    {
        if(debug) Debug.Log("DialogManager -> PlayerActionPressed()");

        if(dialogTypewritter)
        {
            if(dialogTypewritter.isShowingText)
            {
                dialogTypewritter.SkipTypewriter();
                return;
            }

            // Dialog is finished typing
            DisplayBubble(currBubbleId + 1); // Display next bubble
        }
    }

    public void FinishedTyping()
    {
        if (buttonText)
        {
            buttonText.text = "E";
        }
    }

    public void TypedCharacter() // For Sounds and Animations
    {

    }

    public void StartDialog(WorldCharacter chara, DialogData dialog)
    {
        Init();

        if(!dialog.IsValid())
        {
            Debug.LogError("Dialog is invalid");
            return;
        }

        if (debug) Debug.Log("DialogManager -> StartDialog()");

        currDial = dialog;
        currentChara = chara;
        canvasGroup.alpha = 1; // Display dialog

        DisplayBubble(0);
    }

    private void DisplayBubble(int id)
    {
        currBubbleId = id;

        if(!currDial.IsValid() || currBubbleId >= currDial.bubbleArray.Length)
        {
            FinishDialog();
            return;
        }

        bool charaIsTalking = currDial.bubbleArray[id].speaker == 0;

        if (!currentChara)
        {
            speakerText.text = "???";
        }
        else if (speakerText)
        {
            speakerText.text = charaIsTalking ? currentChara.PublicName : "You";
        }

        if (currentChara && textAnimator)
        {
            bool evilMode = currentChara.IsEvil || !charaIsTalking;
            textAnimator.DefaultAppearancesTags = new[] { evilMode ? demonAnim : peopleAnim };
        }

        if (buttonText)
        {
            buttonText.text = "...";
        }

        if (dialogTypewritter)
        {
            dialogTypewritter.ShowText(currDial.bubbleArray[id].text);
        }
    }

    private void FinishDialog()
    {
        var chara = currentChara;
        CleanDialog();
        HideDialog();

        if (canvasManager) // Allow player to move back
        {
            canvasManager.game.World.DialogEnded();
        }

        if(chara)
        {
            chara.DialogEnded();
        }
    }

    private void CleanDialog()
    {
        currDial = new();
        currentChara = null;
        currBubbleId = 0; // Display dialog
    }    

    public void HideDialog()
    {
        if (canvasGroup)
        {
            canvasGroup.alpha = 0;
        }
    }
}
