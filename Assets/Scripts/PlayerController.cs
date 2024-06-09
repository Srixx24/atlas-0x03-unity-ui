using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Rigidbody for player.
    private Rigidbody rb; 

    // Movement along X and Y axes.
    private float movementX;
    private float movementY;

    // Player speed.
    public float speed = 0;

    // Player health
    public int health = 5;

    // Update score text
    public TextMeshProUGUI scoreText;

    // Update player health
    public TextMeshProUGUI healthText;

    void Start()
    {
        // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();

        // Find ScoreText and HealthText
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        healthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();

        // Initialize score and health methods
        SetScoreText();
        SetHealthText();
    }
 
    // Imput detected.
    void OnMove(InputValue movementValue)
    {
        // Change value to a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Stores movement.
        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }

    // Updates after health hits 0
    void Update()
    {
        if (health == 0)
        {
            Debug.Log($"Game Over!");
            ResetScene();
        }
    }

    // FixedUpdate is called once per fixed frame-rate frame.
    private void FixedUpdate() 
    {
        // Creates 3D movement.
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);

        // Force to the Rigidbody if not null
        if (rb != null)
        {
            rb.AddForce(movement * speed);
        }
    }

    // Player score
    private int score = 0;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            // Increment score and print total
            score++;
            SetScoreText();
            // Debug.Log($"Score: {score}");

            // Destroy coin after contact
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Trap"))
        {
            // Decrease health and print new value
            health--;
            SetHealthText();
            // Debug.Log($"Health: {health}");
        }

        if (other.CompareTag("Goal"))
        {
            if (score == 22)
            {
                // Print goal message
                Debug.Log($"You win!");
            }
            else
            {
                Debug.Log($"Keep looking for more coins!");
            }
        }
    }

    // Resets scene to start
    private void ResetScene()
    {
        health = 5;
        score = 0;

        // Reloads the start scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Update score text
    void SetScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    // Update player health
    void SetHealthText()
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {health}";
        }
    }
}