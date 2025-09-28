using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBag : StackableObject
{
    private static readonly int CLOSE = Animator.StringToHash("Close");
    private static readonly int RESET = Animator.StringToHash("Reset");


    private Animator animator;
    public StackContainer stackContainer;
    private bool isGetAllBread = false;
    public bool IsGetAllBread => isGetAllBread;

    protected override void Start()
    {
        base.Start();
        transform.TryGetComponent(out animator);
        transform.TryGetComponent(out stackContainer);
    }

    public bool CheckGetAllBread()
    {
        return isGetAllBread;
    }

    private void SetGetAllBread()
    {
        isGetAllBread = true;
    }

    public void SetClose()
    {
        animator.SetTrigger(CLOSE);
    }

    private void OnDisable()
    {
        isGetAllBread = false;
        animator.ResetTrigger(CLOSE);
        animator.SetTrigger(RESET);
    }
    
    
}
