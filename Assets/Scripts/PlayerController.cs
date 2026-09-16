using System;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class PlayerController : MonoBehaviour
{
    private float elapsedTime = 0f;
    private float score = 0f;
    private float scoreMultiplier = 10f;
    private float highScore = 0f;
    public float thrustForce = 0.25f;
    Rigidbody2D rb;
    public GameObject boosterFlame;
    public UIDocument uiDocument;
    private Label scoreText;
    private Label highScoreText;
    private Button restartButton;
    private Button resetHighScoreButton;
    public GameObject explosionEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");

        highScoreText = uiDocument.rootVisualElement.Q<Label>("HighScore");
        highScoreText.style.display = DisplayStyle.None;
        highScore = PlayerPrefs.GetFloat("HighScore");

        resetHighScoreButton = uiDocument.rootVisualElement.Q<Button>("ResetHighScore");
        resetHighScoreButton.style.display = DisplayStyle.None;

        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;
        resetHighScoreButton.clicked += ResetHighScore;
    }

    // Update is called once per frame
    void Update()
    {

        UpdateScore();
        MovePlayer();
        
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        restartButton.style.display = DisplayStyle.Flex;


        
        if (score> highScore){
            PlayerPrefs.SetFloat("HighScore",score);
            highScore = score;
        }
        highScoreText.text = "High Score: " + highScore;
        highScoreText.style.display = DisplayStyle.Flex;
        resetHighScoreButton.style.display = DisplayStyle.Flex;


    }
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void ResetHighScore()
    {
        PlayerPrefs.SetFloat("HighScore",0f);
        highScoreText.text = "High Score: " + 0;
    }

    void UpdateScore()
    {
        // implements score
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        scoreText.text = "Score: " + score;
        
    }

    void MovePlayer()
    {
        Vector2 direction;
        // checks if any of the movement keys are pressed (WASD)
        Boolean keyPressed = Keyboard.current.wKey.isPressed || Keyboard.current.sKey.isPressed || Keyboard.current.dKey.isPressed || Keyboard.current.aKey.isPressed || Keyboard.current.upArrowKey.isPressed || Keyboard.current.downArrowKey.isPressed || Keyboard.current.leftArrowKey.isPressed || Keyboard.current.rightArrowKey.isPressed;
        
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
                if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
                {
                    direction += Vector2.up;
                }
                if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                {
                    direction += Vector2.down;
                }
                if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                {
                    direction += Vector2.right;
                }
                if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
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
}
