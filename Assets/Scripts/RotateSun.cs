using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RotateSun : MonoBehaviour {

    public float speed = 1.0f;

    // 230 -> -130

    public Material[] skyboxes;
    [SerializeField]
    private int currentSkybox = 0;
    public int skyboxChangeTime = 15;
    private bool canChange = true;
    [SerializeField]
    private bool reverse = false;

    private void Start() {
        //RenderSettings.skybox = skyboxes[currentSkybox];
    }

    void Update() {
        /*if (canChange) {
            StartCoroutine(ChangeSkybox());
            canChange = false;
        }*/
        if (transform.rotation.eulerAngles.x < 0 || transform.rotation.eulerAngles.x > 180)
            RenderSettings.skybox.SetFloat("_Exposure", .25f);
        else
            RenderSettings.skybox.SetFloat("_Exposure", 1f);

        transform.Rotate(Vector3.right * Time.deltaTime * speed);
    }

    IEnumerator ChangeSkybox() {
        yield return new WaitForSeconds(skyboxChangeTime);
        if (currentSkybox == skyboxes.Length - 1)
            reverse = true;
        else if (currentSkybox == 0)
            reverse = false;
        currentSkybox = reverse ? (currentSkybox - 1) % skyboxes.Length : (currentSkybox + 1) % skyboxes.Length;
        if (transform.rotation.eulerAngles.x > -30 && transform.rotation.eulerAngles.x < -130) {
            RenderSettings.skybox.SetFloat("_Exposure", 1f);
            RenderSettings.skybox = skyboxes[currentSkybox];
        } else {
            RenderSettings.skybox.SetFloat("_Exposure", 0.5f);
        }
        canChange = true;
    }
}
