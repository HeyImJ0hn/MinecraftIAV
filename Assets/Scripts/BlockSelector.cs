using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlockSelector : MonoBehaviour {
    public GameObject goSlots;

    private BlockData.Type[] hotbar = new BlockData.Type[9];
    private int index = 0;

    private Color selectedColor = new Color(0.76470588235f, 0.76470588235f, 0.76470588235f, 1.0f);
    private Color defaultColor = new Color(1.0f, 1.0f, 1.0f, 0.58823529411f);
    private Color imgColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color emptyImgColor = new Color(1.0f, 1.0f, 1.0f, 0f);
    private Color textColor = new Color(0f, 0f, 0f, 1f);
    private Color emptyTextColor = new Color(0f, 0f, 0f, 0f);

    void Start() {
        for (int i = 0; i < hotbar.Length; i++)
            hotbar[i] = BlockData.Type.AIR;

        goSlots.transform.GetChild(index).GetComponentsInChildren<Image>()[0].color = selectedColor;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E))
            SwitchBlock(true);
        else if (Input.GetKeyDown(KeyCode.Q))
            SwitchBlock(false);
    }


    public void SwitchBlock(bool next) {
        index = (index + (next ? 1 : -1) + hotbar.Length) % hotbar.Length;
        for (int i = 0; i < hotbar.Length; i++)
            goSlots.transform.GetChild(i).GetComponentsInChildren<Image>()[0].color = (i == index ? selectedColor : defaultColor);
    }

    public BlockData.Type GetCurrentBlockType() {
        return hotbar[index];
    }

    public void AddBlock(BlockData.Type bType) {
        int amount = BlockData.GetAmount(bType);
        BlockData.SetAmount(bType, ++amount);

        bool found = false;
        for (int i = 0; i < hotbar.Length; i++) {
            if (hotbar[i] == bType) {
                goSlots.transform.GetChild(i).GetComponentInChildren<TMP_Text>().text = amount.ToString();
                found = true;
            }
        }

        if (!found)
            for (int i = 0; i < hotbar.Length; i++) {
                if (hotbar[i] == BlockData.Type.AIR) {
                    hotbar[i] = bType;
                    goSlots.transform.GetChild(i).GetComponentsInChildren<Image>()[1].sprite = BlockData.GetSprite(bType);
                    goSlots.transform.GetChild(i).GetComponentsInChildren<Image>()[1].color = imgColor;
                    goSlots.transform.GetChild(i).GetComponentInChildren<TMP_Text>().color = textColor;
                    goSlots.transform.GetChild(i).GetComponentInChildren<TMP_Text>().text = amount.ToString();
                    return;
                }
            }
    }

    public void RemoveBlock() {
        int amount = BlockData.GetAmount(hotbar[index]);
        BlockData.SetAmount(hotbar[index], --amount);

        if (amount == 0) {
            hotbar[index] = BlockData.Type.AIR;
            goSlots.transform.GetChild(index).GetComponentInChildren<TMP_Text>().text = amount.ToString();
            goSlots.transform.GetChild(index).GetComponentInChildren<TMP_Text>().color = emptyTextColor;
            goSlots.transform.GetChild(index).GetComponentsInChildren<Image>()[1].color = emptyImgColor;
        } else {
            goSlots.transform.GetChild(index).GetComponentInChildren<TMP_Text>().text = amount.ToString();
        }
    }

}
