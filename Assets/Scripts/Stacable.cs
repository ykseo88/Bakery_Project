using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stacable : MonoBehaviour
{
    [SerializeField] private int maxStacNum;
    [SerializeField] private Stack<GameObject> currentStack = new Stack<GameObject>();

    public bool isHasStack = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Stack<GameObject> GetCurrentStack()
    {
        return currentStack;
    }
    
}
