using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PatchaController : MonoBehaviour
{
    // Patcha Variables
    public int maxHealth = 100;
    public int currentHealth;
    public int Lives = 3;
    public int Pickups = 0;
    public int pickupCounter;

    // Movement variables
    public float speed = 10f;
   
    public Vector2 move;

    // Component References
    private Rigidbody rb;
   
    // Script References
    public HealthBar healthBar;

    // Jumping Variables
    public float jumpForce = 10f;
    public bool isJumpPressed;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private void OnEnable()
    {
        InputSystem.EnableDevice((InputSystem.GetDevice<Keyboard>()));
    }

    private void OnDisable()
    {
        InputSystem.DisableDevice((InputSystem.GetDevice<Keyboard>()));
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialise components
        rb = GetComponent<Rigidbody>();

        // Initialise variables
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector3>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isJumpPressed = true;
        }
        else if (context.canceled)
        {
            isJumpPressed = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Damage test
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(20);
        }
        // Gain life Test
        if (Input.GetKeyDown(KeyCode.L))
        {
            AddLife(1);
        }
        // Lose Life Test
        if (currentHealth <= 0)
        {
            LoseLife(1);
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }
        // Pickup Test
        if (Input.GetKeyDown(KeyCode.P))
        {
            PickupFunc(1);
        }
        // Life from pickup Test
        if (pickupCounter >= 100)
        {
            AddLife(1);
            pickupCounter = 0;
            Debug.Log("You have gained a life from Pickups");
        }

        movePlayer();
        handleJump();
        
    }

    public void movePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f );
        }

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    private void handleJump()
    {
        if (isJumpPressed)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumpPressed = false;
        }

        if (rb.velocity.y < 0) // If the character is falling
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        else if
            (rb.velocity.y > 0 && !isJumpPressed) // i.e if the character releases the jump button mid-jump for a lower jump
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    

    // Custom Functions for Patcha
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        Debug.Log("Take Damage");
    }

    void AddLife(int Life)
    {
        Lives += Life;
        // Life Adding Test
        Debug.Log("Your Current Lives Are");
        Debug.Log(Lives);
    }

    void LoseLife(int lifelost)
    {
        Lives -= lifelost;
        Debug.Log("You have");
        Debug.Log(Lives);
        Debug.Log("Remaining");
    }

    void PickupFunc(int PickupValue)
    {
        Pickups += PickupValue;
        pickupCounter += PickupValue;
        Debug.Log("You have picked up a coin");
    }




  
}
