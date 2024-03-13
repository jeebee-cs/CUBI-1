using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ABlock;

public class MoveableBlock : ABlock
{
    public bool IsMoving { get; set; }
    [SerializeField] LayerMask _layerCollision;
    [SerializeField] LayerMask _layerBlock;

    private void OnCollisionEnter(Collision other)
    {
        if (_layerCollision == (_layerCollision | (1 << other.gameObject.layer)))
        {
            Vector3 directionToOther = other.gameObject.transform.position - transform.position;
            Vector3 directionToPush;

            if (directionToOther.y > 1) return;

            directionToOther = directionToOther.normalized;

            Debug.Log(directionToOther);
            directionToOther.x = Mathf.Round(directionToOther.x);
            directionToOther.z = Mathf.Round(directionToOther.z);
            directionToOther.y = 0;


            directionToPush = directionToOther * -1;


            if (Physics.Raycast(transform.position, directionToPush, 1f, _layerBlock)) return;

            transform.position += directionToPush;
        }
    }
}