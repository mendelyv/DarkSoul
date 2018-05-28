using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

    public GameObject model;
    public PlayerInput pInput;
    public float walkSpeed = 2.0f;
    public float runMultiplier = 2.0f;

    [SerializeField]
    private Animator anim;
    private Rigidbody rig;

    private Vector3 movingVec;
    public bool lockPlanar = false;

	void Awake () {
        anim = model.GetComponent<Animator>();
        pInput = GetComponent<PlayerInput>();
        rig = GetComponent<Rigidbody>();
	}
	

	void Update () {
        //print(pInput.dirForward);
        anim.SetFloat("forward", pInput.Dirmag * Mathf.Lerp(anim.GetFloat("forward"), ((pInput.run) ? 2.0f : 1.0f),0.3f));
        if (pInput.jump)
            anim.SetTrigger("jump");

        if(pInput.Dirmag > 0.1f)
        {
            model.transform.forward = Vector3.Slerp(model.transform.forward, pInput.planarVec, 0.25f); ;
        }
        if(!lockPlanar)
            movingVec = pInput.Dirmag * model.transform.forward * walkSpeed * ((pInput.run)?runMultiplier:1.0f);

	}

    void FixedUpdate()
    {
        //rig.position += movingVec * Time.fixedDeltaTime;
        //rig.velocity = movingVec;//并未算入地心引力
        rig.velocity = new Vector3(movingVec.x, rig.velocity.y, movingVec.z);
    }


    public void OnJumpEnter()
    {
        pInput.interactive = false;
        lockPlanar = true;
    }

    public void OnJumpExit()
    {
        pInput.interactive = true;
        lockPlanar = false;
    }


//class end
}

