using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 20f;
    public float range = 100f;
    public AudioSource silahSesi;
    public AudioSource mermiDegistirme;
    public ParticleSystem silahAtesiPrefab;
    private ParticleSystem silahAtesiInstance;

    public int maxAmmo = 30;
    public int reserveAmmo = 120;
    private int currentAmmo;
    private int totalReserveAmmo;
    public float fireRate = 0.5f;
    private float nextTimeToFire = 0f;
    public TextMeshProUGUI ammoText;

    private void Start()
    {
        currentAmmo = maxAmmo;
        totalReserveAmmo = reserveAmmo;
        UpdateAmmoText();
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        if (silahSesi == null)
            silahSesi = GetComponent<AudioSource>();

        if (silahAtesiPrefab == null)
            silahAtesiPrefab = GetComponentInChildren<ParticleSystem>();

        if (mermiDegistirme == null)
            mermiDegistirme = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            mermiDegistirme.Play();
            Reload();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint = ray.GetPoint(range);

        if (Physics.Raycast(ray, out hit, range))
            targetPoint = hit.point;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        Vector3 direction = (targetPoint - firePoint.position).normalized;
        rb.AddForce(direction * bulletForce, ForceMode.VelocityChange);

        Destroy(bullet, 5f);

        PlayShootEffects();

        currentAmmo--;
        UpdateAmmoText();
    }

    private void PlayShootEffects()
    {
        if (silahSesi != null)
            silahSesi.Play();

        if (silahAtesiPrefab != null)
        {
            ParticleSystem tempMuzzle = Instantiate(silahAtesiPrefab, firePoint.position, firePoint.rotation);
            tempMuzzle.Play();
            Destroy(tempMuzzle.gameObject, tempMuzzle.main.duration);
        }
    }

    private void Reload()
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

    private void UpdateAmmoText()
    {
        ammoText.text = $"{currentAmmo}/{totalReserveAmmo}";
    }
}
