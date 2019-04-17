using UnityEngine;
using System.Collections;

public class ShopWeaponUI : MonoBehaviour {
	
	public static ShopWeaponUI _instance;
	public int[] weaponidArray;
	public UIGrid grid;
	public GameObject weaponItem;
	private TweenPosition tween;
	private bool isShow = false;
	private GameObject numberDialog;
	private UIInput numberInput;
	private int buyid = 0;
	
	void Awake() {
		_instance = this;
		tween = this.GetComponent<TweenPosition>();
		numberDialog = transform.Find("Panel/NumberDialog").gameObject;
		numberInput = transform.Find ("Panel/NumberDialog/NumberInput").GetComponent<UIInput> ();
		numberDialog.SetActive (false);
	}

	void Start() {
		InitShopWeapon();
	}
	
	public void TransformState() {
		if (isShow) {
			tween.PlayReverse(); isShow = false;
		} else {
			tween.PlayForward(); isShow = true;
		}
	}
	
	public void OnCloseButtonClick() {
		TransformState();
	}
	
	void InitShopWeapon() {//初始化武器商店的信息
		foreach (int id in weaponidArray) {
			GameObject itemGo = NGUITools.AddChild(grid.gameObject, weaponItem);
			grid.AddChild(itemGo.transform);
			itemGo.GetComponent<ShopWeaponItem>().SetId(id);
		}
	}	
	public void OnOkBtnClick(){
		int count = int.Parse (numberInput.value);
		if (count>0) {
			int price = ObjectsInfo._instance.GetObjectInfoById (buyid).price_buy;
			int total_price = price * count;
			bool success = Inventory._instance.GetCoin (total_price);
			if (success) {
				Inventory._instance.GetId(buyid,count);
			}
		}
		buyid = 0;
		numberInput.value = "0";
		numberDialog.SetActive (false);
	}
	public void OnBuyClick(int id){
		buyid = id;
		numberDialog.SetActive (true);
		numberInput.value = "0";

	}
	public void OnClick(){
		numberDialog.SetActive (false);
	}
}

