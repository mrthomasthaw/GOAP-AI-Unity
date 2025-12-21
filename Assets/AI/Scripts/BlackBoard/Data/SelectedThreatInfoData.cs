using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedThreatInfoData : BlackBoardData
{
    public override BlackBoardKey BlackBoardKey
    {
        get { return BlackBoardKey.SelectedPrimaryThreat; }
    }

    public HealthControl HealthControl { get; set; }

    public Transform ThreatTransform { get; set; }

}
