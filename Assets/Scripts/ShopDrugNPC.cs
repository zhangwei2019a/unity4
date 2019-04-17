using UnityEngine;
using System.Collections;

public class ShopDrugNPC : NPC {
	public void OnMouseOver(){
		if (Input.GetMouseButtonDown (0)) {
			audio.Play();
			ShopDrug._instance.TransformState();
		}
	}
}
