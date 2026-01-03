using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalPositionSensor : GSensor
{
    private Transform agentTransform;
    private BlackBoardManager blackBoardManager;
    private LayerMask tacticalPositionLayer;
    private Collider[] scannedCols = new Collider[50];
    private List<AttackPositionData> tacticalPositionList = new List<AttackPositionData>();

    public TacticalPositionSensor(Transform transform, BlackBoardManager blackBoardManager, LayerMask tacticalPositionLayer)
    {
        this.agentTransform = transform;
        this.blackBoardManager = blackBoardManager;
        this.tacticalPositionLayer = tacticalPositionLayer;
        VisualDebugger.instance.gizmosDrawList.Add(DebugScannedPoint);
    }

    public override void OnUpdate()
    {
        // Scan nearby points

        SelectedThreatInfoData primaryThreatData = blackBoardManager.GetOneDataByKey<SelectedThreatInfoData>(BlackBoardKey.SelectedPrimaryThreat);
        if (primaryThreatData == null)
            return;
        //Collider[] pointCols = new Collider[20];
        //Physics.OverlapSphereNonAlloc(agentTransform.position, 10f, scannedCols, LayerMask.GetMask("TacticalPoint"));
        scannedCols = Physics.OverlapSphere(primaryThreatData.ThreatTransform.position, 10f, LayerMask.GetMask("TacticalPoint"));
        // Evaluate points

        tacticalPositionList.Clear();
        float minDistToPoint = float.MaxValue;
        for (int x = 0; x < scannedCols.Length; x++)
        {
            Transform colT = scannedCols[x].transform;
            float dist = Vector3.Distance(agentTransform.position, colT.position);
            if (dist < minDistToPoint)
            {
                minDistToPoint = dist;
            }

            if (dist < 8f)
            {
                tacticalPositionList.Add(new AttackPositionData(){
                    Position = colT.position
                });
            }

        }
    }

    private void DebugScannedPoint()
    {
        for (int x = 0; x < scannedCols.Length; x++)
        {
            if (scannedCols[x] == null)
                continue;
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(scannedCols[x].transform.position, new Vector3(0.5f, 0.5f, 0.5f));
        }    

        for (int x = 0; x < tacticalPositionList.Count; x++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(tacticalPositionList[x].Position, new Vector3(0.5f, 0.5f, 0.5f));
        }    
    }

}
