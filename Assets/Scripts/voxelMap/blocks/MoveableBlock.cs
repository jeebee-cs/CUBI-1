using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ABlock;

public class MoveableBlock : ABlock
{
    public bool IsMoving { get; set; }

    private bool IsMovable = false;
    private Vector3  directionToPush;
    [SerializeField] LayerMask _layerCollision;
    [SerializeField] LayerMask _layerBlock;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveBlock();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_layerCollision == (_layerCollision | (1 << other.gameObject.layer)))
        {
            Vector3 directionToOther = other.gameObject.transform.position - transform.position;

            if (Mathf.Abs(directionToOther.y) > .5f) return;

            directionToOther = directionToOther.normalized;

            Debug.Log(directionToOther);
            directionToOther.x = Mathf.Round(directionToOther.x * .6f);
            directionToOther.z = Mathf.Round(directionToOther.z * .6f);
            directionToOther.y = 0;

            directionToPush = directionToOther * -1;

            if (Physics.Raycast(transform.position, directionToPush, 1f, _layerBlock)) return;

            IsMovable = true;
            //AkSoundEngine.PostEvent("Block_Push", this.gameObject);
        }
    }

    private void OnCollisionExit(Collision other){
        IsMovable = false;
    }

    void MoveBlock()
    {
        if(IsMovable){
            transform.position += directionToPush;
            IsMovable = false;
        }
    }
}