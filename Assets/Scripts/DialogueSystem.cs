using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class DialogueSystem : SerializedMonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    public float appearanceTime;
    public float fadeAwayTime;

    [ValueDropdown("typingSpeedDropDown")]
    public float typingSpeed;

    private ValueDropdownList<float> typingSpeedDropDown = new ValueDropdownList<float>()
    {
        { "Fast", 0.04f },
        { "Normal", 0.05f },
        { "Slow", 0.075f },
    };

    public bool inEvent = false;

    public void Awake()
    {
        dialogueText.gameObject.SetActive(false);
    }

    public IEnumerator DisplayDialogue(string dialogue)
    {
        /*if (inEvent)
        {
            Debug.Log("Already in dialogue");
            yield return null;
        }*/

        inEvent = true; 
        dialogueText.gameObject.SetActive(true);

        yield return DisplayText(dialogue);

        yield return new WaitForSeconds(appearanceTime);

        yield return FadeTextAway();

        dialogueText.gameObject.SetActive(false);
        inEvent = false;
    }

    public bool isShowingText()
    {
        return inEvent;
    }

    IEnumerator DisplayText(string dialogue)
    {
        dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b);
        dialogueText.text = dialogue;
        dialogueText.ForceMeshUpdate();
        // Type sentence //
        int totalVisibleCharacters = dialogueText.textInfo.characterCount; // Get # of Visible Character in text object
        int counter = 0;
        int visibleCount = 0;

        while (visibleCount < totalVisibleCharacters)
        {
            visibleCount = counter % (totalVisibleCharacters + 1);

            dialogueText.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

            counter += 1; //Increase by one then wait

            yield return new WaitForSeconds(typingSpeed);
        }

        yield return null;
    }

    public IEnumerator FadeTextAway()
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, i);
            yield return null;
        }
    }
}
