using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public PlayerInput pInput;
    public float horizontalSpeed = 100.0f;
    public float verticalSpeed = 80.0f;

    private float tmpEulerX;
    private GameObject playerHandle;
    private GameObject cameraHandle;

	void Awake () {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tmpEulerX = 20.0f;
	}
	
	void Update () {
        playerHandle.transform.Rotate(Vector3.up, pInput.jRight * horizontalSpeed * Time.deltaTime);
        //使用减等于翻转方向
        tmpEulerX -= pInput.jUp * verticalSpeed * Time.deltaTime;
        tmpEulerX = Mathf.Clamp(tmpEulerX, -40.0f, 40.0f);
        cameraHandle.transform.localEulerAngles = new Vector3(tmpEulerX, 0, 0);
	}
}
