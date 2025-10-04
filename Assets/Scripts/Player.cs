using UnityEngine;

public class Player : MovingEntity
{
    [SerializeField] private WorldManager worldManager;

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

    private void FixedUpdate()
    {
        if (!rb) return;

        velocity = rb.linearVelocity;

        if (controlInput.magnitude < 0.1f || CurrentlyInDialog || InMenu)
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

        if (!OnGround && velocity.y < 0) velocity.y *= 1.25f; // Force stronger gravity for test

        // Apply speed
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, velocity, 0.3f);
        if(desiredVelocity.magnitude > 0.001f)
        {
            rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(desiredVelocity), 0.2f);
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

        hasGiftFromChara = null;
        currentGift = null;
    }

    #endregion Dialog
}
