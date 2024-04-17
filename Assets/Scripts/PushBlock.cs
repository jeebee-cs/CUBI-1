using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PushBlock : MonoBehaviour
{
    private List<MoveableBlock> _moveableBlocks = new List<MoveableBlock>();
    public List<MoveableBlock> moveableBlocks { get => _moveableBlocks; }
    Vector3 _lastGroundPosition = Vector3.zero;
    public Vector3 lastGroundPosition { get => _lastGroundPosition; }
    PlayerMovements _playerMovements;
    void Start()
    {
        _playerMovements = GetComponent<PlayerMovements>();
    }

    void OnCollisionEnter(Collision other)
    {
        MoveableBlock moveableBlock = other.gameObject.GetComponent<MoveableBlock>();


        if (moveableBlock != null)
        {
            if (moveableBlock.bigBlock)
            {
                if (transform.position.y - other.gameObject.transform.position.y > 1.2f) _lastGroundPosition = other.gameObject.transform.position + Vector3.up * 2;
            }
            else
            {
                if (transform.position.y - other.gameObject.transform.position.y > .6f) _lastGroundPosition = other.gameObject.transform.position + Vector3.up * 2;
            }
            // Ajoute le MoveableBlock à la liste
            _moveableBlocks.Add(moveableBlock);
        }
        else _lastGroundPosition = other.gameObject.transform.position + Vector3.up * 2;
    }

    void OnCollisionExit(Collision other)
    {
        MoveableBlock moveableBlock = other.gameObject.GetComponent<MoveableBlock>();
        if (moveableBlock != null)
        {
            // Retire le MoveableBlock de la liste s'il n'est plus en collision
            _moveableBlocks.Remove(moveableBlock);
        }
    }

    void Update()
    {
        // Vérifie si un clic de souris est détecté
        if (Input.GetAxis("Fire1") != 0.0)
        {
            // Trouve le MoveableBlock le plus proche dans la liste
            MoveableBlock nearestBlock = FindNearestMoveableBlock();

            // Si un MoveableBlock est trouvé, appelle sa méthode MoveBlock
            if (nearestBlock != null && !nearestBlock.IsMoving)
            {
                if (nearestBlock.bigBlock)
                {
                    PlayerMovements otherPlayer = null;
                    foreach (PlayerMovements playerMovement in GameManager.instance.playerMovements)
                    {
                        if (playerMovement != _playerMovements) otherPlayer = playerMovement;
                    }
                    if (otherPlayer == null) return;

                    if (Vector3.Distance(otherPlayer.transform.position, nearestBlock.transform.position) < 1.75f && Vector3.Distance(otherPlayer.transform.position, transform.position) < 1.35f)
                    {
                        if (UsefulFunctions.IsBetween(otherPlayer.transform.position.x - transform.position.x, -.5f, .5f) || UsefulFunctions.IsBetween(otherPlayer.transform.position.z - transform.position.z, -.5f, .5f))
                        {
                            nearestBlock.MoveBlock(transform.position, _playerMovements.OwnerClientId);
                        }
                    }
                }
                else nearestBlock.MoveBlock(transform.position, _playerMovements.OwnerClientId);
            }
        }
    }

    MoveableBlock FindNearestMoveableBlock()
    {
        MoveableBlock nearestBlock = null;
        float nearestDistance = Mathf.Infinity;

        // Parcourt tous les _MoveableBlocks dans la liste
        foreach (MoveableBlock block in _moveableBlocks)
        {
            // Calcule la distance entre le bloc actuel et cet objet
            float distance = Vector3.Distance(transform.position, block.transform.position);
            if (block.bigBlock)
            {
                if (Mathf.Abs(transform.position.y - block.transform.position.y) > 1f) continue;
                distance *= .5f;
            }
            else if (Mathf.Abs(transform.position.y - block.transform.position.y) > .5f) continue;

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
