using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    [SerializeField]

    private bool overlapping = false;
    [SerializeField]
    private AudioClip scareSound;

	// Use this for initialization
	void Start () {
     
	}
	
	// Update is called once per frame
	void Update () {
        if (overlapping) {
            if (!GameManager.isEyeClosed) {
                StartCoroutine(Lose());
            }
        }
	}

    void OnTriggerEnter (Collider other) {
        if (!GameManager.isEyeClosed)
        {
            overlapping = true;
        }
    }

    public void Defeat() {
        Debug.Log("enemy killed");
        if (gameObject != null) {
            Destroy(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    IEnumerator Lose() {
        GameManager.instance.scarePlayer(scareSound);
        overlapping = false;
        GetComponent<AudioSource>().Stop();

        yield return null;
    }
}
