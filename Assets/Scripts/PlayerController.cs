using System.Collections;
using UnityEngine;

public class PlayerController : MovementController
{
    public Vector2 moveDirection;
    [SerializeField] HealthController healthBar;
    private Transform[] enemies;
    private bool targetIsRelevant;
    private bool activated;

    private protected override void Start()
    {
        base.Start();
        transport.SetHealthBar(healthBar);
    }

    private void Update()
    {
        if (activated)
        {
            transport.moveDirection = moveDirection;
            if (!targetIsRelevant && enemies != null) StartCoroutine(GetNearestTarget());
        }
    }

    public override void Attack(Transform[] targets, GameManager manager)
    {
        base.Attack(enemies, manager);
        this.enemies = targets;
        activated = true;
    }

    public override void Defeat()
    {
        base.Defeat();
        enemies = null;
    }

    public void DeleteDefetedTarget(Transform target)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == target)
            {
                enemies[i] = null;
                break;
            }
        }
        transport.target = null;
    }

    private IEnumerator GetNearestTarget()
    {
        targetIsRelevant = true;
        Transform target = transport.target;
        float currentDistance = target == null ? 100 : Vector2.Distance(transform.position, target.position);
        foreach (Transform enemy in enemies)
        {
            if (enemy == null || enemy == target) continue;
            float distance = Vector2.Distance(transform.position, enemy.position);
            if (distance < currentDistance)
            {
                target = enemy;
                currentDistance = distance;
            }
        }
        transport.target = target;
        yield return new WaitForSeconds(0.5f);
        targetIsRelevant = false;
    }
}
