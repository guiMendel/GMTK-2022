using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public float jumpForce = 20f;
    
    Rigidbody2D rigidBody;

    public void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    
    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && callbackContext.ReadValueAsButton()) {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }



}
