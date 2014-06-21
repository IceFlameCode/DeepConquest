using UnityEngine;
using System.Collections;

public class CombatUnitData {

	public string Name = "soldier";
	public string Description = "This is a brainless soldier.";

	public float[] TrainingCost;

	public float Health;
	public float AttackDamage;
	public float AttackTime;
	public float MovementSpeed;

	public float Weight;
	public float MaxCarriedWeight;
	public bool CanCarryUnits;

	public CombatUnitData() {
		TrainingCost = new float[(int)ResourceType.Count];
	}
}
