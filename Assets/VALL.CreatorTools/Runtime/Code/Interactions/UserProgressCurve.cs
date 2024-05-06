using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Vall.Events;

public class UserProgressCurve : MonoBehaviour {
	public Transform start, end;
	public FloatEvent OnProgressUpdated;


	private void Update() {
		OnProgressUpdated?.Invoke(GetProgress());
	}
	public float GetProgress() {
		Vector3 v = Vector3.Project(Camera.main.transform.position - start.position, end.position - start.position);
		float g = v.magnitude / (end.position - start.position).magnitude;
		return Mathf.Clamp(g, 0, 1);
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.white;
		Gizmos.DrawLine(start.position, end.position);
		float prog = GetProgress();
		Gizmos.color = Color.Lerp(Color.red, Color.green, prog);
		Gizmos.DrawLine(start.position, start.position + (end.position - start.position) * prog);

		Vector3 v = Vector3.Project(Camera.main.transform.position - start.position, end.position - start.position);
		v += start.position;

		Gizmos.DrawLine(v, Camera.main.transform.position);
		Gizmos.color = Color.white;
	}
}
