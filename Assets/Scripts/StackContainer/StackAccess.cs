using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StackAccess
{
    public EStackableType stackContainer;
    public StackableObjectAccess[] stackableObjectAccess;

    public StackAccess(EStackableType stackContainer, StackableObjectAccess[] stackableObjectAccess)
    {
        this.stackContainer = stackContainer;
        this.stackableObjectAccess = stackableObjectAccess;
    }
}
