using System;
using UnityEngine;

public class Shama:MonoBehaviour
{
    [SerializeField]
    private Animator selfAnimator;
    
    private static readonly int X = Animator.StringToHash("X");
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Spell1 = Animator.StringToHash("Spell1");
    private static readonly int Spell2 = Animator.StringToHash("Spell2");

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag.Equals("Player"))
        {
            selfAnimator.SetBool(Spell1,true);
        }
    }
}
