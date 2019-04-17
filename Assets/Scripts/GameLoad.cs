using UnityEngine;
using System.Collections;

public class GameLoad : MonoBehaviour {
	public GameObject magicianPrefab;
	public GameObject swordmanPrefab;
	void Awake(){
		//PlayerPrefs.SetInt ("SelectedCharacterIndex",selectedIndex);
		//PlayerPrefs.SetString ("name", nameInput.value);
		int selectedIndex = PlayerPrefs.GetInt ("SelectedCharacterIndex");
		string name = PlayerPrefs.GetString ("name");
		GameObject go = null;
		if (selectedIndex == 0) {
			go = GameObject.Instantiate(magicianPrefab) as GameObject;
		}else if(selectedIndex == 1){
			go = GameObject.Instantiate(swordmanPrefab) as GameObject;
		}
		go.GetComponent<PlayerStatus> ().name = name;
	}
}
