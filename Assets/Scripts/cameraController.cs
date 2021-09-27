using UnityEngine;

// Controller for the camera in game
public class CameraController : MonoBehaviour
{
    public Transform player;
    public float xMin;   
    public float xMax;   
    public float yMin;
    public float yMax;   

    void Start()
    {
    }

    void Update()
    {
        // setting min and max xy values for the camera
        float x = Mathf.Clamp(player.transform.position.x, xMin, xMax);
        float y = Mathf.Clamp(player.transform.position.y, yMin, yMax);
        gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z);
    }
}
