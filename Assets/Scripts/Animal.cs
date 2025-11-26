using TMPro;
using UnityEngine;

// POLYMORPHISM
public abstract class Animal : MonoBehaviour
{
    [SerializeField] protected GameObject particleEffect;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected int health = 20;

    protected GameObject target;

    void Start()
    {
        AssignTarget();
    }

    void FixedUpdate()
    {
        ChaseTarget();
    }

    protected virtual void AssignTarget()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Objective");
        }
    }

    protected virtual void Die()
    {
        Instantiate(particleEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    // POLYMORPHISM
    protected abstract void OnCollisionEnter(Collision collision);

    // ABSTRACTION
    protected virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    // ABSTRACTION
    private void ChaseTarget()
    {
        if (target == null) return;

        Vector3 targetPosition = target.transform.position;
        targetPosition.y = transform.position.y;

        // Move to the target
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

        LookAtTarget(targetPosition);
    }

    private void LookAtTarget(Vector3 targetPosition)
    {
        Vector3 lookDir = targetPosition - transform.position;
        lookDir.y = 0f;
        if (lookDir.sqrMagnitude > 0.000001f)
        {
            transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }
}
