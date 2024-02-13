using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
  bool _isShowed = false;
  public bool isShowed { get => _isShowed; }

  public void Toggle(bool show)
  {
    _isShowed = show;
    gameObject.SetActive(_isShowed);

    EventSystem.current.SetSelectedGameObject(null);
  }
}
