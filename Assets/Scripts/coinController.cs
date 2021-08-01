using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinController : MonoBehaviour
{
    public int coinValue;
    public string iD;

    // Using this controller to make sure a single instance of double jump doesn't get picked up several times
    // causing problems with the game
    // https://forum.unity.com/threads/ontriggerenter-is-called-twice-sometimes.95187/ 

    public bool pickedUp; 

    public void SetUp()
    {
        coinValue = Random.Range(SETTINGS.level1MinCoinScore, SETTINGS.level1MaxCoinScore);
        pickedUp = false;
        // dirty way to create a persistent ID
        iD = (transform.position.x.ToString() + transform.position.y.ToString()).Replace(",", "").Substring(0, 6);

        Debug.Log("name = " + name + " - value = " + coinValue + " - ID = " + iD);
    }

}
