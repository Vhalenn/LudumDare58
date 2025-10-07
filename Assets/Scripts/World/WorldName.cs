using UnityEngine;

using TMPro;

public class WorldName : MonoBehaviour
{
    [SerializeField] private WorldElement parent;
    [SerializeField] private TextMeshPro text;
    
    public void SetState(bool state)
    {
        if(parent && text)
        {
            text.text = parent.PublicName;
        }

        gameObject.SetActive(state);
    }
}
