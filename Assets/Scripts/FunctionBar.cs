using UnityEngine;
using System.Collections;

public class FunctionBar : MonoBehaviour {
	public void OnStatusButtonClick(){
		Status._instance.TransformState ();
	}
	public void OnBagButtonClick(){
		Inventory._instance.TransformState ();
	}
	public void OnEquipButtonClick(){
		EquipmentUI._instance.TransformState ();
	}
	public void OnSkillButtonClick(){
		SkillUI._instance.TransformState ();
	}
	public void OnSettingButtonClick(){

		}
	/*void Update(){
		if (Input.GetKeyDown (KeyCode.X)) {
			Inventory._instance.GetId(Random.Range(1001,1004));
		}
	}*/
}
