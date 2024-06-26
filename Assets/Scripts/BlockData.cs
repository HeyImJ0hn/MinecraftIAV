using UnityEngine;
public class BlockData {

    private static int grass_amount = 0;
    private static int log_amount = 0;
    private static int dirt_amount = 0;
    private static int cobblestone_amount = 0;
    private static int diamond_ore_amount = 0;

    public enum Type {
        GRASS,
        LOG,
        DIRT,
        STONE,
        COBBLESTONE,
        BEDROCK,
        DIAMOND_ORE,
        LEAVES,
        STONE_VARIANT_1,
        AIR
    };

    public static int GetAmount(Type type) {
        switch (type) {
            case Type.GRASS:
                return grass_amount;
            case Type.LOG:
                return log_amount;
            case Type.DIRT:
                return dirt_amount;
            case Type.COBBLESTONE:
                return cobblestone_amount;
            case Type.DIAMOND_ORE:
                return diamond_ore_amount;
            default:
                return 0;
        }
    }

    public static void SetAmount(Type type, int amount) {
        switch (type) {
            case Type.GRASS:
                grass_amount = amount;
                break;
            case Type.LOG:
                log_amount = amount;
                break;
            case Type.DIRT:
                dirt_amount = amount;
                break;
            case Type.COBBLESTONE:
                cobblestone_amount = amount;
                break;
            case Type.DIAMOND_ORE:
                diamond_ore_amount = amount;
                break;
        }
    }

    public static Sprite GetSprite(Type type) {
        switch (type) {
            case Type.GRASS:
                return Resources.Load("GRASS", typeof(Sprite)) as Sprite;
            case Type.LOG:
                return Resources.Load("LOG", typeof(Sprite)) as Sprite;
            case Type.DIRT:
                return Resources.Load("DIRT", typeof(Sprite)) as Sprite;
            case Type.COBBLESTONE:
                return Resources.Load("COBBLESTONE", typeof(Sprite)) as Sprite;
            case Type.DIAMOND_ORE:
                return Resources.Load("DIAMOND_ORE", typeof(Sprite)) as Sprite;
            case Type.LEAVES:
                return Resources.Load("LEAVES", typeof(Sprite)) as Sprite;
            case Type.STONE_VARIANT_1:
                return Resources.Load("STONE_VARIANT_1", typeof(Sprite)) as Sprite;
            case Type.BEDROCK:
                return Resources.Load("BEDROCK", typeof(Sprite)) as Sprite;
            case Type.STONE:
                return Resources.Load("STONE", typeof(Sprite)) as Sprite;
            default:
                return Resources.Load("DIRT", typeof(Sprite)) as Sprite;
        }
    }

}


