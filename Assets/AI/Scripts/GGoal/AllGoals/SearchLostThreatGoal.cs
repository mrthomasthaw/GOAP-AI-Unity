using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchLostThreatGoal : GGoal
{
    public override void SetUp(BlackBoardManager blackBoardManager)
    {
        base.SetUp(blackBoardManager);
        EndGoal.Add(AIWorldStateKey.LastKnownPositionInvestigated.ToString(), true);
    }

	public override int CalculatePriority()
	{
		LastKnownThreatPositionData lastKnownPositionData = blackBoardManager.GetOneDataByKey<LastKnownThreatPositionData>(BlackBoardKey.LastKnownThreatPosition);
		if (lastKnownPositionData != null && ! lastKnownPositionData.Investigated) Priority = 2;
		else Priority = 0;

		return Priority;
	}

    public override string ToString()
    {
        return string.Format("[SearchLostThreatGoal] : priority={0}", Priority);
    }
}
