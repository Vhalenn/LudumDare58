using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ui_ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    Button btn;
    AudioSource audioSource;
    public AudioClip buttonHoverSound, buttonClickSound;

    public void Start() {
        btn = GetComponent<Button>();
        GetAudio();
    }

    // EVENT
    public void OnPointerEnter(PointerEventData eventData) {  
        if(btn.IsInteractable()) {
            if(!audioSource.isPlaying) audioSource.PlayOneShot(buttonHoverSound);
        }
    }
    
    public void OnPointerClick(PointerEventData eventData) {
        if(btn.IsInteractable()) {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }

    // Init
    void GetAudio() {
        audioSource = transform.parent.GetComponent<AudioSource>();
        if(audioSource == null) audioSource = transform.parent.gameObject.AddComponent<AudioSource>();
    }

}
