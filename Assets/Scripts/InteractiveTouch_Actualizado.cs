using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InteractiveTouch_Actualizado : MonoBehaviour
{
    [Header("Configuración de cámara")]
    [SerializeField] private Camera sceneCamera;

    [Header("Configuración de rotación")]
    [SerializeField] private float rotationSpeed = 0.1f;

    [Header("Configuración de zoom")]
    [SerializeField] private float minScaleFactor = 0.5f;
    [SerializeField] private float maxScaleFactor = 2f;

    // Guarda las transformaciones iniciales de los objetos interactuables.
    private readonly Dictionary<Transform, InitialTransformState> initialStates =
        new Dictionary<Transform, InitialTransformState>();

    // Objeto que el usuario ha seleccionado.
    private Transform selectedObject;

    // Escala original del objeto seleccionado.
    private Vector3 originalScale;

    // Factor de escala aplicado al objeto.
    private float currentScaleFactor = 1f;

    // Datos para detectar cambios en la cantidad de dedos.
    private int previousTouchCount;

    // Datos para controlar el movimiento de un solo dedo.
    private int singleFingerId = -1;
    private Vector2 previousSingleTouchPosition;

    // Distancia anterior entre los dos dedos para calcular el zoom.
    private float previousPinchDistance;


    private void Awake()
    {
        // Busca todos los objetos que tengan la etiqueta Interactuable.
        foreach (GameObject interactable in GameObject.FindGameObjectsWithTag("Interactuable"))
        {
            Transform objectTransform = interactable.transform;

            // Guarda su posición, rotación y escala inicial.
            initialStates[objectTransform] = new InitialTransformState(
                objectTransform.localPosition,
                objectTransform.localRotation,
                objectTransform.localScale
            );
        }
    }


    private void Update()
    {
        // Comprueba si existe una pantalla táctil.
        Touchscreen touchscreen = Touchscreen.current;

        if (touchscreen == null)
        {
            return;
        }

        // Utiliza la cámara asignada o, si no existe, la cámara principal.
        Camera cameraToUse = sceneCamera != null ? sceneCamera : Camera.main;

        if (cameraToUse == null)
        {
            return;
        }

        // Referencias para almacenar los primeros contactos táctiles.
        TouchControl firstTouch = null;
        TouchControl secondTouch = null;

        int touchCount = 0;

        // Recorre los contactos de la pantalla táctil.
        foreach (TouchControl touch in touchscreen.touches)
        {
            // Ignora los dedos que no están presionando la pantalla.
            if (!touch.press.isPressed)
            {
                continue;
            }

            touchCount++;

            if (firstTouch == null)
            {
                firstTouch = touch;
            }
            else if (secondTouch == null)
            {
                secondTouch = touch;
            }
        }

        // Ejecuta la función correspondiente según la cantidad de dedos.
        switch (touchCount)
        {
            case 0:
                // No hay dedos en pantalla.
                singleFingerId = -1;
                previousPinchDistance = 0f;
                break;

            case 1:
                // Si el dedo acaba de tocar la pantalla, intenta seleccionar un objeto.
                if (firstTouch.press.wasPressedThisFrame)
                {
                    SelectObjectAt(firstTouch.position.ReadValue(), cameraToUse);
                    SelectMariposa(firstTouch.position.ReadValue(), cameraToUse);
                }

                HandleOneTouch(firstTouch, cameraToUse);
                break;

            case 2:
                // Con dos dedos se controla el zoom.
                HandleTwoTouches(firstTouch, secondTouch);
                break;

            case 3:
                // Restaura los objetos una sola vez al detectar el tercer dedo.
                if (previousTouchCount != 3)
                {
                    HandleThreeTouches();
                }
                break;

            default:
                // Evita ejecutar el mensaje repetidamente mientras hay más de tres dedos.
                if (touchCount > 3 && previousTouchCount <= 3)
                {
                    HandleMoreThanThreeTouches();
                }
                break;
        }

        // Guarda la cantidad de dedos para la siguiente actualización.
        previousTouchCount = touchCount;
    }


    // =========================================================
    // SELECCIÓN DE OBJETOS
    // =========================================================

    private void SelectMariposa(Vector2 screenPosition, Camera cameraToUse)
    {
        if (Physics.Raycast(cameraToUse.ScreenPointToRay(screenPosition), out RaycastHit hit) &&
            hit.collider.CompareTag("Mariposa"))
        {
            selectedObject = hit.collider.transform;
            selectedObject.gameObject.SetActive(false);
        }
        else
        {
            selectedObject = null;
        }
    }
    private void SelectObjectAt(Vector2 screenPosition, Camera cameraToUse)
    {
        // Convierte la posición de la pantalla en un rayo desde la cámara.
        Ray ray = cameraToUse.ScreenPointToRay(screenPosition);

        // Comprueba si el rayo golpea un objeto con la etiqueta correcta.
        if (Physics.Raycast(ray, out RaycastHit hit) &&
            hit.collider.CompareTag("Interactuable"))
        {
            selectedObject = hit.collider.transform;

            // Guarda la escala original para utilizarla durante el zoom.
            originalScale = selectedObject.localScale;

            currentScaleFactor = 1f;

            Debug.Log($"Objeto seleccionado: {selectedObject.name}");
        }
        else
        {
            // Si se toca un lugar vacío, se cancela la selección.
            selectedObject = null;
        }
    }


    // =========================================================
    // ROTACIÓN CON UN DEDO
    // =========================================================

    private void HandleOneTouch(TouchControl touch, Camera cameraToUse)
    {
        int fingerId = touch.touchId.ReadValue();
        Vector2 currentPosition = touch.position.ReadValue();

        // Detecta si acaba de comenzar un contacto o si cambió el dedo.
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
            // Calcula el desplazamiento del dedo desde el fotograma anterior.
            Vector2 delta = currentPosition - previousSingleTouchPosition;

            // Rotación horizontal según el movimiento del dedo.
            selectedObject.Rotate(
                cameraToUse.transform.up,
                delta.x * rotationSpeed,
                Space.World
            );

            // Rotación vertical según el movimiento del dedo.
            selectedObject.Rotate(
                cameraToUse.transform.right,
                -delta.y * rotationSpeed,
                Space.World
            );
        }

        // Actualiza la posición anterior del dedo.
        previousSingleTouchPosition = currentPosition;
    }


    // =========================================================
    // ZOOM CON DOS DEDOS
    // =========================================================

    private void HandleTwoTouches(TouchControl firstTouch, TouchControl secondTouch)
    {
        // Al utilizar dos dedos, se reinicia la identificación del dedo único.
        singleFingerId = -1;

        // Calcula la distancia actual entre los dos dedos.
        float currentDistance = Vector2.Distance(
            firstTouch.position.ReadValue(),
            secondTouch.position.ReadValue()
        );

        // Al comenzar el gesto, establece la distancia de referencia.
        if (previousTouchCount != 2)
        {
            previousPinchDistance = currentDistance;
            return;
        }

        if (selectedObject != null && previousPinchDistance > 0f)
        {
            // Compara la distancia actual con la distancia anterior.
            float scaleMultiplier = currentDistance / previousPinchDistance;

            // Limita el tamaño para evitar que el objeto sea demasiado grande o pequeño.
            currentScaleFactor = Mathf.Clamp(
                currentScaleFactor * scaleMultiplier,
                minScaleFactor,
                maxScaleFactor
            );

            // Aplica la nueva escala respetando las proporciones originales.
            selectedObject.localScale = originalScale * currentScaleFactor;
        }

        // Guarda la distancia para el siguiente fotograma.
        previousPinchDistance = currentDistance;
    }


    // =========================================================
    // RESTAURAR OBJETOS CON TRES DEDOS
    // =========================================================

    private void HandleThreeTouches()
    {
        // Recorre todos los objetos registrados al iniciar la escena.
        foreach (KeyValuePair<Transform, InitialTransformState> state in initialStates)
        {
            Transform objectTransform = state.Key;

            if (objectTransform == null)
            {
                continue;
            }

            // Restaura posición, rotación y escala originales.
            objectTransform.localPosition = state.Value.Position;
            objectTransform.localRotation = state.Value.Rotation;
            objectTransform.localScale = state.Value.Scale;
        }

        selectedObject = null;
        currentScaleFactor = 1f;

        Debug.Log("Objetos restaurados a sus transformaciones iniciales.");
    }


    // =========================================================
    // CONTROL DE MÁS DE TRES DEDOS
    // =========================================================

    private void HandleMoreThanThreeTouches()
    {
        Debug.Log("Se han detectado más de tres toques. No se realizará ninguna acción.");
    }


    // =========================================================
    // CLASE PARA GUARDAR TRANSFORMACIONES INICIALES
    // =========================================================

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
