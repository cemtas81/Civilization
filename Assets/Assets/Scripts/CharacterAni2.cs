using UnityEngine;

public class CharacterAni2 : MonoBehaviour
{
 
    private AnimatedMesh ani;
    void Awake()
    {

        ani = GetComponentInChildren<AnimatedMesh>();

    }

    public void Attack()
    {
       
        ani.Play("Ataque");
    }

    public void Movement()
    {
       
        ani.Play("Zumbi_Andar");
    }

    public void Die()
    {
      
        ani.Play("Die");
    }
}

