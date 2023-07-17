using System.Collections;
using UnityEngine;

public class BarbSlashDamage : MonoBehaviour
{
    public GameObject particle1;
    void OnTriggerEnter(Collider other)
    {
        Quaternion rotation = Quaternion.LookRotation(-transform.forward);
        switch (other.tag)
        {
            case "Enemy":
                BarbEnemyCont enemy = other.GetComponent<BarbEnemyCont>();
                enemy.LoseHealth(1);
                enemy.BloodParticle(transform.position, rotation);
                break;
            case "Boss":
                BossCont2 boss = other.GetComponent<BossCont2>();
                boss.LoseHealth(1);
                boss.BloodParticle(transform.position, rotation);
                break;
            case "Untagged":
                GetComponent<AxeBoomerang>().enabled = false;
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Collider>().isTrigger = false;
                //particle1.SetActive(false);
                //Destroy(this.gameObject,2);
                StartCoroutine(Come());
                break;
        }
        IEnumerator Come()
        {
            yield return new WaitForSeconds(.3f);
            GetComponent<AxeBoomerang>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = true;
            //GetComponent<Collider>().isTrigger = true;
            //particle1.SetActive(true);

        }

    }
}
