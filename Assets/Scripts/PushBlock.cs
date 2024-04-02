using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MoveableBlock;

public class PushBlock : MonoBehaviour
{
    private List<MoveableBlock> moveableBlocks = new List<MoveableBlock>();

    void OnCollisionEnter(Collision other)
    {
        MoveableBlock moveableBlock = other.gameObject.GetComponent<MoveableBlock>();
        if (moveableBlock != null)
        {
            // Ajoute le MoveableBlock à la liste
            moveableBlocks.Add(moveableBlock);
        }
    }

    void OnCollisionExit(Collision other)
    {
        MoveableBlock moveableBlock = other.gameObject.GetComponent<MoveableBlock>();
        if (moveableBlock != null)
        {
            // Retire le MoveableBlock de la liste s'il n'est plus en collision
            moveableBlocks.Remove(moveableBlock);
        }
    }

    void Update()
    {
        // Vérifie si un clic de souris est détecté
        if (Input.GetMouseButtonDown(0))
        {
            // Trouve le MoveableBlock le plus proche dans la liste
            MoveableBlock nearestBlock = FindNearestMoveableBlock();

            // Si un MoveableBlock est trouvé, appelle sa méthode MoveBlock
            if (nearestBlock != null)
            {
                nearestBlock.MoveBlock(transform.position);
            }
        }
    }

    MoveableBlock FindNearestMoveableBlock()
    {
        MoveableBlock nearestBlock = null;
        float nearestDistance = Mathf.Infinity;

        // Parcourt tous les MoveableBlocks dans la liste
        foreach (MoveableBlock block in moveableBlocks)
        {
            // Calcule la distance entre le bloc actuel et cet objet
            float distance = Vector3.Distance(transform.position, block.transform.position);

            // Vérifie si cette distance est plus proche que la distance actuelle
            if (distance < nearestDistance)
            {
                nearestBlock = block;
                nearestDistance = distance;
            }
        }

        return nearestBlock;
    }
}
