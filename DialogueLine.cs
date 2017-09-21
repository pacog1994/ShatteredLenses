using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Dialogue/Line")]
public class DialogueLine : ScriptableObject {

    [TextArea]
    public string text;                     // text to display
    public AudioClip soundEffect;           // audio clip to be played
    public Item useItem;
    public Item reward;
    public bool die = false;
    public DialogueOption[] query;      // if non-empty, then options will be available for the player to select
}
