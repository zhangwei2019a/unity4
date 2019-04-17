using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Inventory : MonoBehaviour {
	public static Inventory _instance;
	private TweenPosition tween;
	private int coinCount = 1000;
	public List<InventoryItemGrid> itemGridList = new List<InventoryItemGrid>();
	public UILabel coinNumberLabel;
	public GameObject inventoryItem;
	void Awake(){
		_instance = this;
		tween = this.GetComponent<TweenPosition> ();
		//tween.AddOnFinished (this.OnTweenPlayFinished);
	}
    void Update(){
		if (Input.GetKeyDown (KeyCode.X)) {
			GetId(Random.Range(2001,2023),1);
		}
	}
	public void GetId(int id,int count){
		InventoryItemGrid grid = null;
		foreach (InventoryItemGrid temp in itemGridList) {
			if(temp.id == id){
				grid = temp;
				break;
			}
		}
		if (grid != null) {
			grid.PlusNumber(count);
		} else {
			foreach(InventoryItemGrid temp in itemGridList){
				if(temp.id == 0){
					grid = temp;
					break;
				}
			}
			if(grid != null){
				GameObject itemGo = NGUITools.AddChild(grid.gameObject,inventoryItem);
				itemGo.transform.localPosition = Vector3.zero;
				itemGo.GetComponent<UISprite>().depth = 4;
				grid.SetId(id,count);
			}
		}
	}
	private bool isShow = false;
	void Show(){
		isShow = true;
		tween.PlayForward ();
	}
	void Hide(){
		isShow = false;
		tween.PlayReverse ();
	}
	/*void OnTweenPlayFinished(){
		if (isShow == false) {
			this.gameObject.SetActive(false);
		}
	}*/
	public void TransformState(){
		if (isShow == false) {
			Show ();
		} else {
			Hide();
		}
	}
	public void AddCoin(int count){
		coinCount += count;
		coinNumberLabel.text = coinCount.ToString();
	}
	public bool GetCoin(int count){
		if (coinCount >= count) {
			coinCount -= count;
			coinNumberLabel.text = coinCount.ToString();
			return true;
		}
		return false;
	}
	public bool MinusId(int id,int count){
		InventoryItemGrid grid = null;
		foreach (InventoryItemGrid temp in itemGridList) {
			if(temp.id == id){
				grid = temp;
				break;
			}
		}
		if (grid == null) {
						return false;
				} else {
			bool isSuccess = grid.MinusNumber(count);
			return isSuccess;
				}
	}
}