using UnityEngine;
using System.Collections;

public class SkyboxRotation : MonoBehaviour {

	public float RotationSpeed = 0.3f;

	Vector3 axis;

	void Start () {
		axis = Random.onUnitSphere;
	}

	void Update () {
		transform.localRotation *= Quaternion.AngleAxis(RotationSpeed * Time.deltaTime, axis);
	}
}
