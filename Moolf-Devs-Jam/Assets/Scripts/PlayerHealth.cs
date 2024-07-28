using UnityEngine;
using TMPro; // TextMeshPro kütüphanesini ekleyin

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;
    public bool isDead = false;
    public float damageCooldown = 3f; // Yeni: Hasar alma bekleme süresi
    private float lastDamageTime = 0f; // Yeni: Son hasar alýnan zamaný takip etmek için

    public TextMeshProUGUI healthText; // Yeni: TextMeshPro referansý

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText(); // Baþlangýçta caný güncelle
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isDead && Time.time >= lastDamageTime + damageCooldown)
        {
            TakeDamage(1);
            lastDamageTime = Time.time; // Son hasar alýnan zamaný güncelle
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player health: " + currentHealth);
        UpdateHealthText(); // Can deðiþikliðinde UI'ý güncelle

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Öldün");
        // Burada ölüm ile ilgili ek iþlemler yapabilirsiniz (örneðin, oyunu yeniden baþlatma)
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth;
        }
        else
        {
            Debug.LogError("Health Text is not assigned in the inspector!");
        }
    }
}
