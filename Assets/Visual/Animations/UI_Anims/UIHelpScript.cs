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
        if (Input.GetAxis("Next") != 0.0)
        {
            uiAnimator.ResetTrigger("Previous");
            uiAnimator.SetTrigger("Next");
        }
        if (Input.GetAxis("Previous") != 0.0)
        {
            uiAnimator.ResetTrigger("Next");
            uiAnimator.SetTrigger("Previous");
        }
        if (Input.GetAxis("Fire2") != 0.0)
        {
            uiAnimator.SetTrigger("Called");
            uiAnimator.ResetTrigger("Next");
            uiAnimator.ResetTrigger("Previous");
            uiAnimator.ResetTrigger("Closed");
        }
        if (Input.GetAxis("Cancel") != 0.0)
        {
            uiAnimator.SetTrigger("Closed");
            uiAnimator.ResetTrigger("Next");
            uiAnimator.ResetTrigger("Previous");
            uiAnimator.ResetTrigger("Called");
        }

    }
}
