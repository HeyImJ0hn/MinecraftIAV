using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInteraction : MonoBehaviour {

    public Camera cam;

    enum InteractionType { DESTROY, BUILD };
    InteractionType interactionType;
    private BlockSelector blockSelector;

    void Start() {
        blockSelector = GetComponent<BlockSelector>();
    }

    void Update() {
        bool interaction = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1);
        if (interaction) {
            interactionType = Input.GetMouseButtonDown(0) ? InteractionType.DESTROY : InteractionType.BUILD;
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 10)) {
                string chunkName = hit.collider.gameObject.name;
                float chunkx = hit.collider.gameObject.transform.position.x;
                float chunky = hit.collider.gameObject.transform.position.y;
                float chunkz = hit.collider.gameObject.transform.position.z;

                Vector3 hitBlock = (interactionType == InteractionType.DESTROY ? hit.point - hit.normal / 2f : hit.point + hit.normal / 2f);

                int blockx = (int)(Mathf.Round(hitBlock.x) - chunkx);
                int blocky = (int)(Mathf.Round(hitBlock.y) - chunky);
                int blockz = (int)(Mathf.Round(hitBlock.z) - chunkz);

                string[] coords = chunkName.Split(" ");
                Vector3[] updatedCoords = UpdateCoords(coords, blockx, blocky, blockz);

                chunkName = World.CreateChunkName(updatedCoords[0]);
                blockx = (int)updatedCoords[1].x;
                blocky = (int)updatedCoords[1].y;
                blockz = (int)updatedCoords[1].z;

                if (interactionType == InteractionType.DESTROY)
                    BreakBlock(chunkName, blockx, blocky, blockz);
                else
                    BuildBlock(chunkName, blockx, blocky, blockz);
            }
        }
    }

    private Vector3[] UpdateCoords(string[] coords, int blockx, int blocky, int blockz) {

        Vector3 chunkCoords = new Vector3(int.Parse(coords[0]), int.Parse(coords[1]), int.Parse(coords[2]));
        if (blockx == World.chunkSize || blockx == -1) {
            chunkCoords = new Vector3(chunkCoords.x + ((blockx == World.chunkSize) ? World.chunkSize : -World.chunkSize), chunkCoords.y, chunkCoords.z);
            blockx = (blockx == World.chunkSize) ? 0 : 15;
        }
        if (blocky == World.chunkSize || blocky == -1) {
            chunkCoords = new Vector3(chunkCoords.x, chunkCoords.y + ((blocky == World.chunkSize) ? World.chunkSize : -World.chunkSize), chunkCoords.z);
            blocky = (blocky == World.chunkSize) ? 0 : 15;
        }
        if (blockz == World.chunkSize || blockz == -1) {
            chunkCoords = new Vector3(chunkCoords.x, chunkCoords.y, chunkCoords.z + ((blockz == World.chunkSize) ? World.chunkSize : -World.chunkSize));
            blockz = (blockz == World.chunkSize) ? 0 : 15;
        }

        return new Vector3[] { chunkCoords, new Vector3(blockx, blocky, blockz) };
    }

    private void BuildBlock(string chunkName, int blockx, int blocky, int blockz) {
        Chunk c;
        World.chunkDict.TryGetValue(chunkName, out c);
        c.chunkData[blockx, blocky, blockz].SetType(blockSelector.GetCurrentBlockType());
        blockSelector.RemoveBlock();
        UpdateChunk(c.goChunk.name, blockx, blocky, blockz);
    }

    public void BuildBlock() {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 10)) {
            string chunkName = hit.collider.gameObject.name;
            float chunkx = hit.collider.gameObject.transform.position.x;
            float chunky = hit.collider.gameObject.transform.position.y;
            float chunkz = hit.collider.gameObject.transform.position.z;

            Vector3 hitBlock = hit.point + hit.normal / 2f;

            int blockx = (int)(Mathf.Round(hitBlock.x) - chunkx);
            int blocky = (int)(Mathf.Round(hitBlock.y) - chunky);
            int blockz = (int)(Mathf.Round(hitBlock.z) - chunkz);

            string[] coords = chunkName.Split(" ");

            Vector3[] updatedCoords = UpdateCoords(coords, blockx, blocky, blockz);

            chunkName = World.CreateChunkName(updatedCoords[0]);

            blockx = (int)updatedCoords[1].x;
            blocky = (int)updatedCoords[1].y;
            blockz = (int)updatedCoords[1].z;

            BuildBlock(chunkName, blockx, blocky, blockz);
        }
    }

    private void BreakBlock(string chunkName, int blockx, int blocky, int blockz) {
        Chunk c;
        World.chunkDict.TryGetValue(chunkName, out c);
        BlockData.Type type;
        switch (c.chunkData[blockx, blocky, blockz].GetBlockType()) {
            case BlockData.Type.GRASS:
                type = BlockData.Type.DIRT;
                break;
            case BlockData.Type.STONE:
                type = BlockData.Type.COBBLESTONE;
                break;
            case BlockData.Type.STONE_VARIANT_1:
                type = BlockData.Type.COBBLESTONE;
                break;
            case BlockData.Type.LEAVES:
                type = BlockData.Type.AIR;
                break;
            default:
                type = c.chunkData[blockx, blocky, blockz].GetBlockType();
                break;
        }

        blockSelector.AddBlock(type);
        c.chunkData[blockx, blocky, blockz].SetType(BlockData.Type.AIR);

        UpdateChunk(c.goChunk.name, blockx, blocky, blockz);

    }

    public void BreakBlock() {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 10)) {
            string chunkName = hit.collider.gameObject.name;
            float chunkx = hit.collider.gameObject.transform.position.x;
            float chunky = hit.collider.gameObject.transform.position.y;
            float chunkz = hit.collider.gameObject.transform.position.z;

            Vector3 hitBlock = hit.point - hit.normal / 2f;

            int blockx = (int)(Mathf.Round(hitBlock.x) - chunkx);
            int blocky = (int)(Mathf.Round(hitBlock.y) - chunky);
            int blockz = (int)(Mathf.Round(hitBlock.z) - chunkz);
            BreakBlock(chunkName, blockx, blocky, blockz);
        }

    }

    private void UpdateChunk(string chunkName, int blockx, int blocky, int blockz) {
        string[] coords = chunkName.Split(" ");
        int chunkx = int.Parse(coords[0]);
        int chunky = int.Parse(coords[1]);
        int chunkz = int.Parse(coords[2]);

        List<string> updates = new();
        updates.Add(chunkName);
        if (blockx == 0)
            updates.Add(World.CreateChunkName(new Vector3(chunkx - World.chunkSize, chunky, chunkz)));
        if (blockx == World.chunkSize - 1)
            updates.Add(World.CreateChunkName(new Vector3(chunkx + World.chunkSize, chunky, chunkz)));
        if (blocky == 0)
            updates.Add(World.CreateChunkName(new Vector3(chunkx, chunky - World.chunkSize, chunkz)));
        if (blocky == World.chunkSize - 1)
            updates.Add(World.CreateChunkName(new Vector3(chunkx, chunky + World.chunkSize, chunkz)));
        if (blockz == 0)
            updates.Add(World.CreateChunkName(new Vector3(chunkx, chunky, chunkz - World.chunkSize)));
        if (blockz == World.chunkSize - 1)
            updates.Add(World.CreateChunkName(new Vector3(chunkx, chunky, chunkz + World.chunkSize)));

        Chunk c;
        foreach (string name in updates) {
            if (World.chunkDict.TryGetValue(name, out c)) {
                DestroyImmediate(c.goChunk.GetComponent<MeshFilter>());
                DestroyImmediate(c.goChunk.GetComponent<MeshRenderer>());
                DestroyImmediate(c.goChunk.GetComponent<MeshCollider>());
                c.DrawChunk();
            }
        }
    }

}
