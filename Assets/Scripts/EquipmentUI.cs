using UnityEngine;
using System.Collections;

public class EquipmentUI : MonoBehaviour {
	public static EquipmentUI _instance;
	private TweenPosition tween;
	private bool isShow = false;
	private GameObject headgear;
	private GameObject armor;
	private GameObject rightHand;
	private GameObject leftHand;
	private GameObject shoe;
	private GameObject accessory;
	private PlayerStatus ps;
	public GameObject equipmentItem;
	public int attack = 0;
	public int def = 0;
	public int speed = 0;
	void Awake(){
		_instance = this;
		tween = this.GetComponent<TweenPosition> ();
		headgear = transform.Find ("Headgear").gameObject;
		armor = transform.Find ("Armor").gameObject;
		rightHand = transform.Find ("RightHand").gameObject;
		leftHand = transform.Find ("LeftHand").gameObject;
		shoe = transform.Find ("Shoe").gameObject;
		accessory = transform.Find ("Accessory").gameObject;

		ps = GameObject.FindGameObjectWithTag (Tags.player).GetComponent<PlayerStatus>();
	}
	public void TransformState(){
		if (isShow == false) {
			tween.PlayForward ();
			isShow = true;
	    } else {
			tween.PlayReverse();
			isShow = false;
		}

	}
	public bool Dress(int id){
		ObjectInfo info = ObjectsInfo._instance.GetObjectInfoById (id);
		if (info.type != ObjectType.Equip) {
			return false;
		}
		//if (ps.heroType == HeroType.Magician) {
			//if(info.applicationType == ApplicationType.Swordman){
			//	return false;
			//}
		//}
		//if (ps.heroType == HeroType.Swordman) {
		//	if(info.applicationType == ApplicationType.Magician){
		//		return false;
		//	}
		//}

		GameObject parent = null;
	    switch (info.dressType) {
		    case DressType.Headgear:
			    parent = headgear;
			    break;
		    case DressType.Armor:
			    parent = armor;
			    break;
		    case DressType.RightHand:
			    parent = rightHand;
			    break;
		    case DressType.LeftHand:
			    parent = leftHand;
			    break;
		    case DressType.Shoe:
			    parent = shoe;
			    break;
		    case DressType.Accessory:
			    parent = accessory;
			    break;
		}
		EquipmentItem item = parent.GetComponentInChildren<EquipmentItem> ();
		if (item != null) {
			Inventory._instance.GetId(item.id,1);
			item.SetInfo(info);
		} else {
			GameObject itemGo = NGUITools.AddChild(parent,equipmentItem);
			itemGo.transform.localPosition = Vector3.zero;
			itemGo.GetComponent<EquipmentItem>().SetInfo(info);
		}
		UpdateProperty ();
		return true;
	}
	public void TakeOff(int id,GameObject go){
		Inventory._instance.GetId(id,1);
		GameObject.Destroy(go);
		UpdateProperty ();
	}
	void UpdateProperty(){
		this.attack = 0;
		this.def = 0;
		this.speed = 0;
		EquipmentItem headgearItem = headgear.GetComponentInChildren<EquipmentItem> ();
		PlusProperty (headgearItem);
		EquipmentItem armorItem = armor.GetComponentInChildren<EquipmentItem> ();
		PlusProperty (armorItem);
		EquipmentItem leftHandItem = leftHand.GetComponentInChildren<EquipmentItem> ();
		PlusProperty (leftHandItem);
		EquipmentItem rightHandItem = rightHand.GetComponentInChildren<EquipmentItem> ();
		PlusProperty (rightHandItem);
		EquipmentItem shoeItem = shoe.GetComponentInChildren<EquipmentItem> ();
		PlusProperty (shoeItem);
		EquipmentItem accessoryItem = accessory.GetComponentInChildren<EquipmentItem> ();
		PlusProperty (accessoryItem);
		
	}
	void PlusProperty(EquipmentItem item){
		if (item != null) {
			ObjectInfo equipInfo = ObjectsInfo._instance.GetObjectInfoById (item.id);
			this.attack += equipInfo.attack;
			this.def += equipInfo.def;
			this.speed += equipInfo.speed;
		}
	}
}
