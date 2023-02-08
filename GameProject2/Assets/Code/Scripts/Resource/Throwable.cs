using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class Throwable : MonoBehaviour
{
	[SerializeField] private float energyConsumption = 1.0f;
	[SerializeField] private float areaRadius = 5f;
	[SerializeField] private float duration = 10.0f;
	[SerializeField] private float actionsPerSecond = 2.0f;
	[SerializeField] private float actionValue = 1.0f;
	[SerializeField] private ThrowableAction type = ThrowableAction.Healing;
	[SerializeField] private Material material;
	[SerializeField] private float moveSpeed = 0.0001f;

	[HideInInspector]
	public List<Vector3> path = new List<Vector3>();

	private const string meshName = "Mesh";
	private const string coloringName = "Coloring";

	private const string playerTag = "Player";
	private const string throwableTag = "Throwable";

	private int current = 0;
	private GameObject mesh;
	private GameObject coloring;

	private Coroutine doAction;
	private Coroutine timer;

	private GameObject[] players;
	private bool moving = true;

	private void Awake()
	{
		mesh = gameObject.transform.Find(meshName).gameObject;
		coloring = gameObject.transform.Find(coloringName).gameObject;

		coloring.GetComponent<MeshRenderer>().material = material;
		coloring.GetComponent<MeshRenderer>().enabled = false;

		UpdateInternalSize();
	}

	private void Start()
	{
		players = GameObject.FindGameObjectsWithTag(playerTag);

		foreach (GameObject throwable in GameObject.FindGameObjectsWithTag(throwableTag))
		{
			if (throwable != this.gameObject)
			{
				throwable.GetComponent<Throwable>().DestroyThrowable();
			}
		}
	}

	private void Update()
	{
		if (!moving) return;
		if (transform.position != path[current])
		{
			var pos = Vector3.MoveTowards(transform.position, path[current], moveSpeed * Time.deltaTime);
			transform.position = pos;
		}
		else
		{
			current++;

			if (current < path.Count) return;

			moving = false;
			Landed();
		}
	}

	private void Landed()
	{
		coloring.GetComponent<MeshRenderer>().enabled = true;
		StartCoroutine(Timer());
		doAction = StartCoroutine(DoAction());
	}

	public void DestroyThrowable()
	{
		// Debug.Log($"Ran out: {this.name}");
		StopCoroutine(doAction);
		Destroy(this.gameObject);
	}

	IEnumerator DoAction()
	{
		var waitTime = 1.0f / actionsPerSecond;

		while (true)
		{
			Action();
			yield return new WaitForSeconds(waitTime);
		}
	}


	private void Action()
	{
		var position = transform.position;

		foreach (var player in players)
		{
			var distance = Mathf.Abs(Vector3.Distance(position, player.transform.position));

			if (distance < areaRadius)
			{
				IThrowableAction playerInterface;
				if (player.TryGetComponent<IThrowableAction>(out playerInterface))
				{
					playerInterface.ThrowAction(type, actionValue);
				}
			}
		}
	}

	IEnumerator Timer()
	{
		yield return new WaitForSeconds(duration);

		DestroyThrowable();
	}

	private void OnRenderObject()
	{
		UpdateInternalSize();
	}

	// Keep the size of the field correct.
	private void UpdateInternalSize()
	{
		var scale = new Vector3(areaRadius * 2, 0.15f, areaRadius * 2);
		coloring.transform.localScale = scale;
	}
}