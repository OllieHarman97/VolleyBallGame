using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    // Start is called before the first frame update

    //Maybe I can later create a system where you can have more than two players. However, I don't know when we'll ever need to do this...
    private GameObject playerOne;
    private GameObject playerTwo;
    private GameObject ball;
    
    public MainMenu MM;
    public Text P1Score;
    public Text P2Score;
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public GameObject ballPrefab;
    public int playerOneScore;
    public int playerTwoScore;

    private UnityAction resetListener;

    void OnEnable(){
        EventManager.StartListening("reset",resetListener);
    }
    void OnDisable(){
        EventManager.StopListening("reset",resetListener);
    }

    void Awake(){
        resetListener = new UnityAction(updateScore);
    }
    void Start()
    {
        playerOne = Instantiate(player1Prefab, Vector3.zero, Quaternion.identity);
        playerOne.GetComponent<PlayerScript>().isPlayerOne = true;

        playerTwo = Instantiate(player2Prefab, Vector3.zero, Quaternion.identity);
        playerTwo.GetComponent<PlayerScript>().isPlayerOne = false;

        //
        //playerTwo.AddComponent(typeof(SillyAI));



        ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);

        ball.GetComponent<BallScript>().enableGravity();

        playerOneScore = 0;
        playerTwoScore = 0;

        EventManager.TriggerEvent("initCamera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void updateScore() //Where the actual score updating happens
    {
        //In bounds score handling
        //This is if red hit ball last and lands on red side
        if (ball.GetComponent<BallScript>().lastHitPlayer && (ball.GetComponent<BallScript>().lastHitFloor == 1)) 
        {
            playerTwoScore += 1;
        }
        //This is if red hit ball and lands on blue side
        else if(ball.GetComponent<BallScript>().lastHitPlayer && (ball.GetComponent<BallScript>().lastHitFloor == 2))
        {
            playerOneScore += 1;
        }
        //This is if blue hit ball and lands on red side
        else if (!ball.GetComponent<BallScript>().lastHitPlayer && (ball.GetComponent<BallScript>().lastHitFloor == 1))
        {
            playerTwoScore += 1;
        }
        //This is if blue hit ball and lands on blue side
        else if (!ball.GetComponent<BallScript>().lastHitPlayer && (ball.GetComponent<BallScript>().lastHitFloor == 2))
        {
            playerOneScore += 1;
        }

        //Out Of Bounds score handling
        //This is if red hits the ball and lands out of bounds
        if (ball.GetComponent<BallScript>().lastHitPlayer && ball.GetComponent<BallScript>().outOfBounds)
        {
            playerTwoScore += 1;
        }
        //This is if blue hits the ball and lands out of bounds
        else if (!ball.GetComponent<BallScript>().lastHitPlayer && ball.GetComponent<BallScript>().outOfBounds)
        {
            playerOneScore += 1;
        }

        //Bounces score handling
        //If blue hits the ball and has too many bounces
        else if(!ball.GetComponent<BallScript>().lastHitPlayer && ball.GetComponent<BallScript>().bounces > 2)
        {
            playerOneScore += 1;
        }
        //If red hits the ball and has too many bounces
        else if(ball.GetComponent<BallScript>().lastHitPlayer && ball.GetComponent<BallScript>().bounces > 2)
        {
            playerTwoScore += 1;
        }

        Debug.Log("playerOneScore " + playerOneScore.ToString());
        Debug.Log("playerTwoScore " + playerTwoScore.ToString());

        updateScoreboard();
    }

    void updateScoreboard()
    {
        P1Score.text = playerOneScore.ToString();
        P2Score.text = playerTwoScore.ToString();
    }
}
