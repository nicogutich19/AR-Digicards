using UnityEngine;
using UnityEngine.InputSystem;


public class InteractiveTouch : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;

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

        foreach (var touch in touchscreen.touches)
        {
            if (!touch.press.wasPressedThisFrame)
            {
                continue;
            }

            Vector2 touchPosition = touch.position.ReadValue();
            Ray ray = cameraToUse.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log($"Objeto tocado: {hit.collider.gameObject.name}, posicion: {hit.point}");
            }
        }
    }
}
