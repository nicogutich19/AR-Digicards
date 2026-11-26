using UnityEngine;
using System.Collections.Generic;

public class EventOne : MonoBehaviour
{
    public static EventOne Instance;

    public GameObject test;

    [Header("Objetos Interactivos")]

    public GameObject tiara;
    public GameObject pastel;
    public List<GameObject> flores;
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
        IniciarSecuencia();
    }
    public void IniciarSecuencia()
    {
        LeanTween.rotateX(tiara, 3600f, 60f);
    }
}
