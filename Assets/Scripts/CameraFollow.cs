using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField]
	private Transform player;
	private Vector3 followDirection;

	// Use this for initialization
	void Start () {
		followDirection = transform.position - player.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = Vector3.Lerp(transform.position, followDirection + player.position, Time.deltaTime * 10f);
	}
}
