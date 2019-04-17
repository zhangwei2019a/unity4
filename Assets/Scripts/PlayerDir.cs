using UnityEngine;
using System.Collections;

public class PlayerDir : MonoBehaviour {
	public Vector3 targetPosition = Vector3.zero;
	public GameObject effect_click_prefab;
	private bool isMoving = false;
	private PlayerMove playermove;
	private PlayerAttack attack;
	void Start(){
		targetPosition = transform.position;
		playermove = this.GetComponent<PlayerMove>();
		attack = this.GetComponent<PlayerAttack> ();
		}
	void Update () {
		if (attack.state == PlayerState.Death) return;
		if (attack.isLockingTarget == false && Input.GetMouseButtonDown (0)&&UICamera.hoveredObject == null) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		    RaycastHit hitInfo;		
		    bool isCollider = Physics.Raycast (ray, out hitInfo);
			if (isCollider && hitInfo.collider.tag == Tags.ground) {
			isMoving = true;
			ShowClickEffect (hitInfo.point); 
			LookAtTarget(hitInfo.point);
		    }
		}
		if(Input.GetMouseButtonUp(0)){
			isMoving = false;
		}
		if (isMoving) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;
			bool isCollider = Physics.Raycast (ray, out hitInfo);
			if (isCollider && hitInfo.collider.tag == Tags.ground) {
				LookAtTarget(hitInfo.point);
			}else{
				if(playermove.isMoving){
					LookAtTarget(targetPosition);
				}
			}
				}
		}

	void ShowClickEffect( Vector3 hitPoint ) {
		hitPoint = new Vector3( hitPoint.x,hitPoint.y + 0.1f ,hitPoint.z );
		GameObject.Instantiate(effect_click_prefab, hitPoint, Quaternion.identity);
	}
	void LookAtTarget(Vector3 hitPoint){
		targetPosition = hitPoint;
		targetPosition = new Vector3(targetPosition.x,transform.position.y,targetPosition.z);
		this.transform.LookAt (targetPosition);
		}
}
