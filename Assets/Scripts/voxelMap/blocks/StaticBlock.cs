using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ABlock;

public class StaticBlock : ABlock
{
    // Propriétés spécifiques aux blocs fixes si nécessaire
    public Color Color { get; set; }

    // Constructeur
    public StaticBlock(Vector3 position, Color color)
    {
        Position = position;
        Color = color;
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }
}