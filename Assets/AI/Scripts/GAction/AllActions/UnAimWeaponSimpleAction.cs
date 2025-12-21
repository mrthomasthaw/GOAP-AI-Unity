using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnAimWeaponSimpleAction : GAction 
{

	public override void SetUp(BlackBoardManager blackBoardManager)
	{
		base.SetUp(blackBoardManager);

		Preconditions.Add(AIWorldStateKey.HasPrimaryTarget.ToString(), false);
		Preconditions.Add(AIWorldStateKey.AimWeapon.ToString(), true);

		Effects.Add(AIWorldStateKey.AimWeapon.ToString(), false);
	}

	public override void OnActionStart()
	{

	}

	public override bool OnActionPerform()
	{
		return true;
	}
}
