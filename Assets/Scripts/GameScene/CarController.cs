using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public int id;
    public static Vector2 moveDirection;
    [SerializeField] private Rigidbody2D frontTireRB;
    [SerializeField] private Rigidbody2D backTireRB;
    [SerializeField] private Rigidbody2D carRB;

    private float moveSpeedChange = 4000f;
    private float maxMoveSpeed = 1000f;

    private float rotateSpeedChange = 9000f;
    private float maxRotateSpeed = 3000f;

    private float force = 1000f;

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
        if(Input.GetKey(KeyCode.W) || moveDirection.x == 1)
        {
            moveSpeed += moveSpeedChange * Time.deltaTime;
            moveSpeed = Mathf.Clamp(moveSpeed, 0f, maxMoveSpeed);
        }
        else if(Input.GetKey(KeyCode.S) || moveDirection.x == -1)
        {
            moveSpeed -= moveSpeedChange * Time.deltaTime;
            moveSpeed = Mathf.Clamp(moveSpeed, -maxMoveSpeed, 0f);
        }
        else
        {
            if(moveSpeed > 0)
            {
                moveSpeed -= moveSpeedChange * Time.deltaTime;
                moveSpeed = Mathf.Max(moveSpeed, 0f);
            }
            else if(moveSpeed < 0)
            {
                moveSpeed += moveSpeedChange * Time.deltaTime;
                moveSpeed = Mathf.Min(moveSpeed, 0f);
            }
        }

        if(Input.GetKey(KeyCode.D) || moveDirection.y == 1)
        {
            rotateSpeed += rotateSpeedChange * Time.deltaTime;
            rotateSpeed = Mathf.Clamp(rotateSpeed, 0f, maxRotateSpeed);
        }
        else if(Input.GetKey(KeyCode.A) || moveDirection.y == -1)
        {
            rotateSpeed -= rotateSpeedChange * Time.deltaTime;
            rotateSpeed = Mathf.Clamp(rotateSpeed, -maxRotateSpeed, 0f);
        }
        else
        {
            if(rotateSpeed > 0)
            {
                rotateSpeed -= rotateSpeedChange * Time.deltaTime;
                rotateSpeed = Mathf.Clamp(rotateSpeed, 0f, maxRotateSpeed);
            }
            else if(rotateSpeed < 0)
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

    public void DelayExplode(Bomb bomb)
    {
        StartCoroutine(WaitForExplode(bomb));
    }

    private IEnumerator WaitForExplode(Bomb bomb)
    {
        GameObject newExplode = Instantiate(bomb.partical, bomb.transform.position, Quaternion.identity);
        if(Vector2.Distance(transform.position, bomb.transform.position) < bomb.maxDistance)
        {
            carRB.AddForce((transform.position - bomb.transform.position).normalized * force);
        }
        SoundManager.Instance.PlaySound(SoundManager.Instance.bombSound);
        Destroy(bomb.gameObject);

        yield return new WaitForSeconds(1);
        Destroy(newExplode);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.transform.tag == "Thorn" && !gameManager.isGameOver)
        {
            Debug.Log(collider.name);
            gameManager.OnGameOver(false);
            box.KillAnimation();
            SoundManager.Instance.PlaySound(SoundManager.Instance.failedSound);
        }
        else if(collider.transform.tag == "Box" && !gameManager.isGameOver)
        {
            Debug.Log(collider.name);
            gameManager.OnGameOver(true);
            if(!box.isTouched)
            {
                box.isTouched = true;
                box.Touched();
                SoundManager.Instance.PlaySound(SoundManager.Instance.completedSound);
            }
        }
        else if(collider.transform.tag == "Increase")
        {
            carRB.AddForce(Vector2.right * force);
            gameManager.AddForce(collider.gameObject, true);
            SoundManager.Instance.PlaySound(SoundManager.Instance.increaseSpeedSound);
        }
        else if(collider.transform.tag == "Decrease")
        {
            carRB.AddForce(-Vector2.right * force);
            gameManager.AddForce(collider.gameObject, false);
            SoundManager.Instance.PlaySound(SoundManager.Instance.decreaseSpeedSound);
        }
        else if(collider.transform.tag == "Coin")
        {
            Coin coin = collider.GetComponent<Coin>();
            if(!coin.isPicked)
            {
                coin.isPicked = true;
                coin.transform.rotation = Quaternion.identity;
                coin.Picked();
                SaveLoadData.Instance.playerInfo.coins ++;
                if(SaveLoadData.Instance.playerInfo.coins > 999) SaveLoadData.Instance.playerInfo.coins = 999;
                SaveLoadData.Instance.SaveData();
                gameManager.SetCoinText();
                SoundManager.Instance.PlaySound(SoundManager.Instance.coinSound);
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
