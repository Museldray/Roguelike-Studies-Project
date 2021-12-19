using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int itemCost;
    public Sprite gunShopSprite;

    // Showing gun in UI to player
    public string weaponName;
    public Sprite gunUI;

    // Firerate
    public float timeBetweenShots;
    private float shotCounter;

    // Bullets
    public GameObject bulletToFire;
    public Transform firePoint;
    public float spreadRange = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.canMove && !LevelManager.instance.isPause)
        {
            if(shotCounter > 0)
            {
                shotCounter -= Time.deltaTime;
            } else
            {

                // Fire guns (0 - LMB, 1 - RMB), hold LMP to auto fire
                // Fire basic bullets with left mouse button
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
                {
                    if(weaponName == "Shotgun")
                    {
                        int randomBulletsToShot = Random.Range(4, 8);

                        for(int i = 0; i < randomBulletsToShot; i++)
                        {
                            Instantiate(bulletToFire, firePoint.position, Quaternion.Euler(firePoint.eulerAngles.x, firePoint.eulerAngles.y, firePoint.eulerAngles.z + Random.Range(-spreadRange, spreadRange)));
                        }
                        shotCounter = timeBetweenShots;

                        AudioManager.instance.PlaySFX(12);
                    } 
                    else
                    {
                        Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                        shotCounter = timeBetweenShots;

                        AudioManager.instance.PlaySFX(12);
                    }
                }
            }
        }
    }
}
