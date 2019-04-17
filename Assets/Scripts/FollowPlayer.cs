using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
	private Transform player;
	private Vector3 offsetPosition;
	private bool isRotating = false; 
	public float distance = 0;
	public float scrollSpeed = 1;
	public float rotateSpeed = 1;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag (Tags.player).transform;
		transform.LookAt (player.position);
		offsetPosition = transform.position - player.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = offsetPosition + player.position;
		RotateView ();
		ScrollView ();
	}
	void ScrollView(){
		//print (Input.GetAxis ("Mouse ScrollWheel"));
		distance = offsetPosition.magnitude;
		distance += Input.GetAxis ("Mouse ScrollWheel") * scrollSpeed;
		distance = Mathf.Clamp (distance, 3, 18);
		offsetPosition = offsetPosition.normalized * distance;
		}
	void RotateView(){
		//Input.GetAxis ("Mouse X");
		//Input.GetAxis ("Mouse Y");
		if(Input.GetMouseButtonDown(1)){
			isRotating = true;
		}
		if(Input.GetMouseButtonUp (1)){
			isRotating = false;
		}
		if (isRotating) {
			transform.RotateAround (player.position,player.up,Input.GetAxis ("Mouse X")*rotateSpeed);
			Vector3 originalPos = transform.position;
			Quaternion originalRotation = transform.rotation;
			transform.RotateAround (player.position,transform.right,Input.GetAxis ("Mouse Y")*-rotateSpeed);
			float x = transform.eulerAngles.x;
			if(x<10||x>80){
				transform.position = originalPos;
				transform.rotation = originalRotation;
			}
			//transform.RotateAround (player.position,transform.right,Input.GetAxis ("Mouse Y")*-rotateSpeed);
		}
		offsetPosition = transform.position - player.position;
	}
}
