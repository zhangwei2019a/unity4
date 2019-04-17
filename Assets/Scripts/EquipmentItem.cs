using UnityEngine;
using System.Collections;

public class EquipmentItem : MonoBehaviour {
	private UISprite sprite;
	public int id;
	private bool isHover = false;
	void Awake(){
		sprite = this.GetComponent<UISprite> ();
	}
	void Update(){
		if (isHover) {
			if(Input.GetMouseButtonDown(1)){
				EquipmentUI._instance.TakeOff(id,this.gameObject);
			}
		}
	}
	public void SetId(int id){
		this.id = id;
		ObjectInfo info = ObjectsInfo._instance.GetObjectInfoById (id);
		SetInfo (info);
	}
	public void SetInfo(ObjectInfo info){
		this.id = info.id;
		sprite.spriteName = info.icon_name;
	}
	public void OnHover(bool isOver){
		isHover = isOver;
	}
}
