using UnityEngine;


public class CharacterMovement : MonoBehaviour {

	private Rigidbody myRigidbody;
    private CharacterAnimation playerAnimation;
    void Awake () {
		myRigidbody = GetComponent<Rigidbody>();
		playerAnimation = GetComponent<CharacterAnimation>();	
	} 
	
	public void Movement (Vector3 direction, float speed) {
		// moves the enemy as in the PlayerController but 
		// instead using the GetAxis method it uses the normalized direction vector
		myRigidbody.MovePosition (myRigidbody.position + (speed * Time.deltaTime * direction.normalized));
	}

	public void Rotation (Vector3 direction) 
	{
		// rotates the enemy towards the player
		Quaternion newRotation = Quaternion.LookRotation(direction);
		myRigidbody.MoveRotation (newRotation);
	
    }
	public void Rotation(Quaternion smoothdir)
	{
		myRigidbody.MoveRotation(smoothdir);
		//playerAnimation.Turning(smoothdir.y - transform.rotation.y);
	}
	public void Die() {
		//myRigidbody.constraints = RigidbodyConstraints.None;
		//myRigidbody.velocity = Vector3.zero;
		//myRigidbody.isKinematic = false;
		//GetComponent<Collider>().enabled = false;
		myRigidbody.isKinematic = true;
		GetComponent<Collider>().enabled = false;
	}

}
