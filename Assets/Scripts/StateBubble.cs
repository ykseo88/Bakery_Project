using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBubble : MonoBehaviour
{
    // Start is called before the first frame update

    private Transform mainCameraTransform;
    void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = mainCameraTransform.rotation;
    }
}
