using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamDisplayer : DialogueSystem
{
    public DreamDialogues dialoguesPool;

    public void Start()
    {
        DisplayDreamDialogue(DreamType.NEUTRAL);
    }

    public void DisplayDreamDialogue(DreamType dreamType)
    {
        string dialogue = RandomDialogue(dreamType);

        switch (dreamType)
        {
            case DreamType.NEUTRAL:
                dialogueText.color = Color.white;
                break;
            case DreamType.GOOD:
                dialogueText.color = Color.green;
                break;
            case DreamType.BAD:
                dialogueText.color = Color.red;
                break;
        }

        if (GameManager.instance.dreamDisplayer.inEvent)
        {
            StopCoroutine(DisplayDialogue(dialogue));
            StopCoroutine(FadeTextAway(1));
        }
        StartCoroutine(DisplayDialogue(dialogue));
    }

    string RandomDialogue(DreamType dreamType)
    {
        string dialogue;
        switch (dreamType)
        {
            case DreamType.NEUTRAL:
                dialogue = dialoguesPool.neutralDialogues[Random.Range(0, dialoguesPool.neutralDialogues.Count)];
                break;
            case DreamType.GOOD:
                dialogue = dialoguesPool.goodDialogues[Random.Range(0, dialoguesPool.goodDialogues.Count)];
                break;
            case DreamType.BAD:
                dialogue = dialoguesPool.badDialogues[Random.Range(0, dialoguesPool.badDialogues.Count)];
                break;
            default:
                dialogue = dialoguesPool.neutralDialogues[Random.Range(0, dialoguesPool.neutralDialogues.Count)];
                break;
        }
        return dialogue;
    }
}
