using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonalControllerNew : MonoBehaviour
{
    public float speed = 5;
    private CharacterController _characterController;
    private const float gravity = -9.8f;
    void Awake()
    {
        /*_characterController = GetComponent<CharacterController>();
        _characterController ??= gameObject.AddComponent<CharacterController>();*/
        _characterController = gameObject.AddComponent<CharacterController>();
    }

    void Update()
    {
        var moveX = Input.GetAxis("Horizontal") * speed;
        var moveZ = Input.GetAxis("Vertical") * speed;
        var movement = new Vector3(moveX, 0, moveZ);
        movement = Vector3.ClampMagnitude(movement, speed);
        movement.y = gravity;
        movement *= Time.deltaTime;


        movement = transform.TransformDirection(movement);
        _characterController.Move(movement);
        /*
        // Get targetVelocity from input.
        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * speed,
            Input.GetAxis("Vertical") * speed);

        // Apply movement.
        rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
        */
    }
}