using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public Text GoldLabel;
	public Text WalletLabel;

	void Update () {
		GoldLabel.text = "Gold: " + Thief.Instance.Gold;
		WalletLabel.text = "Wallet: " + Thief.Instance.WalletGold + "/" + Thief.Instance.WalletCapacity;
	}
}
