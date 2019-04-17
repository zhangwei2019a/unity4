using UnityEngine;
using System.Collections;
public enum HeroType{
	Swordman,
	Magician,
	Common
}
public class PlayerStatus : MonoBehaviour {
	public HeroType heroType;
	public string name = "Dog.thunder";
	public int hp = 100;
	public int mp = 100;
	public float hp_remain = 100;
	public float mp_remain = 100;
	public int level = 1;
	public float exp = 0;
	public int coin = 200;
	public float attack = 20;
	public int attack_plus = 0;
	public float def = 20;
	public int def_plus = 0;
	public float speed = 20;
	public int speed_plus = 0;
	public int point_remain = 10;
	void Start(){
		GetExp (0);	
	}
	public void GetCoint(int count){
		coin += count;
		}
	public bool GetPoint(int point){
		if (point_remain >= point) {
			point_remain -= point;
			return true;
		}
		return false;
	}
	public void GetDrug(int hp,int mp){
		hp_remain += hp;
		mp_remain += mp;
		if (hp_remain > this.hp) {
			hp_remain = this.hp;
		}
		if (mp_remain > this.mp) {
			mp_remain = this.mp;
		}
		HeadStatusUI._instance.UpdateShow ();
	}
	public void GetExp(int exp){
		this.exp += exp;
		int total_exp = 100 + level * 30;
		while(this.exp >= total_exp) {
			this.level++;
			point_remain ++;
			this.exp -= total_exp;
			total_exp = 100 + level * 30;
		}
		ExpBar._instance.SetValue (this.exp/total_exp);
	}
	public bool TakeMP(int count){
		if (mp_remain >= count) {
			mp_remain -= count;
			HeadStatusUI._instance.UpdateShow();
			return true;
				} else {
			return false;
				}
	}
}
