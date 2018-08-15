using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer  {

    public enum STATE
    {
        IDLE,
        RUN,
        FINISHED,
    }

    public STATE state;
    public float duration = 1.0f;
    private float elapsedTime = 0.0f;

    public void Tick()
    {
        switch(state)
        {
            case STATE.IDLE:break;

            case STATE.RUN:
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= duration)
                    state = STATE.FINISHED;
            break;

            case STATE.FINISHED:
            break;
        }

    }

    public void Go()
    {
        state = STATE.RUN;
        elapsedTime = 0.0f;
    }


}
