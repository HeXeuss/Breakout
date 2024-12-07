using System;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.gameObject.CompareTag("Paddle"))
      {
         this.ApplyEffect();
         Destroy(this.gameObject);
      }
      if (other.gameObject.CompareTag("DeathWall") || other.gameObject.CompareTag("Paddle"))
      {
         Destroy(this.gameObject);
      }
   }

   protected abstract void ApplyEffect();
}
