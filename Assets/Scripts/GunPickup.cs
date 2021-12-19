using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public Gun theGun;

    public float waitToBeCollected = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && waitToBeCollected <= 0)
        {
            bool hasGun = false;

            foreach(Gun gunToCheck in PlayerController.instance.avilableGuns)
            {
                if(theGun.weaponName == gunToCheck.weaponName)
                {
                    hasGun = true;
                }
            }

            if (!hasGun)
            {
                Gun gunClone = Instantiate(theGun);

                gunClone.transform.parent = PlayerController.instance.gunArm;
                gunClone.transform.position = PlayerController.instance.gunArm.position;
                gunClone.transform.localRotation = Quaternion.identity;
                gunClone.transform.localScale = Vector3.one;

                PlayerController.instance.avilableGuns.Add(gunClone);
                PlayerController.instance.currentGun = PlayerController.instance.avilableGuns.Count - 1;
                PlayerController.instance.SwitchGun();
            }

            Destroy(gameObject);

            AudioManager.instance.PlaySFX(7);
        }
    }
}
