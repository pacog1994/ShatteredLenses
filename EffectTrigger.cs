using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTrigger : MonoBehaviour {

    [SerializeField]
    private AudioSource sound;
    [SerializeField]
    private int triggerCount;

    public bool triggerOnce = false;
    public float vignetteIntensity = 0;
    public float vignetteSmoothness = 0;
    public float bgmVolume = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other) {
        if (!triggerOnce || triggerCount < 1) {
            if (sound != null) {
                sound.Play();
            }
            GameManager.instance.setBGMVolume(bgmVolume);
            triggerCount++;
        }
    }
}
