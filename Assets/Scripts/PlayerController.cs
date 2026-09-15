using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{    
    public float thrustForce = 0.25f;
    Rigidbody2D rb;
    public GameObject boosterFlame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction;
        // checks if any of the movement keys are pressed (WASD)
        Boolean keyPressed = Keyboard.current.wKey.isPressed || Keyboard.current.sKey.isPressed || Keyboard.current.dKey.isPressed || Keyboard.current.aKey.isPressed;
        
        if (Mouse.current.leftButton.isPressed)
        {   
            // Calculate Mouse Position
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            direction = (mousePos - (Vector2) transform.position).normalized;

            // Move player in direction of mouse
            transform.up = direction;
            rb.AddForce(direction * thrustForce);
        }
        else if (keyPressed)
        {       // adds each direction to the vector so its calculates the vector direction 
                direction = Vector2.zero;
                if (Keyboard.current.wKey.isPressed)
                {
                    direction += Vector2.up;
                }
                if (Keyboard.current.sKey.isPressed)
                {
                    direction += Vector2.down;
                }
                if (Keyboard.current.dKey.isPressed)
                {
                    direction += Vector2.right;
                }
                if (Keyboard.current.aKey.isPressed)
                {
                    direction += Vector2.left;
                }

                // Move player in the sum of directions 
                transform.up = direction; // change the tip of the rocket to the direction
                Debug.Log(direction +"  ,  "+ direction.normalized); // Do I really need normalization?
                rb.AddForce(direction.normalized * thrustForce);
        }
        // adding booster
        if (Mouse.current.leftButton.isPressed || keyPressed)
        {
            boosterFlame.SetActive(true);
        }
        else if (!Mouse.current.leftButton.isPressed || !keyPressed)
        {
            boosterFlame.SetActive(false);
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
