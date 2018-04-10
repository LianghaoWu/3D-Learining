using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;


public class SSActionManager : System.Object {
	private static SSActionManager _instance;
	public static SSActionManager GetInstance() {
		if (_instance == null) _instance = new SSActionManager();
		return _instance;
	}

	public SSAction ApplyCCMoveToAction(GameObject obj, Vector3 target, float speed, ISSActionCallback completed) {
		CCMoveToAction ac = obj.AddComponent<CCMoveToAction>();
		ac.RunAction(target, speed, completed);
		return ac;
	}

	public SSAction ApplyCCMoveToAction(GameObject obj, Vector3 target, float speed) {
		return ApplyCCMoveToAction(obj, target, speed, null);
	}

	public SSAction ApplyCCMoveToYZAction(GameObject obj, Vector3 target, float speed, ISSActionCallback completed) {
		CCMoveToYZAction ac = obj.AddComponent<CCMoveToYZAction>();
		ac.RunAction(obj, target, speed, completed);
		return ac;
	}

	public SSAction ApplyCCMoveToYZAction(GameObject obj, Vector3 target, float speed) {
		return ApplyCCMoveToYZAction(obj, target, speed, null);
	}
}