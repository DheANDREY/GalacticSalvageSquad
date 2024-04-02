using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalHandler : MonoBehaviour
{
    public GameObject portalNext, portalStart; private GameObject spawnnedPortal, spawnnedPortal2;
    public Transform parentPortal;
    public void SpawnPortal()
    {
        Vector3 posSpawn = new Vector3(14, Random.Range(0, 7), 0);
        spawnnedPortal = Instantiate(portalNext, posSpawn, Quaternion.identity);
        //spawnnedPortal.transform.SetParent(parentPortal);
    }

    
    public void SpawnPortalStart(Vector3 posSpawn2)
    {
        //Vector3 posSpawn2 = new Vector3(0, Random.Range(0, 7), 0);
        spawnnedPortal2 = Instantiate(portalStart, posSpawn2, Quaternion.identity);
        //spawnnedPortal2.transform.SetParent(parentPortal);
        Destroy(spawnnedPortal2, 6);
    }
}
