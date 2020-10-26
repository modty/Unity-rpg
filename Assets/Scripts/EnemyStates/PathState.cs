using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathState : IState
{
    private Vector3 destination;

    private Vector3 current;

    private Vector3 goal;

    private Transform transform;

    private Enemy parent;

    private Vector3 targetPos;

    public void Enter(Enemy parent)
    {
        this.parent = parent;

        this.transform = parent.transform.parent;

        targetPos = Player.Instance.CurrentTile.position;

        if (targetPos != parent.CurrentTile.position)
        {
            parent.Path = parent.Astar.Algorithm(parent.CurrentTile.position, targetPos);
        }
        if (parent.Path != null)
        {
            current = parent.Path.Pop();
            destination = parent.Path.Pop();
            this.goal = parent.CurrentTile.position;
        }
        else
        {
            parent.ChangeState(new EvadeState());
        }
       

   
    }

    public void Exit()
    {
        parent.Path = null;
    }

    public void Update()
    {
        if (parent.Path != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, parent.CurrentSpeed * Time.deltaTime);

            parent.ActivateLayer("WalkLayer");
           
            Vector3Int dest = parent.Astar.Tilemap.WorldToCell(destination);
            Vector3Int cur = parent.Astar.Tilemap.WorldToCell(current);

            float distance = Vector2.Distance(destination, transform.position);

            float totalDistance = Vector2.Distance(parent.Target.transform.parent.position, transform.position);

            if (cur.y > dest.y)
            {
                parent.Direction = Vector2.down;
            }
            else if (cur.y < dest.y)
            {
                parent.Direction = Vector2.up;
            }
            if (cur.y == dest.y)
            {
                if (cur.x > dest.x)
                {
                    parent.Direction = Vector2.left;
                }
                else if (cur.x < dest.x)
                {
                    parent.Direction = Vector2.right;
                }
            }
            if (totalDistance <= parent.AttackRange)
            {
                parent.ChangeState(new AttackState());
            }
            else if (Player.Instance.CurrentTile.position == parent.CurrentTile.position)
            {
                parent.ChangeState(new FollowState());
            }

            if (distance <= 0f)
            {
                if (parent.Path.Count > 0)
                {
                    current = destination;
                    destination = parent.Path.Pop();

                    if (targetPos != Player.Instance.CurrentTile.position)
                    {
                        parent.ChangeState(new PathState());
                    }
                }
                else
                {
                    parent.Path = null;
                    parent.ChangeState(new PathState());
                }
            }
        }
    }
}
