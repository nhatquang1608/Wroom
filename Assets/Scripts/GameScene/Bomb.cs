using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private bool isTouched;
    private float timeDelay = 1f;
    public float maxDistance = 5f;
    private CarController carController;
    [SerializeField] private GameObject bling;
    public GameObject partical;

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.transform.tag == "Player" && !isTouched)
        {
            Debug.Log(gameObject);
            isTouched = true;
            carController = GameObject.FindGameObjectWithTag("Vehicle").GetComponent<CarController>();
            SoundManager.Instance.PlaySound(SoundManager.Instance.bombCountDownSound);
            StartCoroutine(WaitForExplode());
        }
    }

    private IEnumerator WaitForExplode()
    {
        while(timeDelay > 0)
        {
            timeDelay -= 0.1f;
            bling.SetActive(!bling.activeSelf);
            yield return new WaitForSeconds(0.1f);
        }

        carController.DelayExplode(this);
    }
}
