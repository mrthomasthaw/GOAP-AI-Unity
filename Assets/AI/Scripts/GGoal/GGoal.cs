using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GGoal
{
    protected BlackBoardManager blackBoardManager;

    private Dictionary<string, object> endGoal = new Dictionary<string, object>();

    protected int priority;

    public Dictionary<string, object> EndGoal 
    {
        get 
        {
            return endGoal;
        }
        protected set
        {
            endGoal = value;
        }
    }

    public int minPriority;

    public int maxPriority;

    public int Priority
    {
        get { return priority;}
        protected set
        {
            priority = value;
        }
    }

    public virtual void SetUp(BlackBoardManager blackBoardManager)
    {
        this.blackBoardManager = blackBoardManager;
    }

    public abstract int CalculatePriority();
}
