using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class name : ActorManager
/// description : 负责控制人物表演和接收其他Manager的信息
/// /// time : 2018.8.28
/// @author : 杨浩然
/// </summary>
public class ActorManager : MonoBehaviour {

    public BattleManager bm;

    public ActorController ac;
    public WeaponManager wm;
    public StateManager sm;

    void Awake() {
        GameObject sensor = transform.Find("sensor").gameObject;
        ac = GetComponent<ActorController>();
        bm = Bind<BattleManager>(sensor);
        wm = Bind<WeaponManager>(ac.model);
        sm = Bind<StateManager>(this.gameObject);

    }

    /// <summary>
    /// 给低权限管理员绑定ActorManager高权限管理员
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_gameObject">挂载管理员的物体</param>
    /// <returns></returns>
    private T Bind<T>(GameObject _gameObject) where T : IActorManagerInterface
    {
        T ret;
        ret = _gameObject.GetComponent<T>();
        if(ret == null)
        {
            _gameObject.AddComponent<T>();
        }
        ret.am = this;
        return ret;
    }
	

    public void TryDoDamage()
    {
        if(sm.isImmortal)
        {
            //Do Nothing
        }
        else if(sm.isDefense)
        {
            //attack should be blocked
            Blocked();
        }
        else
        {
            if(sm.HP <= 0.0f)
            {
                //already dead
            }
            else
            {
                sm.AddHP(-5.0f);
                if (sm.HP > 0)
                {
                    Hit();
                }
                else
                {
                    Die();
                }
            }
        }

    }

    public void Blocked()
    {
        ac.IssueTrigger("blocked");
    }

    public void Hit()
    {
        ac.IssueTrigger("hit");
    }

    public void Die()
    {
        ac.IssueTrigger("die");
        ac.pInput.interactive = false;
        if(ac.cameraController.lockState)
        {
            ac.cameraController.LockUnlock();
        }

        ac.cameraController.enabled = false;
    }


}
