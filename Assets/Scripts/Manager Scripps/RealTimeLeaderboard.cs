using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealTimeLeaderboard : MonoBehaviour
{
    public Text[] runnersName;
    public string runnerNameFirst, runnerNameSecond, runnerNamethirth;


    private void Update()
    {
        runnersName[0].text = runnerNameFirst;
        runnersName[1].text = runnerNameSecond;
        runnersName[2].text = runnerNamethirth;
    }
}
