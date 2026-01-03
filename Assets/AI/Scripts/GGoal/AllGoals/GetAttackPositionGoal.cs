using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAttackPositionGoal : GGoal 
{
    public override void SetUp(BlackBoardManager blackBoard)
    {
        base.SetUp(blackBoard);



        EndGoal.Add(AIWorldStateKey.AttackFromAPosition.ToString(), true);
    }

    public override int CalculatePriority()
    {
        Priority = 0;
        SelectedThreatInfoData primaryThreat = blackBoardManager.GetOneDataByKey<SelectedThreatInfoData>(BlackBoardKey.SelectedPrimaryThreat);

        if (primaryThreat != null && primaryThreat.IsStillValid)
        {
            Priority = 10;
        }

        ThreatAssessmentData threatAssessment = blackBoardManager.GetOneDataByKey<ThreatAssessmentData>(BlackBoardKey.ThreatAssessmentInfo);

        if (threatAssessment != null)
        {
            if (threatAssessment.threatLevel == ThreatAssessmentData.ThreatLevel.Normal)
            {
                Priority += 5;
            }
            else if (threatAssessment.threatLevel == ThreatAssessmentData.ThreatLevel.Weak)
            {
                Priority += 10;
            }
        }

        //Priority = Mathf.Clamp(Priority, minPriority, maxPriority);
        return Priority;
    }

    public override string ToString()
    {
        return string.Format("[GetAttackPositionGoal] : priority={0}", Priority);
    }
}
