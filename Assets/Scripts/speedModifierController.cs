using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedModifierController : MonoBehaviour
{
    // Using this controller to make sure a single instance of double jump doesn't get picked up several times
    // causing problems with the game
    // https://forum.unity.com/threads/ontriggerenter-is-called-twice-sometimes.95187/ 

    public bool pickedUp;
    public string iD;

    void Start()
    {
        pickedUp = false;
        // dirty way to create a persistent ID
        iD = (this.transform.position.x.ToString() + this.transform.position.y.ToString()).Replace(",", "").Substring(0, 6);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
