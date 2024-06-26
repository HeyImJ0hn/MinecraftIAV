using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;

public class ZombieAgent : Agent {
    public GameObject player;
    //public Floors floors;
    public float distToGround;
    public float jumpSpeed;
    public float movementStrength;
    public float detectableDistance = 25;

    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    //public override void OnEpisodeBegin() {
    /*
    //floors.SetAtRandom();
    player.Restart();
    this.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(255, 0, 0));
    //float x = UnityEngine.Random.Range(-9.5f, 9.5f);
    float x = UnityEngine.Random.Range(-20.5f, 20.5f);
    float z;
    if (x > 0) z = UnityEngine.Random.Range(-9.5f, 9.5f);
    else z = UnityEngine.Random.Range(0.5f, 9.5f);
    transform.localPosition = new Vector3(x, 1, z);
    OnEpisodeBeginEvent?.Invoke(this, EventArgs.Empty);
    */
    //}

    public override void CollectObservations(VectorSensor sensor) {
        if (Vector3.Distance(transform.position, player.transform.position) <= detectableDistance) {
            Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
            sensor.AddObservation(dirToPlayer.x);
            sensor.AddObservation(dirToPlayer.z);
        } else {
            sensor.AddObservation(0);
            sensor.AddObservation(0);
        }
    }

    public override void OnActionReceived(ActionBuffers actions) {
        int moveX = actions.DiscreteActions[0];
        int moveZ = actions.DiscreteActions[1];
        bool jump = actions.DiscreteActions[3] == 1;

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

        if (jump) {
            if (IsGrounded()) {
                rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                AddReward(-0.4f);
            }
        }

        rb.AddForce(force * movementStrength);

        AddReward(-1f / MaxStep == 0 ? 3000 : MaxStep);
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

        discreteAction[2] = Input.GetMouseButton(0) ? 1 : 0;
        discreteAction[3] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            AddReward(1f);
            //EndEpisode();
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Wall")) {
            AddReward(-1f);
            EndEpisode();
        }
    }

    private bool IsGrounded() {
        return Physics.Raycast(rb.position, Vector3.down, distToGround + 0.1f);
    }
}

















