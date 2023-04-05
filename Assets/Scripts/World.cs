using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

    public GameObject player;
    public Material material;
    public static int chunkSize = 16;
    public static int radius = 2;
    public static ConcurrentDictionary<string, Chunk> chunkDict;
    public static List<string> toRemove = new List<string>();
    private Vector3 lastBuildPos;
    private bool drawing;

    private float removeTimer = 10f;
    private float timeToRemove = 10f;

    // Start is called before the first frame update
    void Start() {
        player.SetActive(false);
        chunkDict = new ConcurrentDictionary<string, Chunk>();
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        Vector3 ppos = player.transform.position;
        player.transform.position = new Vector3(ppos.x, Utils.GenerateHeight(ppos.x, ppos.z) + 1, ppos.z);
        lastBuildPos = player.transform.position;

        Building(WhichChunk(lastBuildPos), radius);
        Drawing();
        player.SetActive(true);
    }

    // Update is called once per frame
    void Update() {
        Vector3 movement = player.transform.position - lastBuildPos;
        if (movement.magnitude < chunkSize) {
            lastBuildPos = player.transform.position;
            Building(WhichChunk(lastBuildPos), radius);
            Drawing();
        }

        if (!drawing)
            Drawing();

        if (Time.time >= timeToRemove) {
            timeToRemove = Time.time + removeTimer;
            Removing();
        }
    }

    public static string CreateChunkName(Vector3 v) {
        return (int)v.x + " " + (int)v.y + " " + (int)v.z;
    }

    void BuildChunkAt(Vector3 chunkPos) {
        string name = CreateChunkName(chunkPos);
        Chunk c;
        if (!chunkDict.TryGetValue(name, out c)) {
            c = new Chunk(chunkPos, material);
            c.goChunk.transform.parent = transform;
            chunkDict.TryAdd(c.goChunk.name, c);
        }
    }

    IEnumerator RemoveChunks() {
        foreach (KeyValuePair<string, Chunk> c in chunkDict) {
            if (c.Value.goChunk && Vector3.Distance(player.transform.position, c.Value.goChunk.transform.position) > chunkSize * radius && !toRemove.Contains(c.Key))
                toRemove.Add(c.Key);
        }

        for (int i = 0; i < toRemove.Count; i++) {
            string name = toRemove[i];
            Chunk c;
            if (chunkDict.TryGetValue(name, out c)) {
                Destroy(c.goChunk);
                chunkDict.TryRemove(name, out c);
            }
        }

        toRemove.Clear();
        yield return null;
    }

    IEnumerator DrawChunks() {
        drawing = true;
        foreach (KeyValuePair<string, Chunk> c in chunkDict) {
            if (c.Value.goChunk && c.Value.status == Chunk.ChunkStatus.DRAW) {
                c.Value.DrawChunk();
                yield return null;
            }
        }
        drawing = false;
    }

    void Drawing() {
        StartCoroutine(DrawChunks());
    }

    void Removing() {
        StartCoroutine(RemoveChunks());
    }

    Vector3 WhichChunk(Vector3 pos) {
        Vector3 chunkPos = new Vector3();
        chunkPos.x = Mathf.Floor(pos.x / chunkSize) * chunkSize;
        chunkPos.y = Mathf.Floor(pos.y / chunkSize) * chunkSize;
        chunkPos.z = Mathf.Floor(pos.z / chunkSize) * chunkSize;
        return chunkPos;
    }

    IEnumerator BuildRecursiveWorld(Vector3 chunkPos, int rad) {
        int x = (int)chunkPos.x;
        int y = (int)chunkPos.y;
        int z = (int)chunkPos.z;

        BuildChunkAt(chunkPos);
        yield return null;

        if (--rad < 0) yield break;
        Building(new Vector3(x, y, z + chunkSize), rad);
        yield return null;
        Building(new Vector3(x, y, z - chunkSize), rad);
        yield return null;
        Building(new Vector3(x + chunkSize, y, z), rad);
        yield return null;
        Building(new Vector3(x - chunkSize, y, z), rad);
        yield return null;
        Building(new Vector3(x, y + chunkSize, z), rad);
        yield return null;
        Building(new Vector3(x, y - chunkSize, z), rad);
    }

    void Building(Vector3 chunkPos, int rad) {
        StartCoroutine(BuildRecursiveWorld(chunkPos, rad));
    }

}
