using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogues", menuName = "Custom/Dialogues")]
public class DreamDialogues : ScriptableObject
{
    public List<string> goodDialogues = new List<string>();
    public List<string> neutralDialogues = new List<string>();
    public List<string> badDialogues = new List<string>();
}
