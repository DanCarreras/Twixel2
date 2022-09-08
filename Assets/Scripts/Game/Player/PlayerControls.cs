using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public GameObject LeftCube, RightCube;
    private PlayerInput playerControls;
    public float Force = 5.0f;

    void Awake()
    {
        playerControls = new PlayerInput();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        MoveLeftCube();
        MoveRightCube();
    }

    void MoveLeftCube()
    {
        if(playerControls.Player.MoveLeft.inProgress)
        {
            LeftCube.gameObject.GetComponent<Rigidbody>().AddForce(playerControls.Player.MoveLeft.ReadValue<Vector2>().x*Force,0,playerControls.Player.MoveLeft.ReadValue<Vector2>().y*Force);
        }
        else
        {
            LeftCube.gameObject.GetComponent<Rigidbody>().Sleep();
        }
    }

    void MoveRightCube()
    {
        if(playerControls.Player.MoveRight.inProgress)
        {
            RightCube.gameObject.GetComponent<Rigidbody>().AddForce(playerControls.Player.MoveRight.ReadValue<Vector2>().x*Force,0,playerControls.Player.MoveRight.ReadValue<Vector2>().y*Force);
        }
        else
        {
            RightCube.gameObject.GetComponent<Rigidbody>().Sleep();
        }
    }
    public void MoveLeft(InputAction.CallbackContext context)
    {
        Debug.Log("Fire!");
    }
}
