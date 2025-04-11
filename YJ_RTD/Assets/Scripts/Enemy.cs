using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int maxHealth = 1;
    private int currentHealth;
    private int currentIndex = 0;
    void Start()
    {
        currentHealth = maxHealth;
    }
    void Update()
    {
        if (PathManager.Instance == null || PathManager.Instance.pathPoints.Count == 0)
            return;

        Transform target = PathManager.Instance.pathPoints[currentIndex];
        Vector3 dir = target.position - transform.position;

        transform.position += dir.normalized * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            currentIndex++;
            if (currentIndex >= PathManager.Instance.pathPoints.Count)
            {
                ReachGoal();
            }
        }
    }

    void ReachGoal()
    {
        Debug.Log("목표 지점 도달!");
        Destroy(gameObject);
        // GameManager.Instance.TakeDamage(); 예정
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
