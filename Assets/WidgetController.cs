using UnityEngine;
using System.Collections;

public class WidgetController : MonoBehaviour {
	public float rollSpeed = 4.0f;
	public float rotateSpeed = 4.0f;
	
	private float moveHorz = 0.0f;
	private CharacterController controller;

	private Vector3 moveDiretion = Vector3.zero;
	private Vector3 rotateDirection = Vector3.zero;
	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void FixedUpdate(){
		float h = Input.GetAxis("Horizontal");//A D
		float v = Input.GetAxis("Vertical");//W S
		moveDiretion = new Vector3 (h,0,v);
		moveDiretion = transform.TransformDirection (moveDiretion);
		moveDiretion *= rollSpeed;

		moveHorz = Input.GetAxis ("Horizontal");
		if (moveHorz > 0) {
			rotateDirection = new Vector3(0,1,0);
		}else if(moveHorz < 0){
			rotateDirection = new Vector3(0,-1,0);
		}else{
			rotateDirection = new Vector3(0,0,0);
		}

		controller.Move(moveDiretion*Time.deltaTime);
		controller.transform.Rotate (rotateDirection * Time.deltaTime, rotateSpeed);
	}
}
