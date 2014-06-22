using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatUnit {

	public CombatUnitType Type;
	public int ID;

	public bool isBeingTrained = false;
	public float TrainingFinishTime = 0f;

	public float Health = 1f;
	public float AttackDamage = 1f;
	public float MovementSpeed = 1f;
	public float AttackTime = 1f;
	public float Weight = 1f; // the actual weight, adding in carried resources and other goods

	public float MaxCarriedWeight = 1f;
	public List<CombatUnit> CarriedUnits; // if our unit is put inside a different unit, all units onboard are to be put into that unit too, and disembark this one
	public float[] CarriedGoods;

	public CombatUnit() {
		CarriedUnits = new List<CombatUnit>();
		CarriedGoods = new float[(int)ResourceType.Count];
		ID = Random.Range(1, 10000000);
	}
}
