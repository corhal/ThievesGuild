using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thief : MonoBehaviour {

	public static Thief Instance;

	public int Gold;

	public int WalletGold;
	public int WalletCapacity;

	public KeyCode StealButton;
	public KeyCode StashButton;

	public List<Bystander> Bystanders;
	public List<Chest> Chests;

	public GameObject CanvasObject;
	public Image BullseyeImage;

	public Bystander CurrentTarget;

	void Awake () {
		if (Instance == null) {			
			Instance = this;
		} else if (Instance != this) {
			Destroy (gameObject);  
		}
		DontDestroyOnLoad(gameObject);
	}

	bool followingTarget;
	void Update () {
		if (Input.GetKeyDown(StashButton)) {
			Stash ();
		}

		if (Input.GetKeyDown(StealButton)) {
			Unstash ();
		}

		if (followingTarget && CurrentTarget.Gold > 0) {
			CanvasObject.transform.position = CurrentTarget.transform.position;
			BullseyeImage.transform.localScale = Vector3.one * Mathf.PingPong (Time.time, 1.0f);

			if (Input.GetKeyDown(StealButton)) {
				Steal ();
			}
		}
	}

	public void Steal () {
		if (BullseyeImage.transform.localScale.x > 0.35) {
			Debug.Log ("fail!");
			return;
		}
		int goldToSteal = Mathf.Min (CurrentTarget.Gold, (WalletCapacity - WalletGold));
		CurrentTarget.Gold -= goldToSteal;
		WalletGold += goldToSteal;		
		if (CurrentTarget.Gold <= 0) {
			Bystanders.Remove (CurrentTarget);
			ChooseTarget ();
		}
	}

	public void Unstash () {
		// Gold += WalletGold;
		if (Chests.Count > 0) {
			foreach (var chest in Chests) {
				int goldToTake = Mathf.Min (chest.Gold, (WalletCapacity - WalletGold));
				chest.Gold -= goldToTake;
				WalletGold += goldToTake;
			}
		}
	}

	public void Stash () {
		// Gold += WalletGold;
		if (Chests.Count > 0) {
			foreach (var chest in Chests) {
				int goldToGive = Mathf.Min (chest.Capacity - WalletGold, WalletGold);
				chest.Gold += goldToGive;
				WalletGold -= goldToGive;
			}
		}
	}

	void ChooseTarget () {
		if (Bystanders.Count > 0) {
			foreach (var bystander in Bystanders) {
				if (bystander.Gold > 0) {
					CurrentTarget = bystander;
					CanvasObject.SetActive (true);
					followingTarget = true;
					return;
				}
			}
		}
		CanvasObject.SetActive (false);
		followingTarget = false;
		CurrentTarget = null;
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.GetComponentInParent<Bystander> () != null && other.gameObject.GetComponentInParent<Bystander> ().Gold > 0) {
			Bystanders.Add (other.gameObject.GetComponentInParent<Bystander> ());
			if (CurrentTarget == null) {
				ChooseTarget ();
			}
		}
		if (other.gameObject.GetComponentInParent<Chest> () != null) {
			Chests.Add (other.gameObject.GetComponentInParent<Chest> ());
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.GetComponentInParent<Bystander> () != null) {
			Bystanders.Remove (other.gameObject.GetComponentInParent<Bystander> ());
			ChooseTarget ();
		}
		if (other.gameObject.GetComponentInParent<Chest> () != null) {
			Chests.Remove (other.gameObject.GetComponentInParent<Chest> ());
		}
	}
}
