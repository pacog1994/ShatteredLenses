using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectReplacer : MonoBehaviour {

    public GameObject disableObject;
    public GameObject enableObject;

    void Awake() {
        disableObject.SetActive(false);
        enableObject.SetActive(true);
        Destroy(this);
    }
}
