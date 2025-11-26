using UnityEngine;

// INHERITANCE
public class Friend : Animal
{
    [SerializeField] protected int healing = 50;

    // POLYMORPHISM
    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            TakeDamage(GameManager.Instance.Damage);
        }
        else if (collision.gameObject.CompareTag("Objective"))
        {
            GameManager.Instance.HealObjective(healing);
            Die();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Die();
        }
    }

}
