using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBag : StackableObject
{
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        transform.TryGetComponent(out animator);
    }
}
