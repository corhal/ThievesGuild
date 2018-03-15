using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bystander : MonoBehaviour {

	public int Gold;

	float randomDelay;
	float timer = 0.0f;

	Vector2 initialPosition;
	Vector2 destination;
	Rigidbody2D rigidBody;

	void Awake () {
		Gold = Random.Range (10, 21);
		rigidBody = GetComponent <Rigidbody2D> ();
		initialPosition = transform.position;
	}

	void Start () {
		randomDelay = Random.Range (0.0f, 3.0f);
		destination = RandomPointNearby ();
		transform.LookAt (destination, Vector3.back);
	}

	void Update () {
		timer += Time.deltaTime;
		if (timer >= randomDelay) {
			timer = 0.0f;
			randomDelay = Random.Range (0.0f, 3.0f);
			destination = RandomPointNearby ();
			transform.LookAt (destination);
		}

		if (Vector2.Distance(transform.position, destination) > 0.01f) {
			Vector2 movement = destination - new Vector2(transform.position.x, transform.position.y);
			rigidBody.velocity = movement;
		}
	}

	Vector2 RandomPointNearby () {
		float dx = Random.Range (0.0f, 3.0f);
		float dy = Random.Range (0.0f, 3.0f);
		return new Vector2 (initialPosition.x + dx, initialPosition.y + dy);
	}
}
