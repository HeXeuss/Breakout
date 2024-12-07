using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    #region Singleton
    
    private static Paddle instance;
    public static Paddle Instance => instance;

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
    
    private Camera paddleCamera;
    private float paddleInitialY;
    private SpriteRenderer spriteRenderer;
    private float screenLeft;
    private float screenRight;
    public bool PaddleIsTransforming { get;  set; }
    public float paddleWidth;
    private BoxCollider2D boxColl;
    private float paddleHeight;
    public bool inverted = false;
    
    void Start()
    {
        
        paddleCamera = FindObjectsByType<Camera>(FindObjectsSortMode.None)[0];
        paddleInitialY = this.transform.position.y;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Calculate the world bounds of the screen
        screenLeft = paddleCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + 1.1f;
        screenRight = paddleCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - 1.1f;
        
        boxColl = GetComponent<BoxCollider2D>();
        paddleWidth = spriteRenderer.size.x;
        paddleHeight = spriteRenderer.size.y;

    }
    
   
    void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            Paddlemovement();
        }
    }
   
    private void Paddlemovement()
    {
        
        // Adjust clamps to account for half the paddle's width
        float halfPaddleWidth = spriteRenderer.bounds.extents.x;
        float leftClamp = screenLeft + halfPaddleWidth;
        float rightClamp = screenRight - halfPaddleWidth;
        
        // Get mouse position in world coordinates
        
        Vector3 mousePositionWorld = paddleCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 0));
        if (inverted)
        {
            mousePositionWorld *= -1;
        }
        // Clamp X position to adjusted boundaries
        float clampedX = Mathf.Clamp(mousePositionWorld.x, leftClamp, rightClamp);

        // Update paddle position
        this.transform.position = new Vector3(clampedX, paddleInitialY, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = collision.contacts[0].point;
            Vector3 paddleCenter = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
            
            ballRb.linearVelocity = Vector2.zero;
            
            float difference = paddleCenter.x - hitPoint.x;
    
            if (hitPoint.x < paddleCenter.x)
            {
                ballRb.AddForce(new Vector2(-(Mathf.Abs(difference * 200)), BallsManager.Instance.initialBallSpeed));
            }
            else
            {
                ballRb.AddForce(new Vector2((Mathf.Abs(difference * 200)), BallsManager.Instance.initialBallSpeed));
            }
        }
    }

    public void InvertControls(float duration)
    {
        inverted = true;
        StartCoroutine(StopInvertedControls(duration));

    }
    
    private IEnumerator StopInvertedControls(float duration)
    {
        yield return new WaitForSeconds(duration);
        inverted = false;
    }

    public void StartWidthAnimation(float width, float duration)
    {
        StartCoroutine(AnimatePaddlePaddleWidth(width,duration));
    }

    public IEnumerator AnimatePaddlePaddleWidth(float width, float duration)
    {
        this.PaddleIsTransforming = true;

        // Perform the width change animation
        if (width > this.spriteRenderer.size.x)
        {
            float currentWidth = this.spriteRenderer.size.x;
            while (currentWidth < width)
            {
                currentWidth += Time.deltaTime * 2;
                this.spriteRenderer.size = new Vector2(currentWidth, paddleHeight);
                boxColl.size = new Vector2(currentWidth, paddleHeight);
                yield return null;
            }
        }
        else
        {
            float currentWidth = this.spriteRenderer.size.x;
            while (this.spriteRenderer.size.x > width)
            {
                currentWidth -= Time.deltaTime * 2;
                this.spriteRenderer.size = new Vector2(currentWidth, paddleHeight);
                boxColl.size = new Vector2(currentWidth, paddleHeight);
                yield return null;
            }
        }

        // After width animation finishes, start reset width animation with delay
        StartCoroutine(ResetPaddleWidth(duration));
    }

    private IEnumerator ResetPaddleWidth(float durationl)
    {
        // Wait for the specified delay (e.g., 10 seconds)
        yield return new WaitForSeconds(durationl);
        if (paddleWidth > this.spriteRenderer.size.x)
        {
            float currentWidth = this.spriteRenderer.size.x;
            while (currentWidth < paddleWidth)
            {
                currentWidth += Time.deltaTime * 2;
                this.spriteRenderer.size = new Vector2(currentWidth, paddleHeight);
                boxColl.size = new Vector2(currentWidth, paddleHeight);
                yield return null;
            }
        }
        else
        {
            float currentWidth = this.spriteRenderer.size.x;
            while (this.spriteRenderer.size.x > paddleWidth)
            {
                currentWidth -= Time.deltaTime * 2;
                this.spriteRenderer.size = new Vector2(currentWidth, paddleHeight);
                boxColl.size = new Vector2(currentWidth, paddleHeight);
                yield return null;
            }
        }
        this.PaddleIsTransforming = false;
    }
}
