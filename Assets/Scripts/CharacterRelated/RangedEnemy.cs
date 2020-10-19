using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField]
    private GameObject arrowPrefab;

    [SerializeField]
    private Transform[] exitPoints;

    private float fieldOfView = 120;

    private bool updateDirection = false;

    protected override void Update()
    {
        LookAtTarget();
        base.Update();
    }

    private void LateUpdate()
    {
        UpdateDirection();
    }

    public void Shoot(int exitIndex)
    {
        SpellScript s = Instantiate(arrowPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();

        s.Initialize(MyTarget.MyHitbox, damage, this);
    }

    private void UpdateDirection()
    {
        if (updateDirection)
        {
            Vector2 dir = Vector2.zero;

            if (MySpriteRenderer.sprite.name.Contains("up"))
            {
                dir = Vector2.up;
            }
            else if (MySpriteRenderer.sprite.name.Contains("down"))
            {
                dir = Vector2.down;
            }
            else if (MySpriteRenderer.sprite.name.Contains("left"))
            {
                dir = Vector2.left;

            }
            else if (MySpriteRenderer.sprite.name.Contains("right"))
            {
                dir = Vector2.right;
            }

            MyAnimator.SetFloat("x", dir.x);
            MyAnimator.SetFloat("y", dir.y);
            updateDirection = false;
        }

     
    }

    private void LookAtTarget()
    {
        if (MyTarget != null)
        {
            Vector2 directionToTarget = (MyTarget.transform.position - transform.position).normalized;

            Vector2 faceing = new Vector2(MyAnimator.GetFloat("x"), MyAnimator.GetFloat("y"));

            float angleToTarget = Vector2.Angle(faceing, directionToTarget);

            if (angleToTarget > fieldOfView /2 )
            {
                MyAnimator.SetFloat("x", directionToTarget.x);
                MyAnimator.SetFloat("y", directionToTarget.y);

                updateDirection = true;
            }
        }
    }

}
