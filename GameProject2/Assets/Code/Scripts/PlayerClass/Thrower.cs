using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Thrower : MonoBehaviour
{
	[SerializeField] private List<Throwable> throwables;
	// if less than 0, then no throwable is selected
	[SerializeField] private int selected = 0;
	[SerializeField] private float range = 10.0f;
	[SerializeField] private float cooldown = 30.0f;

	[SerializeField] private LineRenderer lineRenderer;

	private const int linePoints = 25;

	private bool aiming = false;

	private Vector3 target;

	private List<Vector3> path;

	private void Awake()
	{
		RemoveLine();

		path = new List<Vector3>(linePoints);

		for (int i = 0; i < linePoints; i++)
		{
			path.Add(Vector3.zero);
		}
	}

	public void OnMouse(InputAction.CallbackContext mouseContext)
	{
		if (selected < 0 && !aiming) return;
		if (!mouseContext.performed) return;

		if (mouseContext.ReadValue<float>() > 0.5f)
		{ // Pressed
			aiming = true;
		}
		else
		{ // Released
			aiming = false;
			Throw();
		}
	}

	private void Throw()
	{
		RemoveLine();

		Instantiate(throwables[selected], path[0], Quaternion.identity).GetComponent<Throwable>().path = path;
	}

	private void Update()
	{
		if (!aiming) return;

		var nullableTarget = CalcTarget();
		if (nullableTarget == null)
		{
			RemoveLine();
			return;
		}
		else
		{
			target = nullableTarget.Value;
		}

		var position = transform.position;

		DrawLine(position, target);
	}

	private void DrawLine(Vector3 start, Vector3 end)
	{
		lineRenderer.enabled = true;
		lineRenderer.positionCount = linePoints;

		var distance = Vector3.Distance(start, end);

		var tSize = 1.0f / (float)linePoints;

		for (int i = 0; i < linePoints; i++)
		{
			float t = tSize * (float)i;
			var point = SampleParabola(start, end, distance / 3, t);
			lineRenderer.SetPosition(i, point);
			path[i] = point;
		}
	}

	private Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t)
	{ // https://forum.unity.com/threads/generating-dynamic-parabola.211681/#post-1426169
		float parabolicT = t * 2 - 1;
		if (Mathf.Abs(start.y - end.y) < 0.1f)
		{ // Start and end are roughly level, pretend they are - simpler solution with less steps
			var travelDirection = end - start;
			var result = start + t * travelDirection;
			result.y += (-parabolicT * parabolicT + 1) * height;
			return result;
		}
		else
		{ //start and end are not level, gets more complicated
			var travelDirection = end - start;
			var levelDirection = end - new Vector3(start.x, end.y, start.z);
			var right = Vector3.Cross(travelDirection, levelDirection);
			var up = Vector3.Cross(right, travelDirection);
			if (end.y > start.y) up = -up;
			var result = start + t * travelDirection;
			result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
			return result;
		}
	}

	private void RemoveLine()
	{
		lineRenderer.enabled = false;
	}

	private Vector3? CalcTarget()
	{
		var mousePos = Input.mousePosition;

		Ray ray = Camera.main.ScreenPointToRay(mousePos);

		RaycastHit hit;
		Vector3 target;
		if (Physics.Raycast(ray, out hit, Mathf.Infinity))
		{
			target = hit.point;
		}
		else return null;

		var currentPosition = transform.position;
		var distance = Mathf.Abs(Vector3.Distance(currentPosition, target));

		if (distance <= range)
		{
			if (distance < 1f) return null;

			return target;
		}

		var direction = (target - currentPosition).normalized;
		var newTarget = currentPosition + (direction * range);

		RaycastHit hitUp;
		bool up = Physics.Raycast(newTarget, Vector3.up, out hitUp, Mathf.Infinity);

		RaycastHit hitDn;
		bool dn = Physics.Raycast(newTarget, Vector3.down, out hitDn, Mathf.Infinity);

		if (up && dn)
		{
			if (Vector3.Distance(newTarget, hitUp.point) < Vector3.Distance(newTarget, hitDn.point)) return hitUp.point;
			else return hitDn.point;
		}
		else if (up) return hitUp.point;
		else if (dn) return hitDn.point;
		else return null;
	}
}
