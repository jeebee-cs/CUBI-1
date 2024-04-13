using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        AkSoundEngine.PostEvent("Stop_All_Audio", this.gameObject);
    }
}
