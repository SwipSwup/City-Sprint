using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{

    public Transform[] lanes;

    public float speed = 20f;
    public float jumpSpeed = 10f;
    public float jumpHeight = 2f;
    public float gravity = 9.81f;
    public float distToGround = 0.15f;

    private Rigidbody Rigidbody;
    private Collider Collider;

    private int curLane;
    private int movement = 0;
    private bool isMoving = false;
    private bool isJumping = false;
    private bool applyGravity = true;

    private Vector3 movementTarget;
    private Vector3 jumpingTarget;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();

        if (Rigidbody == null)
        {
            Debug.LogError("Player does not have an assigned Rigidbody! Quitting Application.");
            Application.Quit(-1);

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        if (lanes == null || lanes.Length < 1)
        {
            Debug.LogError("Player does not have enough lanes! Quitting Application.");
            Application.Quit(-1);

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        Rigidbody.useGravity = false;

        curLane = (lanes.Length - 1) / 2;
        transform.position = lanes[curLane].position;
        movementTarget = transform.position;

        Collider = GetComponent<Collider>();
    }

    void Update()
    {
        
        if (isMoving)
        {
            movementTarget = new Vector3(movementTarget.x, transform.position.y, movementTarget.z);
            
            transform.position = Vector3.MoveTowards(transform.position, movementTarget, speed * Time.deltaTime);
            
            if (Math.Abs(transform.position.z - movementTarget.z) < 0.01f && Math.Abs(transform.position.x - movementTarget.x) < 0.01f)
            {
                transform.position = movementTarget;
                isMoving = false;
            }


            //return;
        }


        movement = 0;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movement -= 1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement += 1;
        }

        if (movement != 0 && curLane + movement >= 0 && curLane + movement <= lanes.Length - 1)
        {
            curLane += movement;
            movementTarget = new Vector3(lanes[curLane].position.x, transform.position.y, lanes[curLane].position.z);
            isMoving = true;

            return;
        }

        if (isJumping)
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
        else if (!IsGrounded())
        {
            /*
            transform.position = Vector3.MoveTowards(transform.position, 
                new Vector3(transform.position.x, jumpingTarget.y - jumpHeight, transform.position.z), 
                jumpSpeed * Time.deltaTime);

            if (IsGrounded())
            {
                applyGravity = true;
            }
            */

            return;
            
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded())
        {

            if (!isJumping)
            {
                jumpingTarget = new Vector3(transform.position.x, transform.position.y + jumpHeight, transform.position.z);
                isJumping = true;
                applyGravity = false;

                return;
            }

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {

        }

    }

    private void FixedUpdate()
    {
        if (!IsGrounded() && applyGravity)
        {
            Rigidbody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        }
    }

    void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag.Equals("Coin"))
        {
            Destroy(target.gameObject);

            onCollectCoin?.Invoke();
            return;
        }

        if (target.gameObject.tag.Equals("Obstacle"))
        {
            Rigidbody.constraints = RigidbodyConstraints.None;

            onGameOver?.Invoke();
            return;
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, distToGround);
    }

    public static Action onGameOver;

    public static Action onCollectCoin;
}
