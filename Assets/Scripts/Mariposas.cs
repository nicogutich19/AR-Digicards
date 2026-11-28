using UnityEngine;
using System.Collections.Generic;

public class Mariposas : MonoBehaviour
{
    public Camera sceneCamera;
    public List<GameObject> mariposas;

    void Start()
    {
        if (mariposas == null || mariposas.Count == 0)
        {
            Debug.LogWarning("No se han asignado mariposas en el inspector.");
            return;
        }
        InvokeRepeating("MovimientoAleatorio", 0f, 2f);
    }
    void MovimientoAleatorio()
    {
        foreach (var mariposa in mariposas)
        {
            lock (mariposa)
            {
                Vector3 randomPosition = new Vector3(
                    Random.Range(-5f, 5f),
                    Random.Range(-5f, 5f),
                    Random.Range(-5f, 5f)
                );
                mariposa.transform.position = randomPosition;
            }
        }
    }
}
