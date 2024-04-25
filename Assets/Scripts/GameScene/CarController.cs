using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D frontTireRB;
    [SerializeField] private Rigidbody2D backTireRB;
    [SerializeField] private Rigidbody2D carRB;

    private float moveSpeedChange = 4000f;
    private float maxMoveSpeed = 1000f;

    private float rotateSpeedChange = 9000f;
    private float maxRotateSpeed = 3000f;

    private float force = 3000f;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    [SerializeField] private Box box;
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        box = GameObject.Find("Box").GetComponent<Box>();
    }

    private void Update()
    {
        if(gameManager.isGameOver) return;
        if (Input.GetKey(KeyCode.W))
        {
            moveSpeed += moveSpeedChange * Time.deltaTime;
            moveSpeed = Mathf.Clamp(moveSpeed, 0f, maxMoveSpeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveSpeed -= moveSpeedChange * Time.deltaTime;
            moveSpeed = Mathf.Clamp(moveSpeed, -maxMoveSpeed, 0f);
        }
        else
        {
            if (moveSpeed > 0)
            {
                moveSpeed -= moveSpeedChange * Time.deltaTime;
                moveSpeed = Mathf.Max(moveSpeed, 0f);
            }
            else if (moveSpeed < 0)
            {
                moveSpeed += moveSpeedChange * Time.deltaTime;
                moveSpeed = Mathf.Min(moveSpeed, 0f);
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            rotateSpeed += rotateSpeedChange * Time.deltaTime;
            rotateSpeed = Mathf.Clamp(rotateSpeed, 0f, maxRotateSpeed);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rotateSpeed -= rotateSpeedChange * Time.deltaTime;
            rotateSpeed = Mathf.Clamp(rotateSpeed, -maxRotateSpeed, 0f);
        }
        else
        {
            if (rotateSpeed > 0)
            {
                rotateSpeed -= rotateSpeedChange * Time.deltaTime;
                rotateSpeed = Mathf.Clamp(rotateSpeed, 0f, maxRotateSpeed);
            }
            else if (rotateSpeed < 0)
            {
                rotateSpeed += rotateSpeedChange * Time.deltaTime;
                rotateSpeed = Mathf.Clamp(rotateSpeed, -maxRotateSpeed, 0f);
            }
        }
    }

    private void FixedUpdate()
    {
        frontTireRB.AddTorque(-moveSpeed  * Time.fixedDeltaTime);
        backTireRB.AddTorque(-moveSpeed * Time.fixedDeltaTime);
        carRB.AddTorque(-rotateSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.transform.tag == "Thorn" && !gameManager.isGameOver)
        {
            Debug.Log(collider.name);
            gameManager.OnGameOver(false);
            box.KillAnimation();
        }
        else if(collider.transform.tag == "Box" && !gameManager.isGameOver)
        {
            Debug.Log(collider.name);
            gameManager.OnGameOver(true);
            if(!box.isTouched)
            {
                box.isTouched = true;
                box.Touched();
            }
        }
        else if(collider.transform.tag == "Increase")
        {
            carRB.AddForce(Vector2.right * force);
        }
        else if(collider.transform.tag == "Decrease")
        {
            carRB.AddForce(-Vector2.right * force);
        }
        else if(collider.transform.tag == "Coin")
        {
            Coin coin = collider.GetComponent<Coin>();
            if(!coin.isPicked)
            {
                coin.isPicked = true;
                coin.transform.rotation = Quaternion.identity;
                coin.Picked();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.transform.tag == "Switch")
        {
            Debug.Log("Switch");
            if(transform.position.x > collider.transform.position.x)
            {
                gameManager.SwitchCamera(false);
            }
            else
            {
                gameManager.SwitchCamera(true);
            }
        }
    }
}
