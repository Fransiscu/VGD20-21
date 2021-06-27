using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class savePointsController : MonoBehaviour
{
    public Transform milestonePoint;
    public RaycastHit2D raycastHit2D;
    public bool milestoneReached;

    public float distance = 50f;

    void Start()
    {
        milestoneReached = false;  // player can only reach checkpoint once
    }

    // Update is called once per frame
    void Update()
    {
        raycastHit2D = Physics2D.Raycast(milestonePoint.position, Vector2.up, distance);
        if (raycastHit2D.collider == true && raycastHit2D.collider.tag == "Player" && !milestoneReached)
        {
            milestoneReached = true;
            // checkpoint reached, do stuff
            Debug.Log("walking over raycast");
        }
    }
}
