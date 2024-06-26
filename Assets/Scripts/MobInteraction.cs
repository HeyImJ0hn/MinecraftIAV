using System.Collections;
using System.Collections.Generic;
using Unity.Sentis.Layers;
using UnityEngine;

public class MobInteraction : MonoBehaviour {
    public Camera cam;

    void Update() {
        if (Input.GetMouseButtonDown(0))
            DestroyMob();
    }

    public void DestroyMob() {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 10)) {
            if (hit.collider.gameObject.CompareTag("Mob")) {
                Debug.Log("Destroying mob");
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
