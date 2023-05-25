using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float jumpForce = 10f;
    public Vector3 movement;

    // Component References
    private Rigidbody Rbody;
    public bool isGrounded; // Flag to track if the player is grounded

    // Script References
    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        // Initialise components
        Rbody = GetComponent<Rigidbody>();

        // Initialise variables
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
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
        // Movement code
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Jumping input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        MoveChar(movement);
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

    void MoveChar(Vector3 direction)
    {
        Rbody.velocity = direction * speed;
    }

    void Jump()
    {
        Rbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // Detect if the player is grounded
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
