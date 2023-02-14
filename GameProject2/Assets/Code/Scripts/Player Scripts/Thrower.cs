using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class Thrower : NetworkBehaviour
{
    [SerializeField] private Throwable healingThrowable;
    // if less than 0, then no throwable is selected
    [SerializeField] private int selected = 0;
    [SerializeField] private float range = 10.0f;
    [SerializeField] private float cooldown = 20.0f;

    [SerializeField] private LineRenderer lineRenderer;

    private const int linePoints = 25;

    private bool aiming = false;

    private Vector3 target;

    private List<Vector3> path;

    private Vector2 mousePosition = new Vector2(0.0f, 0.0f);
    private float lastThrown;

    private void Awake()
    {
        lastThrown = -cooldown;

        RemoveLine();
    }

    public void OnMouse(InputAction.CallbackContext mouseContext)
    {
        if (selected < 0 && !aiming) return;

        if (mouseContext.started)
        {
            if (lastThrown + cooldown >= Time.time) return;
            aiming = true;
        }
        else if (mouseContext.canceled)
        {
            aiming = false;
            Throw();
        }
    }

    public void OnLook(InputAction.CallbackContext lookContext)
    {
        mousePosition = lookContext.ReadValue<Vector2>();
    }

    private void Throw()
    {
        if (path == null) return;

        CMDThrow(target);
        RemoveLine();
    }

    [Command] // Spawns it on server and then forces that onto the clients.
    void CMDThrow(Vector3 target)
    {
        if (lastThrown + cooldown >= Time.time) return;
        lastThrown = Time.time;

        var distance = Vector3.Distance(target, transform.position);

        var tSize = 1.0f / (float)linePoints;

        var path = new List<Vector3>();

        for (int i = 0; i < linePoints; i++)
        {
            float t = tSize * (float)(i + 1);
            var point = SampleParabola(transform.position, target, distance / 3, t);
            path.Add(point);
        }

        var obj = Instantiate(healingThrowable, path[0], Quaternion.identity);
        obj.path = path;

        NetworkServer.Spawn(obj.gameObject);
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

        path = new List<Vector3>();

        for (int i = 0; i < linePoints; i++)
        {
            float t = tSize * (float)(i + 1);
            var point = SampleParabola(start, end, distance / 3, t);
            lineRenderer.SetPosition(i, point);
            path.Add(point);
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
        path = null;
    }

    private Vector3? CalcTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        RaycastHit hit;
        Vector3 target;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            target = hit.point;
        }
        else return null;

        var currentPosition = transform.position;
        var distance = Mathf.Abs(Vector3.Distance(currentPosition, target));

        if (distance <= range)
        {
            if (distance < 2f) return null;

            return target;
        }

        var direction = (target - currentPosition).normalized;
        var newTarget = currentPosition + (direction * range);

        newTarget.y += 2.0f;

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
