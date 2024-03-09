using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ABlock;

public class MoveableBlock : ABlock
{
   public bool IsMoving { get; set; }

    // Constructeur
    public MoveableBlock(Vector3 position)
    {
        Position = position;
        IsMoving = false; // Par défaut, le bloc mobile est immobile
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }
}