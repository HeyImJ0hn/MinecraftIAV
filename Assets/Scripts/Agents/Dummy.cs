using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour {

    public Transform environment;
    public LayerMask groundLayer;

    public Transform agent;
    public float moveSpeed = 5f;
    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        Vector3 targetPosition = Vector3.MoveTowards(transform.position, agent.position, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(targetPosition);
    }

    public void Reset() {
        Vector3 newPosition;

        newPosition = new Vector3(UnityEngine.Random.Range(-19, 19), 2, UnityEngine.Random.Range(-19, 19));

        transform.localPosition = newPosition;
    }

}

