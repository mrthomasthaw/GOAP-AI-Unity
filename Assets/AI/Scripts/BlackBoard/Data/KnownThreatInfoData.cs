using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnownThreatInfoData : BlackBoardData
{
    public override BlackBoardKey BlackBoardKey 
    {
        get { return BlackBoardKey.KnownThreatInfo; }
    }
    
    public Transform ThreatTransform {  get; set; }
    public HealthControl HealthControl {  get; set; }
    public Vector3 LastKnownPosition { get; set; }

    public override string ToString()
    {
		return string.Format(
			"{{ BlackBoardKey = {0}, ThreatTransform = {1}, HealthControl = {2} }}",
			BlackBoardKey,
			ThreatTransform,
			HealthControl
		);
    }


}
