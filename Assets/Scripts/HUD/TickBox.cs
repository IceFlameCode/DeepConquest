using UnityEngine;
using System.Collections;

public static class TickBox {

	public static bool Draw (bool value) {
		return GUILayout.Button(value ? "Yes" : "No") ? !value : value;
	}

	public static bool Draw (Rect r, bool value) {
		return GUI.Button(r, value ? "Yes" : "No") ? !value : value;
	}
}
