using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCanControl : MonoBehaviour {

	public float wateringAngle;

	public enum State { Idle, Watering };
	public State state;

	private Vector3 vecOrigUp;
	private Vector3 vecOrigRight;
	private ParticleSystem particleSystem;

	// Use this for initialization
	void Start () {
		state = State.Idle;

		vecOrigUp = transform.up;
		vecOrigRight = transform.right;

		particleSystem = GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float angleY = Vector3.Angle(vecOrigUp, transform.up);
		float angleX = Vector3.Angle(vecOrigRight, transform.right);

		if (angleX > wateringAngle && angleY > wateringAngle) {
			Watering();
		} else {
			Idle();
		}

	}

	void Watering() {
		if (state == State.Watering) return;
		
		state = State.Watering;
		particleSystem.Play();
	}

	void Idle() {
		if (state == State.Idle) return;

		state = State.Idle;
		particleSystem.Stop();
	}

}
