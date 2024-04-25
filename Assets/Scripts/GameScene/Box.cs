using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Box : MonoBehaviour
{
    public bool isTouched;
    [SerializeField] private Vector3 initPosition;
    [SerializeField] private Vector3 endPoint = new Vector3(0, 0, 270);
    [SerializeField] private GameObject particle;
    [SerializeField] private GameManager gameManager;
    private Tween moveTween;

    private void Start() 
    {
        initPosition = transform.position;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Animate();
    }

    private void Animate()
    {
        moveTween = transform.DOMoveY(transform.position.y + 1, 1).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }

    public void KillAnimation()
    {
        moveTween.Kill();
    }

    public async void Touched()
    {
        KillAnimation();
        await TouchedAnimation();
    }

    private async UniTask TouchedAnimation()
    {
        GameObject effect = Instantiate(particle, initPosition, Quaternion.identity);
        transform.position = initPosition;
        transform.DORotate(endPoint, 1f);
        transform.DOScale(0, 1);

        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        Destroy(effect);
        Destroy(gameObject);
    }
}
