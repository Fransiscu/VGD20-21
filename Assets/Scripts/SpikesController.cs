using System.Collections;
using UnityEngine;

// Controller for the spikes 
public class SpikesController : MonoBehaviour
{
    public float hitDamage;

    private void Awake()
    {
        // Setting up their damage for the current level 
        switch (GameController.GetCurrentGameLevel())
        {
            case 1:
                hitDamage = SETTINGS.level1EnemyDamage;
                break;
            case 2:
                hitDamage = SETTINGS.level2EnemyDamage;
                break;
            case 3:
                hitDamage = SETTINGS.level3EnemyDamage;
                break;
            default:
                Debug.LogWarning("???");
                break;
        }
    }

    public void DisableColliderEnabler()
    {
        StartCoroutine("DisableColliderMomentarily");
    }

    // Disable collisions for x seconds coroutine method
    private IEnumerator DisableColliderMomentarily()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(SETTINGS.disablingTime);
        gameObject.GetComponent<Collider2D>().enabled = true;
    }
}
