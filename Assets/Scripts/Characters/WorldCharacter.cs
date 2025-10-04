using UnityEngine;

using Sirenix.OdinInspector;

public class WorldCharacter : MonoBehaviour
{
    [SerializeField] protected WorldManager world;

    [Header("Stats")]
    [SerializeField] private bool hasSomethingToGive;
    [SerializeField, ShowIf(nameof(hasSomethingToGive))] private float giftValue;

}
