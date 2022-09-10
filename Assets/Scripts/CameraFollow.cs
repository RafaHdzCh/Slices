using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0.2f, 0.0f, -10.0f);
    public float dampTime = 0.3f;
    public Vector3 velocity = Vector3.zero;

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void ResetCameraPosition()
    {
        Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);   //Devuelve de la escena, las coordenadas pasadas a coordenadas de la camara de modo que el punto en el que esta el personaje, lo transfomo a coordenadas de pantalla.
        Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(offset.x , offset.y , point.z));
        Vector3 destination = point + delta;
        destination = new Vector3(target.position.x, offset.y, offset.z);
        this.transform.position = destination;
    }

    void Update()
    {
        Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);   //Devuelve de la escena, las coordenadas pasadas a coordenadas de la camara de modo que el punto en el que esta el personaje, lo transfomo a coordenadas de pantalla.
        Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(offset.x , offset.y , point.z));
        Vector3 destination = point + delta;
        destination = new Vector3(target.position.x, offset.y, offset.z);
        this.transform.position = Vector3.SmoothDamp(this.transform.position , destination, ref velocity, dampTime);
    }
}
