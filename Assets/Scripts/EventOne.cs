using UnityEngine;

public class EventOne : MonoBehaviour
{
    public static EventOne Instance;

    public GameObject test;

    [Header("Objetos Interactivos")]

    public GameObject tiara;
    public GameObject pastel;
    
    [Header("Objetos de la escena")]

    public GameObject flores;
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
    public void TestFuncional()
    {
        Debug.Log("Test comfirmado");
        Prueba();
    }
    public void Prueba()
    {
        test.SetActive(true);
    }
    public void Detectado()
    {
        Debug.Log("imagen detectada");
        IniciarSecuencia();
    }
    public void IniciarSecuencia()
    {
        
    }
}
