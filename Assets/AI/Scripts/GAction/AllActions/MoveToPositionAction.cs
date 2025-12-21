using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToPositionAction : GAction 
{
    private MoveToPositionData moveToData;

    private NavMeshAgent agent;

    public MoveToPositionAction(NavMeshAgent agent)
    {
        this.agent = agent;
    }

    public override void SetUp(BlackBoardManager blackBoardManager)
    {
        base.SetUp(blackBoardManager);
        Preconditions.Add(AIWorldStateKey.HasMovedToPosition.ToString(), false);

        Effects.Add(AIWorldStateKey.HasMovedToPosition.ToString(), true);
    }

    public override void OnActionStart()
    {
        Debug.Log("blackBoard " + blackBoardManager);
        moveToData = blackBoardManager.GetOneDataByKey<MoveToPositionData>(BlackBoardKey.MoveTo);
        if (moveToData != null && moveToData.IsStillValid) 
        {
            agent.destination = moveToData.Position;
        }
        else
        {
            AbortAction = true;
        }
    }

    public override bool OnActionPerform()
    {
        return HasReachedDestination();
    }

    public override void OnActionComplete()
    {
        if (!moveToData.IsStillValid)
        {
            agent.destination = agent.transform.position;
        }

        moveToData = null;
    }

    private bool HasReachedDestination()
    {
        float dist = Vector3.Distance(agent.transform.position, moveToData.Position);
        if (dist < 0.2f)
            return true;

        return false;
    }
}
