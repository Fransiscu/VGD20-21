using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinController : MonoBehaviour
{
    public int coinValue;
    public string iD;

    /* https://forum.unity.com/threads/ontriggerenter-is-called-twice-sometimes.95187/ 
    I think the prudent approach is to disable the ability for picked up object to be processed more than once. 
    This way it can be checked for ability to pick up, handled if it can be picked up, skipped if it cannot.
    */
    public bool pickedUp; 

    public void SetUp()
    {
        coinValue = Random.Range(SETTINGS.level1MinCoinScore, SETTINGS.level1MaxCoinScore);
        pickedUp = false;

        // dirty way to create a persistent ID
        iD = (transform.position.x.ToString() + transform.position.y.ToString()).Replace(",", "").Substring(0, 6);

        Debug.Log("value = " + coinValue + " - ID = " + iD);
    }

}
