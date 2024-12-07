using UnityEngine;

public class InvertedControls : Collectable
{
    public float duration = 10;
    protected override void ApplyEffect()
    {
        if (Paddle.Instance != null && !Paddle.Instance.inverted)
        {
            Paddle.Instance.InvertControls(duration);
        }
    }
}
