using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractObject: MonoBehaviour {

    public bool interactWhenEyesClosed = false;
    public Dialogue defaultDialogue;

    public Item questItem;
    public Dialogue questDialogue;
    public UnityEvent questTrigger;

    private bool overlapping = false;
    
	void Update () {

        if (Input.GetButtonDown("Interact") &&
            ((!GameManager.isEyeClosed && !interactWhenEyesClosed) || (GameManager.isEyeClosed && interactWhenEyesClosed)) &&
            !GameManager.isDialoguePlaying &&
            !GameManager.isPlayerFrozen)
        {
            if (overlapping)
            {
                //Debug.Log("worked");
                if (GameManager.instance.findItem(questItem)) {
                    InteractWithItem(questItem);
                }
                else {
                    Interact();
                }
            }
            else
            {
                //Debug.Log("not working");
            }
        }
	}
    
    private void OnTriggerEnter(Collider other)
    {
            if(other.tag == "Player")
            {
                overlapping = true;
            //Debug.Log("overlapping is " + overlapping);
            }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            overlapping = false;
            //Debug.Log("overlapping is " + overlapping);
        }
    }

    public void Interact() {
        GameManager.instance.showDialogue(defaultDialogue);
        if (defaultDialogue.replacementDialogue != null) {
            defaultDialogue = defaultDialogue.replacementDialogue;
        }
    }

    public void InteractWithItem(Item item) {
        if (questDialogue != null) {
            if (questItem == item) {
                changeDialogue(questDialogue);
            }
        }
        Interact();
    }

    public void changeDialogue(Dialogue dialogue) {
        defaultDialogue = dialogue;
    }
}
