using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deadZone : MonoBehaviour
{
    Timer _timerDeadZone = new Timer(.25f);
    private void OnTriggerEnter(Collider collision)
    {
        if (!_timerDeadZone.IsOver()) return;
        _timerDeadZone.Restart();
        collision.gameObject.transform.position = collision.gameObject.GetComponent<PushBlock>().lastGroundPosition;
        collision.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f,0f,0f);
        GameManager.instance.SetNeutralDreamCollectedServerRpc(GameManager.instance.neutralDreamCollected - 1);
    }
}