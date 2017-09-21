using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDictionary : MonoBehaviour {

	public Dictionary<DialogueLine, LogicRelay> eventDict;
    [System.Serializable]
    public struct DialogueEvent {
        public DialogueLine line;
        public LogicRelay trigger;
    }
    public DialogueEvent[] eventList;

	// Use this for initialization
	void Start () {
		eventDict = new Dictionary<DialogueLine, LogicRelay>();
        foreach (DialogueEvent e in eventList) {
            eventDict.Add(e.line, e.trigger);
        }
	}
}
