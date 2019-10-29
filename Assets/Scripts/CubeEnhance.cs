using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CubeEnhance : MonoBehaviour
{
    [Header("Transform Variables")]

    [Space(10)]

    [Tooltip("Edit the position of Cube")]
    public Vector3 position;

    [Tooltip("Edit the rotation of Cube")]
    public Vector3 rotation;

    [Tooltip("Edit the size of Cube")]
    [Range(1f, 10f)]
    public float size;


    // Update is called once per frame
    void Update()
    {
        transform.position = position;
        transform.eulerAngles = rotation;
        transform.localScale = new Vector3(size, size, size);
    }
}
