using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DreamCollection : MonoBehaviour
{

    public DreamCollection DreamCollectionList(int nbDreamsNeutral)
    {
        DreamCollection dreamsList = new DreamCollection();
        dreamsList._nbDreamsN = nbDreamsNeutral;
        return dreamsList;
    }

    private int _nbDreamsN { get; set; }
    public int dreamsCollectionN { get => _nbDreamsN; set => _nbDreamsN = value; }
    public void DreamNCollect()
    {
        if (_nbDreamsN <= 6)
        {
            _nbDreamsN++;
            GameManager.instance.uIManager.neutralDreams.text = _nbDreamsN.ToString();
        }
    }
}