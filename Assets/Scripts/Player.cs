using UnityEngine;
using System;

public class Player : MonoBehaviour
{

    [Header("Required Objects")]

    [Tooltip("Positions of the lanes the players moves between")]
    [SerializeField] private Transform[] lanes = new Transform[2];
    [Tooltip("The Transform of the Model of the player")]
    [SerializeField] private Transform playerModel;

    [Space]
    [Header("Movement Settings")]

    [Range(0f, 100f)]
    [Tooltip("Speed at wich the players moves between the lanes")]
    [SerializeField] private float speed = 30f;

    [Range(0f, 1f)]
    [Tooltip("The distance the player checks below itsself to decide whether its touching the ground")]
    [SerializeField] private float levitateDistToGround = 1f;

    [Range(0f, 100f)]
    [Tooltip("The force at wich the player gets shot away when the game is over")]
    [SerializeField] private float collisionForce = 10f;

    [Space]

    [Range(0f, 100f)]
    [Tooltip("Speed at wich the players moves up when jumping")]
    [SerializeField] private float jumpSpeed = 20f;

    [Range(0f, 100f)]
    [Tooltip("Height the player jumps at")]
    [SerializeField] private float jumpHeight = 2f;

    [Range(0f, 100f)]
    [Tooltip("Duration the player stays in the air when jumping")]
    [SerializeField] private float jumpDuration = 4f;

    [Space]

    [Range(0f, 100f)]
    [Tooltip("Duration the player sneaks when pressing the sneak button")]
    [SerializeField] private float sneakDuration = 10f;

    [Range(0f, 1f)]
    [Tooltip("The distance the player levitates above the ground")]
    [SerializeField] private float sneakDistToGround = 0.01f;

    public bool controlsLocked = false;

    private Rigidbody playerRigidbody;
    //private CapsuleCollider playerCollider;
    private float playerBoxColliderHeight;

    private int curLane;
    private int oldLane;
    private int movement = 0;
    private bool isMoving = false;
    private bool isJumping = false;
    private float jumpDurationLeft = 0f;
    private bool isSneaking = false;
    private float sneakDurationLeft = 0f;
    private bool applyGravity = true;
    private float distToGround;
    private bool isGrounded = false;

    private Vector3 movementTarget;
    private Vector3 oldLocation;
    private Vector3 jumpingTarget;
    private Vector3 logicModelDifference;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        //playerCollider = GetComponent<CapsuleCollider>();
        logicModelDifference = playerModel.position - transform.position;

        CheckRigidbody();
        CheckLanes();

        playerRigidbody.useGravity = false;
        distToGround = levitateDistToGround;
        playerBoxColliderHeight = GetComponent<BoxCollider>().size.y;

        curLane = (lanes.Length - 1) / 2;
        transform.position = lanes[curLane].position + Vector3.up * 3;
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

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    void Update()
    {
        ManageMovement();
        UpdateModelMovement();
    }

    private void UpdateModelMovement()
    {
        playerModel.position = transform.position + logicModelDifference;

        if (controlsLocked)
        {
            playerModel.transform.position = this.transform.position;
            playerModel.transform.rotation = this.transform.rotation;
        }
    }

    private void ManageMovement()
    {
        if (controlsLocked) return;

        ManageMovementInput();

        if (isMoving) ApplyMovement();

        if (isJumping) ApplyJumpingMovement();

        if (isSneaking) ApplySneaking();

        //ApplyGravity();
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
        transform.position = Vector3.MoveTowards(transform.position, jumpingTarget, jumpSpeed * Time.deltaTime);

        if (Math.Abs(transform.position.y - jumpingTarget.y) < 0.01f)
        {
            JumpLevitate();
        }
    }

    private void JumpLevitate()
    {
        if (jumpDurationLeft > 0)
        {
            jumpDurationLeft -= Time.deltaTime * 10;
        }
        else
        {
            CancelJumping();
        }
    }

    private void CancelJumping()
    {
        jumpDurationLeft = 0;
        isJumping = false;
        applyGravity = true;
    }

    private void ApplySneaking()
    {
        if (sneakDurationLeft > 0)
        {
            sneakDurationLeft -= Time.deltaTime * 10;
        }
        else
        {
            CancelSneaking();
        }
    }

    private void CancelSneaking()
    {
        sneakDurationLeft = 0;
        isSneaking = false;
        distToGround = levitateDistToGround;
        transform.position += Vector3.up * (levitateDistToGround - sneakDistToGround);
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

    private void MoveLeft()
    {
        if (controlsLocked) return;
        movement--;
    }

    private void MoveRight()
    {
        if (controlsLocked) return;
        movement++;
    }

    private void Jump()
    {
        if (controlsLocked || !CheckGrounded()) return;

        if (isSneaking) CancelSneaking();

        //playerRigidbody.AddForce(-playerRigidbody.velocity, ForceMode.VelocityChange);

        jumpingTarget = new Vector3(transform.position.x, transform.position.y + jumpHeight, transform.position.z);
        isJumping = true;
        applyGravity = false;

        jumpDurationLeft = jumpDuration;
    }

    private void Sneak()
    {
        if (controlsLocked) return;
        if (isJumping)
        {
            CancelJumping();
            //playerRigidbody.AddForce(Vector3.down * gravity, ForceMode.VelocityChange);
        }
        //else if (isGrounded)
        //{
        //    playerRigidbody.AddForce(Vector3.down * gravity, ForceMode.VelocityChange);
        //}

        distToGround = sneakDistToGround;
        sneakDurationLeft = sneakDuration;
        isSneaking = true;
    }

    private void ApplyGravity()
    {
        if (CheckGrounded() || !applyGravity) return;

        transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.down * 10, jumpSpeed / 60);


        //Debug.Log(playerRigidbody.velocity);
        //if (!IsGrounded() && applyGravity && !controlsLocked)
        //    playerRigidbody.velocity = Vector3.down * jumpSpeed; //AddForce(Vector3.down * jumpSpeed, ForceMode.VelocityChange);
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
        //playerRigidbody.AddForce(Vector3.Normalize(Camera.position - playerTransform.position) * collisionForce, ForceMode.VelocityChange);
        playerRigidbody.AddForce(Vector3.up * collisionForce, ForceMode.Force);
        controlsLocked = true;
    }

    private bool CheckGrounded()
    {
        //Debug.DrawRay(transform.position, Vector3.down, Color.magenta, distToGround + playerBoxColliderHeight / 2);
        isGrounded = Physics.Raycast(transform.position, Vector3.down, distToGround + playerBoxColliderHeight / 2);
        return isGrounded;
    }

    public static Action OnGameOver;

    public static Action OnCollectCoin;

    public static Action OnObstacleCollision;

    public static Action OnScreenTab;
}
