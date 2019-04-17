using UnityEngine;
using System.Collections;

public class SkillItemIcon : UIDragDropItem {
	
	private int skillId;
	
	protected override void OnDragDropStart() {//在克隆的icon上调用的
		base.OnDragDropStart();
		skillId = transform.parent.GetComponent<SkillItem>().id;
		transform.parent = transform.root;
		this.GetComponent<UISprite>().depth = 40;
	}
	
	protected override void OnDragDropRelease(GameObject surface) {
		base.OnDragDropRelease(surface);
		if (surface != null && surface.tag == Tags.shortcut) {//当一个技能拖到了快捷方式上的时候
			surface.GetComponent<ShortCutGrid>().SetSkill(skillId);
		}
	}
}

