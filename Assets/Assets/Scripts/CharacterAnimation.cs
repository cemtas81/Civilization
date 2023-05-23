using UnityEngine;

public class CharacterAnimation : MonoBehaviour {

	private Animator animator;
	
	void Awake () {

		animator = GetComponent<Animator>();

	}

	public void Attack(bool state)
	{
		animator.SetBool("Attacking", state);
	}
	public void Movement (float value) {

		animator.SetFloat("Running", value,0.1f,Time.deltaTime);

	}
	public void VelocityZ (float value) {

		animator.SetFloat("VelocityZ", value,0.1f,Time.deltaTime);
      
    }
	public void VelocityX (float value) {

		animator.SetFloat("VelocityX", value,0.1f,Time.deltaTime);
	
	}
	public void Turning(float value)
	{
		animator.SetFloat("Turn", value * 2000 * Time.deltaTime);
	}

	public void Die () {

		animator.SetTrigger("Die");

	}
	public void Boom () {

		animator.SetTrigger("Boom");

	}
	public void UnBoom () {

		animator.SetTrigger("UnBoom");

	}
	public void PlayerAttack()
	{
		animator.SetTrigger("Attack");
	}
	public void PlayerThrow()
	{
		animator.SetTrigger("Throw");
	}
}
