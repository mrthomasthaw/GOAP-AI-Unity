using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchLastKnownThreatPositionAction : GAction 
{

    public override void SetUp(BlackBoardManager blackBoardManager)
    {
        base.SetUp(blackBoardManager);
        Preconditions.Add(AIWorldStateKey.HasMovedToPosition.ToString(), true);
        Effects.Add(AIWorldStateKey.LastKnownPositionInvestigated.ToString(), true);
    }

    public override bool OnActionPerform()
    {
        return true;
    }
}
