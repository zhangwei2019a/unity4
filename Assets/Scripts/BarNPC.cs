using UnityEngine;
using System.Collections;

public class BarNPC : NPC {
	public static BarNPC _instance;
	public bool isInTask = false;
	public int killCount = 0;
	public TweenPosition questTween;
	public UILabel desLabel;
	public GameObject accepctBtnGo;
	public GameObject cancelBtnGo;
	public GameObject okBtnGo;
	private bool tag = false;
	private PlayerStatus status;
	void Awake(){
		_instance = this;
	}
	void ShowTaskProgress (){
		desLabel.text = "老爷爷:\n    你能帮我清理一下它们吗?\n    任务提示 ：\n    你已经杀死了" + killCount + "/10只食人鱼";
		okBtnGo.SetActive (true);
		accepctBtnGo.SetActive (false);
		cancelBtnGo.SetActive(false);
		}
	void ShowTaskDes(){
		desLabel.text = "老爷爷:\n    最近河里多了许多食人鱼,把我的金鱼都吃光了";
		okBtnGo.SetActive (false);
		accepctBtnGo.SetActive (true);
		cancelBtnGo.SetActive(true);
		}
	void OnMouseOver(){
		if (Input.GetMouseButtonDown (0)) {
			audio.Play();
			if(isInTask && tag == false){
				ShowTaskProgress();
			}
			else if(!isInTask && tag == false){
				ShowTaskDes();
			}
			else{
				desLabel.text = "谢谢你，给你1000当报酬吧\n什么？想要去冒险？传闻村子里有个镇村之宝,在村外的小屋里,你最好带上它再出发，不过那是战狼的领地，你最好小心点";
				okBtnGo.SetActive (false);
			}
			ShowQuest ();
				}
		}
	public void OnCloseButtonClick(){
		HideQuest ();
		}
	void HideQuest(){
		questTween.PlayReverse ();
		}
	void ShowQuest(){
		questTween.gameObject.SetActive (true);
		questTween.PlayForward ();
		}
	public void OnKillWolf(){
		if (isInTask) {
			killCount++;
		}
	}
	public void OnAcceptButtonClick(){
		ShowTaskProgress ();
		isInTask = true;
		}
	public void OnOkButtonClick(){
		if (killCount >= 10) {
			Inventory._instance.AddCoin(1000);
			desLabel.text = "谢谢你，给你1000当报酬吧\n什么？想要去冒险？传闻村子里有个镇村之宝,在村外的小屋里,你最好带上它再出发，不过那是战狼的领地，你最好小心点";
			okBtnGo.SetActive (false);
			tag = true;
		} else {
			HideQuest();
		}
	}
	public void OnCancelButtonClick(){
			HideQuest ();
	}
	// Use this for initialization
	void Start () {
		status = GameObject.FindGameObjectWithTag (Tags.player).GetComponent<PlayerStatus>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
