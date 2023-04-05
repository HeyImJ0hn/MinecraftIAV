using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World0 : MonoBehaviour {

    [SerializeField] private GameObject cube;
    [SerializeField] private int size;

    // Start is called before the first frame update
    void Start() {
        Debug.Log("Starting Start");
        StartCoroutine(BuildWorld());
        Debug.Log("Ending Start");
    }

    // Update is called once per frame
    void Update() {
        
    }

    IEnumerator BuildWorld() {
        Debug.Log("Startng Building");
        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {
                for (int z = 0; z < size; z++) {
                    Vector3 pos = new Vector3(x, y, z);
                    GameObject c = Instantiate(cube, pos, Quaternion.identity);
                    c.transform.parent = this.transform;
                    c.name = x + " " + y + " " + z;
                    Debug.Log("Startng Resting");
                    yield return null;
                    Debug.Log("Ending Resting");
                }
            }
        }
    }
    
}
