using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMob : MonoBehaviour {

    public GameObject zombie;
    public GameObject pig;
    public Camera cam;

    void Update() {
        if (Input.GetKeyDown(KeyCode.O))
            Spawn(zombie);
        else if (Input.GetKeyDown(KeyCode.P))
            Spawn(pig);
    }

    private void Spawn(GameObject mob) {
        Debug.Log("Spawning mob");
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 10)) {
            Vector3 spawnPosition = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
            Debug.Log("Spawning " + mob.name + " at " + spawnPosition);
            Instantiate(mob, spawnPosition, Quaternion.identity);
        }
    }

    public void SpawnZombie() {
        Spawn(zombie);
    }

    public void SpawnAnimal() {
        Spawn(pig);
    }
}
