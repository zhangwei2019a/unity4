using UnityEngine;
using System.Collections;

public class PressAnyKey : MonoBehaviour {
	private bool isPressAnyKey = false;
	private GameObject button;
	// Use this for initialization
	void Start () {
		button = this.transform.parent.Find("button").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	if (isPressAnyKey == false){
						if (Input.anyKey) {
								ShowButton ();
						}
				}
	}
	void ShowButton(){
				button.SetActive (true);
				this.gameObject.SetActive (false);
				isPressAnyKey = true;
		}
}
