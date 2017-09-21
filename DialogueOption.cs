using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Dialogue/Option")]
public class DialogueOption : ScriptableObject {

    [TextArea]
    public string description;
	public Dialogue response;

}
