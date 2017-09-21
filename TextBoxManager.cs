using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour
{
    //The box that holds the text to be displayed
    public GameObject textBox;
    //The inputted text file
    public TextAsset textfile;
    //The text displayed
    public Text theText;
    //current line
    public int currentLine;
    //end reading here
    public int endAtLine; 
    public string[] textLines;

    public GameObject player;

    public bool stopPlayerMovement;

    public bool isActive;

    // Use this for initialization
    void Start()
    {
        //get textFile and split it by break line
        if (textfile != null)
        {
            textLines = (textfile.text.Split('\n'));
        }

        if(endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }
        if(isActive)
        {
            EnableTextBox();
        } else
        {
            DisableTextBox();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive)
        {
            return;
        }

        if (currentLine >= endAtLine)
        {
            //DisableTextBox();
        }
        else
        {
            updateText(textLines[currentLine]);

            if (Input.GetButtonDown("Interact"))
            {
                currentLine += 1;
            }
        }

    }

    void updateText(string text) {
        theText.text = text;
    }

    public void EnableTextBox()
    {
        textBox.SetActive(true);
        isActive = true;
        if(stopPlayerMovement)
        {
          //  player.GetComponent<playerMovement>().canMove = false;
        }
    }

    public void DisableTextBox()
    {
        textBox.SetActive(false);
        
        //player.GetComponent<playerMovement>().canMove = true;
        isActive = false;
    }

    //Reloads a different text for use
   //@param theText the textfile to read
    public void ReloadScript(TextAsset theText) 
    {
        if(theText != null)
        {
            textLines = new string[1];
            textLines = (theText.text.Split('\n'));
        }
    }
}
