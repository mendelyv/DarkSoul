using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

    public GameObject model;
    public PlayerInput pInput;
    public float walkSpeed = 2.0f;

    [SerializeField]
    private Animator anim;
    private Rigidbody rig;

    private Vector3 movingVec;

	void Awake () {
        anim = model.GetComponent<Animator>();
        pInput = GetComponent<PlayerInput>();
        rig = GetComponent<Rigidbody>();
	}
	

	void Update () {
        //print(pInput.dirForward);
        anim.SetFloat("forward", pInput.Dirmag);
        if(pInput.Dirmag > 0.1f)
            model.transform.forward = pInput.Dirvec;
        movingVec = pInput.Dirmag * model.transform.forward * walkSpeed;
	}

    void FixedUpdate()
    {
        //rig.position += movingVec * Time.fixedDeltaTime;
        //rig.velocity = movingVec;//并未算入地心引力
        rig.velocity = new Vector3(movingVec.x, rig.velocity.y, movingVec.z);
    }



//class end
}

