using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ball : MonoBehaviour
{
    public static event Action<Ball> OnBallDeath;
    public static event Action<Ball> OnLightningBallEnable;
    public static event Action<Ball> OnLightningBallDisable;
    
    
    public bool isLightningBall = false;
    public SpriteRenderer ballRenderer;
    public SpriteRenderer lightningBallSpriteRenderer;
    public SpriteRenderer lightningBallEffectRenderer;
    public Animator animator;
    private float lightingBallDuration = 10;
    public void Die()
    {
        OnBallDeath?.Invoke(this);
        Destroy(gameObject, 1);
    }

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }
    

    public void StartLightningBall()
    {
        if (!this.isLightningBall)
        {
            this.isLightningBall = true;
            ballRenderer.enabled = false;
            animator.enabled = true;
            lightningBallEffectRenderer.enabled = true;
            lightningBallSpriteRenderer.enabled = true;
            StartCoroutine(StopLightningEfeect(lightingBallDuration));
            
            OnLightningBallEnable?.Invoke(this);
        }
    }

    private IEnumerator StopLightningEfeect(float duration)
    {
        yield return new WaitForSeconds(duration);
        StopLightningball();
    }

    private void StopLightningball()
    {
        if (this.isLightningBall)
        {
            this.isLightningBall = false;
            ballRenderer.enabled = true;
            animator.enabled = false;
            lightningBallEffectRenderer.enabled = false;
            lightningBallSpriteRenderer.enabled = false;
            
            OnLightningBallDisable?.Invoke(this);
        }
    }
}
