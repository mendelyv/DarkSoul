using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour {

    public BattleManager bm;

    public ActorController ac;

	// Use this for initialization
	void Awake () {
        GameObject sensor = transform.Find("sensor").gameObject;
        ac = GetComponent<ActorController>();
        bm = sensor.GetComponent<BattleManager>();
        if(bm == null)
        {
            bm = sensor.AddComponent<BattleManager>();
        }
        bm.am = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DoDamage()
    {
        ac.IssueTrigger("hit");
    }


}
