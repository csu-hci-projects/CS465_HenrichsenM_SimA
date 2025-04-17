using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Tutorials.Core.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit.Attachment;
using UnityEngine.XR.Management;

public class PenController : MonoBehaviour
{
    public Transform leftDrawOrigin;
    public Transform rightDrawOrigin;
    public XRHandSubsystem handSubsystem;
    private bool isRightHand = true;
    public Material drawingMaterial;

    private LineRenderer currentDrawing;
    public float minDistance = 0.01f;
    private List<Vector3> points = new List<Vector3>();
    private Vector3 lastPoint;

    public GameObject lineRenderer;

    private int colorIndex;
    private int index;

    private float rightValue;
    private float leftValue;

    public PlayerInput input;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        colorIndex = 0;
        input = new PlayerInput();

        input.vrInput.LeftHandDrawing.performed += OnLeftHandDrawing;
        input.vrInput.RightHandDrawing.performed += OnRightHandDrawing;

        handSubsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRHandSubsystem>();
    }

    private void OnLeftHandDrawing(InputAction.CallbackContext context)
    {
        leftValue = context.ReadValue<float>();
    }
    private void OnRightHandDrawing(InputAction.CallbackContext context)
    {
        rightValue = context.ReadValue<float>();
    }

    // Update is called once per frame
    void Update()
    {
        if (handSubsystem != null)
        {
            XRHand hand = isRightHand ? handSubsystem.rightHand : handSubsystem.leftHand;

            if (hand.isTracked)
            {
                var indexTip = hand.GetJoint(XRHandJointID.IndexTip);
                var thumbTip = hand.GetJoint(XRHandJointID.ThumbTip);

                if (indexTip.TryGetPose(out Pose indexPose) && thumbTip.TryGetPose(out Pose thumbPose))
                {
                    float pinchDistance = Vector3.Distance(indexPose.position, thumbPose.position);

                    if (pinchDistance < 0.02f)
                    {
                        if (currentDrawing == null)
                        {
                            GameObject line = Instantiate(lineRenderer);
                            currentDrawing = lineRenderer.GetComponent<LineRenderer>();
                            points.Clear();

                            points.Add(indexPose.position);
                            currentDrawing.positionCount = points.Count;
                            currentDrawing.SetPositions(points.ToArray());
                            lastPoint = indexPose.position;
                        }

                        Vector3 currentPosition = indexPose.position;
                        if (Vector3.Distance(lastPoint, currentPosition) > minDistance)
                        {
                            points.Add(currentPosition);
                            currentDrawing.positionCount = points.Count;
                            currentDrawing.SetPositions(points.ToArray());
                            lastPoint = currentPosition;
                        }
                    }
                    else
                    {
                        currentDrawing = null;
                        points.Clear();
                    }
                }
            }
        }
        if (leftValue > 0.4f)
        {
            if (currentDrawing == null)
            {
                GameObject line = Instantiate(lineRenderer);
                currentDrawing = lineRenderer.GetComponent<LineRenderer>();
                points.Clear();

                points.Add(leftDrawOrigin.position);
                currentDrawing.positionCount = points.Count;
                currentDrawing.SetPositions(points.ToArray());
                lastPoint = leftDrawOrigin.position;
            }

            Vector3 currentPosition = leftDrawOrigin.position;
            if (Vector3.Distance(lastPoint, currentPosition) > minDistance)
            {
                points.Add(currentPosition);
                currentDrawing.positionCount = points.Count;
                currentDrawing.SetPositions(points.ToArray());
                lastPoint = currentPosition;
            }
        }
        else if (rightValue > 0.4f)
        {
            if (currentDrawing == null)
            {
                GameObject line = Instantiate(lineRenderer);
                currentDrawing = lineRenderer.GetComponent<LineRenderer>();
                points.Clear();

                points.Add(rightDrawOrigin.position);
                currentDrawing.positionCount = points.Count;
                currentDrawing.SetPositions(points.ToArray());
                lastPoint = rightDrawOrigin.position;
            }

            Vector3 currentPosition = rightDrawOrigin.position;
            if (Vector3.Distance(lastPoint, currentPosition) > minDistance)
            {
                points.Add(currentPosition);
                currentDrawing.positionCount = points.Count;
                currentDrawing.SetPositions(points.ToArray());
                lastPoint = currentPosition;
            }
        }
        else
        {
            currentDrawing = null;
            points.Clear();
        }
    }

    void OnEnable()
    {
        input.vrInput.Enable();
    }

    void OnDisable()
    {
        input.vrInput.LeftHandDrawing.performed -= OnLeftHandDrawing;
        input.vrInput.RightHandDrawing.performed -= OnRightHandDrawing;

        input.vrInput.Disable();
    }
}
