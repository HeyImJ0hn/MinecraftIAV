using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {

    public Block[,,] chunkData;
    public GameObject goChunk;
    public enum ChunkStatus { DRAW, DONE };
    public ChunkStatus status;
    public Material material;
<<<<<<< Updated upstream
    public PhysicMaterial physicMaterial;
=======
>>>>>>> Stashed changes
    private Vector3 position;
    public Chunk[] neighbours = new Chunk[6];
    public bool regrow = false;

<<<<<<< Updated upstream
    public Chunk(Vector3 pos, Material material, PhysicMaterial physicMaterial) {
=======
    public Chunk(Vector3 pos, Material material) {
>>>>>>> Stashed changes
        goChunk = new GameObject(World.CreateChunkName(pos));
        goChunk.transform.position = pos;
        this.position = pos;
        this.material = material;
        this.physicMaterial = physicMaterial;
        BuildChunk();
    }

    void CombineQuads() {
        // 1. Combine all children meshes
        MeshFilter[] meshFilters = goChunk.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length) {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        // 2. Create a new mesh on the parent object
        MeshFilter mf = goChunk.AddComponent<MeshFilter>();
        mf.mesh = new Mesh();

        // 3. Add combined meshes on children as parent's mesh
        mf.mesh.CombineMeshes(combine);

        // 4. Create a renderer for the parent
        MeshRenderer renderer = goChunk.AddComponent<MeshRenderer>();
        renderer.material = material;

        // 5. Delete all uncombined children
        foreach (Transform quad in goChunk.transform)
            Object.Destroy(quad.gameObject);
    }

    public void BuildChunk() {
        chunkData = new Block[World.chunkSize, World.chunkSize, World.chunkSize];

        for (int z = 0; z < World.chunkSize; z++) {
            for (int y = 0; y < World.chunkSize; y++) {
                for (int x = 0; x < World.chunkSize; x++) {
                    Vector3 pos = new Vector3(x, y, z);
                    int worldX = (int)goChunk.transform.position.x + x;
                    int worldY = (int)goChunk.transform.position.y + y;
                    int worldZ = (int)goChunk.transform.position.z + z;
                    int h = Utils.GenerateHeight(worldX, worldZ);
                    int hs = Utils.GenerateStoneHeight(worldX, worldZ);

                    if (chunkData[x, y, z] == null) {
                        if (worldY <= hs && worldY > 0) {
                            if (Utils.fBM3D(worldX, worldY, worldZ, 1, 0.5f) < 0.55f)
<<<<<<< Updated upstream
                                chunkData[x, y, z] = new Block(Random.Range(0f, 1f) < 0.05f && worldY <= 20 ? BlockData.Type.DIAMOND_ORE : (Random.Range(0f, 1f) < 0.2f ? BlockData.Type.STONE : (Random.Range(0f, 1f) < 0.5f ? BlockData.Type.COBBLESTONE : BlockData.Type.STONE_VARIANT_1)), pos, this);
=======
                                chunkData[x, y, z] = new Block(Random.Range(0f, 1f) < 0.05f && worldY <= 20 ? BlockData.Type.DIAMOND_ORE : BlockData.Type.STONE, pos, this);
>>>>>>> Stashed changes
                            else
                                chunkData[x, y, z] = new Block(BlockData.Type.AIR, pos, this);
                        } else if (worldY == h)
                            chunkData[x, y, z] = new Block(BlockData.Type.GRASS, pos, this);
                        else if (worldY < h && worldY > 0)
                            chunkData[x, y, z] = new Block(BlockData.Type.DIRT, pos, this);
                        else if (worldY == 0)
                            chunkData[x, y, z] = new Block(BlockData.Type.BEDROCK, pos, this);
                        else
                            chunkData[x, y, z] = new Block(BlockData.Type.AIR, pos, this);
                    }

<<<<<<< Updated upstream
                    FillNeighbours();
                    if (worldY == h + 1 && (y - 1 > 0 && chunkData[x, y - 1, z].GetBlockType() == BlockData.Type.GRASS && CheckNeighbourBlocks(1, false, BlockData.Type.GRASS, new Vector3(x, y, z))))
=======
                    if (worldY == h + 1)
>>>>>>> Stashed changes
                        if (Random.Range(0f, 1f) < 0.01)
                            new Tree(this, pos);
                }
            }
        }
        status = ChunkStatus.DRAW;
    }

    public void DrawChunk() {
        for (int x = 0; x < World.chunkSize; x++)
            for (int y = 0; y < World.chunkSize; y++)
                for (int z = 0; z < World.chunkSize; z++)
                    chunkData[x, y, z].Draw();
        CombineQuads();
        MeshCollider collider = goChunk.AddComponent<MeshCollider>();
        collider.material = physicMaterial;
        collider.sharedMesh = goChunk.GetComponent<MeshFilter>().mesh;
        status = ChunkStatus.DONE;
    }

    public void RegrowGrass() {
        FillNeighbours();

        for (int z = 0; z < World.chunkSize; z++) {
            for (int y = 0; y < World.chunkSize; y++) {
                for (int x = 0; x < World.chunkSize; x++) {
                    Vector3 pos = new Vector3(x, y, z);
                    if (chunkData[x, y, z].GetBlockType() == BlockData.Type.DIRT) {
                        if ((CheckNeighbourBlocks(0, true, BlockData.Type.GRASS, pos)
                            || CheckNeighbourBlocks(0, false, BlockData.Type.GRASS, pos)
                            || CheckNeighbourBlocks(2, true, BlockData.Type.GRASS, pos)
                            || CheckNeighbourBlocks(2, false, BlockData.Type.GRASS, pos))
                            && CheckNeighbourBlocks(1, true, BlockData.Type.AIR, pos)) {
                            chunkData[x, y, z].SetType(BlockData.Type.GRASS);
                            regrow = true;
                        }
                    }
                }
            }
        }
    }

    public void FillNeighbours() {
        string xPositive = ((int)position.x + 16) + " " + (int)position.y + " " + (int)position.z;
        Chunk cXPositive;
        World.chunkDict.TryGetValue(xPositive, out cXPositive);
        neighbours[0] = cXPositive;

        string xNegative = ((int)position.x - 16) + " " + (int)position.y + " " + (int)position.z;
        Chunk cXNegative;
        World.chunkDict.TryGetValue(xNegative, out cXNegative);
        neighbours[1] = cXNegative;

        string yPositive = (int)position.x + " " + ((int)position.y + 16) + " " + (int)position.z;
        Chunk cYPositive;
        World.chunkDict.TryGetValue(yPositive, out cYPositive);
        neighbours[2] = cYPositive;

        string yNegative = (int)position.x + " " + ((int)position.y - 16) + " " + (int)position.z;
        Chunk cYNegative;
        World.chunkDict.TryGetValue(yNegative, out cYNegative);
        neighbours[3] = cYNegative;

        string zPositive = (int)position.x + " " + (int)position.y + " " + ((int)position.z + 16);
        Chunk cZPositive;
        World.chunkDict.TryGetValue(zPositive, out cZPositive);
        neighbours[4] = cZPositive;

        string zNegative = (int)position.x + " " + (int)position.y + " " + ((int)position.z - 16);
        Chunk cZNegative;
        World.chunkDict.TryGetValue(zNegative, out cZNegative);
        neighbours[5] = cZNegative;
    }

    public bool CheckNeighbourBlocks(int direction, bool positive, BlockData.Type bType, Vector3 pos) {
        // direction: 0 = X, 1 = Y, 2 = Z

        int bx = (int)pos.x;
        int by = (int)pos.y;
        int bz = (int)pos.z;

        switch (direction) {
            case 0:
                if ((positive ? bx + 1 == 16 : bx - 1 < 0) && (positive ? neighbours[0] != null : neighbours[1] != null))
                    return positive ? neighbours[0].chunkData[0, by, bz].GetBlockType() == bType : neighbours[1].chunkData[15, by, bz].GetBlockType() == bType;
                else if (positive ? bx + 1 < 16 : bx - 1 >= 0)
                    return positive ? chunkData[bx + 1, by, bz].GetBlockType() == bType : chunkData[bx - 1, by, bz].GetBlockType() == bType;
                break;
            case 1:
                if ((positive ? by + 1 == 16 : by - 1 < 0) && (positive ? neighbours[2] != null : neighbours[3] != null))
                    return positive ? neighbours[2].chunkData[bx, 0, bz].GetBlockType() == bType : neighbours[3].chunkData[bx, 15, bz].GetBlockType() == bType;
                else if (positive ? by + 1 < 16 : by - 1 >= 0)
                    return positive ? chunkData[bx, by + 1, bz].GetBlockType() == bType : chunkData[bx, by - 1, bz].GetBlockType() == bType;
                break;
            case 2:
                if ((positive ? bz + 1 == 16 : bz - 1 < 0) && (positive ? neighbours[4] != null : neighbours[5] != null))
                    return positive ? neighbours[4].chunkData[bx, by, 0].GetBlockType() == bType : neighbours[5].chunkData[bx, by, 15].GetBlockType() == bType;
                else if (positive ? bz + 1 < 16 : bz - 1 >= 0)
                    return positive ? chunkData[bx, by, bz + 1].GetBlockType() == bType : chunkData[bx, by, bz - 1].GetBlockType() == bType;
                break;
        }

        return false;
    }

}
