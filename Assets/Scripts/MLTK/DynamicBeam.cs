using System.Collections;
using System.Collections.Generic;
using MagicLeap;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

[RequireComponent(typeof(LineRenderer))]
public class DynamicBeam : MonoBehaviour
{
    private MLInput.Controller controller;
    private LineRenderer beamLine;
    private readonly List<LinePoint> linePoints = new List<LinePoint>();
    [SerializeField] private LayerMask interactionLayerMask;
    [SerializeField] private float maxLineLength = 10;
    [SerializeField] private int maxSegmentCount = 1;
    [SerializeField] private Color startColor = Color.blue;
    [SerializeField] private Color endColor = Color.magenta;
    [SerializeField] private float turnOffPeriod;
    [SerializeField] private float drawLineMovementControllerThreshold = .05f;
    private Vector3 previousControllerPosition;
    private float timer;

    private void Start()
    {
        controller = MLInput.GetController(MLInput.Hand.Left);
        beamLine = GetComponent<LineRenderer>();
        beamLine.startColor = startColor;
        beamLine.endColor = endColor;
    }

    private void Update()
    {
        UpdateBeam();
    }

    private void GetLinePoints(List<LinePoint> linePoints, out RaycastHit hit)
    {
        hit = new RaycastHit();
        linePoints.Clear();
        var singleLength = maxLineLength / maxSegmentCount;
        linePoints.Add(new LinePoint(transform.position, hit));

        for (var i = 0; i < maxSegmentCount; i++)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, singleLength, interactionLayerMask))
            {
                linePoints.Add(new LinePoint(hit.point, hit));
                break;
            }
            else
            {
                var currentPointPosition = linePoints[linePoints.Count - 1].Position + transform.forward * singleLength;
                linePoints.Add(new LinePoint(currentPointPosition, hit));
            }

        }
    }

    private void UpdateBeam()
    {

        if (Vector3.Distance(previousControllerPosition, transform.position) <
            drawLineMovementControllerThreshold)
        {
            if (timer <= 0)
            {
                beamLine.enabled = false;
                timer = 0;
            }

            timer -= Time.deltaTime;
        }
        else
        {
            beamLine.enabled = true;
            timer = turnOffPeriod;
        }

        if (beamLine.enabled)
        {
            GetLinePoints(linePoints, out _);
            DrawLine();
        }

        previousControllerPosition = transform.position;
    }

    private void DrawLine()
    {
        if (linePoints.Count < 2)
        {
            return;
        }

        if (beamLine == null)
        {
            beamLine = GetComponent<LineRenderer>();
        }
        for (var i = 0; i < linePoints.Count; i++)
        {
            beamLine.SetPosition(i, linePoints[i].Position);
        }
    }

    public struct LinePoint
    {
        public LinePoint(Vector3 position, RaycastHit hit)
        {
            Position = position;
            Hit = hit;
        }

        public RaycastHit Hit;
        public Vector3 Position;
    }
}
