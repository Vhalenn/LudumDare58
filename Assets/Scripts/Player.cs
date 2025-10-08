using System.Collections.Generic;

using UnityEngine;

public class Player : MovingEntity
{
    [SerializeField] private WorldManager worldManager;
    [SerializeField] private Animator animator;

    [Header("Base Components")]
    public Vector2 controlInput;
    public bool desiredJump;
    [HideInInspector] private Transform camTransform;
    [HideInInspector] private Camera mainCam;

    [Header("Elements")]
    [SerializeField] private Transform giftParent;

    [Header("Stats")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float brakeForce = 0.9f;

    [Header("On Ground")]
    [SerializeField, Range(0f, 90f)] float maxGroundAngle = 65f;
    [SerializeField] int groundContactCount;
    public bool OnGround => groundContactCount > 0;
    float minGroundDotProduct;
    public Vector3 contactNormal;


    [Header("SafeZone")]
    [SerializeField] private Vector3 lastSafePos;
    [SerializeField] private List<TriggerSafeZone> safeAreaList;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSourceWalking;
    [SerializeField] private AudioSource audioSourceGift;
    [SerializeField] private AudioClip stepSound;
    [SerializeField] private AudioClip takeGiftSound;
    [SerializeField] private AudioClip talkSound;


    [Header("Storage")]
    [SerializeField] private Vector3 desiredVelocity;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private WorldCharacter inDialog; public bool CurrentlyInDialog => inDialog != null;
    [SerializeField] private WorldCharacter hasGiftFromChara; public bool HasGiftFromChara => hasGiftFromChara != null;
    [SerializeField] private GameObject currentGift;
    public bool InMenu => worldManager && worldManager.Game && worldManager.Game.InMenu;


    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;

        //OnValidate();
    }

    public void Teleport(Vector3 pos)
    {
        rb.position = pos;
    }

    private void FixedUpdate()
    {
        if (!rb) return;

        if(rb.position.y < -1f) // Kill plane
        {
            Teleport(lastSafePos + Vector3.up * 0.3f);

            return;
        }

        velocity = rb.linearVelocity;
        bool isNotMoving = controlInput.magnitude < 0.1f || CurrentlyInDialog || InMenu;

        if (isNotMoving)
        {
            velocity.Scale( new (brakeForce, 1, brakeForce));
        }
        else
        {
            // Should not be addition -> Should be capped
            //velocity = Vector3.Lerp(velocity, FollowingCamAngle(controlInput) * speed, 0.01f);

            desiredVelocity = FollowingCamAngle(controlInput) * speed;
            velocity = desiredVelocity;

            velocity.y = rb.linearVelocity.y;
        }

        if(audioSourceWalking)
        {
            audioSourceWalking.volume = Mathf.Lerp(audioSourceWalking.volume, isNotMoving ? 0f : 1f, 0.33f);
        }

        if(animator)
        {
            animator.SetFloat("run", isNotMoving ? 0f : 1f);
        }

        if (!OnGround && velocity.y < 0) velocity.y *= 1.25f; // Force stronger gravity for test

        // Apply speed
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, velocity, 0.3f);
        if(desiredVelocity.magnitude > 0.001f)
        {
            rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(desiredVelocity), 0.2f);
        }

        if(safeAreaList != null && safeAreaList.Count > 0)
        {
            lastSafePos = rb.position;
        }
    }

    public Vector3 FollowingCamAngle(Vector2 input)
    {
        if (!mainCam) Init();
        if (camTransform == null) camTransform = mainCam.transform;

        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 dir = forward * input.y + right * input.x;
        return dir;
    }


    // jumping
    #region Jumping

    // Check if is on ground
    public void ClearState()
    {
        groundContactCount = 0;
        contactNormal = Vector3.zero;
    }

    // Ground check
    void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        ClearState();
    }

    void EvaluateCollision(Collision collision)
    {
        ClearState();
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;

            if (normal.y >= minGroundDotProduct)
            {
                groundContactCount += 1;
                contactNormal += normal;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = OnGround ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }

    public void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }
    #endregion Jumping


    // Dialog
    #region Dialog

    public void SetInDialog(WorldCharacter chara)
    {
        inDialog = chara;
    }

    public void ReceiveGiftFromChara(WorldCharacter chara, GameObject giftObject)
    {
        hasGiftFromChara = chara;
        currentGift = giftObject;

        if(audioSource && takeGiftSound)
        {
            audioSource.PlayOneShot(takeGiftSound);
        }

        if(audioSourceGift)
        {
            audioSourceGift.clip = chara.audioClipGift;
            audioSourceGift.Play();
            audioSourceGift.volume = 1f;
        }

        if (giftObject)
        {
            giftObject.transform.SetParent(giftParent);
            giftObject.transform.localPosition = Vector3.zero;
            giftObject.transform.localRotation = Quaternion.identity;
        }
    }

    public void EndQuest()
    {
        if(currentGift)
        {
            currentGift.SetActive(false);
        }

        // Corrution of village should increase
        if(hasGiftFromChara && hasGiftFromChara.Waypoint)
        {
            hasGiftFromChara.Waypoint.RiseCorruption();
        }

        if(audioSourceGift)
        {
            audioSourceGift.volume = 0f;
        }

        hasGiftFromChara = null;
        currentGift = null;
    }

    #endregion Dialog

    #region SafeZone
    public void AddTriggerZone(TriggerSafeZone safeZone)
    {
        if (safeAreaList == null) safeAreaList = new();

        if (!safeAreaList.Contains(safeZone))
        {
            safeAreaList.Add(safeZone);
        }
    }
    public void RemoveTriggerZone(TriggerSafeZone safeZone)
    {
        if (safeAreaList == null) safeAreaList = new();

        if(safeAreaList.Contains(safeZone))
        {
            safeAreaList.Remove(safeZone);
        }
    }
    #endregion SafeZone
}
