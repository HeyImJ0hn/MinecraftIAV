using UnityEngine;

public class Tree {

    private Chunk chunk;
    private Vector3 pos;

    public Tree(Chunk chunk, Vector3 pos) {
        this.chunk = chunk;
        this.pos = pos;

        CreateTree();
    }

    private void CreateTree() {
        for (int i = 0; i < 4; i++) {
            int x = (int)pos.x;
            int y = (int)pos.y + i;
            int z = (int)pos.z;

            if (y > 15) {
                chunk.chunkData[x, y - 1, z] = new Block(BlockData.Type.LEAVES, new Vector3(x, y - 1, z), chunk);
                return;
            }

            chunk.chunkData[x, y, z] = new Block(BlockData.Type.LOG, new Vector3(x, y, z), chunk);

            if (x - 1 < 0 || x + 1 >= 16 || z - 1 < 0 || z + 1 >= 16)
                continue;

            if (i == 1 || i == 2) {
                chunk.chunkData[x + 1, y, z] = new Block(BlockData.Type.LEAVES, new Vector3(x + 1, y, z), chunk);
                chunk.chunkData[x - 1, y, z] = new Block(BlockData.Type.LEAVES, new Vector3(x - 1, y, z), chunk);
                chunk.chunkData[x, y, z + 1] = new Block(BlockData.Type.LEAVES, new Vector3(x, y, z + 1), chunk);
                chunk.chunkData[x, y, z - 1] = new Block(BlockData.Type.LEAVES, new Vector3(x, y, z - 1), chunk);
            } else if (i == 3)
                chunk.chunkData[x, y, z].SetType(BlockData.Type.LEAVES);

        }
    }

}
