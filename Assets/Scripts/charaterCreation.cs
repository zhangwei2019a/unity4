using UnityEngine;
using System.Collections;

public class charaterCreation : MonoBehaviour {
	public GameObject[] characterPrefabs;
	private GameObject[] characterGameObjects;
	private int selectedIndex = 0;
	private int length = 2;
	public UIInput nameInput;
	// Use this for initialization
	void Start () {
		length = characterPrefabs.Length;
		characterGameObjects = new GameObject[length];
		for (int i = 0; i<length; i++) {
			characterGameObjects[i] = GameObject.Instantiate (characterPrefabs[i],transform.position,transform.rotation) as GameObject;
				}
		UpdateCharacterShow ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void UpdateCharacterShow(){
		characterGameObjects [selectedIndex].SetActive (true);
		for (int i=0; i<length; i++) {
			if(i!=selectedIndex){
				characterGameObjects [i].SetActive (false);
			}
				}
		}
	public void OnNextButtonClick(){
		selectedIndex++;
		selectedIndex %= length;
		UpdateCharacterShow ();
		}
	public void OnpreButtonClick(){
		selectedIndex--;
		if (selectedIndex == -1) {
			selectedIndex = length - 1;
				}
		UpdateCharacterShow ();
		}
	public void OnOkButtonClick(){
		PlayerPrefs.SetInt ("SelectedCharacterIndex",selectedIndex);
		PlayerPrefs.SetString ("name", nameInput.value);
		Application.LoadLevel (2);
		}
}
