using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour {

//    [System.Serializable]
//    public struct DialogueEvent {
//        public DialogueLine line;
//        public LogicRelay trigger;
//    }
//    public DialogueEvent[] eventList;

	public Dictionary<DialogueLine, LogicRelay> eventDict;

    public bool isDialoguePlaying {
        get; private set;
    }

    public bool isMenuActive {
        get; private set;
    }
		
    [SerializeField]
    private Dialogue text;
    [SerializeField]
    private List<DialogueOption> optionList;
    [SerializeField]
    private List<Button> menu;
    [SerializeField]
    private Button optionPrefab;

    private Text textField;
    private AudioSource audioSource;
    private Animator anim;
    private int index = 0;
    private int selected = 0;
    private bool isAnimationPlaying = false;
    private bool isMenuInteractive = false;

    // Use this for initialization
    void Start () {
		
		if (FindObjectOfType<EventDictionary> () == null) {
			Debug.Log ("no dictionary...");
			eventDict = new Dictionary<DialogueLine, LogicRelay> ();
		} else {
			Debug.Log ("dictionary found...");
			eventDict = FindObjectOfType<EventDictionary>().eventDict;
		}
		//SceneManager.sceneLoaded += OnLevelFinishedLoading;
        textField = GetComponentInChildren<Text>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        anim.SetBool("isActive", false);

//        EventDictionary = new Dictionary<DialogueLine, LogicRelay>();
//        foreach (DialogueEvent e in eventList) {
//            EventDictionary.Add(e.line, e.trigger);
//        }
	}

	//void OnDisable() {
	//	SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	//}

//	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
	//	Debug.Log ("reading dictionary...");
	//	eventDict = FindObjectOfType<EventDictionary>().eventDict;
	//}
	
	// Update is called once per frame
	void Update () {
        if (isDialoguePlaying && !isMenuActive) {
            if (Input.GetButtonDown("Interact") && !isAnimationPlaying) {
                index++;
                if (index >= text.lines.Length) {
                    StartCoroutine(HideDialogBox());
                }
                else {
                    setText(text.lines[index]);
                }
            }
        }
        else if (isDialoguePlaying && isMenuActive) {
            int VerticalInput = (int)Input.GetAxis("Vertical");
            //Debug.Log("Vertical Input = " + VerticalInput.ToString());
            if (VerticalInput != 0 && isMenuInteractive) {
                isMenuInteractive = false;
                StartCoroutine(ChangeMenu(VerticalInput));
            }
            if (Input.GetButtonDown("Interact") && isMenuInteractive){
                //Debug.Log("Interact Input");
                if (text != null) {
                    playDialogue();
                }
                else {
                    StartCoroutine(HideDialogBox());
                }
                StartCoroutine(HideMenu());
            }
        }
	}

    public void playDialogue() {
        if (text != null) {
            index = 0;
            setText(text.lines[index]);
            if (!isDialoguePlaying) {
                isDialoguePlaying = true;
                StartCoroutine(ShowDialogBox());
            }
        }
    }

    public void setText(DialogueLine line) {
        if (line.die) {
            StartCoroutine(HideDialogBox());
            StartCoroutine(die(line.soundEffect));
            return;
        }
        audioSource.Play();
        if (line.soundEffect != null) {
            audioSource.PlayOneShot(line.soundEffect);
        }
        textField.text = line.text;
        if (line.useItem != null) {
            GameManager.instance.removeItem(line.useItem);
        }
        if (line.reward) {
            GameManager.instance.addItem(line.reward);
        }
        LogicRelay trigger;
		getDictionary ();
		if (eventDict != null && eventDict.ContainsKey(line)) {
            if (eventDict.TryGetValue(line, out trigger)) {
                trigger.Trigger();
            }
            else {
                Debug.Log("Trigger not found");
            }
        }
        if (line.query.Length > 0) {
            selected = 0;
            optionList = new List<DialogueOption>();
            menu = new List<Button>();

            for (int i = 0; i < line.query.Length; i++) {
                DialogueOption option = line.query[i];
                Button newOption = Instantiate<Button>(optionPrefab);

                newOption.GetComponentInChildren<Text>().text = option.description;
                newOption.transform.SetParent(transform, false);
                newOption.transform.Translate(new Vector2(0, i * optionPrefab.GetComponent<Image>().rectTransform.rect.height));

                optionList.Add(option);
                menu.Add(newOption);
            }

            setDialogue(optionList[selected].response);
            menu[selected].Select();
            menu[selected].OnSelect(null);

            StartCoroutine(ShowMenu());
        }
    }

    public void setDialogue(Dialogue dialogue) {
        index = 0;
        text = dialogue;
    }

    public void setAndPlayDialogue(Dialogue dialogue) {
        setDialogue(dialogue);
        playDialogue();
    }

    IEnumerator ShowDialogBox() {
        isAnimationPlaying = true;
        anim.SetBool("isActive", true);

        yield return new WaitForSeconds(0.25f);

        isAnimationPlaying = false;
    }

    IEnumerator HideDialogBox() {
        isAnimationPlaying = true;
        anim.SetBool("isActive", false);
        
        yield return new WaitForSeconds(0.25f);

        isAnimationPlaying = false;
        isDialoguePlaying = false;
    }

    IEnumerator ShowMenu() {
        isMenuActive = true;
        isAnimationPlaying = true;

        foreach (Button button in menu) {
            button.GetComponent<Animator>().SetBool("isActive", true);
        }

        yield return new WaitForSeconds(0.25f);

        isAnimationPlaying = false;
        isMenuInteractive = true;
    }

    IEnumerator HideMenu() {
        isMenuInteractive = false;
        isMenuActive = false;
        isAnimationPlaying = true;

        foreach (Button button in menu) {
            button.GetComponent<Animator>().SetBool("isActive", false);
        }

        yield return new WaitForSeconds(0.25f);

        foreach (Button button in menu) {
            DialogueManager.Destroy(button.gameObject);
        }

        menu.Clear();
        optionList.Clear();

        isAnimationPlaying = false;
    }

    IEnumerator ChangeMenu(int input) {
        if (input > 0 && selected < menu.Count - 1) {
            selected++;
        }
        else if (input < 0 && selected > 0) {
            selected--;
        }

        setDialogue(optionList[selected].response);
        menu[selected].Select();
        menu[selected].OnSelect(null);

        yield return new WaitForSeconds(0.2f); // change to modify input delay

        isMenuInteractive = true;
    }

    IEnumerator die(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
        GameManager.instance.gameOver();
        yield return new WaitForSeconds(1);
        GameManager.instance.toggleFade();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.resetCrack();
        GameManager.instance.toggleFade();
        GameManager.instance.clearItem();
        yield return new WaitForSeconds(3);
    }
    void getDictionary() {
		if (FindObjectOfType<EventDictionary> () == null) {
			Debug.Log ("no dictionary...");
			eventDict = new Dictionary<DialogueLine, LogicRelay> ();
		} else {
			Debug.Log ("dictionary found...");
			eventDict = FindObjectOfType<EventDictionary>().eventDict;
		}
	}
}
