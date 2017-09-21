using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class UnlockedDoor : InteractObject {

    public string sceneToLoad;

    private AudioSource sound;
    private bool overlap = false;

	// Use this for initialization
	void Start () {
        sound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Interact") &&
            ((!GameManager.isEyeClosed && !interactWhenEyesClosed) || interactWhenEyesClosed) &&
            !GameManager.isDialoguePlaying) {
            if (overlap) {
                StartCoroutine(ChangeLevel());
            }
        }

    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            overlap = true;
            //Debug.Log("overlapping is " + overlapping);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            overlap = false;
            //Debug.Log("overlapping is " + overlapping);
        }
    }

    IEnumerator ChangeLevel() {
        overlap = false;
        DontDestroyOnLoad(gameObject);
        sound.Play();
        GameManager.instance.setGamePaused(true);
        GameManager.instance.toggleFade();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneToLoad);
        GameManager.instance.toggleFade();
        yield return new WaitForSeconds(1);
        GameManager.instance.setGamePaused(false);
        Debug.Log("Transition to scene complete");
        Destroy(gameObject);
    }
}
