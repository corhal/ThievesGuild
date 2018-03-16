using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bystander : MonoBehaviour {

	public int Gold;
	public int Attention; // 0..100

	float randomDelay;
	float timer = 0.0f;

	Vector3 initialPosition;
	Vector3 destination;
	Rigidbody2D rigidBody;

	public List <Thief> ThievesInVision;

	public GameObject WalletObject;

	public delegate void ThiefSpottedEventHandler (Bystander bystander, Thief thief);
	public static event ThiefSpottedEventHandler OnThiefSpotted;
	public static event ThiefSpottedEventHandler OnThiefUnspotted;

	void Awake () {
		rigidBody = GetComponent <Rigidbody2D> ();
		initialPosition = transform.position;
		ThievesInVision = new List<Thief> ();
		Thief.OnThiefSteals += Thief_OnThiefSteals;
	}

	void Thief_OnThiefSteals (Bystander bystander, Thief thief) {
		if (bystander != this && ThievesInVision.Contains (thief)) {
			int randomRoll = Random.Range (0, 101);
			if (randomRoll <= Attention) {
				Debug.Log ("Thief! Thief!");
			}
		}
	}

	void Start () {
		randomDelay = Random.Range (0.0f, 3.0f);
		destination = RandomPointNearby ();
		transform.up = destination - transform.position;
		Gold = Random.Range (10, 21);
		Attention = Random.Range (1, 101);
	}

	void Update () {
		timer += Time.deltaTime;
		if (timer >= randomDelay) {
			timer = 0.0f;
			randomDelay = Random.Range (5.0f, 10.0f);
			destination = RandomPointNearby ();
			transform.up = destination - transform.position;
		}

		if (Vector2.Distance(transform.position, destination) > 0.01f) {
			Vector3 movement = destination - transform.position;
			rigidBody.velocity = movement * 0.5f;
		}
	}

	Vector3 RandomPointNearby () {
		float dx = Random.Range (0.0f, 3.0f);
		float dy = Random.Range (0.0f, 3.0f);
		return new Vector3 (initialPosition.x + dx, initialPosition.y + dy, 0.0f);
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.GetComponentInParent<Thief> () != null && !ThievesInVision.Contains (other.gameObject.GetComponentInParent<Thief> ())) {
			if (other.tag == "vision") {
				return;
			}
			ThievesInVision.Add (other.gameObject.GetComponentInParent<Thief> ());
			if (OnThiefSpotted != null) {
				OnThiefSpotted (this, other.gameObject.GetComponentInParent<Thief> ());
			}
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.GetComponentInParent<Thief> () != null) {
			if (other.tag == "vision") {
				return;
			}
			ThievesInVision.Remove (other.gameObject.GetComponentInParent<Thief> ());
			if (OnThiefUnspotted != null) {
				OnThiefUnspotted (this, other.gameObject.GetComponentInParent<Thief> ());
			}
		}
	}
}
