using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public static bool Swapping {
        get; private set;
    }

    public static bool isPlayerFrozen {
        get; private set;
    }

    public static bool isEyeClosed {
        get; private set;
    }

    public static bool isDialoguePlaying {
        get; private set;
    }

    public delegate void EyeClose();
    public static event EyeClose OnEyeClose;
    public delegate void EyeOpen();
    public static event EyeOpen OnEyeOpen;

    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private PostProcessingProfile _Profile;
    [SerializeField]
    private Material _glassShader;
    [SerializeField]
    private GameObject _glassCrack;
    [SerializeField]
    private Material _eyeShader;
    [SerializeField]
    private AnimationCurve _eyeTransition;
    [SerializeField]
    private AudioSource ambient;
    [SerializeField]
    private Animator jumpScare;
    [SerializeField]
    private Animator fade;

    public float transitionMultiplier;

    private ItemDatabase inventory;
    private DialogueManager dialogueBox;
    private float audioVolume;

    void Awake() {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        Cursor.visible = false;
        Player = FindObjectOfType<playerMovement>().gameObject;
        dialogueBox = GetComponentInChildren<DialogueManager>();
        inventory = GetComponent<ItemDatabase>();
        ambient = GetComponent<AudioSource>();
        _eyeShader.SetFloat("_Cutoff", 0.0f);
        audioVolume = ambient.volume;
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

	void Update () {
        if (Input.GetKey("escape")) {
            Application.Quit();
        }
        if (!Swapping && Input.GetButtonDown("Fire1") && !isPlayerFrozen) {
            if (isEyeClosed) {
                StartCoroutine(OpenEye());
            }
            else {
                StartCoroutine(CloseEye());
            }
        }
        if (isDialoguePlaying) {
            if (!dialogueBox.isDialoguePlaying) {
                Debug.Log("dialogue not playing");
                isDialoguePlaying = false;
                isPlayerFrozen = false;
            }
        }
	}

    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        Player = FindObjectOfType<playerMovement>().gameObject;
    }

    IEnumerator OpenEye() {
        Swapping = true;
        ambient.volume = audioVolume;

        for (float t = 1.0f; t > 0f; t -= Time.unscaledDeltaTime * transitionMultiplier) {
            _eyeShader.SetFloat("_Cutoff", _eyeTransition.Evaluate(t));

            yield return null;
        }

        _eyeShader.SetFloat("_Cutoff", _eyeTransition.Evaluate(0));

        if (OnEyeOpen != null) {
            OnEyeOpen();
        }

        isEyeClosed = false;
        Swapping = false;
    }

    IEnumerator CloseEye() {
        Swapping = true;

        for (float t = 0; t < 1.0f; t += Time.unscaledDeltaTime * transitionMultiplier) {
            _eyeShader.SetFloat("_Cutoff", _eyeTransition.Evaluate(t));

            yield return null;
        }

        _eyeShader.SetFloat("_Cutoff", _eyeTransition.Evaluate(1.0f));
        ambient.volume = audioVolume * 0.15f;

        if (OnEyeClose != null) {
            OnEyeClose();
        }

        isEyeClosed = true;
        Swapping = false;
    }

    IEnumerator Scare() {

        jumpScare.SetTrigger("Activate");

        yield return new WaitForSeconds(0.4f);

        gameOver();

        yield return new WaitForSeconds(1);

        toggleFade();

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        resetCrack();
        toggleFade();

        yield return new WaitForSeconds(3);

    }

    public void resetCrack()
    {
        _glassCrack.SetActive(false);
        isPlayerFrozen = false;

    }
    public void addItem(Item item) {
        inventory.AddItem(item);
    }

    public void removeItem(Item item) {
        inventory.RemoveItem(item);
    }
    public void clearItem()
    {
        inventory.Clear();
    }

    public bool findItem(Item item) {
        return inventory.FindItem(item);
    }

    public void setBGM(AudioClip sound) {
        ambient.clip = sound;
    }

    public void setBGMVolume(float volume) {
        audioVolume = volume;
        ambient.volume = volume;
    }

    public void showDialogue(Dialogue dialogue) {
        dialogueBox.setAndPlayDialogue(dialogue);
        isDialoguePlaying = true;
        isPlayerFrozen = true;
    }

    public void toggleFade() {
        fade.SetTrigger("Toggle");
    }

    public void scarePlayer(AudioClip sound) {
        isPlayerFrozen = true;
        if (isEyeClosed) {
            StartCoroutine(OpenEye());
        }
        ambient.PlayOneShot(sound);
        StartCoroutine(Scare());
    }

    public void setGamePaused(bool value) {
        isPlayerFrozen = value;
    }

    public void gameOver() {
        FindObjectOfType<SimpleBlit>().setShader(_glassShader);
        _glassCrack.SetActive(true);
    }
}
