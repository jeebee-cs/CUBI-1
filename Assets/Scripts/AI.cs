using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AI : MonoBehaviour
{
    AIMode _mode = AIMode.Collect;
    [SerializeField] float _distanceToFlight;
    [SerializeField] Timer _timerBeforeCollectMode;

    void FixedUpdate()
    {
        if (_mode == AIMode.Flight)
        {

        }
        else if (_mode == AIMode.Collect)
        {

        }

        if (Vector3.Distance(GameManager.instance.playerMovement.transform.position, transform.position) > _distanceToFlight)
        {
            _mode = AIMode.Flight;
            _timerBeforeCollectMode.Restart();
        }
        else if (_timerBeforeCollectMode.IsOver())
        {
            _mode = AIMode.Collect;
        }
        Debug.Log(_mode);
    }
}

public enum AIMode
{
    Collect,
    Flight
}
