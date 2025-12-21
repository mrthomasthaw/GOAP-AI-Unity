using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastKnownThreatPositionData : BlackBoardData
{
    public override BlackBoardKey BlackBoardKey
    {
        get {return BlackBoardKey.LastKnownThreatPosition;}
    }

    public Vector3 LastKnownPosition { get; set; }

    public bool Investigated { get; set; } // has the area been checked
}
