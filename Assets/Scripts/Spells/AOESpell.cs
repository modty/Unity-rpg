using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public abstract class AOESpell : MonoBehaviour
{
    protected List<Enemy> enemies = new List<Enemy>();

    [SerializeField]
    protected ParticleSystem particleSystem;

    protected MainModule main;

    [SerializeField]
    private SpriteRenderer cloudRenderer;

    [SerializeField]
    private SpriteRenderer shadowRenderer;

    [SerializeField]
    private Color shadowColor;

    [SerializeField]
    private Color cloudColor;

    protected float duration;

    protected float damage;

    protected float elapsed;

    protected float tickElapsed;

    [SerializeField]
    protected Color outOfRangeColor;

    private void Awake()
    {
        main = particleSystem.main;
    }

    void Update()
    {
        elapsed += Time.deltaTime;

        if (elapsed >= duration)
        {
            Remove();
        }

        Execute();
    }

    public void InRange()
    {
        main.startColor = Color.white;
        shadowRenderer.color = shadowColor;
        cloudRenderer.color = cloudColor;
    }

    public void OutOfRange()
    {
        main.startColor = outOfRangeColor;
        shadowRenderer.color = outOfRangeColor;
        cloudRenderer.color = outOfRangeColor;
    }


    public virtual void Execute()
    { 
    
    }

    public void Initialize(float damage, float duration)
    {
        this.damage = damage;
        this.duration = duration;
        enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Enter(collision.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
           
            Exit(collision.GetComponent<Enemy>());
        }
    }

    public virtual void Enter(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public virtual void Exit(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    public virtual void Remove()
    {
        Destroy(gameObject);
    }
}
