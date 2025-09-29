using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloatEmoji : MonoBehaviour
{
    private ParticleSystem currentParticle;
    [SerializeField] private float height;
    [SerializeField] private float duration;

    private void Start()
    {
        transform.TryGetComponent(out currentParticle);
        currentParticle.Pause();
    }

    public void OnFloatEmoji()
    {
        currentParticle.Play();
        transform.DOLocalMove(transform.localPosition + Vector3.up * height, duration);
    }
}
