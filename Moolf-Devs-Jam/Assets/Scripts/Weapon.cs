using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 20f;
    public float range = 100f;  // Raycast mesafesi

    public int maxAmmo = 30;
    public int reserveAmmo = 120;
    private int currentAmmo;
    private int totalReserveAmmo;

    public float fireRate = 0.5f;
    private float nextTimeToFire = 0f;

    public TextMeshProUGUI ammoText;  // TextMeshPro için uygun bileþen türü

    void Start()
    {
        currentAmmo = maxAmmo;
        totalReserveAmmo = reserveAmmo;
        UpdateAmmoText();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    void Shoot()
    {
        // Raycast ile hedef noktasýný bul
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 targetPoint = ray.origin + ray.direction * range;

        if (Physics.Raycast(ray, out hit, range))
        {
            targetPoint = hit.point;
        }

        // Mermi prefab'ýný ateþ konumundan instantiate ediyoruz
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Mermiyi hedef noktasýna doðru hareket ettiriyoruz
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        Vector3 direction = (targetPoint - firePoint.position).normalized;
        rb.AddForce(direction * bulletForce, ForceMode.VelocityChange);

        // Mermiyi 5 saniye sonra yok ediyoruz
        Destroy(bullet, 5f);

        currentAmmo--;
        UpdateAmmoText();
    }

    void Reload()
    {
        if (totalReserveAmmo <= 0 || currentAmmo == maxAmmo)
            return;

        int ammoNeeded = maxAmmo - currentAmmo;
        if (totalReserveAmmo >= ammoNeeded)
        {
            totalReserveAmmo -= ammoNeeded;
            currentAmmo = maxAmmo;
        }
        else
        {
            currentAmmo += totalReserveAmmo;
            totalReserveAmmo = 0;
        }
        UpdateAmmoText();
    }

    void UpdateAmmoText()
    {
        ammoText.text = $"{currentAmmo}/{totalReserveAmmo}";
    }
}
