using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script moves the attached object along the Y-axis with the defined speed
/// </summary>
public class BackgroundMoving : MonoBehaviour
{

    [Tooltip("Moving speed on X axis in local space")]
    public float speed;

    //moving the object with the defined speed
    private void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
