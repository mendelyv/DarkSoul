using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// class name : CameraController
/// description : 相机控制
/// time : 2018.7.3
/// @author : 杨浩然
/// </summary>
public class CameraController : MonoBehaviour {

    private IUserInput pInput;
    public float horizontalSpeed = 100.0f;
    public float verticalSpeed = 80.0f;
    public float cameraDampValue = 0.07f;//相机追赶的Damp时间值
    public Image lockDot;//锁定的提示点
    public bool isLock;

    private float tmpEulerX;//相机的X轴的欧拉度数
    //private float tmpEulerY;//相机的Y轴的欧拉度数
    private GameObject playerHandle;
    private GameObject cameraHandle;
    private GameObject model;
    private GameObject _camera;
    private LockTarget lockTarget;

    private Vector3 cameraDampVelocity;

	void Awake () {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tmpEulerX = 20.0f;
        ActorController ac = playerHandle.GetComponent<ActorController>();
        model = ac.model;
        pInput = ac.pInput;
        _camera = Camera.main.gameObject;
        //Cursor.lockState = CursorLockMode.Locked;//设置鼠标隐藏
        lockDot.enabled = false;
        isLock = false;
	}
	
    //由于模型的移动大部分使用的是物理引擎，所以相机的跟随也在物理引擎中做
	void FixedUpdate () {

        if(lockTarget == null)
        {
            Vector3 tmpModelEuler = model.transform.eulerAngles;//用于防止模型旋转的中间值

            playerHandle.transform.Rotate(Vector3.up, pInput.jRight * horizontalSpeed * Time.fixedDeltaTime);
            //使用减等于翻转方向
            tmpEulerX -= pInput.jUp * verticalSpeed * Time.fixedDeltaTime;
            tmpEulerX = Mathf.Clamp(tmpEulerX, -40.0f, 40.0f);
            cameraHandle.transform.localEulerAngles = new Vector3(tmpEulerX, 0, 0);

            model.transform.eulerAngles = tmpModelEuler;

        }
        else
        {
            Vector3 tempForward = lockTarget.obj.transform.position - model.transform.position;
            tempForward.y = 0;
            playerHandle.transform.forward = tempForward;
        }

        _camera.transform.position = Vector3.SmoothDamp(_camera.transform.position,
                                                        transform.position,
                                                        ref cameraDampVelocity,
                                                        cameraDampValue);
        //_camera.transform.position = transform.position;
        //_camera.transform.eulerAngles = transform.eulerAngles;
        _camera.transform.LookAt(cameraHandle.transform);
    }



    public void LockUnlock()
    {
        Vector3 modelOrigin1 = model.transform.position;
        Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
        Vector3 boxCenter = modelOrigin2 + model.transform.forward * 5.0f;
        Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5.0f), model.transform.rotation, LayerMask.GetMask("Enemy"));
        
        if(cols.Length == 0)
        {
            lockTarget = null;
            lockDot.enabled = false;
            isLock = false;
        }
        else
        {
            foreach (var item in cols)
            {
                if(lockTarget != null && lockTarget.obj == item.gameObject)
                {
                    lockTarget = null;
                    lockDot.enabled = false;
                    isLock = false;
                    break;
                }
                lockTarget = new LockTarget(item.gameObject,item.bounds.extents.y);
                lockDot.enabled = true;
                isLock = true;
                break;
            }
        }
        
    }

    private class LockTarget
    {
        public GameObject obj;
        public float halfHeight;

        public LockTarget(GameObject obj,float halfHeight)
        {
            this.obj = obj;
            this.halfHeight = halfHeight;
        }
    }


}
