using UnityEngine;
using UnityEngine.Windows.Speech;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class SpeechRecognition : MonoBehaviour {
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;

    private KeywordRecognizer recognizer;
    private Dictionary<string, System.Action> actions = new Dictionary<string, System.Action>();
    private string word;

    public GameObject player;

    public Camera cam;
    public BlockInteraction bi;
    public FirstPersonMovement fpm;
    public Jump jump;
    public Crouch crouch;
    public FirstPersonLook fpl;
    public BlockSelector bs;

    public TMP_Text uiText;

    void Start() {
        actions.Add("break", bi.BreakBlock);
        actions.Add("place", bi.BuildBlock);
        actions.Add("walk", fpm.StartWalk);
        actions.Add("stop", fpm.StopWalk);
        actions.Add("jump", jump.CharacterJump);
        actions.Add("crouch", () => {
            crouch.CharacterCrouch(true);
        });
        actions.Add("stand", crouch.CharacterStand);
        actions.Add("up", fpl.LookUp);
        actions.Add("down", fpl.LookDown);
        actions.Add("left", fpl.LookLeft);
        actions.Add("right", fpl.LookRight);
        actions.Add("next", () => {
            bs.SwitchBlock(true);
        });
        actions.Add("previous", () => {
            bs.SwitchBlock(false);
        });

        recognizer = new KeywordRecognizer(actions.Keys.ToArray(), confidence);
        recognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        recognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args) {
        word = args.text;
        System.Action action;
        if (actions.TryGetValue(word, out action)) {
            action.Invoke();
            uiText.text = word;
        }
        word = "";
    }

    private void OnApplicationQuit() {
        recognizer.OnPhraseRecognized -= KeywordRecognizer_OnPhraseRecognized;
        recognizer.Stop();
    }

}
