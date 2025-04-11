using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    private Transform target;

    public void SetTarget(GameObject targetObj)
    {
        target = targetObj.transform;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // 타겟과 매우 가까워지면 삭제 (충돌 처리 전 임시)
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1); // 기본 데미지 1
            }
            Destroy(gameObject);
        }
    }
}
