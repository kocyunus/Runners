using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingSystem : MonoBehaviour
{
    public int currectCheckPoint = 1, lapCount;
    public float distance;
    private Vector3 _checkPoint;
    public float counter;
    public int rank;
    private float lapscore = 2000f , pointscore = 50f; 
    
    void Start()
    {
        currectCheckPoint = 1;
    }

    void Update()
    {
        CalculateDistance();
    }
    void CalculateDistance() 
    {
        distance = Vector3.Distance(transform.position, _checkPoint);
        distance = distance * 0.1f;
        counter = lapCount * lapscore + currectCheckPoint * pointscore + distance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag =="CheckPoint")
        {
            currectCheckPoint = other.GetComponent<CurrentCheckPoint>().currentCheckNumber;
            _checkPoint = GameObject.Find("CheckPoint" + currectCheckPoint).transform.position;
        }
        if (other.tag == "Finish")
        {
            lapCount += 1;
            GameManager._MyInstance.pass += 1;
        }
    }
}
