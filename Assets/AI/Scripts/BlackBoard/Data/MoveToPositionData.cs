using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPositionData : BlackBoardData 
{
    public override BlackBoardKey BlackBoardKey
    {
        get
        {
            return BlackBoardKey.MoveTo;
        }
    }

    public Vector3 Position { get; set;}

    public MoveToPositionData() {
        IsStillValid = true;
    }
}
