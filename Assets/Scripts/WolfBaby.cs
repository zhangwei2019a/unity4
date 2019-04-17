using UnityEngine;
using System.Collections;
public enum WolfState{
	Idle,
	Walk,
	Attack,
	Death
}
public class WolfBaby : MonoBehaviour {
	public WolfState state = WolfState.Idle;
	public string aniname_death;
	public string aniname_now;
	public string aniname_walk;
	public string aniname_idle;
	public float time = 1;
	private float timer = 0;
	private CharacterController cc;
	public float speed = 1;
	public int hp = 100;
	public int attack = 10;
	public int exp = 20;
	public float miss_rate = 0.2f;
	private Color normal;
	public AudioClip miss_sound;
	private GameObject hudtextFollow;
	private GameObject hudtextGo;
	public GameObject hudtextPrefab;
	public GameObject body;
	public HUDText hudtext;
	private UIFollowTarget followTarget;
	public string aniname_normalattack;
	public float time_normalattack;

	public string aniname_crazyattack;
	public float crazyattack_rate;
	public float time_crazyattack;

	public string aniname_attack_now;
	public int attack_rate = 1;
	private float attack_timer = 0;

	public Transform target;
	public float minDistance = 2;
	public float maxDistance = 5;

	public WolfSpawn spawn;
	private PlayerStatus ps;
	void Awake(){
		aniname_now = aniname_idle;
		//body = transform.Find ("Wolf_Baby").gameObject;
		cc = this.GetComponent<CharacterController> ();
		normal = body.renderer.material.color;
		hudtextFollow = transform.Find ("HUDText").gameObject;
	}
	void Start(){
		//hudtextGo = GameObject.Instantiate (hudtextPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		//hudtextGo.transform.parent = HUDTextParent._instance.gameObject.transform;
		hudtextGo = NGUITools.AddChild (HUDTextParent._instance.gameObject, hudtextPrefab);
		hudtext = hudtextGo.GetComponent<HUDText>();
		followTarget = hudtextGo.GetComponent<UIFollowTarget> ();
		followTarget.target = hudtextFollow.transform;
		followTarget.gameCamera = Camera.main;
		//followTarget.uiCamera = UICamera.current.GetComponent<Camera>();
		ps = GameObject.FindGameObjectWithTag (Tags.player).GetComponent<PlayerStatus>();
	}
	void Update(){
		if (state == WolfState.Death) {
						animation.CrossFade (aniname_death);
				} else if (state == WolfState.Attack) {
			AutoAttack();
				} else {
			animation.CrossFade(aniname_now);
			if(aniname_now == aniname_walk){
				cc.SimpleMove(transform.forward*speed);
			}
			timer += Time.deltaTime;
			if(timer>time){
				timer = 0;
				RandomState();
			}
				}
	}
	void AutoAttack(){
		if (target != null) {
			PlayerState playerState = target.GetComponent<PlayerAttack>().state;
			if(playerState == PlayerState.Death){
				target = null;
				playerState = PlayerState.Death;
				return;
			}
			float distance = Vector3.Distance(target.position,transform.position);
			if(distance > maxDistance){
				target = null;
				state = WolfState.Idle;
			}else if(distance<=minDistance){
				attack_timer +=  Time.deltaTime;
				animation.CrossFade(aniname_attack_now);
				if(aniname_attack_now == aniname_normalattack){
					if(attack_timer > time_normalattack){
						target.GetComponent<PlayerAttack>().TakeDamage(attack);
						aniname_attack_now = aniname_idle;
					}
				}else if(aniname_attack_now == aniname_crazyattack){
					if(attack_timer > time_crazyattack){
						target.GetComponent<PlayerAttack>().TakeDamage(attack);
						aniname_attack_now = aniname_idle;
					}
				}
				if(attack_timer > (1f/attack_rate)){
					RandomAttack();
					attack_timer = 0;
				}
			}else{
				transform.LookAt(target);
				cc.SimpleMove(transform.forward*speed);
				animation.CrossFade(aniname_walk);
			}
				} else {
			state = WolfState.Idle;
				}
	}
	void RandomState(){
		int value = Random.Range (0,2);
		if (value == 0) {
						aniname_now = aniname_idle;
				} else {
			if(aniname_now != aniname_walk){
				transform.Rotate(transform.up*Random.Range(0,360));
			}
			aniname_now = aniname_walk;
				}
	}
	public void TakeDamage(int attack){
		if (state == WolfState.Death)
			return;
		state = WolfState.Attack;	
		target = GameObject.FindGameObjectWithTag (Tags.player).transform;
		float value = Random.Range (0f, 1f);
		if (value < miss_rate) {
			AudioSource.PlayClipAtPoint (miss_sound, transform.position);
			hudtext.Add ("Miss", Color.gray, 1);
		} else {
			hudtext.Add ("-" + attack, Color.red, 1);
			this.hp -= attack;
			StartCoroutine (ShowBodyRed ());
			if (hp <= 0) {
				state = WolfState.Death;
				Destroy (this.gameObject, 2);
			}
		}
		HeadStatusUI._instance.UpdateShow ();
	}
	IEnumerator ShowBodyRed(){
		body.renderer.material.color = Color.red;
		yield return new WaitForSeconds (1f);
		body.renderer.material.color = normal;
	}
	void RandomAttack(){
		float value = Random.Range (0f, 1f);
		if (value < crazyattack_rate) {
			aniname_attack_now = aniname_crazyattack;
				} else {
			aniname_attack_now = aniname_normalattack;
				}
	}
	void OnDestroy(){
		ps.GetExp (exp);
		BarNPC._instance.OnKillWolf ();
		spawn.MinusNumber ();
		GameObject.Destroy (hudtextGo);
	}
	void OnMouseEnter(){
		if(PlayerAttack._instance.isLockingTarget == false)
		CursorManager._instance.SetAttack ();
	}
	void OnMouseExit(){
		if(PlayerAttack._instance.isLockingTarget == false)
		CursorManager._instance.SetNormal ();
	}
}