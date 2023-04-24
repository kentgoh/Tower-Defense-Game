using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WayPointsInit: MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int totalWayPoint = gameObject.transform.childCount;
        // Stage one have 6 wayPoints
        // 1
        // 2
        // 3, 4
        // 5
        // 6 (EndPoint)
        for (int i = 0; i < totalWayPoint; i ++)
        {
            Transform wayPoint = gameObject.transform.GetChild(i);
           
            // Get the wayPointValueScript script
            WayPointValue wayPointValueScript = wayPoint.GetComponent<WayPointValue>();

            // Set the currentWayPoint value in wayPoint by name
            wayPointValueScript.currentWayPoint = int.Parse(wayPoint.name);

            // Customize all wayPoint
            switch (wayPointValueScript.currentWayPoint)
            {
                case 1: { wayPointValueScript.nextWayPoint.Add(2); break; }
                case 2:
                    {
                        wayPointValueScript.nextWayPoint.Add(3);
                        wayPointValueScript.nextWayPoint.Add(4);
                        break;
                    }
                case 3: { wayPointValueScript.nextWayPoint.Add(5); break; }
                case 4: { wayPointValueScript.nextWayPoint.Add(5); break; }
                case 5: { wayPointValueScript.nextWayPoint.Add(6); break; }
            }

        }
    }

}
