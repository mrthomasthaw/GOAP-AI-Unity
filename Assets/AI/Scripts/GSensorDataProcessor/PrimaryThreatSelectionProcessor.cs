using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrimaryThreatSelectionDataProcessor : GSensorDataProcessor
{
    private BlackBoardManager blackBoardManager;

    private GWorldState agentWorldState;

    private SelectedThreatInfoData selectedThreat;

    private Vector3 lastknownPositionDebug;



	public PrimaryThreatSelectionDataProcessor (BlackBoardManager blackBoardManager, GWorldState agentWorldState) 
	{
		this.blackBoardManager = blackBoardManager;
		this.agentWorldState = agentWorldState;

        VisualDebugger.instance.gizmosDrawList.Add(DrawGizmos);
	}

	public override void OnUpdate()
    {
		Debug.Log ("Updateing threat selection ");
        List<KnownThreatInfoData> knownThreatList = blackBoardManager.GetAllDataByKey<KnownThreatInfoData>
            (BlackBoardKey.KnownThreatInfo);

        if (knownThreatList.Count > 0)
        {
            knownThreatList = knownThreatList.OrderByDescending(t => t.Score).ToList();

            KnownThreatInfoData highPriorityThreat = knownThreatList[0];

            selectedThreat = new SelectedThreatInfoData
            {
                HealthControl = highPriorityThreat.HealthControl,
                ThreatTransform = highPriorityThreat.ThreatTransform,
                IsStillValid = highPriorityThreat.IsStillValid,
                Score = highPriorityThreat.Score,
            };

            //It make sure that only one data in the list of blackboard
            //blackBoardManager.RemoveLastData<SelectedThreatInfoData>(BlackBoardKey.SelectedPrimaryThreat);
            blackBoardManager.AddOrReplace<SelectedThreatInfoData>(selectedThreat, BlackBoardKey.SelectedPrimaryThreat);

            agentWorldState.Set(AIWorldStateKey.HasPrimaryTarget.ToString(), true);
        }
        else
        {
			if (selectedThreat != null) 
			{
				blackBoardManager.RemoveLastData<SelectedThreatInfoData>(BlackBoardKey.SelectedPrimaryThreat);
				blackBoardManager.AddOrReplace<LastKnownThreatPositionData>(new LastKnownThreatPositionData()
				{
					LastKnownPosition = selectedThreat.ThreatTransform.position
				}, BlackBoardKey.LastKnownThreatPosition);
                
                blackBoardManager.AddData<MoveToPositionData>(new MoveToPositionData()
                {
                    Position = selectedThreat.ThreatTransform.position
                }, BlackBoardKey.MoveTo);

                lastknownPositionDebug = selectedThreat.ThreatTransform.position;
				
				agentWorldState.Set(AIWorldStateKey.HasPrimaryTarget.ToString(), false);
				selectedThreat = null;
			} 
        }
    }

    private void DrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(lastknownPositionDebug, 0.5f);
    }

}
