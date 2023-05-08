using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using TMPro;

public class PlayerShooter : MonoBehaviour
{
    public GameObject laserPrefab;
    public int ammoCapacity = 3; // maximum ammo capacity
    public float reloadTime = 5f; // time it takes to reload ammo in seconds
    public float bulletLifetime = 5f; // lifetime of bullet in seconds
    private int currentAmmo; // current ammo count
    public int CurrentAmmo {get {return currentAmmo;}}
    private bool isReloading = false; // flag for reloading
    private bool laserActive;
    [SerializeField] private GameObject laser;
    

    private void Start()
    {
        currentAmmo = ammoCapacity;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isReloading && currentAmmo > 0 && GameManager.instance.GetGameStatus())
        {
            Shoot();
            currentAmmo--;
            if (currentAmmo == 0)
            {
                StartCoroutine(Reload());
            }
        }

        if (laserActive)
        {
            laser.SetActive(true);
        } else laser.SetActive(false);
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        Destroy(bullet, bulletLifetime);
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = ammoCapacity;
        isReloading = false;
    }

    public void DeactivateLaser()
    {
        laserActive = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Destroy(other);
            laserActive = true;
            Invoke("DeactivateLaser", 5f);
        }
    }
}
