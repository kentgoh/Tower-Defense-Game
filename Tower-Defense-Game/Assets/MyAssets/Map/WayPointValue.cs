using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointValue : MonoBehaviour
{
    public int currentWayPoint;
    public List<int> nextWayPoint;
    public int destinatedWayPoint;

    private void Update()
    {
        // if there is more than next wayPoint, randomly pick one of it by index
        if (nextWayPoint.Capacity > 1)
        {
            int randomIndexValue = UnityEngine.Random.Range(0, nextWayPoint.Count);

            destinatedWayPoint = nextWayPoint[randomIndexValue];
        }
    }
}
