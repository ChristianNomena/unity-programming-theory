using UnityEngine;

// INHERITANCE
public class Enemy : Animal
{
    [SerializeField] protected int deathScore = 2;
    [SerializeField] protected int damage = 50;

    // POLYMORPHISM
    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            TakeDamage(GameManager.Instance.Damage);
        }
        else if (collision.gameObject.CompareTag("Objective"))
        {
            GameManager.Instance.HurtObjective(damage);
            Die();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
        }
    }

    // POLYMORPHISM
    protected override void Die()
    {
        GameManager.Instance.Score = GameManager.Instance.Score + deathScore;
        base.Die();
    }
}
