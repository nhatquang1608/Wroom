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
    [SerializeField] private GameObject particle;
    private Tween moveTween;

    private void Start() 
    {
        initPosition = transform.position;
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
        await TouchedAnimation();
    }

    private async UniTask TouchedAnimation()
    {
        KillAnimation();
        GameObject effect = Instantiate(particle, initPosition, Quaternion.identity);
        transform.position = initPosition;
        transform.DORotate(new Vector3(0f, 0f, 180f), 1f);
        transform.DOScale(0f, 1f);

        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        Destroy(effect);
        Destroy(gameObject);
    }
}
