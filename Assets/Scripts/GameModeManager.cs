using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour {

    public enum GameMode { Survival, Creative };
    public GameMode gameMode;
    public BlockSelector blockSelector;

    void Update() {
        if (Input.GetKeyDown(KeyCode.G))
            SwitchGameMode();
    }

    public void SetGameMode(GameMode mode) {
        gameMode = mode;
    }

    public void SwitchGameMode() {
        gameMode = (gameMode == GameMode.Survival ? GameMode.Creative : GameMode.Survival);
        blockSelector.SwitchHotBar();
    }
}
