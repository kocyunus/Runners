using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    [HideInInspector] public GameObject[] checkPoint;
    [HideInInspector] public int currentCheckPoint = 1;



    private void Awake()
    {
        checkPoint = GameObject.FindGameObjectsWithTag("CheckPoint");
        currentCheckPoint = 1;
    }
    private void Start()
    {
        foreach (GameObject cp in checkPoint)
        {
            cp.AddComponent<CurrentCheckPoint>();
            cp.GetComponent<CurrentCheckPoint>().currentCheckNumber = currentCheckPoint;
            cp.name = "CheckPoint" + currentCheckPoint;
            currentCheckPoint++;
            Debug.Log(cp.name);
        }
    }
}
