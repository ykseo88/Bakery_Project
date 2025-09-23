using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StackableObjectAccess
{
    public EStackableObjects stackableObjectType;
    public EStackableType[] inputAbleContainers;
    public EStackableType[] outputAbleContainers;

    public StackableObjectAccess(EStackableObjects SObj, EStackableType[] isIn, EStackableType[] isOut)
    {
        stackableObjectType = SObj;
        inputAbleContainers = isIn;
        outputAbleContainers = isOut;
    }
}
