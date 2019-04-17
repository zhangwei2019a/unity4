using UnityEngine;
using System.Collections;
public enum ControlWalkState{
	Moving,
	Idle
}
public class PlayerMove : MonoBehaviour {
	public bool isMoving = false;
	public float speed = 1;
	private PlayerDir dir;
	private CharacterController controller;
	public ControlWalkState state = ControlWalkState.Idle;
	private PlayerAttack attack;
	// Use this for initialization
	void Start () {
		dir = this.GetComponent<PlayerDir>();
		controller = this.GetComponent<CharacterController> ();
		attack = this.GetComponent<PlayerAttack> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (attack.state == PlayerState.ControlWalk) {
			float distance = Vector3.Distance (dir.targetPosition, transform.position);
			if (distance > 0.3f) {
				isMoving = true;
				state = ControlWalkState.Moving;
				controller.SimpleMove (transform.forward * speed);
			} else {
			    state = ControlWalkState.Idle;
				isMoving = false;
			}
		}
	}
	public void SimpleMove(Vector3 targetPos){
		transform.LookAt (targetPos);
		controller.SimpleMove (transform.forward * speed);
	}
}
