using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float maxSpeed = 5f;
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (joystick.Direction != Vector2.zero)
        {
            direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
            direction.Normalize();
            controller.Move(direction * speed * Time.deltaTime);
            if(speed < maxSpeed)
            {
                speed += 0.1f;
            }
        }
        else
        {
            if(speed > 0)
            {
                speed -= 0.1f;
                controller.Move(direction * speed * Time.deltaTime);
            }

        }
    }
}
