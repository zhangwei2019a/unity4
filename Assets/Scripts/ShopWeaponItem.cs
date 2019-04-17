using UnityEngine;
using System.Collections;

public class ShopWeaponItem : MonoBehaviour {
	
	private int id;
	private ObjectInfo info;
	
	private UISprite icon_sprite;
	private UILabel name_label;
	private UILabel effect_label;
	private UILabel pricesell_label;
	
	void Awake() {
		icon_sprite = transform.Find("icon").GetComponent<UISprite>();
		name_label = transform.Find("name").GetComponent<UILabel>();
		effect_label = transform.Find("effect").GetComponent<UILabel>();
		pricesell_label = transform.Find("price_sell").GetComponent<UILabel>();
	}

	public void SetId(int id) {
		this.id = id;
		info = ObjectsInfo._instance.GetObjectInfoById(id);
		
		icon_sprite.spriteName = info.icon_name;
		name_label.text = info.name;
		if (info.attack > 0) {
			effect_label.text = "+伤害 " + info.attack;
		} else if (info.def > 0) {
			effect_label.text = "+防御 " + info.def;
		} else if(info.speed>0){
			effect_label.text = "+速度 " + info.speed;
		}
		pricesell_label.text = info.price_sell.ToString();
	}
	public void OnBuyClick(){
		ShopWeaponUI._instance.OnBuyClick (id);
	}
}
