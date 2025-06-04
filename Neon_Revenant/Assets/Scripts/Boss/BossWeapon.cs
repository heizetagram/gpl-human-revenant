using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    public int attackDamage = 20;
    public int enragedAttackDamage = 40;

    public Vector3 attackOffset;
    public float attackRange = 3f;
    public LayerMask attackMask;

    public void Attack()
    {
        if (GetComponent<BossHealth>().isEnraged)
            attackDamage = enragedAttackDamage;
        Vector3 pos = transform.position;
        Vector3 direction = (transform.localScale.x > 0) ? Vector3.right : Vector3.left;
        pos += direction * attackOffset.x;
        pos += Vector3.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }
    }
    
}