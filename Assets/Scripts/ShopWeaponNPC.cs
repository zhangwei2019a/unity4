using UnityEngine;
using System.Collections;

public class ShopWeaponNPC : NPC {
	public void OnMouseOver(){
		if (Input.GetMouseButtonDown (0)) {
			audio.Play();
			ShopWeaponUI._instance.TransformState();
		}
	}

}
