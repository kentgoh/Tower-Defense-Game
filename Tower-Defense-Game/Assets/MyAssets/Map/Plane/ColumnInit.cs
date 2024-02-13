using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GlobalPredefinedModel;

public class ColumnInit : MonoBehaviour
{
    // ===== Input manually during map creation =====
    // Plane that need to hide
    public List<int> inactivePlaneIndexList = new List<int>();

    void Start()
    {
        InitAllPlane();
    }

    private void InitAllPlane()
    {
        int totalPlane = transform.childCount;

        for(int i = 0; i < totalPlane; i++)
        {
            // Disable plane
            if (inactivePlaneIndexList.Contains(i))
                transform.Find(i.ToString()).gameObject.SetActive(false);
        }
    }
}
