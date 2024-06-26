using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class EnemyAgent : Agent {
    private Rigidbody rb;
    public float jumpStrength = 10f; // Increased jump strength
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    private Transform groundCheck;
    public float rayDistance = 5f;
    public Transform player;
    private Vector3 originalPos;
    public float moveSpeed = 10f; // Increased move speed

    void Start() {
        rb = GetComponent<Rigidbody>();
        groundCheck = transform.Find("GroundCheck");
        originalPos = transform.position;
    }

    public override void OnEpisodeBegin() {
        rb.velocity = Vector3.zero;
        transform.position = originalPos;
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(player.position);
        sensor.AddObservation(transform.position);
        sensor.AddObservation(rb.velocity);
        sensor.AddObservation(IsGrounded());
        sensor.AddObservation(DistanceToObstacle());
    }

    public override void OnActionReceived(ActionBuffers actions) {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        int jump = actions.DiscreteActions[0];

        Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed;
        rb.AddForce(move, ForceMode.VelocityChange);

        if (jump == 1 && IsGrounded()) {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        }

        float distanceToPlayer = DistanceToPlayer();
        if (distanceToPlayer < 5f) {
            SetReward(10f);
            EndEpisode();
        } else {
            SetReward(-0.1f);
        }

        Debug.Log("Distance: " + distanceToPlayer);
        Debug.Log(GetCumulativeReward());
    }

    private float DistanceToPlayer() {
        return Vector3.Distance(transform.position, player.position);
    }

    private float DistanceToObstacle() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance)) {
            return hit.distance;
        }
        return rayDistance;
    }

    private bool IsGrounded() {
        return Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Wall")) {
            SetReward(-1f);
            EndEpisode();
        }
    }
}
