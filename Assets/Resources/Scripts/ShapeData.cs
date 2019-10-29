using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;


[CreateAssetMenu(fileName = "New Shape Data", menuName = "Shape Data")]
public class ShapeData : ScriptableObject
{
    public ShapeTypes _shapeType;
    public Vector3 position;
    public string shapeName;

}
