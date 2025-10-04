using UnityEngine;

public class Player : MovingEntity
{
    [Header("Base Components")]
    public Vector2 controlInput;
    public bool desiredJump;
    [HideInInspector] private GameManager gameManager;
    [HideInInspector] private Transform camTransform;
    [HideInInspector] private Camera mainCam;

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
    [SerializeField] private Vector3 velocity;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        gameManager = GameManager.instance;
        if (!rb) rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;

        //OnValidate();
    }

    private void FixedUpdate()
    {
        if (!rb) return;


        if(controlInput.magnitude < 0.1f)
        {
            velocity *= brakeForce;
        }
        else
        {
            // Should not be addition -> Should be capped
            velocity = Vector3.Lerp(velocity, FollowingCamAngle(controlInput) * speed, 0.5f);
            //velocity = rb.linearVelocity + FollowingCamAngle(controlInput) * speed;
        }

        rb.rotation = Quaternion.LookRotation(velocity);

        // Apply speed
        rb.linearVelocity = velocity;
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

    void EvaluateCollision(Collision collision)
    {
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
    public void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }
    
}
