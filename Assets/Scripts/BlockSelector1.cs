using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlockSelector1 : MonoBehaviour {
    private List<BlockData.Type> blockTypes = new List<BlockData.Type>();

    private int blockIndex = 0;
    private BlockData.Type currentBlock;

    public TMP_Text blockText;

    void Start() {
        foreach (BlockData.Type type in Enum.GetValues(typeof(BlockData.Type)))
            if (type != BlockData.Type.AIR)
                blockTypes.Add(type);
        SetCurrentBlockType(blockTypes[blockIndex]);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E))
            switchBlock(true);
        else if (Input.GetKeyDown(KeyCode.Q))
            switchBlock(false);
    }


    private void switchBlock(bool next) {
        if (next) {
            if (blockIndex == blockTypes.Count - 1)
                blockIndex = 0;
            else
                blockIndex++;
        } else {
            if (blockIndex == 0)
                blockIndex = blockTypes.Count - 1;
            else
                blockIndex--;
        }
        SetCurrentBlockType(blockTypes[blockIndex]);
    }

    public BlockData.Type GetCurrentBlockType() {
        return currentBlock;
    }

    private void SetCurrentBlockType(BlockData.Type bType) {
        currentBlock = bType;
        blockText.text = currentBlock.ToString();
    }

}
