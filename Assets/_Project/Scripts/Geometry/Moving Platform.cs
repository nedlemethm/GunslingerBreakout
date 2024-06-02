using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MovingGeometry
{
    [SerializeField] private GameObject[] points;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int startingIndex;

    private int currentPosIndex;
    private int nextPosIndex;
    private Vector3 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        if (startMoving)
        {
            SetMoving();
        }
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
        if (GetMoving())
        {
            lastPos = gameObject.transform.position;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,
                points[nextPosIndex].transform.position, moveSpeed * Time.deltaTime);
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

    private void OnTriggerStay(Collider other)
    {
        if (GetMoving())
        {
            other.transform.SetParent(transform, true);  
            //other.transform.position += transform.position - lastPos;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}
