using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;

public class AnimalAgent : Agent {

    private Rigidbody rb;
    public float jumpStrength = 10f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    private Transform groundCheck;
    public float rayDistance = 5f;
    public GameObject player;
    public Dummy dummy;
    public float moveSpeed = 5f;

    public float runDistance = 5f;
    public float distLimit = 20f;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        groundCheck = transform.Find("GroundCheck");
    }

    public override void OnEpisodeBegin() {
        if (dummy != null)
            Reset();
    }

    public override void CollectObservations(VectorSensor sensor) {
        if (dummy != null)
            sensor.AddObservation(dummy.gameObject.transform.position);
        else
            sensor.AddObservation(player.transform.position);
        sensor.AddObservation(transform.position);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        int moveX = actions.DiscreteActions[0];
        int moveZ = actions.DiscreteActions[1];
        bool jump = actions.DiscreteActions[2] == 1;

        Vector3 force = Vector3.zero;

        switch (moveX) {
            case 0: force.x = 0; break;
            case 1: force.x = 1; break;
            case 2: force.x = -1; break;
        }
        switch (moveZ) {
            case 0: force.z = 0; break;
            case 1: force.z = 1; break;
            case 2: force.z = -1; break;
        }

        rb.AddForce(force * moveSpeed);

        if (jump) {
            if (IsGrounded()) {
                rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
                AddReward(-1.3f);
            }
        }

        AddReward(0.001f);
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<int> discreteAction = actionsOut.DiscreteActions;
        int horizontal = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        int vertical = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));

        switch (horizontal) {
            case 0: discreteAction[0] = 0; break;
            case -1: discreteAction[0] = 2; break;
            case 1: discreteAction[0] = 1; break;
        }

        switch (vertical) {
            case 0: discreteAction[1] = 0; break;
            case -1: discreteAction[1] = 2; break;
            case 1: discreteAction[1] = 1; break;
        }

        discreteAction[2] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Wall")) {
            AddReward(-1f);
            if (dummy != null)
                EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            AddReward(-10f);
            if (dummy != null)
                EndEpisode();
        }
    }

    private bool IsGrounded() {
        return Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);
    }

    public void Reset() {
        rb.velocity = Vector3.zero;
        Vector3 newPosition;

        newPosition = new Vector3(UnityEngine.Random.Range(-19, 19), 1, UnityEngine.Random.Range(-19, 19));
        if (newPosition.x > 4)
            newPosition.y = 2f;

        transform.localPosition = newPosition;

        if (dummy != null)
            dummy.Reset();
    }

}
