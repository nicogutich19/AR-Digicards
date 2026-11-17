using UnityEngine;

public class EventOne : MonoBehaviour
{
    public static EventOne Instance;

    public GameObject test;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //testeo de funciones mandar a esta funcion en el update de otro script para ver si funciona
    public void TestFuncional()
    {
        Debug.Log("Test comfirmado");
        Prueba(); //en caso que quieran hacer algo mas aparte de mostrar el mensaje pueden agregarlo aqui
    }
    public void Prueba()
    {
        test.SetActive(true);
    }
    public void Detectado()
    {
        Debug.Log("imagen detectada");
    }
}
