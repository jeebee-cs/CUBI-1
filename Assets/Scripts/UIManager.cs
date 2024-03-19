using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Slider _dreamBar;
    [SerializeField] TextMeshProUGUI _neutralDreamText;
    public Slider dreamBar { get => _dreamBar; }
    public TMP_Text neutralDreams { get => _neutralDreamText; }


}
