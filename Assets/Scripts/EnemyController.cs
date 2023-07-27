using System.Collections;
using UnityEngine;

public class EnemyController : MovementController
{
    [SerializeField] private float shootingTime;
    [SerializeField] private float movingDistance;

    private protected override void Start()
    {
        base.Start();
        shootingTime += Random.Range(-0.5f, 0.5f);
        movingDistance += Random.Range(-0.5f, 0.5f);
    }

    public override void Attack(Transform[] targets, GameManager manager)
    {
        base.Attack(targets, manager);
        transport.target = targets[0];
        StartCoroutine(Move());
    }

    private IEnumerator Shooting()
    {
        yield return new WaitForSeconds(shootingTime);
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        transport.moveDirection = direction;
        yield return new WaitForSeconds(movingDistance);
        transport.moveDirection = Vector2.zero;
        StartCoroutine(Shooting());
    }
}
