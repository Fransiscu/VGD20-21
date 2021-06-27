using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedUpController : MonoBehaviour
{
    // Using this controller to make sure a single instance of double jump doesn't get picked up several times
    // causing problems with the game

    public bool pickedUp;

    void Start()
    {
        pickedUp = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
