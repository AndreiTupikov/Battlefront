using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] internal float damage;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        gameObject.transform.Translate(speed * Time.deltaTime * new Vector3(0, 1, 0));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Barrier")) Destroy(gameObject);
    }
}
