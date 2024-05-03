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

    // Start is called before the first frame update
    void Start()
    {
        moving = startMoving;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            
            
        }
    }

    Vector3 CurrentTarget()
    {

    }
}
