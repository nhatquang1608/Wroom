using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public bool isPicked;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private Vector3 endPoint;

    private void Start() 
    {
        endPoint = transform.position + new Vector3(50, 30, 0);
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        while(!isPicked)
        {
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public async void Picked()
    {
        await PickedAnimation();
    }

    private async UniTask PickedAnimation()
    {
        transform.DOMove(endPoint, 1f);
        transform.DOScale(5, 1);

        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        Destroy(gameObject);
    }
}
