using System.Collections.Generic;
using NUnit.Framework.Internal.Execution;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using EU = UnityEngine.InputSystem.EnhancedTouch;

public class InteractiveTouch : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private float rotationSpeed = 0.1f;
    [SerializeField] private float minScaleFactor = 0.5f;
    [SerializeField] private float maxScaleFactor = 2f;

    private readonly Dictionary<Transform, InitialTransformState> initialStates =
        new Dictionary<Transform, InitialTransformState>();

    private Transform selectedObject;
    private Vector3 originalScale;
    private float currentScaleFactor = 1f;

    private int previousTouchCount;
    private int singleFingerId = -1;
    private Vector2 previousSingleTouchPosition;
    private float previousPinchDistance;

    private void Awake()
    {
        foreach (GameObject interactable in GameObject.FindGameObjectsWithTag("Interactuable"))
        {
            Transform objectTransform = interactable.transform;

            initialStates[objectTransform] = new InitialTransformState(
                objectTransform.localPosition,
                objectTransform.localRotation,
                objectTransform.localScale);
        }
    }

    private void Update()
    {
        Touchscreen touchscreen = Touchscreen.current;

        if (touchscreen == null)
        {
            return;
        }

        Camera cameraToUse = sceneCamera != null ? sceneCamera : Camera.main;

        if (cameraToUse == null)
        {
            return;
        }

        int touchCount = EU.Touch.activeTouches.Count;

        switch (touchCount)
        {
            case 1:
                if (EU.Touch.activeTouches[0].phase == UnityEngine.InputSystem.TouchPhase.Began)
                {
                    Ray ray = cameraToUse.ScreenPointToRay(EU.Touch.activeTouches[0].screenPosition);

                    if (Physics.Raycast(ray, out RaycastHit hit) &&
                        hit.collider.CompareTag("Interactuable"))
                    {
                        selectedObject = hit.collider.transform;
                        originalScale = selectedObject.localScale;
                        currentScaleFactor = 1f;

                        Debug.Log($"Objeto seleccionado: {selectedObject.name}");
                    }
                    else
                    {
                        selectedObject = null;
                    }
                }
                else if (EU.Touch.activeTouches[0].phase == UnityEngine.InputSystem.TouchPhase.Moved)
                {
                    if (selectedObject != null)
                    {
                        Vector2 direction = previousSingleTouchPosition - EU.Touch.activeTouches[0].screenPosition;
                        selectedObject.Rotate(direction, Space.Self);
                    }
                }

                previousSingleTouchPosition = EU.Touch.activeTouches[0].screenPosition;
                break;

            case 2:
                EU.Touch secondTouch = EU.Touch.activeTouches[1];


                break;
            default:
                break;
        }





        //TouchControl firstTouch = null;
        //TouchControl secondTouch = null;
        //int touchCount = 0;

        //foreach (TouchControl touch in touchscreen.touches)
        //{
        //    if (!touch.press.isPressed)
        //    {
        //        continue;
        //    }

        //    touchCount++;

        //    if (firstTouch == null)
        //    {
        //        firstTouch = touch;
        //    }
        //    else if (secondTouch == null)
        //    {
        //        secondTouch = touch;
        //    }

        //    if (touch.press.wasPressedThisFrame)
        //    {
        //        SelectObjectAt(touch.position.ReadValue(), cameraToUse);
        //    }
        //}

        //switch (touchCount)
        //{
        //    case 1:
        //        HandleOneTouch(firstTouch, cameraToUse);
        //        break;

        //    case 2:
        //        HandleTwoTouches(firstTouch, secondTouch);
        //        break;

        //    case 3:
        //        if (previousTouchCount != 3)
        //        {
        //            HandleThreeTouches();
        //        }

        //        break;

        //    default:
        //        if (touchCount > 3)
        //        {
        //            HandleMoreThanThreeTouches();
        //        }

        //        break;
        //}

        //previousTouchCount = touchCount;
    }

    private void SelectObjectAt(Vector2 screenPosition, Camera cameraToUse)
    {
        Ray ray = cameraToUse.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit) &&
            hit.collider.CompareTag("Interactuable"))
        {
            selectedObject = hit.collider.transform;
            originalScale = selectedObject.localScale;
            currentScaleFactor = 1f;

            Debug.Log($"Objeto seleccionado: {selectedObject.name}");
        }
        else
        {
            selectedObject = null;
        }
    }

    private void HandleOneTouch(TouchControl touch, Camera cameraToUse)
    {
        int fingerId = touch.touchId.ReadValue();
        Vector2 currentPosition = touch.position.ReadValue();

        if (previousTouchCount != 1 ||
            touch.press.wasPressedThisFrame ||
            fingerId != singleFingerId)
        {
            singleFingerId = fingerId;
            previousSingleTouchPosition = currentPosition;
            return;
        }

        if (selectedObject != null)
        {
            Vector2 delta = currentPosition - previousSingleTouchPosition;

            selectedObject.Rotate(
                cameraToUse.transform.up,
                delta.x * rotationSpeed,
                Space.World);

            selectedObject.Rotate(
                cameraToUse.transform.right,
                -delta.y * rotationSpeed,
                Space.World);
        }

        previousSingleTouchPosition = currentPosition;
    }

    private void HandleTwoTouches(TouchControl firstTouch, TouchControl secondTouch)
    {
        singleFingerId = -1;

        float currentDistance = Vector2.Distance(
            firstTouch.position.ReadValue(),
            secondTouch.position.ReadValue());

        if (previousTouchCount != 2)
        {
            previousPinchDistance = currentDistance;
            return;
        }

        if (selectedObject != null && previousPinchDistance > 0f)
        {
            float scaleMultiplier = currentDistance / previousPinchDistance;

            currentScaleFactor = Mathf.Clamp(
                currentScaleFactor * scaleMultiplier,
                minScaleFactor,
                maxScaleFactor);

            selectedObject.localScale = originalScale * currentScaleFactor;
        }

        previousPinchDistance = currentDistance;
    }

    private void HandleThreeTouches()
    {
        foreach (KeyValuePair<Transform, InitialTransformState> state in initialStates)
        {
            Transform objectTransform = state.Key;

            if (objectTransform == null)
            {
                continue;
            }

            objectTransform.localPosition = state.Value.Position;
            objectTransform.localRotation = state.Value.Rotation;
            objectTransform.localScale = state.Value.Scale;
        }

        selectedObject = null;
        currentScaleFactor = 1f;
        Debug.Log("Objetos restaurados a sus transformaciones iniciales.");
    }

    private void HandleMoreThanThreeTouches()
    {
        Debug.Log("Se han detectado m�s de tres toques. No se realizar� ninguna acci�n.");
    }

    private sealed class InitialTransformState
    {
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        public Vector3 Scale { get; }

        public InitialTransformState(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }
}
