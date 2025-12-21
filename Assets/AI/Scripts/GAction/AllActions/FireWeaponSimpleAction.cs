using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeaponSimpleAction : GAction 
{
	private Transform gunBarrelTransform;
	private Rigidbody bulletPrefab;
	private Transform agentTransform;

	public FireWeaponSimpleAction(Transform gunBarrelTransform, Rigidbody bulletPrefab, Transform agentTransform)
	{
		this.gunBarrelTransform = gunBarrelTransform;
		this.bulletPrefab = bulletPrefab;
		this.agentTransform = agentTransform;
	}

	public override void SetUp(BlackBoardManager blackBoardManager)
	{
		base.SetUp(blackBoardManager);
		Preconditions.Add(AIWorldStateKey.HasPrimaryTarget.ToString(), true);
		Preconditions.Add(AIWorldStateKey.AimWeapon.ToString(), true);


		Effects.Add(AIWorldStateKey.AssaultTarget.ToString(), true);
	}

	public override bool OnActionPerform()
	{
		SelectedThreatInfoData selectedThreatInfoData = blackBoardManager.GetOneDataByKey<SelectedThreatInfoData>(BlackBoardKey.SelectedPrimaryThreat);
		if (selectedThreatInfoData != null && selectedThreatInfoData.IsStillValid)
		{
			Aim (selectedThreatInfoData);
			Shoot ();
		}

		return selectedThreatInfoData == null || ! selectedThreatInfoData.IsStillValid;
	}

	public override void OnActionComplete()
	{
		
	}


	private void Shoot()
	{
		Rigidbody bullet = Object.Instantiate (bulletPrefab, gunBarrelTransform.position, gunBarrelTransform.rotation) as Rigidbody;
		bullet.velocity = gunBarrelTransform.forward * 100f;
	}

	private void Aim(SelectedThreatInfoData selectedThreatInfoData) 
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
