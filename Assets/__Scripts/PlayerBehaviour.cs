using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{   
    public float timeLeft; // value of time remaining
    public float defaultSpeedMod; // default speed modifier
    public float mineStrength; // reduction in speed after hitting a mine
    public float timeBoost; // amount of time added to the clock by the boosts
    public Text scoreText; // text for the score to display on the UI
    public Text timer; // text fof the timer in top corner
    public Text endText; // text that displays when the game is concluded
    public Text scoreDiff; // text of score differential displayed (+/- X)
    public Text mineCooldownText; // text of how long mine debuff is active (Xs)

    private Rigidbody playerRB; // rigidbody component for the player
    private float speedMod; // variable movement speed modifier in game
    private int score; // used to keep score for the game, different pickups give different scores
    private float scoreDiffTime; // display time for score difference work
    private float mineCooldown; // cooldown time for the mine debuff
    private int pickupTally; // keeps track of how many pickups have been collected (max 18)
   
    void Start () // start function, entrance to game
    {
        playerRB = GetComponent<Rigidbody>(); // assigns the variable to the actual rigidbody component
        playerRB.isKinematic = false; // allowing the player to move
        speedMod = defaultSpeedMod; // sets the variable speed modifier to the default speed modifier
        score = 0; // score starts at zero
        SetScoreText(); // add score text to UI
        endText.text = ""; //default of not winning 
    }

    // Update is called every frame
    void Update() 
    {   
        // counting down the time and displaying it
        timeLeft -= Time.deltaTime;
        SetTimer();

        // lower the time to display the differnce in score and reduce cooldown on mine debuff
        scoreDiffTime -= Time.deltaTime;
        mineCooldown -= Time.deltaTime;

        ResetScoreDiff(); // removes score differential text if time is up
        ResetMineDebuff(); // removes mine debuff if cooldown is complete

        // checking to see if all the time has elapsed
        if (timeLeft <= 0)
        {
            // restart function
            Restart();
        }

        // checking to see if all pickups have been collected
        if (pickupTally == 18) 
        {
            // restart function
            Restart();
        }
    }

    void FixedUpdate ()  // called before physics calculations each frame
    {
        float moveHorizontal = Input.GetAxis ("Horizontal"); // gets horizontal displacement data
        float moveVertical = Input.GetAxis ("Vertical"); // gets vertical displacement data

        Vector3 playerMovement = new Vector3 (moveHorizontal, 0.0f, moveVertical); // create the movement vector

        playerRB.AddForce (playerMovement * speedMod); // adds the force to the palyer
    }

    // collision event handler
    void OnTriggerEnter (Collider other) 
    {    
        if (other.gameObject.CompareTag("Pickup")) // pickups increase score
        {
            other.gameObject.SetActive(false);
            score += 2; // changes the score
            SetScoreText(); // updates score
            scoreDiffTime = 0.75f; // score difference display time
            SetScoreDiff("+2"); // sets score difference text
            pickupTally++; // augments the pickup tally
        } 
        else if (other.gameObject.CompareTag("Mine")) // mines reduce score, apply a speed decrease
        {
            other.gameObject.SetActive(false);
            score -= 3; // changes the score
            SetScoreText(); // updates score
            scoreDiffTime = 0.75f; // score difference display time
            SetScoreDiff("-3"); // sets score difference text
            if (mineCooldown <= 0)
            {
                mineCooldown = 0f; // resetting cooldown
            }
            mineCooldown += 3.0f; // adding 3 more seconds, makes debuff time stack
            SetMineDebuff(); // applying the debuff
        }
        else if (other.gameObject.CompareTag("Boost")) // boost slightly increase score, adds time to clock
        {
            other.gameObject.SetActive(false);
            score += 1; // changes the score
            SetScoreText(); // updates score
            scoreDiffTime = 0.75f; // score difference display time
            SetScoreDiff("+1"); // sets score differnce text
            timeLeft += timeBoost; // adds time to the time remaining
        }
        else if (other.gameObject.CompareTag("Teleporter")) // teleporter moves the position of the player to L island
        {
            transform.position = new Vector3(-61, 0.6f, -78); // L island coordinates
        }
        else if (other.gameObject.CompareTag("Teleporter A")) // teleporter moves the position of the player to octo island
        {
            transform.position = new Vector3(75, 0.6f, 65); // octo island coordinates
        } 
        else if (other.gameObject.CompareTag("Teleporter B")) // teleporter moves the position of the player to center island
        {
            transform.position = new Vector3(0, 0.6f, 0); // center island coordinates
        }  
    }

    // restarts the game
    public void Restart()
    {
        playerRB.isKinematic = true; // freezes player in place
        
        if (timeLeft > 0) // if called with time remaining (e.g. all pickups acquired)
        {
            timeLeft += Time.deltaTime; // required so score remains stable (not decreasing), and time remains the same
            endText.text = "Winner!\nFinal score: " + (score+(int)timeLeft).ToString(); // display final score
            StartCoroutine(PauseThenRestart(5)); // restarts level after a delay
        }
        else // if called with no time remaining (e.g. time ran out) 
        {   
            timeLeft += Time.deltaTime; // required to keep end time at 0, also so score remains stable
            endText.text = "Out of time\nPlease play again"; // loss message
            StartCoroutine(PauseThenRestart(5)); // restarts level after a delay
        }
        
    }

    // udates the timer text
    void SetTimer()
    {
        timer.text = "Time: " + timeLeft.ToString("#.#"); // displays the current time remaining
    }

    // updates the scoreboard text
    void SetScoreText() 
    {
        if (score < 0) 
        {
            score = 0; // ensures that the score can never go negative
        }

        scoreText.text = "Score: " + score.ToString(); // displays the current score
    }

    // shows the score differential
    void SetScoreDiff(string a)
    {
        scoreDiff.text = a; // sets the score differnce text
    }

    // removes the score differential
    void ResetScoreDiff() 
    {
        if (scoreDiffTime <= 0) // reset score differential for pickups
        {
            scoreDiff.text = ""; // text disappears
        }
    }

    // reduces player speed
    void SetMineDebuff() 
    {
        speedMod = defaultSpeedMod*mineStrength; // variable speed is the product of the default speed and the mine debuff
    }

    // returns player speed to normal
    void ResetMineDebuff() 
    {
        if (mineCooldown <= 0) // cooldown is over
        {
            speedMod = defaultSpeedMod; // speed returns to default speed
            mineCooldownText.text = ""; // text disappears
        }
        else 
        {
            mineCooldownText.text = mineCooldown.ToString("#.##") + "s"; // displaying seconds of debuff left
        }
    }

    // coroutine to pause game momentarily and then restart it
    IEnumerator PauseThenRestart(float pause)
    {
        yield return new WaitForSeconds(pause); // how long to wait for
        Application.LoadLevel(0); // reloads the level
    }
}