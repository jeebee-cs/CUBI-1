using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ABlock;

public class MoveableBlock : ABlock
{
    public bool IsMoving=false;
    private Vector3  directionToPush;
    [SerializeField] LayerMask _layerCollision;
    [SerializeField] LayerMask _layerBlock;

    [SerializeField, Range(0f, 1f)]
	private float deplacementTime = 0.5f;

    public void MoveBlock(Vector3 otherPos)
    {

        Vector3 directionToOther = otherPos - transform.position;

        if (Mathf.Abs(directionToOther.y) > .5f) return;

        directionToOther = directionToOther.normalized;

        Debug.Log(directionToOther);
        directionToOther.x = Mathf.Round(directionToOther.x * .6f);
        directionToOther.z = Mathf.Round(directionToOther.z * .6f);
        directionToOther.y = 0;

        directionToPush = directionToOther * -1;

        if (Physics.Raycast(transform.position, directionToPush, 1f,_layerBlock))
        {
            Debug.Log("Block hit");
            return;
        }

        //AkSoundEngine.PostEvent("Block_Push", this.gameObject);
        IsMoving = true;
        StartCoroutine(SmoothLerp(transform.position + directionToPush, deplacementTime));
    }

    private IEnumerator SmoothLerp (Vector3 newPosition, float time)
    {
            Vector3 startingPos  = transform.position;

            float elapsedTime = 0;

            while (elapsedTime < time)
            {
                transform.position = Vector3.Lerp(startingPos, newPosition, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = newPosition;
            IsMoving = false;
    }
}