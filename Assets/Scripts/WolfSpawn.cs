using UnityEngine;
using System.Collections;

public class WolfSpawn : MonoBehaviour {
	public int maxnum = 5;
	private int currentnum = 0;
	public float time =3;
	private float timer = 0;
	public GameObject prefab;
	void Update(){
		if (currentnum < maxnum) {
			timer += Time.deltaTime;
			if(timer > time){
				Vector3 pos = transform.position;
				pos.x += Random.Range(-1,1);
				pos.z += Random.Range (-1,1);
				GameObject go = GameObject.Instantiate(prefab,pos,Quaternion.identity) as GameObject;
				go.GetComponent<WolfBaby>().spawn = this;
				timer = 0;
				currentnum++;
			}
		}
	}
	public void MinusNumber(){
		this.currentnum--;
	}
}
