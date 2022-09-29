using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D other)								//Metodo que se activa al entrar a una zona de colision.
	{
		if(other.tag == "Player")										//Si la etiqueta del objeto que ingresa a la zona tiene la etiqueta player...
		{
			PlayerController.sharedInstance.Kill();						//y el jugador muere.
		}
	}
}
