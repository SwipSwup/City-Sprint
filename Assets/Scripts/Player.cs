using UnityEngine;
using System;

public class Player : MonoBehaviour
{

    public Transform[] lanes;
    public Transform Camera;

    public float speed = 20f;
    public float jumpSpeed = 10f;
    public float jumpHeight = 2f;
    public float sneakDuration = 10f;
    public float gravity = 9.81f;
    public float distToGround = 0.01f;
    public float collisionForce = 10f;

    public bool controlsLocked = false;

    private Rigidbody Rigidbody;

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
        Rigidbody = GetComponent<Rigidbody>();

        CheckRigidbody();
        CheckLanes();

        Rigidbody.useGravity = false;

        curLane = (lanes.Length - 1) / 2;
        transform.position = lanes[curLane].position;
        movementTarget = transform.position;
    }

    private void CheckRigidbody()
    {
        if (Rigidbody == null)
        {
            Debug.LogError("Player does not have an assigned Rigidbody! Quitting Application.");
            Application.Quit(-1);

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }

    private void CheckLanes()
    {
        if (lanes == null || lanes.Length < 1)
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

        if (isMoving) ApplyMovement();
        GetMovementInput();

        if (isJumping) ApplyJumpingMovement();
        HandleJumping();

        HandleSneaking();
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

    private void GetMovementInput()
    {
        movement = 0;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) movement -= 1;
        if (Input.GetKeyDown(KeyCode.RightArrow)) movement += 1;

        if (movement != 0 && curLane + movement >= 0 && curLane + movement <= lanes.Length - 1)
        {
            oldLane = curLane;
            curLane += movement;
            oldLocation = transform.position;
            movementTarget = new Vector3(lanes[curLane].position.x, transform.position.y, lanes[curLane].position.z);
            isMoving = true;
        }
    }

    private void ApplyJumpingMovement()
    {
        jumpingTarget = new Vector3(transform.position.x, jumpingTarget.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, jumpingTarget, jumpSpeed * (Math.Abs(jumpingTarget.y - transform.position.y) / jumpHeight + 0.2f) * Time.deltaTime);

        if (Math.Abs(transform.position.y - jumpingTarget.y) < 0.01f)
        {
            transform.position = jumpingTarget;
            isJumping = false;
            applyGravity = true;
        }
    }

    private void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded() && !isJumping)
        {
            jumpingTarget = new Vector3(transform.position.x, transform.position.y + jumpHeight, transform.position.z);
            isJumping = true;
            applyGravity = false;
            transform.LeanScaleY(1f, 0.05f);
        }
    }

    private void HandleSneaking()
    {
        if (isSneaking)
        {
            if (sneakDurationLeft > 0)
            {
                sneakDurationLeft -= Time.deltaTime * 10;
            }
            else
            {
                isSneaking = false;
                transform.LeanScaleY(1f, 0.1f);
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (isJumping)
            {
                isJumping = false;
                applyGravity = true;

                Rigidbody.AddForce(Vector3.down * gravity, ForceMode.VelocityChange);
            }
            else if (!IsGrounded())
            {
                Rigidbody.AddForce(Vector3.down * gravity, ForceMode.VelocityChange);
            }

            transform.LeanScaleY(0.5f, 0.1f);
            sneakDurationLeft = sneakDuration;
            isSneaking = true;
        }
    }

    private void FixedUpdate()
    {
        if (!IsGrounded() && applyGravity && !controlsLocked) 
            Rigidbody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag.Equals("Coin"))
        {
            Destroy(target.gameObject);

            OnCollectCoin?.Invoke();
        }

        if (target.gameObject.tag.Equals("Obstacle") && !controlsLocked)
        {
            ObstacleCollision();
        }
    }

    private void ObstacleCollision()
    {
        if (!isMoving)
        {
            GameOver();
            return;
        }
        curLane = oldLane;
        movementTarget = oldLocation;

        OnObstacleCollision?.Invoke();
    }

    private void GameOver()
    {
        Rigidbody.constraints = RigidbodyConstraints.None;
        Rigidbody.useGravity = true;
        Rigidbody.AddForce(Vector3.Normalize(Camera.position - transform.position) * collisionForce, ForceMode.VelocityChange);
        controlsLocked = true;

        OnGameOver?.Invoke();
    }

    private bool IsGrounded() => Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, distToGround + 0.2f);

    public static Action OnGameOver;

    public static Action OnCollectCoin;

    public static Action OnObstacleCollision;
}
