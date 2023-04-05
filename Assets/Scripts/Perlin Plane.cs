using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinPlane : MonoBehaviour {

    [SerializeField] private float smooth = 1/11f;
    [SerializeField] private float max_height = 10;
    [SerializeField] private float offset = 12000f;

    Mesh mesh;
    Vector3[] vertices;

    // Start is called before the first frame update
    void Start() {
        MeshFilter mf = GetComponent<MeshFilter>();
        mesh = mf.mesh;
        vertices = mesh.vertices;
    }

    // Update is called once per frame
    void Update() {
        for (int i = 0; i < vertices.Length; i++) {
            Vector3 v = vertices[i];
            float h = Mathf.PerlinNoise(v.x * smooth, v.y * smooth);
            vertices[i] = new Vector3(v.x, h, v.z);
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }
}
