using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    private protected GameManager gameManager;
    private protected TransportController transport;

    private protected virtual void Start()
    {
        transport = GetComponent<TransportController>();
    }

    public virtual void Attack(Transform[] targets, GameManager manager)
    {
        gameManager = manager;
    }

    public virtual void Defeat()
    {
        gameManager.Defeat(transform);
    }
}
