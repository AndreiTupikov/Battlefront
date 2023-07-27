using UnityEngine;

public class HelicopterController : TransportController
{
    private void Update()
    {
        TurnToTarget(transform);
        if (target != null)
        {
            if (moveDirection != Vector2.zero) Move();
            else if (gunReady) Shooting(transform);
        }
    }

    private void Move()
    {
        transform.Translate(Time.deltaTime * movingSpeed * moveDirection, Space.World);
    }

    private protected override void Defeat()
    {
        base.Defeat();
        gameObject.GetComponent<Animator>().enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Ground Vehicles");
    }
}
