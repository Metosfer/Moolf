using UnityEngine;
using TMPro; // TextMeshPro k�t�phanesini ekleyin

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;
    public bool isDead = false;
    public float damageCooldown = 3f; // Yeni: Hasar alma bekleme s�resi
    private float lastDamageTime = 0f; // Yeni: Son hasar al�nan zaman� takip etmek i�in

    public TextMeshProUGUI healthText; // Yeni: TextMeshPro referans�

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText(); // Ba�lang��ta can� g�ncelle
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isDead && Time.time >= lastDamageTime + damageCooldown)
        {
            TakeDamage(1);
            lastDamageTime = Time.time; // Son hasar al�nan zaman� g�ncelle
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player health: " + currentHealth);
        UpdateHealthText(); // Can de�i�ikli�inde UI'� g�ncelle

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("�ld�n");
        // Burada �l�m ile ilgili ek i�lemler yapabilirsiniz (�rne�in, oyunu yeniden ba�latma)
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
