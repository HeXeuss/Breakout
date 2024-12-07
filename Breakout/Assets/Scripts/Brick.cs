using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private SpriteRenderer sr;
    public int hitPoints = 1;
    public ParticleSystem destroyEffect;
    public static event Action<Brick> OnDestruction;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        this.sr = this.GetComponent<SpriteRenderer>();
        this.boxCollider = this.GetComponent<BoxCollider2D>();
        Ball.OnLightningBallEnable += OnLightningBallEnable;
        Ball.OnLightningBallDisable += OnLightningBallDisable;
    }
    
    private void OnLightningBallDisable(Ball ball)
    {
        if (this != null)
        {
            this.boxCollider.isTrigger = false;
        }
    }
    private void OnLightningBallEnable(Ball ball)
    {
        if (this != null)
        {
            this.boxCollider.isTrigger = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Ball ball = other.gameObject.GetComponent<Ball>();
            ApplyCollisionLogic(ball);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Ball ball = other.gameObject.GetComponent<Ball>();
            ApplyCollisionLogic(ball);
        }
    }

    private void ApplyCollisionLogic(Ball ball)
    {
        this.hitPoints--;

        if (hitPoints <= 0)
        {
            BricksManager.Instance.remainingBricks.Remove(this);
            OnDestruction?.Invoke(this);
            OnBrickDestruction();
            DestroyAnimation();
            Destroy(this.gameObject);
        }
        else
        {
            this.sr.sprite = BricksManager.Instance.brickSprites[this.hitPoints - 1];
        }
    }

    private void OnBrickDestruction()
    {
        float buffSpawnChance = UnityEngine.Random.Range(0f, 100f);
        float debuffSpawnChance = UnityEngine.Random.Range(0f, 100f);
        bool alreadySpawned = false;

        if (buffSpawnChance <= CollectablesManager.Instance.buffSpawnChance)
        {
            alreadySpawned = true;
            Collectable newBuff = this.SpawnCollectable(true);
        }

        if (debuffSpawnChance <= CollectablesManager.Instance.debuffSpawnChance && !alreadySpawned)
        {
            Collectable newBuff = this.SpawnCollectable(false);
        }
        
    }

    private Collectable SpawnCollectable(bool isBuff)
    {
        List<Collectable> collection;
        
        if (isBuff)
        {
            collection = CollectablesManager.Instance.AvailableBuffs;
        }
        else
        {
            collection = CollectablesManager.Instance.AvailableDebuffs;
        }
        
        int buffIndex = UnityEngine.Random.Range(0,collection.Count);
        Collectable collectable = collection[buffIndex];
        Collectable newCollectable = Instantiate(collectable, this.transform.position, Quaternion.identity) as Collectable;
        return newCollectable;
    }

    private void DestroyAnimation()
    {
        Vector3 brickPos = gameObject.transform.position;
        Vector3 spawnpos = new Vector3(brickPos.x, brickPos.y, brickPos.z - 0.2f);
        GameObject effect = Instantiate(destroyEffect.gameObject, spawnpos, Quaternion.identity);
        
        ParticleSystem.MainModule mn = effect.GetComponent<ParticleSystem>().main;
        Destroy(effect, destroyEffect.main.startLifetime.constant);
    }

    public void Init(Transform container, Sprite sprite, Color color, int brickType)
    {
        this.transform.SetParent(container);
        this.sr.sprite = sprite;
        this.sr.color = color;
        this.hitPoints = brickType;
    }

    private void OnDisable()
    {
        Ball.OnLightningBallEnable -= OnLightningBallEnable;
        Ball.OnLightningBallDisable -= OnLightningBallDisable;
    }
}
