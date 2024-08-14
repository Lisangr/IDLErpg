using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private Transform target;
    private float speed;
    private int damageMultiplier;
    private Vector3 initialPosition;

    public void Initialize(Transform target, float speed, int damageMultiplier, Vector3 initialPosition)
    {
        this.target = target;
        this.speed = speed;
        this.damageMultiplier = damageMultiplier;
        this.initialPosition = initialPosition;
    }

    void Update()
    {
        if (target != null)
        {
            // Move the bullet towards the target's position
            Vector3 targetPosition = target.position;
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Check if the bullet is close enough to the target to apply damage
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                ApplyDamage();
                Destroy(gameObject); // Destroy the bullet
            }
        }
        else
        {
            Destroy(gameObject); // Destroy the bullet if the target is null
        }
    }

    private void ApplyDamage()
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(10 * damageMultiplier); // Example base damage is 10
        }
    }
}
