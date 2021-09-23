using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesController : MonoBehaviour
{
    public float hitDamage;

    private void Awake()
    {
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

    private IEnumerator DisableColliderMomentarily()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<Collider2D>().enabled = true;
    }
}
