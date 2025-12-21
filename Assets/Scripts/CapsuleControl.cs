using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleControl : MonoBehaviour {

	private Rigidbody rigidBody;
	private float inputX;
	private float inputY;

    [SerializeField]
	private float moveSpeed;

	private Vector3 moveVector;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		inputX = Input.GetAxis("Horizontal");
		inputY = Input.GetAxis("Vertical");
		
		moveVector = transform.forward * inputY;
		moveVector += transform.right * inputX;
		moveVector *= moveSpeed;

	}

	void FixedUpdate()
	{
		rigidBody.velocity = moveVector;
	}
}
