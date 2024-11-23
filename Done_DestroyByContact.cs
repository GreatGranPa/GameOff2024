using UnityEngine;
using System.Collections;

public class Done_DestroyByContact : MonoBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;
	private Done_GameController gameController; //Done_GameController is a script!!!!!

	void Start ()
	{
		//This code finds the gamecontrollerobject for ous:
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <Done_GameController>();
		}
		if (gameController == null)
		{
			Debug.Log (" Cannot find 'GameController' script");
		}

	}

	void OnTriggerEnter (Collider other)
	{
		Debug.Log (""+ other);

		if (other.tag == "Boundary" || other.tag == "Enemy")
		{
			return;
		}

        //score 4 destroying the asteroid
        gameController.AddScore(scoreValue);

        if (explosion != null)
		{
			Instantiate(explosion, transform.position, transform.rotation);
		}
		 
		if (other.tag == "Player")
		{
			Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
			gameController.GameOver();
		}

		//destroy laserbolt
		Destroy (other.gameObject);

		//destroy asteroid
		Destroy (gameObject);
	}
}