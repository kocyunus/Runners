using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform _player;
    [SerializeField] Vector3 distance;
    float _followSpeed = 60;
    private void Awake()
    {
        
        _player = GameObject.Find("Player").transform;
       
    }

    // Update is called once per frame
    void Update()
    {

        FollowPlayer();
    }
    void FollowPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, NextPositionForCamera(), _followSpeed * Time.deltaTime);
    }

    Vector3 NextPositionForCamera() 
    {
        distance.x = _player.forward.x + 75f;
        distance.y = _player.transform.position.y + 25f;
        distance.z = _player.transform.position.z -4f;
        return distance;

    }
}
