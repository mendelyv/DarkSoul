using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour {

    public CapsuleCollider capcol;

    private Vector3 cirPoint1 = Vector3.zero;
    private Vector3 cirPoint2 = Vector3.zero;
    private float radius;

	// Use this for initialization
	void Awake () {
        radius = capcol.radius;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        cirPoint1 = transform.position + transform.up * radius;
        cirPoint2 = transform.position + transform.up * capcol.height - transform.up * radius;

        Collider[] outputColliders = Physics.OverlapCapsule(cirPoint1, cirPoint2, radius,LayerMask.GetMask("Ground"));
        if (outputColliders.Length != 0)
        {

        }
	}
}
