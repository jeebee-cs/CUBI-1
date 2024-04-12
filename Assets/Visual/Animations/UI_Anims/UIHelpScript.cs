using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelpScript : MonoBehaviour
{
    public Animator uiAnimator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("u"))
        {
            uiAnimator.ResetTrigger("Previous");
            uiAnimator.SetTrigger("Next");
        }
        if (Input.GetKeyDown("i"))
        {
            uiAnimator.ResetTrigger("Next");
            uiAnimator.SetTrigger("Previous");
        }
        if (Input.GetKeyDown("f"))
        {
            uiAnimator.SetTrigger("Called");
            uiAnimator.ResetTrigger("Next");
            uiAnimator.ResetTrigger("Previous");
            uiAnimator.ResetTrigger("Closed");
        }
        if (Input.GetKeyDown("g"))
        {
            uiAnimator.SetTrigger("Closed");
            uiAnimator.ResetTrigger("Next");
            uiAnimator.ResetTrigger("Previous");
            uiAnimator.ResetTrigger("Called");
        }

    }
}
