using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour {

    [SerializeField]
    private AudioSource source;

    private float audioVolume;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        audioVolume = source.volume;
	}

    void Update() {
        if (GameManager.isEyeClosed) {
            source.volume = 1.0f;
        }
        else {
            source.volume = audioVolume;
        }
    }

    void playSound() {
        source.Play();
    }

    void stopSound() {
        source.Stop();
    }
}
