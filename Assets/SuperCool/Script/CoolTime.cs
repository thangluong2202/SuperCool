using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolTime : MonoBehaviour
{
    public static CoolTime Ins;
    public float curTimeRate = 1f;
    public float normalTimeRate = 1f;
    public float minTimeRate = 0f;

    public float speedTimeRateChange = 0.5f;

    public bool isMoving = false;

    void Start()
    {
        Ins = this;
    }

    public float deltaTime { get { return curTimeRate * Time.deltaTime; } }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            curTimeRate += speedTimeRateChange * Time.deltaTime;
            if(curTimeRate > normalTimeRate)
            {
                curTimeRate = normalTimeRate;
            }
        }
        else
        {
            curTimeRate -= speedTimeRateChange * Time.deltaTime;
            if (curTimeRate < minTimeRate)
            {
                curTimeRate = minTimeRate;
            }
        }
    }
}
