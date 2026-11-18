using UnityEngine;
using UnityEngine.InputSystem;

public class InteractiveObject : MonoBehaviour
{
    void Update()
    {
        //es para evitar que explote
        if (Touchscreen.current != null)
        {
            TouchThisObject();
            return;
        }
    }
    void TouchThisObject()
    {
        if (Touchscreen.current == null)
        {
            return;
        }
        Touchscreen touchscreen = Touchscreen.current;
        if (touchscreen.primaryTouch.press.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(touchscreen.primaryTouch.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    //aqui puede llamar a la funcion que desee en EventOne
                    //el debug log es solo para confirmar que se ha tocado el objeto y la posicion del toque (lo puede borrar si quiere)
                    Debug.Log($"Objeto tocado: {hit.collider.gameObject.name}, posicion: {hit.point}");
                    EventOne.Instance.TestFuncional();
                }
            }
        }
    }
}
