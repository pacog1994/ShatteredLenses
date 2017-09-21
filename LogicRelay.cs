using UnityEngine;
using UnityEngine.Events;

public class LogicRelay : MonoBehaviour {

    public bool triggerOnce = false;
    public UnityEvent OnEvent;

    private int count = 0;

	public void Trigger() {
        if (!triggerOnce || (triggerOnce && count < 1)) {
            OnEvent.Invoke();
            count++;
        }
    }
}