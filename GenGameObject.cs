using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mygame;

public class GenGameObject : MonoBehaviour {
	Stack<GameObject> priests_start = new Stack<GameObject>();
	Stack<GameObject> priests_end = new Stack<GameObject>();
	Stack<GameObject> devils_start = new Stack<GameObject>();
	Stack<GameObject> devils_end = new Stack<GameObject>();

	GameObject[] boat = new GameObject[2];
	GameObject boat_obj;
	int side = 1; 
	public float speed = 6f;
	float gap = 1.2f;

	Vector3 shore_start_position = new Vector3(-6, 0, 0);
	Vector3 shore_end_position = new Vector3(6, 0, 0);
	Vector3 boat_start_position = new Vector3(-1, 0, 0);
	Vector3 boat_end_position = new Vector3(1f, 0, 0);
	Vector3 priest_start_position = new Vector3(-5.7f, 1.5f, 0);
	Vector3 priest_end_Position = new Vector3(3.5f, 1.5f, 0);
	Vector3 devil_start_position = new Vector3(-9f, 1.5f, 0);
	Vector3 devil_end_position = new Vector3(7f, 1.5f, 0);

	void Start() {
		GameSceneController.GetInstance().setGenGameObject(this);
		Instantiate(Resources.Load("Prefabs/Shore"), shore_start_position, Quaternion.identity);
		Instantiate(Resources.Load("Prefabs/Shore"), shore_end_position, Quaternion.identity);
		boat_obj = Instantiate(Resources.Load("Prefabs/Boat"), boat_start_position, Quaternion.identity) as GameObject;
		for (int i = 0; i < 3; ++i) {
			GameObject priest = Instantiate(Resources.Load("Prefabs/Priest")) as GameObject;
			priest.transform.position = getCharacterPosition(priest_start_position, i);
			priests_start.Push(priest);
			GameObject devil = Instantiate(Resources.Load("Prefabs/Devil")) as GameObject;
			devil.transform.position = getCharacterPosition(devil_start_position, i);
			devils_start.Push(devil);
		}
	}
	int boatCapacity() {
		int capacity = 0;
		for (int i = 0; i < 2; ++i) {
			if (boat [i] == null) {
				capacity++;
			}
		}
		return capacity;
	}

	void getOnTheBoat(GameObject obj) {
		if (boatCapacity() != 0) {
			obj.transform.parent = boat_obj.transform;
			Vector3 target = new Vector3();
			if (boat[0] == null) {
				boat[0] = obj;
				target = boat_obj.transform.position + new Vector3(-0.8f, 1.2f, 0);
			}
			else {
				boat[1] = obj;
				target = boat_obj.transform.position + new Vector3(0.8f, 1.2f, 0);
			}
			SSActionManager.GetInstance().ApplyCCMoveToYZAction(obj, target, speed);
		}
	}

	public void moveBoat() {
		if (boatCapacity() != 2) {
			if (side == 1) {
				SSActionManager.GetInstance().ApplyCCMoveToAction(boat_obj, boat_end_position, speed);
				side = 2;
			}
			else if (side == 2) {
				SSActionManager.GetInstance().ApplyCCMoveToAction(boat_obj, boat_start_position, speed);
				side = 1;
			}
		}
	}

	public void getOffTheBoat(int bside) {
		if (boat[bside] != null) {
			boat[bside].transform.parent = null;
			Vector3 target = new Vector3();
			if (side == 1) {
				if (boat[bside].tag == "Priest") {
					priests_start.Push(boat[bside]);
					target = getCharacterPosition(priest_start_position, priests_start.Count - 1);
				}
				else if (boat[bside].tag == "Devil") {
					devils_start.Push(boat[bside]);
					target = getCharacterPosition(devil_start_position, devils_start.Count - 1);
				}
			}
			else if (side == 2) {
				if (boat[bside].tag == "Priest") {
					priests_end.Push(boat[bside]);
					target = getCharacterPosition(priest_end_Position, priests_end.Count - 1);
				}
				else if (boat[bside].tag == "Devil") {
					devils_end.Push(boat[bside]);
					target = getCharacterPosition(devil_end_position, devils_end.Count - 1);
				}
			}
			SSActionManager.GetInstance().ApplyCCMoveToYZAction(boat[bside], target, speed);
			boat[bside] = null;
		}
	}

	public void priestStartOnBoat() {
		if (priests_start.Count != 0 && boatCapacity () != 0 && side == 1) {
			getOnTheBoat(priests_start.Pop());
		}
	}

	public void priestEndOnBoat() {
		if (priests_end.Count != 0 && boatCapacity () != 0 && side == 2) {
			getOnTheBoat(priests_end.Pop());
		}
	}

	public void devilStartOnBoat() {
		if (devils_start.Count != 0 && boatCapacity () != 0 && side == 1) {
			getOnTheBoat(devils_start.Pop());
		}
	}

	public void devilEndOnBoat() {
		if (devils_end.Count != 0 && boatCapacity () != 0 && side == 2) {
			getOnTheBoat(devils_end.Pop());
		}
	}

	Vector3 getCharacterPosition(Vector3 pos, int index) { 
		return new Vector3(pos.x + gap * index, pos.y, pos.z);
	}

	void Update() {
		GameSceneController scene = GameSceneController.GetInstance();
		int pOnb = 0, dOnb = 0;
		int priests_s = 0, devils_s = 0, priests_e = 0, devils_e = 0;

		if (priests_end.Count == 3 && devils_end.Count == 3) {
			scene.setMessage("Win!");
			return;
		}

		for (int i = 0; i < 2; ++i) {
			if (boat[i] != null && boat[i].tag == "Priest") pOnb++;
			else if (boat[i] != null && boat[i].tag == "Devil") dOnb++;
		}
		if (side == 1) {
			priests_s = priests_start.Count + pOnb;
			devils_s = devils_start.Count + dOnb;
			priests_e = priests_end.Count;
			devils_e = devils_end.Count;
		}
		else if (side == 2) {
			priests_s = priests_start.Count;
			devils_s = devils_start.Count;
			priests_e = priests_end.Count + pOnb;
			devils_e = devils_end.Count + dOnb;
		}
		if ((priests_s != 0 && priests_s < devils_s) || (priests_e != 0 && priests_e < devils_e)) {
			scene.setMessage("Lose!");
		}
	}
}