using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWeaponSimpleAction : GAction 
{
	private SelectedThreatInfoData selectedThreatInfoData;
	private Transform agentTransform;

	public AimWeaponSimpleAction(Transform agentTransform) 
	{
		this.agentTransform = agentTransform; 
	}

	public override void SetUp(BlackBoardManager blackBoardManager)
	{
		base.SetUp(blackBoardManager);
		Preconditions.Add(AIWorldStateKey.HasPrimaryTarget.ToString(), true);
		Preconditions.Add(AIWorldStateKey.AimWeapon.ToString(), false);
		RequiredStatesToComplete = true;

		Effects.Add(AIWorldStateKey.AimWeapon.ToString(), true);
	}

	public override void OnActionStart()
	{
		Debug.Log("blackBoard " + blackBoardManager);
		selectedThreatInfoData = blackBoardManager.GetOneDataByKey<SelectedThreatInfoData>(BlackBoardKey.SelectedPrimaryThreat);
		if (selectedThreatInfoData != null && selectedThreatInfoData.IsStillValid) 
		{
			Aim ();
		}
		else
		{
			AbortAction = true;
		}
	}

	public override bool OnActionPerform()
	{
		return true;
	}

	public override void OnActionComplete()
	{
		if (!selectedThreatInfoData.IsStillValid)
		{
			//Un aim
		}

		selectedThreatInfoData = null;
	}

	private void Aim() 
	{
		Vector3 direction = selectedThreatInfoData.ThreatTransform.position - agentTransform.position;
		direction.Normalize ();

		Quaternion lookDir = Quaternion.LookRotation (direction);
		lookDir.x = 0;
		lookDir.z = 0;
		Quaternion smoothRotation = Quaternion.Lerp (agentTransform.rotation, lookDir, Time.deltaTime * 20f);
		agentTransform.rotation = smoothRotation;
	}
}
