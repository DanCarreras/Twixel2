using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject LeftCube, RightCube;

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void MoveLeft(InputAction.CallbackContext context)
    {
        Debug.Log("Fire!");
    }
}
