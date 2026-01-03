using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreatAssessmentData : BlackBoardData 
{
    public override BlackBoardKey BlackBoardKey
    {
        get { return BlackBoardKey.ThreatAssessmentInfo; }
    }

    public enum ThreatLevel
    {
        Strong, Normal, Weak
    }

    public ThreatLevel threatLevel { get; set;}
}
