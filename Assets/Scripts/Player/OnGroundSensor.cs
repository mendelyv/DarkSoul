using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class name : OnGroundSensor
/// description : 检测角色是否与地面接触的类
/// time : 2018.7.2
/// @author : 杨浩然
/// </summary>
public class OnGroundSensor : MonoBehaviour {

    public CapsuleCollider capcol;

    //头部碰撞检测球心
    private Vector3 cirPoint1 = Vector3.zero;
    //脚部碰撞检测球心
    private Vector3 cirPoint2 = Vector3.zero;
    //碰撞检测球的半径
    private float radius;
    //碰撞检测球的半径与人物胶囊碰撞体的半径差
    public float offset = 0.1f;

	// Use this for initialization
	void Awake () {
        radius = capcol.radius;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        cirPoint1 = transform.position + transform.up * (radius - offset);
        cirPoint2 = transform.position + transform.up * (capcol.height - offset) - transform.up * radius;

        Collider[] outputColliders = Physics.OverlapCapsule(cirPoint1, cirPoint2, radius,LayerMask.GetMask("Ground"));

        //根据检测向上级节点发送讯息，调用相关方法
        if (outputColliders.Length != 0)
        {
            SendMessageUpwards("IsGround");
        }
        else
        {
            SendMessageUpwards("IsNotGround");
        }
	}
}
