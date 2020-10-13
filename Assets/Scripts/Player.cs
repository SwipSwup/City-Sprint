using UnityEngine;
using System;

public class Player : MonoBehaviour
{

    [Header("Required Objects")]

    [Tooltip("Positions of the lanes the players moves between")]
    [SerializeField] private Transform[] lanes = new Transform[2];
    [Tooltip("The camera of the player")]
    [SerializeField] private Transform Camera;


    [Header("Movement Settings")]

    [Range(1f, 100f)]
    [Tooltip("Speed at wich the players moves between the lanes")]
    [SerializeField] private float speed = 20f;

    [Range(1f, 100f)]
    [Tooltip("Speed at wich the players moves up when jumping")]
    [SerializeField] private float jumpSpeed = 10f;

    [Range(0f, 100f)]
    [Tooltip("Height the player jumps at")]
    [SerializeField] private float jumpHeight = 2f;

    [Range(0f, 100f)]
    [Tooltip("Duration the player sneaks when pressing the sneak button")]
    [SerializeField] private float sneakDuration = 10f;

    [Range(0f, 100f)]
    [Tooltip("Gravity applied to the player when falling (ignored when jumping)")]
    [SerializeField] private float gravity = 9.81f;

    [Space]

    [Range(0f, 1f)]
    [Tooltip("The distance the player checks below itsself to decide whether its touching the ground")]
    [SerializeField] private float distToGround = 0.01f;

    [Range(0f, 100f)]
    [Tooltip("The force at wich the player gets shot away when the game is over")]
    [SerializeField] private float collisionForce = 10f;

    public bool controlsLocked = false;

    private Rigidbody playerRigidbody;
    private CapsuleCollider playerCollider;

    private int curLane;
    private int oldLane;
    private int movement = 0;
    private bool isMoving = false;
    private bool isJumping = false;
    private bool isSneaking = false;
    private float sneakDurationLeft = 0f;
    private bool applyGravity = true;

    private Vector3 movementTarget;
    private Vector3 oldLocation;
    private Vector3 jumpingTarget;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();

        CheckRigidbody();
        CheckLanes();

        playerRigidbody.useGravity = false;

        curLane = (lanes.Length - 1) / 2;
        transform.position = lanes[curLane].position;
        movementTarget = transform.position;

