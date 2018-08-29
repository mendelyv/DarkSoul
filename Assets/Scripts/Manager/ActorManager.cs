using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour {

    public BattleManager bm;

    

	// Use this for initialization
	void Awake () {
        GameObject sensor = transform.Find("sensor").gameObject;
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
}
