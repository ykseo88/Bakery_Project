using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStay : MonoBehaviour
{
    [SerializeField] private Vector3 stayPosition;
    public bool isStaiable = true;

    private void Start()
    {
        stayPosition = transform.localPosition;
    }

    private void Update()
    {
        StayPos();
    }

    private void StayPos()
    {
        if(isStaiable) transform.localPosition = stayPosition;
    }
    
}
