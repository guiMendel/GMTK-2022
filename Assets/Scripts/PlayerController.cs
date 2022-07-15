using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour
{
    // === INTERFACE
    
    public float jumpForce = 20f;
    
    // === STATE

    // The action that will be executed in the next beat trigger
    UnityAction beatAction;

    // === REFS
    
    Rigidbody2D rigidBody;

    public void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Performs the current beat action
    public void PerformBeatAction() {
        if (beatAction != null) beatAction();
        else StandardAction();
        beatAction = null;
    }

    // Action performed when no other action is selected
    private void StandardAction() {
        rigidBody.AddForce(Vector2.up * jumpForce / 3, ForceMode2D.Impulse);
    }
    
    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && callbackContext.ReadValueAsButton()) {
            beatAction = () => rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }



}
