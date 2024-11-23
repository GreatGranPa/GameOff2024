using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;


[System.Serializable]
public class Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
	public float speed;
	public float tilt;
	public Boundary boundary;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;

    public Done_GameController doneGameController   ; 
	 
	private float nextFire;

	//executed once per *frame:
	void Update ()
	{
		if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) && Time.time > nextFire)   
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			GetComponent<AudioSource>().Play ();
		}
        //FIXME WARP
        if (Input.GetKey(KeyCode.O) && Input.GetKey(KeyCode.P) && Input.GetKeyDown(KeyCode.I) && Input.GetKeyDown(KeyCode.U) )
        {
            doneGameController.warpMe(); 
        }

    }

    //executed once per *physics step:
    void FixedUpdate ()
	{
        //float moveHorizontal = CrossPlatformInputManager.GetAxis("Horizontal");	
        //float moveVertical = CrossPlatformInputManager.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");  
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody>().velocity = movement * speed;

		//ensure we dont leave the game area:
		GetComponent<Rigidbody>().position = new Vector3
		(
			Mathf.Clamp (GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp (GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
		);
		
		GetComponent<Rigidbody>().rotation = 
				Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
	}
}
