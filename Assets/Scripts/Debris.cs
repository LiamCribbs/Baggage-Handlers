using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    public new Rigidbody2D rigidbody;
    public float hitGravityScale = 0.5f;

    public Pigeon.Animator animator;

    void OnCollisionEnter2D(Collision2D collision)
    {
        rigidbody.gravityScale = hitGravityScale;

        if (animator != null)
        {
            animator.Stop();

            if (animator.animations.Length > 0)
            {
                animator.Play(animator.animations[0]);
            }
        }
    }
}