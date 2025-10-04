using UnityEngine;

public class MovingEntity : MonoBehaviour
{
    public Rigidbody rb;

    [Header("Move Alterations")]
    public bool boosted;
    public bool stuned;

    public float boostTime;
    public float stunTime;
}
