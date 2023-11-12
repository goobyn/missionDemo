using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleMovement : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float speed = 3.0f;
    public float distance = 5.0f;
    public float sleepDelay = .5f;
    public float currentTime = 0.0f;

    private Vector3 startPosition;
    private bool movingUp = true;

    void Start()
    {
        startPosition = transform.position;
        currentTime = Time.time;
    }

    void Update()
    {
        Vector3 newPosition = transform.position;

        // only start moving if delta time is greater than sleep delay
        if (Time.time - currentTime < sleepDelay)
        {
            return;
        }
        
        if (movingUp)
        {
            newPosition.y += speed * Time.deltaTime;

            if (newPosition.y > startPosition.y + distance)
            {
                movingUp = false;
            }
        }
        else
        {
            newPosition.y -= speed * Time.deltaTime;

            if (newPosition.y < startPosition.y - distance)
            {
                movingUp = true;
            }
        }

        transform.position = newPosition;
    }
}