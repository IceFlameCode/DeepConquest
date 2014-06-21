using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatUnit {

	public CombatUnitType Type;

	public float Health = 1f;
	public float AttackDamage = 1f;
	public float MovementSpeed = 1f;
	public float AttackTime = 1f;

	public float MaxCarriedWeight = 1f;
	public List<CombatUnit> CarriedUnits;
	public float[] CarriedGoods;

	public CombatUnit() {
		CarriedUnits = new List<CombatUnit>();
		CarriedGoods = new float[(int)ResourceType.Count];
	}
}
