using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum PlayerState{
	ControlWalk,
	NormalAttack,
	SkillAttack, 
	Death
}
public enum AttackState{
	Moving,
	Idle,
	Attack
}
public class PlayerAttack : MonoBehaviour {

	public PlayerState state = PlayerState.ControlWalk;
	public AttackState attack_state = AttackState.Idle;

	public string aniname_normalattack;
	public string aniname_idle;
	public string aniname_now;
	public float time_normalattack;
	public float rate_normalattack;
	private float timer = 0;
	public float min_distance = 5;
	private Transform target_normalattack;

	private PlayerMove move;
	public GameObject effect;
	private bool showEffect = false;
	private PlayerStatus ps;
	public float miss_rate = 0.25f;
	private GameObject hudtextGo;
	public GameObject hudtextPrefab;
	private HUDText hudtext;
	private GameObject hudtextFollow;
	public AudioClip miss_sound;
	public GameObject body;
	private Color normal;
	public string aniname_death;
	public GameObject[] efxArray;
	public bool isLockingTarget = false;
	private SkillInfo info = null;
	public static PlayerAttack _instance;

	private Dictionary<string,GameObject> efxDict = new Dictionary<string,GameObject>();
	void Awake(){
		_instance = this;
		move = this.GetComponent<PlayerMove> ();
		ps = this.GetComponent<PlayerStatus> ();
		hudtextFollow = transform.Find ("HUDText").gameObject;
		normal = body.renderer.material.color;
		foreach (GameObject go in efxArray) {
			efxDict.Add(go.name,go);
		}
	}
	void Start(){
		hudtextGo = NGUITools.AddChild (HUDTextParent._instance.gameObject, hudtextPrefab);
		hudtext = hudtextGo.GetComponent<HUDText> ();

		UIFollowTarget followTarget = hudtextGo.GetComponent<UIFollowTarget> ();
        followTarget.target = hudtextFollow.transform;
        followTarget.gameCamera = Camera.main;

	}
	void Update(){
		if (isLockingTarget == false && Input.GetMouseButtonDown (0) && state != PlayerState.Death) {
						Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						RaycastHit hitInfo;
						bool isCollider = Physics.Raycast (ray, out hitInfo);
						if (isCollider && hitInfo.collider.tag == Tags.enemy) {
								target_normalattack = hitInfo.collider.transform;
								state = PlayerState.NormalAttack;
				                timer = 0;
				showEffect = false;
						} else {
								state = PlayerState.ControlWalk;
								target_normalattack = null;
						}
				}
		if (state == PlayerState.NormalAttack) {
			if(target_normalattack == null){
				state = PlayerState.ControlWalk;
			}else{
			float distance = Vector3.Distance(transform.position,target_normalattack.position);
			if(distance <= min_distance){
				transform.LookAt(target_normalattack.position);
				attack_state = AttackState.Attack;
				timer += Time.deltaTime;
				animation.CrossFade(aniname_now);
				if(timer >= time_normalattack){
					aniname_now = aniname_idle;
					if(showEffect == false){
						showEffect = true;
						GameObject.Instantiate(effect,target_normalattack.position,Quaternion.identity);
						target_normalattack.GetComponent<WolfBaby>().TakeDamage(GetAttack());
					}
				}
				if(timer >= (1/rate_normalattack)){
					timer = 0;
					showEffect = false;
					aniname_now = aniname_normalattack;
				}
			}else{
				attack_state = AttackState.Moving;
				move.SimpleMove(target_normalattack.position);
			}
			}
		}else if(state == PlayerState.Death){
			animation.CrossFade(aniname_death);
		}
		if (isLockingTarget && Input.GetMouseButtonDown (0)) {
			OnLockTarget();
		}
	}
	public int GetAttack(){
		return (int)(EquipmentUI._instance.attack + ps.attack + ps.attack_plus);
	}
	public void TakeDamage(int attack){
		if (state == PlayerState.Death)
			return;
		float def = EquipmentUI._instance.def + ps.def_plus + ps.def;
		float temp = attack * ((200 - def) / 200);
		if (temp < 1)
			temp = 1;
		float value = Random.Range (0f, 1f);
		if (value < miss_rate) {
			AudioSource.PlayClipAtPoint(miss_sound,transform.position);
			hudtext.Add("MISS",Color.gray,1);
		} else {
			hudtext.Add ("-" + temp,Color.red,1);
			ps.hp_remain -= (int)temp;
			StartCoroutine(ShowBodyRed());
			if(ps.hp_remain <= 0){
				state = PlayerState.Death;
			}
		}
		HeadStatusUI._instance.UpdateShow();
	}
	IEnumerator ShowBodyRed(){
		body.renderer.material.color = Color.red;
		yield return new WaitForSeconds (1f);
		body.renderer.material.color = normal;
	}
	void OnDestroy(){
		GameObject.Destroy (hudtextGo);
	}
	public void UseSkill(SkillInfo info){
		if (ps.heroType == HeroType.Magician) {
			if(info.applicableRole == ApplicableRole.Swordman){
				return;
			}
		}
		if (ps.heroType == HeroType.Swordman) {
			if(info.applicableRole == ApplicableRole.Magician){
				return;
			}
		}
		switch (info.applyType) {
		case ApplyType.Passive:
			StartCoroutine(OnPasslveSkillUse(info));
			break;
		case ApplyType.Buff:
			StartCoroutine(OnBuffSkillUse(info));
			break;
		case ApplyType.SingleTarget:
            OnSingleTargetSkillUse(info);
			break;
		case ApplyType.MultiTarget:
			OnMuitiTargetSkillUse(info);
			break;
		}
	}
	//
	IEnumerator OnPasslveSkillUse(SkillInfo info){
		state = PlayerState.SkillAttack;
		animation.CrossFade (info.aniname);
		yield return new WaitForSeconds (info.anitime);
		state = PlayerState.ControlWalk;
		int hp = 0;
		int mp = 0;
		if (info.applyProperty == ApplyProperty.HP) {
			hp = info.applyValue;
				} else if(info.applyProperty == ApplyProperty.MP){
			mp = info.applyValue;
				}
		ps.GetDrug (hp,mp);
		GameObject prefab = null;
		efxDict.TryGetValue (info.efx_name, out prefab);
		GameObject.Instantiate (prefab, transform.position,Quaternion.identity);
	}
	//
	IEnumerator OnBuffSkillUse(SkillInfo info){
		state = PlayerState.SkillAttack;
		animation.CrossFade (info.aniname);
		yield return new WaitForSeconds (info.anitime);
		state = PlayerState.ControlWalk;

		GameObject prefab = null;
		efxDict.TryGetValue (info.efx_name, out prefab);
		GameObject.Instantiate (prefab, transform.position,Quaternion.identity);

		switch (info.applyProperty) {
		case ApplyProperty.Attack:
			ps.attack *= (info.applyValue/100f);
			break;
		case ApplyProperty.AttackSpeed:
			rate_normalattack *= (info.applyValue/100f);
			break;
		case ApplyProperty.Def:
			ps.def *= (info.applyValue/100f);
			break;
		case ApplyProperty.Speed:
			move.speed *= (info.applyValue/100f);
			break;
		}
		yield return new WaitForSeconds (info.applyTime);
		switch (info.applyProperty) {
		case ApplyProperty.Attack:
			ps.attack *= (info.applyValue*100f);
			break;
		case ApplyProperty.AttackSpeed:
			rate_normalattack *= (info.applyValue*100f);
			break;
		case ApplyProperty.Def:
			ps.def *= (info.applyValue*100f);
			break;
		case ApplyProperty.Speed:
			move.speed *= (info.applyValue*100f);
			break;
		}
	}
	//
	void OnSingleTargetSkillUse(SkillInfo info){
		CursorManager._instance.SetLockTarget ();
		state = PlayerState.SkillAttack;
		isLockingTarget = true;
		this.info = info;
	}
	void OnLockTarget(){
		isLockingTarget = false;
		switch (info.applyType) {
		case ApplyType.SingleTarget:
			StartCoroutine(OnLockSingleTarget());
			break;
		case ApplyType.MultiTarget:
			StartCoroutine(OnLockMultiTarget());
			break;
		}
	}
	IEnumerator OnLockSingleTarget(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;
		bool isCollider = Physics.Raycast (ray, out hitInfo);
		if (isCollider && hitInfo.collider.tag == Tags.enemy) {
			animation.CrossFade (info.aniname);
			yield return new WaitForSeconds (info.anitime);
			state = PlayerState.ControlWalk;

			GameObject prefab = null;
			efxDict.TryGetValue (info.efx_name, out prefab);
			GameObject.Instantiate (prefab, hitInfo.collider.transform.position,Quaternion.identity);

			hitInfo.collider.GetComponent<WolfBaby>().TakeDamage((int)(GetAttack()*(info.applyValue/100f)));
		}else{
			state = PlayerState.NormalAttack;
		}
		CursorManager._instance.SetNormal ();
	}
	//
	IEnumerator OnLockMultiTarget(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;
		bool isCollider = Physics.Raycast (ray, out hitInfo,9);
		if (isCollider) {
			animation.CrossFade (info.aniname);
			yield return new WaitForSeconds (info.anitime);
			state = PlayerState.ControlWalk;

			GameObject prefab = null;
			efxDict.TryGetValue (info.efx_name, out prefab);
			GameObject go = GameObject.Instantiate (prefab, hitInfo.point + Vector3.up * 0.5f,Quaternion.identity) as GameObject;
			go.GetComponent<MagicSphere>().attack = GetAttack()*(int)(info.applyValue / 100f);

		}else{
			state = PlayerState.ControlWalk;
		}
		CursorManager._instance.SetNormal ();
	}
	//
	void OnMuitiTargetSkillUse(SkillInfo info){
		CursorManager._instance.SetLockTarget ();
		state = PlayerState.SkillAttack;
		isLockingTarget = true;
		this.info = info;
	}
}
