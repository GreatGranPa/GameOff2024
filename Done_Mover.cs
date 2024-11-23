using UnityEngine;
using System.Collections;

public class Done_Mover : MonoBehaviour
{
	public float speed;

	//WILL BE EXECUTED ON THE FIRST FRAME ONLY	
	void Start ()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}
}
