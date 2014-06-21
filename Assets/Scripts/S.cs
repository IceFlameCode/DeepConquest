using UnityEngine;
using System.Collections;

public static class S {

	public static bool ClientBuild = true;
	public static int CitySize = 10;
	public static float ResourceUpdateTime = 36f;

	public static Color FlashyColor {
		get {
			float f = Mathf.PingPong(Time.time, 1f);
			return new Color (f, 1f - f, 1f);
		}
	}

	public static bool NotEmpty (string s) {
		for (int i = 0; i < s.Length; i++)
			if (s[i] != ' ') return true;
		return false;
	}

	// faster than floating point powers
	public static float PowInt (float a, int b) {
		if (b == 0) return 1f;
		return a * PowInt(a, b - 1);
	}
}
