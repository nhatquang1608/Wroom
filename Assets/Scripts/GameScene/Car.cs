using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private Transform box;
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        box = GameObject.Find("Box").transform;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.transform.tag == "Switch")
        {
            Debug.Log("Switch");
            gameManager.isSwitchCamera = !gameManager.isSwitchCamera;
            gameManager.SwitchCamera(gameManager.isSwitchCamera);
        }
    }
}
