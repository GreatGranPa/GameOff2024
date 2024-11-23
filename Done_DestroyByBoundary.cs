using UnityEngine;
using System.Collections;

public class Done_DestroyByBoundary : MonoBehaviour
{
	// triggered by exiting the box collider
	void OnTriggerExit (Collider other) 
	{
		Destroy(other.gameObject);
	}
}