using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUGDrawCenterPoints : MonoBehaviour
{
    // Start is called before the first frame update
    MapGenerator2 mapGen;
    void OnDrawGizmosSelected()
    {
        mapGen = GetComponent<MapGenerator2>();
        foreach (Feature feat in MapManager.allFeatures)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(feat.centerPoint, .6f);
        }
    }
}
