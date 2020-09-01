using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class CameraFollow : MonoBehaviour {


    public Vector3 distance = new Vector3(0,8,-18);

    public Camera camera;

    public Vector3 offset = new Vector3(0,5f,0);

    public float speed = 3f;
	// Use this for initialization
	void Start () {
        camera = Camera.main;
        Vector3 pos = transform.position;
        Vector3 forward = transform.forward;
        Vector3 initPos = pos - 30 * forward + Vector3.up * 10;
        camera.transform.position = initPos;

    }


    void LateUpdate()
    {
        Vector3 pos = transform.position;
        Vector3 forward = transform.forward;
        Vector3 targetPos = pos;
        targetPos = pos + forward * distance.z;
        targetPos.y += distance.y;
        Vector3 cameraPos = camera.transform.position;
        cameraPos = Vector3.MoveTowards(cameraPos, targetPos, Time.deltaTime * speed);
        camera.transform.position = cameraPos;
        camera.transform.LookAt(pos+offset);
    }
	
	// Update is called once per frame
	void Update () {
		
	}



}

