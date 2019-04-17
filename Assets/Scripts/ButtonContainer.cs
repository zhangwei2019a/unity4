using UnityEngine;
using System.Collections;

public class ButtonContainer : MonoBehaviour {
	public void OnNewGame(){
		PlayerPrefs.SetInt ("DataFromSave",1);
		Application.LoadLevel (1);
		}
	public void OnLoadGame(){
		PlayerPrefs.SetInt ("DataFromSave",0);
		}
}
