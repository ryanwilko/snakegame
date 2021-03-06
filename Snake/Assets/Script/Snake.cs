using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    // Did the snake eat something?
    bool ate = false;

    public int currentScore;
    public int highScore;

    public Text currentScoreText;
    public Text highScoreText;
    public Text gameOverText;

    //Did user die?
    bool isDead = false;

    // Tail Prefab
    public GameObject tailPrefab;

    // Restart Button
    public Button restartButton;

    // Current Movement Direction
    // (by default it moves to the right)
    Vector2 dir = Vector2.right;

    // Keep Track of Tail
    List<Transform> tail = new List<Transform>();

    // Use this for initialization
    void Start()
    {
        // Move the Snake every 300ms
        InvokeRepeating("Move", 0.3f, 0.3f);

        gameOverText.enabled = false;

        restartButton.gameObject.SetActive(false);

        currentScoreText.text = "Current Score: " + currentScore;

        highScore = PlayerPrefs.GetInt ("highScore", highScore);

        highScoreText.text = "High Score: " + highScore;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            // Move in a new Direction?
            if (Input.GetKey(KeyCode.RightArrow))
                dir = Vector2.right;
            else if (Input.GetKey(KeyCode.UpArrow))
                dir = Vector2.up;
            else if (Input.GetKey(KeyCode.LeftArrow))
                dir = Vector2.left;
            else if (Input.GetKey(KeyCode.DownArrow))
                dir = Vector2.down;
        }
        if (currentScore > highScore)
        {
            highScore = currentScore;
            highScoreText.text = "High Score: " + highScore;
            PlayerPrefs.SetInt ("highScore", highScore);
        }
    }

    void Move()
    {
        if (!isDead)
        {
            // Save current position (gap will be here)
            Vector2 v = transform.position;

            // Move head into new direction (now there is a gap)
            transform.Translate(dir);

            // Ate something? Then insert new Element into the gap
            if (ate)
            {
                // Load Prefab into the world
                GameObject g = (GameObject)Instantiate(tailPrefab,
                                  v,
                                  Quaternion.identity);

                // Keep track of it in our tail list
                tail.Insert(0, g.transform);

                // Reset the flag
                ate = false;
            }
            else if (tail.Count > 0)
            {   // Do we have a Tail?
                // Move last Tail Element to where the Head was
                tail.Last().position = v;

                // Add to front of list, remove from the back
                tail.Insert(0, tail.Last());
                tail.RemoveAt(tail.Count - 1);
            }
        }
    }
    // This is called if the snake hits something
    void OnTriggerEnter2D(Collider2D coll)
    {
        // Food?
        if (coll.name.StartsWith("Food"))
        {
            // Get longer in next Move call
            ate = true;
            // Remove the Food
            Destroy(coll.gameObject);

            currentScore++;
            currentScoreText.text = "Current Score: " + currentScore;
        }
        else
        {   // Collided with Tail or Border
            isDead = true;
            gameOverText.enabled = true;
            restartButton.gameObject.SetActive(true);
            PlayerPrefs.Save();
        }
    }
}
