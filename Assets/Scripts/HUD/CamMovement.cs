using UnityEngine;
using System.Collections;

public class CamMovement : MonoBehaviour {
	
	public float MovementSpeed = 10f;

	Vector3 center = new Vector3(S.CitySize * 0.5f - 0.5f, 0f, S.CitySize * 0.5f - 0.5f);
	Vector3 tempAxis;
	Vector3 tempAxis2;
	bool prevMenu = true;
	Vector3 startPos;
	Quaternion startRot;

	void Start () {
		startPos = transform.position;
		startRot = transform.rotation;
		transform.rotation = Random.rotation;
		tempAxis = Random.onUnitSphere;
		tempAxis2 = Random.onUnitSphere;
		DontDestroyOnLoad(transform.gameObject);
	}

	void Update () {
		if (Application.loadedLevel == 1) {
			if (prevMenu) {
				transform.position = startPos;
				transform.rotation = startRot;
				prevMenu = false;
			}
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
				transform.RotateAround(center, Vector3.up, MovementSpeed * Time.deltaTime);
			} else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
				transform.RotateAround(center, Vector3.up, -MovementSpeed * Time.deltaTime);
			}
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
				if (transform.position.y < 9f)
					transform.RotateAround(center, Vector3.Cross(transform.position - center, Vector3.up).normalized, MovementSpeed * Time.deltaTime);
			} else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
				if (transform.position.y > 1f)
					transform.RotateAround(center, Vector3.Cross(transform.position - center, Vector3.up).normalized, -MovementSpeed * Time.deltaTime);
			}
			transform.LookAt(center);
		} else {
			transform.RotateAround(center, tempAxis, MovementSpeed * Time.deltaTime);
			tempAxis = Quaternion.AngleAxis(MovementSpeed * Time.deltaTime, tempAxis2) * tempAxis;
			tempAxis2 = Quaternion.AngleAxis(MovementSpeed * Time.deltaTime, transform.forward) * tempAxis2;
		}
	}
}
