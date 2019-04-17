using UnityEngine;
using System.Collections;
public enum ShortCutType{
	Skill,
	Drug,
	None
}
public class ShortCutGrid : MonoBehaviour {
	public KeyCode keyCode;
	private ShortCutType type = ShortCutType.None;
	private UISprite icon;
	private int id;
	private SkillInfo skillInfo;
	private ObjectInfo objectInfo;
	private PlayerStatus ps;
	private PlayerAttack pa;
	void Awake(){
		icon = transform.Find ("icon").GetComponent<UISprite> ();
		icon.gameObject.SetActive (false);
	}
	void Start(){
		ps = GameObject.FindGameObjectWithTag (Tags.player).GetComponent<PlayerStatus>();
		pa = GameObject.FindGameObjectWithTag (Tags.player).GetComponent<PlayerAttack> ();
	}
	void Update(){
		if (Input.GetKeyDown (keyCode)) {
			if(type == ShortCutType.Drug){
				OnDrugUse();
			}else if(type == ShortCutType.Skill){
				bool success = ps.TakeMP(skillInfo.mp);
				if(success == false){

				}else{
					pa.UseSkill(skillInfo);
				}
			}
		}
	}
	public void SetSkill(int id){
		this.id = id;
		this.skillInfo = SkillsInfo._instance.GetSkillInfoById (id);
		icon.gameObject.SetActive (true);
		icon.spriteName = skillInfo.icon_name;
		type = ShortCutType.Skill;
	}
	public void SetInventory(int id){
		this.id = id;
		objectInfo = ObjectsInfo._instance.GetObjectInfoById (id);
		if (objectInfo.type == ObjectType.Drug) {
			icon.gameObject.SetActive (true);
			icon.spriteName = objectInfo.icon_name;
			type = ShortCutType.Drug;
		}
	}
	public void OnDrugUse(){
		bool success = Inventory._instance.MinusId (id, 1);
		if (success) {
			ps.GetDrug (objectInfo.hp, objectInfo.mp);
		}
	}
}