        PlayerInput.OnSwipeLeft += MoveLeft;
        PlayerInput.OnSwipeRight += MoveRight;
        PlayerInput.OnSwipeUp += Jump;
        PlayerInput.OnSwipeDown += Sneak;
    }

    private void CheckRigidbody()
    {
        if (playerRigidbody == null)
        {
            Debug.LogError("Player does not have an assigned Rigidbody! Quitting Application.");
            Application.Quit(-1);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    private void OnDestroy()
    {
        PlayerInput.OnSwipeLeft -= MoveLeft;
        PlayerInput.OnSwipeRight -= MoveRight;
        PlayerInput.OnSwipeUp -= Jump;
        PlayerInput.OnSwipeDown -= Sneak;
    }

    private void CheckLanes()
    {
        if (lanes == null || lanes.Length < 2)
        {
            Debug.LogError("Player does not have enough lanes! Quitting Application.");
            Application.Quit(-1);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    void Update()
    {
        ManageMovement();
    }

    private void ManageMovement()
    {
        if (controlsLocked) return;

        GetPCInput();

        ManageMovementInput();

        if (isMoving) ApplyMovement();

        if (isJumping) ApplyJumpingMovement();

        if (isSneaking) ApplySneaking();

        ApplyGravity();
    }

    private void ApplyMovement()
    {
        movementTarget = new Vector3(movementTarget.x, transform.position.y, movementTarget.z);
        transform.position = Vector3.MoveTowards(transform.position, movementTarget, speed * Time.deltaTime);

        if (Math.Abs(transform.position.z - movementTarget.z) < 0.01f &&
            Math.Abs(transform.position.x - movementTarget.x) < 0.01f)
        {
            transform.position = movementTarget;
            isMoving = false;
        }
    }

    private void ApplyJumpingMovement()
    {
        jumpingTarget = new Vector3(transform.position.x, jumpingTarget.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, jumpingTarget, jumpSpeed * (Math.Abs(jumpingTarget.y - transform.position.y) / jumpingTarget.y + 0.2f) * Time.deltaTime);

        if (Math.Abs(transform.position.y - jumpingTarget.y) < 0.01f)
        {
            transform.position = jumpingTarget;
            isJumping = false;
            applyGravity = true;
        }
    }

    private void ApplySneaking()
    {
        if (sneakDurationLeft > 0)
        {
            sneakDurationLeft -= Time.deltaTime * 10;
        }
        else
        {
            isSneaking = false;
            Debug.Log("1");
            playerCollider.height = 2f;
        }
    }

    private void ManageMovementInput()
    {
        if (movement == 0 || curLane + movement < 0 || curLane + movement > lanes.Length - 1)
        {
            movement = 0;
            return;
        }

        oldLane = curLane;
        curLane += movement;
        oldLocation = transform.position;
        movementTarget = new Vector3(lanes[curLane].position.x, transform.position.y, lanes[curLane].position.z);
        movement = 0;
        isMoving = true;
    }

    private void GetPCInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) movement -= 1;
        if (Input.GetKeyDown(KeyCode.RightArrow)) movement += 1;
        if (movement != 0 && !controlsLocked)
        {
            ManageMovementInput();
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && !controlsLocked)
        {
            if (IsGrounded() && !isJumping) Jump();
            return;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && !controlsLocked)
        {
            Sneak();
            return;
        }
    }

    private void MoveLeft()
    {
        if (controlsLocked) return;
        movement --;
    }

    private void MoveRight()
    {
        if (controlsLocked) return;
        movement++;
    }

    private void Jump()
    {
        if (controlsLocked || !IsGrounded()) return;

        playerRigidbody.AddForce(-playerRigidbody.velocity, ForceMode.VelocityChange);

        jumpingTarget = new Vector3(transform.position.x, transform.position.y + jumpHeight, transform.position.z);
        isJumping = true;
        applyGravity = false;
        playerCollider.height = 2f;
    }

    private void Sneak()
    {
        if (controlsLocked) return;
        if (isJumping)
        {
            isJumping = false;
            applyGravity = true;

            playerRigidbody.AddForce(Vector3.down * gravity, ForceMode.VelocityChange);
        }
        else if (!IsGrounded())
        {
            playerRigidbody.AddForce(Vector3.down * gravity, ForceMode.VelocityChange);
        }
        playerCollider.height = 1f;
        sneakDurationLeft = sneakDuration;
        isSneaking = true;
    }

    private void ApplyGravity()
    {
        if (!IsGrounded() && applyGravity && !controlsLocked)
            playerRigidbody.AddForce(Vector3.down * gravity * Time.deltaTime * 60, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag.Equals("Obstacle") && !controlsLocked)
        {
            ObstacleCollision(target.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Coin"))
        {
            CollectCoin(other.gameObject);
        }
    }

    private void CollectCoin(GameObject coin)
    {
        //TODO: animation
        Destroy(coin);
        OnCollectCoin?.Invoke();
    }

    private void ObstacleCollision(GameObject obstacle)
    {
        if (!isMoving || Math.Abs(obstacle.transform.position.z - oldLocation.z) < 0.5f)
        {
            GameOver();
            OnGameOver?.Invoke();
            return;
        }
        curLane = oldLane;
        movementTarget = oldLocation;

        OnObstacleCollision?.Invoke();
    }

    public void GameOver()
    {
        playerRigidbody.constraints = RigidbodyConstraints.None;
        playerRigidbody.useGravity = true;
        playerRigidbody.AddForce(Vector3.Normalize(Camera.position - transform.position) * collisionForce, ForceMode.VelocityChange);
        controlsLocked = true;
    }

    private bool IsGrounded() => Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, distToGround + 0.2f);

    public static Action OnGameOver;

    public static Action OnCollectCoin;

    public static Action OnObstacleCollision;

    public static Action OnScreenTab;
}
