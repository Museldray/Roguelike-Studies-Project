 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    // Movement
    public float moveSpeed;
    private Vector2 moveInput;
    private float activeMoveSpeed;
    public float dashSpeed = 8f, dashLength = 0.5f, dashCooldown = 1f, dashInvincibility = 0.5f;
    private float dashCoolCounter;
    [HideInInspector]
    public float dashCounter;

    public Rigidbody2D theRB;

    public Transform gunArm;

    public Animator anim;

    public List<Gun> avilableGuns = new List<Gun>();
    [HideInInspector]
    public int currentGun;

    // Firerate
    //public float timeBetweenShots;
    //private float shotCounter;

    // Bullets
    //public GameObject bulletToFire;
    //public Transform firePoint;

    public SpriteRenderer bodySR;

    [HideInInspector]
    public bool canMove = true;

    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        activeMoveSpeed = moveSpeed;

        UIController.instance.currentGun.sprite = avilableGuns[currentGun].gunUI;
        UIController.instance.gunText.text = avilableGuns[currentGun].weaponName;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !LevelManager.instance.isPause)
        {
            // Move inputs
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize();

            //transform.position += new Vector3(moveInput.x * Time.deltaTime * moveSpeed, moveInput.y * Time.deltaTime * moveSpeed, 0f);

            theRB.velocity = moveInput * activeMoveSpeed;

            Vector3 mousePosition = Input.mousePosition;
            Vector3 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition);

            // Rotate player if aiming backwards
            if(mousePosition.x < screenPoint.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                gunArm.localScale = new Vector3(-1f, -1f, 1f);
            } else
            {
                transform.localScale = Vector3.one;
                gunArm.localScale = Vector3.one;
            }

            // Rotate gun arm
            Vector3 offset = new Vector3(mousePosition.x - screenPoint.x, mousePosition.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            gunArm.rotation = Quaternion.Euler(0, 0, angle);

            // Fire guns (0 - LMB, 1 - RMB)
            // Fire basic bullets with left mouse button
            /*if (Input.GetMouseButtonDown(0))
            {
                Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                shotCounter = timeBetweenShots;

                AudioManager.instance.PlaySFX(12);
            }
        
                // Hold left mouse to auto fire
            if (Input.GetMouseButton(0))
            {
                shotCounter -= Time.deltaTime;

                if(shotCounter <= 0)
                {
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                    AudioManager.instance.PlaySFX(12);

                    shotCounter = timeBetweenShots;
                }
            }*/

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if(avilableGuns.Count > 0)
                {
                    currentGun++;
                    if(currentGun >= avilableGuns.Count)
                    {
                        currentGun = 0;
                    }

                    SwitchGun();

                } else
                {
                    Debug.LogError("Player has no other guns!");
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(dashCoolCounter <= 0 && dashCounter <= 0)
                {
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;

                    anim.SetTrigger("dash");

                    PlayerHealthController.instance.MakeInvicible(dashInvincibility);

                    AudioManager.instance.PlaySFX(8);

                }
            }

            if(dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if(dashCounter <= 0)
                {
                    activeMoveSpeed = moveSpeed;
                    dashCoolCounter = dashCooldown;
                }
            }

            if(dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;
            }

            // Animations
            if(moveInput != Vector2.zero)
            {
                anim.SetBool("isMoving", true);
            } else
            {
                anim.SetBool("isMoving", false);
            }
        } else
        {
            theRB.velocity = Vector2.zero;
        }
    }

    public void SwitchGun()
    {
        foreach(Gun gun in avilableGuns)
        {
            gun.gameObject.SetActive(false);
        }

        avilableGuns[currentGun].gameObject.SetActive(true);

        UIController.instance.currentGun.sprite = avilableGuns[currentGun].gunUI;
        UIController.instance.gunText.text = avilableGuns[currentGun].weaponName;
    }
}
