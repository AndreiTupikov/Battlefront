using UnityEngine.UI;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private Image healthLevel;
    private Transform owner;
    private float maxHealth;

    private void Update()
    {
        if (owner != null)
        {
            transform.position = new Vector2 (owner.position.x, owner.position.y + 0.6f);
        }
    }

    public void SetOwner(Transform owner, float maxHealth)
    {
        this.owner = owner;
        this.maxHealth = maxHealth;
    }

    public void ChangeHealthLevel(float currentHealth)
    {
        float health = currentHealth / maxHealth;
        healthLevel.fillAmount = health;
        if (health > 0.66) healthLevel.color = Color.green;
        else if (health > 0.33) healthLevel.color = Color.yellow;
        else healthLevel.color = Color.red;
    }
}
