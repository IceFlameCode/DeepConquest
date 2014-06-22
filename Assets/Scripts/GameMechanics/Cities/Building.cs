using UnityEngine;
using System.Collections;

public class Building {

	public BuildingType type = BuildingType.EmptySpace;
	public int Level = 0;
	public float WorkersAtWork = 0;
	public float BuildingFinishMoment = 0f;
	public GameObject BuiltObject;
	public bool isBeingBuilt = false;

	public int x;
	public int y;

	public Building() {
	}

	public string Encode() {
		string s = "";
		switch(type) {
		case BuildingType.EmptySpace:
			s += "0";
			break;
		case BuildingType.Barracks:
			s += "A";
			break;
		case BuildingType.IronMine:
			s += "B";
			break;
		default:
			s += "-";
			break;
		}

		s += (char)(Level + (int)'a');
		s += (char)((int)WorkersAtWork + (int)'a');

		string bt; // building time
		if (Time.time >= BuildingFinishMoment) {
			bt = "000000";
		} else {
			bt = ((int)(BuildingFinishMoment - Time.time)).ToString();
			string z = "";
			for (int i = 0; i < 6 - bt.Length; i++) {
				z += "0";
			}
			bt = z + bt;
		}
		s += bt;
		return s;
	}

	public void Decode(string s) {
		switch(s[0]) {
		case '0':
			type = BuildingType.EmptySpace;
			break;
		case 'A':
			type = BuildingType.Barracks;
			break;
		case 'B':
			type = BuildingType.IronMine;
			break;
		default:
			MonoBehaviour.print("Unknown building type code: " + s[0]);
			break;
		}

		Level = (int)s[1] - (int)'a';
		WorkersAtWork = (int)s[2] - (int)'a';

		string bt = s.Substring(3, 6);
		BuildingFinishMoment = Time.time + float.Parse(bt);

		if (BuildingFinishMoment > Time.time) {
			isBeingBuilt = true;
		} else {
			isBeingBuilt = false;
		}
	}
}
