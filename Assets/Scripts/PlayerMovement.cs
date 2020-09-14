using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{

    public Transform[] lanes;
    public Rigidbody Rigidbody;
    public Collider collider;

    public int speed = 20;
    public int jumpHeight = 30;
    public int laneWidth = 3;
    public float distToGround = 0.1f;
    public float gravity = 9.81f;

    private int curLane;
    private int movement = 0;
    private bool isMoving = false;
    private bool jumpingLocked = false;
    private Vector3 target;

    void Start()
    {
        if (Rigidbody == null)
        {
            Debug.Log("Player does not have an assigned Rigidbody! Quitting Application.");
            Application.Quit(-1);

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        Rigidbody.useGravity = false;

        if (lanes == null || lanes.Length < 1)
        {
            Debug.Log("Player does not have enough lanes! Quitting Application.");
            Application.Quit(-1);

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        curLane = (lanes.Length - 1) / 2;
        transform.position = lanes[curLane].position;
        target = transform.position;

    }

    void Update()
    {
        
        if (isMoving)
        {
            target = new Vector3(target.x, transform.position.y, target.z);
            
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            
            if (Math.Abs(transform.position.z - target.z) < 0.01f && Math.Abs(transform.position.x - target.x) < 0.01f)
            {
                transform.position = target;
                isMoving = false;
            }


            //return;
        }
        
        if (jumpingLocked && Rigidbody.velocity.y < 1)
        {
            jumpingLocked = !IsGrounded();
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
            target = new Vector3(lanes[curLane].position.x, transform.position.y, lanes[curLane].position.z);
            isMoving = true;

            return;
        }




        if (Input.GetKeyDown(KeyCode.UpArrow) && !jumpingLocked)
        {
            Debug.Log("jump");
            Rigidbody.AddForce(Vector3.up * jumpHeight + Vector3.up * gravity, ForceMode.VelocityChange);
            //target = lanes[curLane].position + Vector3.up * jumpHeight;
            //isMoving = true;
            jumpingLocked = true;

            return;

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {

        }

    }

    private void FixedUpdate()
    {
        Rigidbody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision target)
    {

        if (target.gameObject.tag.Equals("Obstacle") == true)
        {
            Rigidbody.constraints = RigidbodyConstraints.None;

            onGameOver?.Invoke();
        }

        if (target.gameObject.tag.Equals("Coin") == true)
        {
            Destroy(target.gameObject);

            onCollectCoin?.Invoke();
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.0001f, Vector3.down, distToGround);
    }

    public static Action onGameOver;

    public static Action onCollectCoin;
}
