using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script is attached to virtualFrameObject->Sphere. The purpose of this script is to detect collision 
 * between the Controller collider and the virtualFrameObject Collider. On collision, the bool 
 * _moveable is toggled to true, and when collision is exited, _moveable is set to false. 
*/

public class CollisionDetector : MonoBehaviour
{
    // Handle changes in color when _moveable is toggled.
    public Material NotMoveableMaterial;
    public Material MoveableMaterial;
    Renderer rend;

    [HideInInspector] public static bool _moveable;

    // Use this for initialization
    void Start()
    {
        _moveable = false;

        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = NotMoveableMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        // When Collision occurs.

        if (other.gameObject.name == "Controller")
        {
            _moveable = true;
            Debug.Log("Object Moveable!");
            rend.sharedMaterial = MoveableMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When Collision is stopped.
        if (other.gameObject.name == "Controller")
        {
            _moveable = false;
            Debug.Log("Object Not Moveable!");
            rend.sharedMaterial = NotMoveableMaterial;
        }
    }
}