using System.Collections;
using UnityEngine;

public class TransportController : MonoBehaviour
{
    public Vector2 moveDirection;
    public Transform target;
    [SerializeField] private protected GameObject bullet;
    [SerializeField] private protected GameObject defeat;
    [SerializeField] private protected float health;
    [SerializeField] private protected float movingSpeed;
    [SerializeField] private protected float reloadTime;
    [SerializeField] private protected int bulletsPerShot;
    [SerializeField] private protected int damage;
    private protected bool gunReady = true;
    private HealthController healthBar;

    public void SetHealthBar(HealthController healthBar)
    {
        this.healthBar = healthBar;
        healthBar.SetOwner(transform, health);
    }

    private protected void TurnToTarget(Transform body)
    {
        if (target != null)
        {
            float angle = Mathf.Atan2(target.position.y - body.position.y, target.position.x - body.position.x) * Mathf.Rad2Deg - 90;
            body.rotation = Quaternion.Slerp(body.rotation, Quaternion.Euler(0, 0, angle), 0.1f);
        }
    }
    private protected void Shooting(Transform body)
    {
        gunReady = false;
        for (int i = 0; i < bulletsPerShot; i++)
        {
            StartCoroutine(Shot(i * 0.1f, body));
        }
        StartCoroutine(Reload());
    }

    private IEnumerator Shot(float delay, Transform body)
    {
        yield return new WaitForSeconds(delay);
        GameObject b = Instantiate(bullet, body.position, body.rotation);
        b.GetComponent<BulletController>().damage = damage;
        Physics2D.IgnoreCollision(b.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        gunReady = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            if (health > 0)
            {
                health -= collision.transform.GetComponent<BulletController>().damage;
                healthBar.ChangeHealthLevel(health);
                if (health <= 0)
                {
                    Defeat();
                }
            }
            collision.transform.GetComponent<BulletController>().Remove();
        }
    }

    private protected virtual void Defeat()
    {
        target = null;
        GetComponent<MovementController>().Defeat();
        Destroy(healthBar.gameObject);
        GameObject explosion = Instantiate(defeat, transform.position, transform.rotation, transform);
        if (DataHolder.soundsOn) explosion.GetComponent<AudioSource>().Play();
    }
}
