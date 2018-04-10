using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class CCMoveToYZAction : SSAction, ISSActionCallback {
	public GameObject obj;
	public Vector3 target;
	public float speed;

	private ISSActionCallback monitor = null;

	public void RunAction(GameObject obj, Vector3 target, float speed, ISSActionCallback monitor) {
		this.obj = obj;
		this.target = target;
		this.speed = speed;
		this.monitor = monitor;
		GameSceneController.GetInstance().setMoving(true);

		if (target.y < obj.transform.position.y) {
			Vector3 targetZ = new Vector3(target.x, obj.transform.position.y, target.z);
			SSActionManager.GetInstance().ApplyCCMoveToAction(obj, targetZ, speed, this);
		}
		else {
			Vector3 targetY = new Vector3(target.x, target.y, obj.transform.position.z);
			SSActionManager.GetInstance().ApplyCCMoveToAction(obj, targetY, speed, this);
		}
	}

	public void OnActionCompleted(SSAction action) {
		SSActionManager.GetInstance().ApplyCCMoveToAction(obj, target, speed, null);
	}

	void Update() {
		if (obj.transform.position == target) {
			GameSceneController.GetInstance().setMoving(false);
			if (monitor != null) monitor.OnActionCompleted(this);
			Destroy(this);
		}
	}
}