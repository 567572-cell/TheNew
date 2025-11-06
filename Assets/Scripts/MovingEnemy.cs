using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingEnemy : MonoBehaviour
{
    public Transform platform; // reference for the platform
    public Transform startPoint; // reference for the start point
    public Transform endPoint; // reference for the end point
    public float speed = 1.5f; // speed between points
    public int direction = 1; // direction of travel 1 or -1

    // Function to show the lines between points
    private void OnDrawGizmos()
    {
        // just for
        if (platform != null && startPoint != null && endPoint != null)
        {
            Gizmos.DrawLine(platform.transform.position, startPoint.position);
            Gizmos.DrawLine(platform.transform.position, endPoint.position);
        }
    }
    // Additional Code 

  void Update()
{
    Vector2 target = currentMovementTarget();
    platform.position = Vector2.Lerp(platform.position, target, speed * Time.deltaTime);
    float distance = (target - (Vector2)platform.position).magnitude;
    //Debug.Log(distance);
    if (distance <= 0.4f)
    {
        direction *= -1;
    }
}

// function to set the direction of travel
Vector2 currentMovementTarget()
{
    if (direction == 1)
    {
        return startPoint.position;
    }
    else
    {
        return endPoint.position;
    }
}  // End of Additional Code
}
// Update is called once per frame


