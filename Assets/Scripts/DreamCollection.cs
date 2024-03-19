using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCollection : MonoBehaviour
{
    public DreamCollection DreamCollectionList (int nbDreamsNeutral)
    {
        DreamCollection dreamsList = new DreamCollection();
        dreamsList.nbDreamsN = nbDreamsNeutral;
        return dreamsList;
    }
    private int nbDreamsN { get; set; }

    public void DreamNCollect() {
        nbDreamsN++;
        GameManager.instance.uIManager.neutralDreams.text = nbDreamsN.ToString();
    }
}
