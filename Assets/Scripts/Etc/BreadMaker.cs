using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadMaker : MonoBehaviour
{
    [SerializeField] private GameObject breadPrefab;
    [SerializeField] private float outPutTerm;
    [SerializeField] private float outPutPower;
    [SerializeField] private Transform bakeTransform;
    [SerializeField] private int MaxBakedAmount;
    private Rigidbody rigidbody;
    private StackContainer stackContainer;
    
    
    private bool isBakealbe = true;
    private int currentBakedAmount = 0;
    
    private void Start()
    {
        transform.TryGetComponent(out stackContainer);
    }

    private void Update()
    {
        currentBakedAmount = stackContainer.GetCurrentStack().Count;
        if(isBakealbe && MaxBakedAmount > currentBakedAmount) MakeBread();
    }

    private void MakeBread()
    {
        isBakealbe = false;
        currentBakedAmount++;
        GameObject tempBread = PoolManager.instance.ActiveObject(breadPrefab, bakeTransform.position, bakeTransform.rotation);
        tempBread.transform.TryGetComponent(out rigidbody);
        tempBread.transform.TryGetComponent(out Bread bread);
        bread.breadMaker = this;
        rigidbody.isKinematic = true;
        StartCoroutine(OutPutBread(rigidbody));
    }

    private IEnumerator OutPutBread(Rigidbody Breadrigidbody)
    {
        yield return new WaitForSeconds(outPutTerm);
        
        Breadrigidbody.isKinematic = false;
        Breadrigidbody.AddForce(bakeTransform.forward * outPutPower, ForceMode.Impulse);
    }

    public void ActiveBakeable()
    {
        isBakealbe = true;
    }
}
