using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject[] points;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool startMoving;
    [SerializeField] private int startingIndex;

    private bool moving;
    private int currentPosIndex;
    private int nextPosIndex;

    // Start is called before the first frame update
    void Start()
    {
        moving = startMoving;
        gameObject.transform.position = points[startingIndex].transform.position;
        currentPosIndex = startingIndex;

        if (startingIndex + 1 < points.Length)
        {
            nextPosIndex = startingIndex + 1;
        }
        else 
        {
            nextPosIndex = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moving)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,
                points[nextPosIndex].transform.position, moveSpeed * Time.deltaTime);
            //moving = true;
        }
        CheckNextPos();
    }

    private void CheckNextPos()
    {
        if (Vector3.Distance(gameObject.transform.position, points[nextPosIndex].transform.position) < 0.001f)
        {
            currentPosIndex = nextPosIndex;

            if (currentPosIndex + 1 < points.Length)
            {
                nextPosIndex = currentPosIndex + 1;
            }
            else
            {
                nextPosIndex = 0;
            }
            //moving = false;
            Debug.Log(nextPosIndex);
        }
    }

    
    void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform, true);
    }
}
