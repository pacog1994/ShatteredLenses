using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Dialogue/Dialogue Script")]
public class Dialogue : ScriptableObject {

    public DialogueLine[] lines;
    public Dialogue replacementDialogue;

}
