using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumpingControl : MonoBehaviour {

	public float dumpingAngle;

	public enum State { Idle, Dumping };
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

		if (angleX > dumpingAngle && angleY > dumpingAngle) {
			Dumping();
		} else {
			Idle();
		}

	}

	void Dumping() {
		if (state == State.Dumping) return;
		
		state = State.Dumping;
		particleSystem.Play();
	}

	void Idle() {
		if (state == State.Idle) return;

		state = State.Idle;
		particleSystem.Stop();
	}

}
