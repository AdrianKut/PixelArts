using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveToPosition : MonoBehaviour
{
	

	public Transform targetPos;
	public float speed = 1f;
	public float teleportDelay = 0.5f;

	public ParticleSystem particleSystem;

	private Rigidbody body;
	public UnityEvent OnDesiredPositionEvent;

	void Awake()
	{
		body = GetComponent<Rigidbody>();
	}

	private bool startOnce = false;
    void FixedUpdate()
	{
		Vector3 direction = (targetPos.position - transform.position).normalized;
		body.AddForce(direction * speed);

        if (startOnce == false)
        {
			startOnce = true;
			StartCoroutine(TeleportToDesiredPosition());
        }

	}


	IEnumerator TeleportToDesiredPosition()
    {
		yield return new WaitForSeconds(teleportDelay);
		OnDesiredPositionEvent?.Invoke();
		body.isKinematic = true;
		this.transform.position = targetPos.position;


	}
}
