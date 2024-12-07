using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallsManager : MonoBehaviour
{
    #region Singleton
    
    private static BallsManager instance;
    public static BallsManager Instance => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    #endregion

    [SerializeField]
    private Ball ballPreefab;

    public int maxBallCount = 20;
    private Ball initialBall;
    private Rigidbody2D initialBallRb;

    public float initialBallSpeed = 500;
    public List<Ball> Balls { get; set; }
    
    public void Start()
    {
        InitBall();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameStarted)
        {
            Vector3 ballPosition = new Vector3(
                Paddle.Instance.gameObject.transform.position.x, 
                Paddle.Instance.gameObject.transform.position.y + .50f, 0);
            initialBall.transform.position = ballPosition;

            if (Input.GetMouseButtonDown(0))
            {
                initialBallRb.bodyType = RigidbodyType2D.Dynamic;
                initialBallRb.AddForce(new Vector2(0, initialBallSpeed));
                GameManager.Instance.IsGameStarted = true;
            }
        }
    }

    private void InitBall()
    {
        Vector3 startingPostion = new Vector3(
            Paddle.Instance.gameObject.transform.position.x, 
            Paddle.Instance.gameObject.transform.position.y + .50f, 0);
        initialBall = Instantiate(ballPreefab, startingPostion, Quaternion.identity);
        initialBallRb = initialBall.GetComponent<Rigidbody2D>();

        this.Balls = new List<Ball>
        {
            initialBall
        };
    }

    public void ResetBalls()
    {
        foreach (var ball in Balls.ToArray())
        {
            Destroy(ball.gameObject);
        }
        InitBall();
    }

    public void SpawnBalls(Vector3 pos, int count, bool isLightningBall)
    {
        for (int i = 0; i < count; i++)
        {
            if (Balls.Count < maxBallCount)
            {
                Ball ball = Instantiate(ballPreefab, pos, Quaternion.identity) as Ball;
                if (isLightningBall)
                {
                    ball.StartLightningBall();
                }
                Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();
                ballRb.bodyType = RigidbodyType2D.Dynamic;
                ballRb.AddForce(new Vector2(0, initialBallSpeed));
                this.Balls.Add(ball);
            }
        }
    }
}
