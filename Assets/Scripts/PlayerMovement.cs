using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{

    public Transform[] lanes;
    public Rigidbody Rigidbody;
    public Collider collider;

    public int speed = 30;
    public int jumpHeight = 3;
    public int laneWidth = 3;
    public float distToGround = 0.1f;
    public float gravity = 9.81f;

    private int curLane;
    private int movement = 0;
    private bool isMoving = false;
    private bool controlsLocked = false;
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
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);


            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                transform.position = target;
                isMoving = false;
                controlsLocked = true;
            }


            return;
        }

        if (controlsLocked)
        {
            controlsLocked = !IsGrounded();
            return;
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
            target = transform.position + new Vector3(0, 0, movement * laneWidth);
            isMoving = true;

            return;
        }




        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            target = lanes[curLane].position + Vector3.up * jumpHeight;
            isMoving = true;

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
        return Physics.Raycast(transform.position + Vector3.up * 0.01f, Vector3.down, distToGround);
    }

    public static Action onGameOver;

    public static Action onCollectCoin;
}
