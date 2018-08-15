using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class name : MyButton
/// description : 使用一个类控制按钮的 按住，点按和抬起三种状态
/// time : 2018.8.14
/// @author : 杨浩然
/// </summary>
public class MyButton {

    public bool IsPressing = false;
    public bool OnPressed = false;
    public bool OnReleased = false;
    public bool IsExtending = false;
    public bool IsDelaying = false;

    public float extendingDuration = 0.15f;//双击的等待时间
    public float delayingDuration = 0.15f;

    private bool curState = false;
    private bool lastState = false;

    private MyTimer extTimer = new MyTimer();
    private MyTimer delayTimer = new MyTimer();

    public void Tick(bool input)
    {
        extTimer.Tick();
        delayTimer.Tick();

        curState = input;
        IsPressing = curState;

        OnPressed = false;
        OnReleased = false;
        if(curState != lastState)
        {
            if(curState)
            {
                OnPressed = true;
                StartTimer(delayTimer, delayingDuration);
            }
            else
            {
                OnReleased = true;
                StartTimer(extTimer,extendingDuration);
            }
        }
        lastState = curState;
        IsExtending = extTimer.state == MyTimer.STATE.RUN ? true : false;
        IsDelaying = delayTimer.state == MyTimer.STATE.RUN ? true : false;
    }

    private void StartTimer(MyTimer timer,float duration)
    {
        timer.duration = duration;
        timer.Go();
    }


//class end
}
