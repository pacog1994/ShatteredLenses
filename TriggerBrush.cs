using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerBrush : MonoBehaviour {

    [SerializeField]
    private Collider brush;

    public bool triggerOnce = false;
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    private int count = 0;

	// Use this for initialization
	void Start () {
        brush = GetComponent<Collider>();
	}

    void OnTriggerEnter() {
        if (!triggerOnce || (triggerOnce && count < 1)) {
            OnEnter.Invoke();
            count++;
        }
    }

    void OnTriggerExit() {
        if (!triggerOnce || (triggerOnce && count < 1)) {
            OnExit.Invoke();
            count++;
        }
    }
}
