using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EventOne : MonoBehaviour
{
    public static EventOne Instance;

    [Header("Objetos Interactivos")]

    public GameObject tiara;
    public GameObject pastel;
    public List<GameObject> mariposas;
    
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
    public void Detectado()
    {
        Debug.Log("imagen detectada");
        IniciarSecuencia();
    }
    public void IniciarSecuencia()
    {
        MovimientoMariposas();
    }
    void MovimientoMariposas()
    {
        if (mariposas == null || mariposas.Count == 0)
        {
            Debug.LogWarning("no se han asignado mariposas");
            return;
        }
        else if (mariposas != null && mariposas.Count > 0)
        {
            foreach (var mariposa in mariposas)
            {
                lock (mariposa)
                {
                    Vector3 randomPosition = new Vector3(Random.Range(-1f, 1f), 0f,Random.Range(-1f, 1f));
                    mariposa.transform.position = randomPosition;
                }
            }
        }
        if (mariposas.All(m => !m.activeInHierarchy))
        {
            Debug.Log("Todas las mariposas han sido detectadas. Secuencia completada.");
        }
    }
}
