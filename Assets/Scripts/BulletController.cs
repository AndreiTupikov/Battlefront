using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] internal float damage;
    private float spawnTime;

    private void Start()
    {
        spawnTime = Time.time;
        if (DataHolder.soundsOn) GetComponent<AudioSource>().Play();
        Destroy(gameObject, 5f);
    }

    public void Remove()
    {
        if (Time.time > spawnTime + 1.2f) Destroy(gameObject);
        else
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, spawnTime + 1.2f - Time.deltaTime);
        }
    }

    private void Update()
    {
        gameObject.transform.Translate(speed * Time.deltaTime * new Vector3(0, 1, 0));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Barrier")) Remove();
    }
}
