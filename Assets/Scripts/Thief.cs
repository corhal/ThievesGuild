using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : MonoBehaviour {

	public static Thief Instance;

	public int Gold;

	public int WalletGold;
	public int WalletCapacity;

	public KeyCode StealButton;
	public KeyCode StashButton;

	public List<Bystander> Bystanders;
	public List<Chest> Chests;

	void Awake () {
		if (Instance == null) {			
			Instance = this;
		} else if (Instance != this) {
			Destroy (gameObject);  
		}
		DontDestroyOnLoad(gameObject);
	}

	void Update () {
		if (Input.GetKeyDown(StealButton)) {
			Steal ();
		}
		if (Input.GetKeyDown(StashButton)) {
			Stash ();
		}
	}

	public void Steal () {
		if (Bystanders.Count > 0) {
			foreach (var bystander in Bystanders) {
				if (bystander.Gold > 0) {
					int goldToSteal = Mathf.Min(bystander.Gold, (WalletCapacity - WalletGold));
					bystander.Gold -= goldToSteal;
					WalletGold += goldToSteal;
				}
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

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.GetComponentInParent<Bystander> () != null) {
			Bystanders.Add (other.gameObject.GetComponentInParent<Bystander> ());
		}
		if (other.gameObject.GetComponentInParent<Chest> () != null) {
			Chests.Add (other.gameObject.GetComponentInParent<Chest> ());
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.GetComponentInParent<Bystander> () != null) {
			Bystanders.Remove (other.gameObject.GetComponentInParent<Bystander> ());
		}
		if (other.gameObject.GetComponentInParent<Chest> () != null) {
			Chests.Remove (other.gameObject.GetComponentInParent<Chest> ());
		}
	}
}
