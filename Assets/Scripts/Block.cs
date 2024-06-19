using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {

    enum Cubeside { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK }

    private BlockData.Type bType;
    private Chunk owner;
    private Vector3 pos;
    private bool isSolid;

    static Vector2 GrassSide_LBC = new Vector2(3f, 15f) / 16;
    static Vector2 GrassTop_LBC = new Vector2(13f, 5f) / 16;
    static Vector2 Dirt_LBC = new Vector2(2f, 15f) / 16;
    static Vector2 Stone_LBC = new Vector2(1f, 15f) / 16;
    static Vector2 Cobblestone_LBC = new Vector2(0f, 14f) / 16;
    static Vector2 LogTop_LBC = new Vector2(5f, 14f) / 16;
    static Vector2 LogSide_LBC = new Vector2(4f, 14f) / 16;
    static Vector2 DiamondOre_LBC = new Vector2(2f, 12f) / 16;
    static Vector2 Bedrock_LBC = new Vector2(1f, 14f) / 16;
    static Vector2 Leaves_LBC = new Vector2(5f, 12f) / 16;
    static Vector2 StoneVariant1_LBC = new Vector2(0f, 15f) / 16;

    Vector2[,] blockUVs = {
        /* GRASS SIDE */ {GrassSide_LBC, GrassSide_LBC + new Vector2(1f, 0f)/16f,
                        GrassSide_LBC + new Vector2(0f, 1f)/16, GrassSide_LBC + new Vector2(1f, 1f)/16},
        /* LOG SIDE */ {LogSide_LBC, LogSide_LBC + new Vector2(1f, 0f)/16f,
                        LogSide_LBC + new Vector2(0f, 1f)/16, LogSide_LBC + new Vector2(1f, 1f)/16},
        /* DIRT */ {Dirt_LBC, Dirt_LBC + new Vector2(1f, 0f)/16f,
                        Dirt_LBC + new Vector2(0f, 1f)/16, Dirt_LBC + new Vector2(1f, 1f)/16},
        /* STONE */ {Stone_LBC, Stone_LBC + new Vector2(1f, 0f)/16f,
                        Stone_LBC + new Vector2(0f, 1f)/16, Stone_LBC + new Vector2(1f, 1f)/16},
        /* COBBLESTONE */ {Cobblestone_LBC, Cobblestone_LBC + new Vector2(1f, 0f)/16f,
                        Cobblestone_LBC + new Vector2(0f, 1f)/16, Cobblestone_LBC + new Vector2(1f, 1f)/16},
        /* BEDROCK */ {Bedrock_LBC, Bedrock_LBC + new Vector2(1f, 0f)/16f,
                        Bedrock_LBC + new Vector2(0f, 1f)/16, Bedrock_LBC + new Vector2(1f, 1f)/16},
        /* DIAMOND_ORE */ {DiamondOre_LBC, DiamondOre_LBC + new Vector2(1f, 0f)/16f,
                        DiamondOre_LBC + new Vector2(0f, 1f)/16, DiamondOre_LBC + new Vector2(1f, 1f)/16},
        /* LEAVES */ {Leaves_LBC, Leaves_LBC + new Vector2(1f, 0f)/16f,
                        Leaves_LBC + new Vector2(0f, 1f)/16, Leaves_LBC + new Vector2(1f, 1f)/16},
        /* STONE VARIANT 1 */ {StoneVariant1_LBC, StoneVariant1_LBC + new Vector2(1f, 0f)/16f,
                        StoneVariant1_LBC + new Vector2(0f, 1f)/16, StoneVariant1_LBC + new Vector2(1f, 1f)/16},
    };

    Vector2[,] blockUVsTop = {
        /* GRASS TOP */ {GrassTop_LBC, GrassTop_LBC + new Vector2(1f, 0f)/16f,
                        GrassTop_LBC + new Vector2(0f, 1f)/16, GrassTop_LBC + new Vector2(1f, 1f)/16},
        /* LOG TOP */ {LogTop_LBC, LogTop_LBC + new Vector2(1f, 0f)/16f,
                        LogTop_LBC + new Vector2(0f, 1f)/16, LogTop_LBC + new Vector2(1f, 1f)/16},
    };

    public Block(BlockData.Type bType, Vector3 pos, Chunk owner) {
        this.pos = pos;
        this.owner = owner;
        SetType(bType);
    }

    public void SetType(BlockData.Type bType) {
        this.bType = bType;
        if (bType == BlockData.Type.AIR)
            isSolid = false;
        else
            isSolid = true;
    }

    public BlockData.Type GetBlockType() {
        return bType;
    }

    void CreateQuad(Cubeside side) {
        Mesh mesh = new Mesh();

        Vector3 v0 = new Vector3(-0.5f, -0.5f, 0.5f);
        Vector3 v1 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 v2 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 v3 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 v4 = new Vector3(-0.5f, 0.5f, 0.5f);
        Vector3 v5 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 v6 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 v7 = new Vector3(-0.5f, 0.5f, -0.5f);

        Vector2 uv00 = new Vector2(0, 0);
        Vector2 uv01 = new Vector2(0, 1);
        Vector2 uv10 = new Vector2(1, 0);
        Vector2 uv11 = new Vector2(1, 1);

        if ((bType == BlockData.Type.GRASS || bType == BlockData.Type.LOG) && side == Cubeside.TOP) {
            uv00 = blockUVsTop[bType == BlockData.Type.GRASS ? 0 : 1, 0];
            uv10 = blockUVsTop[bType == BlockData.Type.GRASS ? 0 : 1, 1];
            uv01 = blockUVsTop[bType == BlockData.Type.GRASS ? 0 : 1, 2];
            uv11 = blockUVsTop[bType == BlockData.Type.GRASS ? 0 : 1, 3];
        } else if (bType == BlockData.Type.GRASS && side == Cubeside.BOTTOM) {
            uv00 = blockUVs[2, 0];
            uv10 = blockUVs[2, 1];
            uv01 = blockUVs[2, 2];
            uv11 = blockUVs[2, 3];
        } else if (bType == BlockData.Type.LOG && side == Cubeside.BOTTOM) {
            uv00 = blockUVsTop[1, 0];
            uv10 = blockUVsTop[1, 1];
            uv01 = blockUVsTop[1, 2];
            uv11 = blockUVsTop[1, 3];
        } else {
            uv00 = blockUVs[(int)(bType), 0];
            uv10 = blockUVs[(int)(bType), 1];
            uv01 = blockUVs[(int)(bType), 2];
            uv11 = blockUVs[(int)(bType), 3];
        }

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        int[] triangles = new int[6];
        Vector2[] uv = new Vector2[4];

        switch (side) {
            case Cubeside.FRONT:
                vertices = new Vector3[] { v4, v5, v1, v0 };
                normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
                break;
            case Cubeside.BOTTOM:
                vertices = new Vector3[] { v0, v1, v2, v3 };
                normals = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down };
                break;
            case Cubeside.TOP:
                vertices = new Vector3[] { v7, v6, v5, v4 };
                normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
                break;
            case Cubeside.LEFT:
                vertices = new Vector3[] { v7, v4, v0, v3 };
                normals = new Vector3[] { Vector3.left, Vector3.left, Vector3.left, Vector3.left };
                break;
            case Cubeside.RIGHT:
                vertices = new Vector3[] { v5, v6, v2, v1 };
                normals = new Vector3[] { Vector3.right, Vector3.right, Vector3.right, Vector3.right };
                break;
            case Cubeside.BACK:
                vertices = new Vector3[] { v6, v7, v3, v2 };
                normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
                break;
        }

        triangles = new int[] { 3, 1, 0, 3, 2, 1 };
        uv = new Vector2[] { uv11, uv01, uv00, uv10 };

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangles;
        mesh.uv = uv;

        mesh.RecalculateBounds();

        GameObject quad = new GameObject("quad");
        quad.transform.position = pos;
        quad.transform.parent = owner.goChunk.transform;

        MeshFilter mf = quad.AddComponent<MeshFilter>();
        mf.mesh = mesh;
    }

    int ConvertToLocalIndex(int i) {
        if (i == -1)
            return World.chunkSize - 1;
        if (i == World.chunkSize)
            return 0;
        return i;
    }

    bool HasSolidNeighbour(int x, int y, int z) {
        Block[,,] chunkData = owner.chunkData;

        if (x < 0 || x >= World.chunkSize || y < 0 || y >= World.chunkSize || z < 0 || z >= World.chunkSize) {
            Vector3 neighChunkPos = owner.goChunk.transform.position + new Vector3(
                (x - (int)pos.x) * World.chunkSize,
                (y - (int)pos.y) * World.chunkSize,
                (z - (int)pos.z) * World.chunkSize);
            string chunkName = World.CreateChunkName(neighChunkPos);

            x = ConvertToLocalIndex(x);
            y = ConvertToLocalIndex(y);
            z = ConvertToLocalIndex(z);

            Chunk neighChunk;
            if (World.chunkDict.TryGetValue(chunkName, out neighChunk))
                chunkData = neighChunk.chunkData;
            else
                return false;
        } else {
            chunkData = owner.chunkData;
        }

        try {
            return chunkData[x, y, z].isSolid;
        } catch (System.IndexOutOfRangeException ex) { Debug.LogWarning(ex); }
        return false;
    }

    public void Draw() {
        if (bType == BlockData.Type.AIR)
            return;

        if (!HasSolidNeighbour((int)pos.x - 1, (int)pos.y, (int)pos.z))
            CreateQuad(Cubeside.LEFT);
        if (!HasSolidNeighbour((int)pos.x + 1, (int)pos.y, (int)pos.z))
            CreateQuad(Cubeside.RIGHT);
        if (!HasSolidNeighbour((int)pos.x, (int)pos.y - 1, (int)pos.z))
            CreateQuad(Cubeside.BOTTOM);
        if (!HasSolidNeighbour((int)pos.x, (int)pos.y + 1, (int)pos.z))
            CreateQuad(Cubeside.TOP);
        if (!HasSolidNeighbour((int)pos.x, (int)pos.y, (int)pos.z - 1))
            CreateQuad(Cubeside.BACK);
        if (!HasSolidNeighbour((int)pos.x, (int)pos.y, (int)pos.z + 1))
            CreateQuad(Cubeside.FRONT);
    }

}
