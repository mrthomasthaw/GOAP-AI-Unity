
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{

    private Queue<GAction> actionSequence = new Queue<GAction>();

    private List<GSensor> sensorList = new List<GSensor>();

	private List<GSensorDataProcessor> sensorDataProcessorList = new List<GSensorDataProcessor> ();

    private GPlanner planner;

    private GAction currentAction;

    private GWorldState agentWorldState;

    private BlackBoardManager blackBoardManager;

    private Transform headT;

    private LayerMask obstacleLayer;

    private bool Replan;

    private float timer;

	private float sensorDataProcessorCountDownTimer;

	[SerializeField]
	private Rigidbody bulletPrefab;

	[SerializeField]
	private Transform gunBarrelT;

    private NavMeshAgent navAgent;

    private void Start()
    {
        SetUpWorldStates();

        blackBoardManager = new BlackBoardManager();

		VisionSensor visionSensor = new VisionSensor(transform, 
            transform, blackBoardManager, obstacleLayer, agentWorldState);
		
        sensorList = new List<GSensor>
        {
            visionSensor
        };

        sensorList.ForEach(s  => s.SetUp());

		sensorDataProcessorList = new List<GSensorDataProcessor>
		{ 
			new PrimaryThreatSelectionDataProcessor(blackBoardManager, agentWorldState)
		};

        AIWeaponControl weaponControl = GetComponent<AIWeaponControl>();
        AIAnimationControl animationControl = GetComponent<AIAnimationControl>();
        
        IdleAction idleAction = new IdleAction(weaponControl);
        AimWeaponSimpleAction aimWeaponAction = new AimWeaponSimpleAction(transform);
		FireWeaponSimpleAction fireWeaponAction = new FireWeaponSimpleAction(gunBarrelT, bulletPrefab, transform);
		UnAimWeaponSimpleAction unAimWeaponAction = new UnAimWeaponSimpleAction();
        MoveToPositionAction moveToAction = new MoveToPositionAction(GetComponent<NavMeshAgent>());
        SearchLastKnownThreatPositionAction searchLastKnownThreatAction = new SearchLastKnownThreatPositionAction();

        List<GAction> actionList = new List<GAction>
        {
            idleAction,
            aimWeaponAction,
            unAimWeaponAction,
            fireWeaponAction,
            moveToAction,
            searchLastKnownThreatAction
        };

        currentAction = actionList.ToArray()[0];


        List<GGoal> goalList = new List<GGoal>
        {
			new IdleGoal(),
			new EliminateThreatGoal(),
			new SearchLostThreatGoal()
        };

        planner = new GPlanner(agentWorldState, goalList, actionList, blackBoardManager);

    }

    private void SetUpWorldStates()
    {
        agentWorldState = new GWorldState();
        agentWorldState.Add(AIWorldStateKey.HasPrimaryTarget.ToString(), false);
        agentWorldState.Add(AIWorldStateKey.AimWeapon.ToString(), false);
        agentWorldState.Add(AIWorldStateKey.HasMovedToPosition.ToString(), false);
        agentWorldState.Add(AIWorldStateKey.LastKnownPositionInvestigated.ToString(), false);
    }

    void Update()
    {
        Debug.Log("current action : " + currentAction);


        Debug.Log("WorldState : " + agentWorldState.PrintWorldStates());
        if (Replan)
        {
            Debug.Log("Replan");

            ResetWorldStates();

            UpdateSensors(); // Update replan

			UpdateSensorDataProcessors ();

            //The sensors should be updated before calculating the goal
            planner.CalculateGoalPriority();

            actionSequence = planner.CalculateActionPlan();
            Debug.Log("New plan : " + CommonUtil.StringJoin(actionSequence));

            if (currentAction != null)
                currentAction.AbortAction = true;

            Replan = false; // reset replan
        }

        planner.PrintCurrentGoal();

        planner.PrintAllGoal();


        UpdateSensors(); // Update replan

		UpdateSensorDataProcessors ();

        //UpdateSystems();

        UpdateActionSequence(); // use replan

        //states.ForEach(s => s.OnUpdate(Blackboard));

    }

    private void ResetWorldStates()
    {
        agentWorldState.Set(AIWorldStateKey.HasMovedToPosition.ToString(), false);
        agentWorldState.Set(AIWorldStateKey.LastKnownPositionInvestigated.ToString(), false);
    }

    private void UpdateActionSequence()
    {

        //NOTE NEED TO UPDATE WORLD STATES HERE

        if (currentAction == null)
        {
            Debug.Log("Assign action");
            if (actionSequence.Count == 0)
            {
                Debug.Log("No action to execute");
                Replan = true;
                return;
            }

            if (actionSequence.Peek().RepeatAction)
            {
                currentAction = actionSequence.Peek();
            }
            else
            {
                currentAction = actionSequence.Dequeue();
            }

            currentAction.OnActionStart();
            if (!currentAction.AbortAction)
            {
                //states.ForEach(s => s.OnActionActivate());
            }
        }
        else
        {
            if (currentAction.AbortAction)
            {
                Debug.Log("Abort action");
                //Plan.Clear();
                ExitCurrentAction(currentAction.AbortAction);
                //Stop the whole action sequence
            }
            else if (currentAction.OnActionPerform()) // is the action completed
            {
                Debug.Log("On action complete");
                if (currentAction.RequiredStatesToComplete)
                {
                    bool allStateComplete = AllStateComplete();

                    if (!allStateComplete)
                        return;
                }


                Debug.Log("All state completed");
                ExitCurrentAction(currentAction.AbortAction);
            }

            //if current action is repeatable, it should update the worldstates
        }

        Debug.Log(agentWorldState.PrintWorldStates());
        //NOTE NEED TO UPDATE WORLD STATES HERE

    }

    private void UpdateSensors()
    {
        if (timer <= 0)
        {
            sensorList.ForEach(x => x.OnUpdate());
            timer = 2f;
        }

        timer -= Time.deltaTime;
    }

	private void UpdateSensorDataProcessors()
	{
		if (sensorDataProcessorCountDownTimer <= 0) 
		{
			sensorDataProcessorList.ForEach (x => x.OnUpdate ());
			sensorDataProcessorCountDownTimer = 2.5f;
		}

		sensorDataProcessorCountDownTimer -= Time.deltaTime;
	}

    private void ExitCurrentAction(bool abortAction)
    {
        currentAction.OnActionComplete(); // when the action is completed

        if(! abortAction) // if not end with prematurely
            agentWorldState.CopyWorldStates(currentAction.Effects);

        currentAction = null;

        //foreach (AIStateSystem state in states)
        //{
        //    state.OnActionExit(Blackboard);
        //}
    }


    private bool AllStateComplete()
    {
        bool allStateComplete = true;
        //foreach (AIStateSystem state in states)
        //{
        //    if (!state.HasStateFinished())
        //    {
        //        allStateComplete = false;
        //        break;
        //    }
        //}

        return allStateComplete;
    }
}
