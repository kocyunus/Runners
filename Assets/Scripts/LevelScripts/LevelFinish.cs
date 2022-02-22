using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelFinish : MonoBehaviour
{
    public Text[] rankText;
    private void Update()
    {
        rankText[0].text = GameManager._MyInstance.firstRunner;
        rankText[1].text = GameManager._MyInstance.secondRunner;
        rankText[2].text = GameManager._MyInstance.thirdRunner;
    }

}
