using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public float moveSpeed;

    [Header("Chase Player")]
    // Aggro on player
    public bool shouldChasePlayer;
    public float agroRadius;
    private Vector3 moveDirection;

    [Header("Run away from Player")]
    // Run away from player
    public bool shouldRunAway;
    public float runAwayRange;

    [Header("Wandering")]
    // Wandering
    public bool shouldWander;
    public float wanderLength, pauseLength;
    private float wanderCounter, pauseCounter;
    private Vector3 wanderDirection;

    [Header("Patrolling")]
    // Patroling
    public bool shouldPatrol;
    public Transform[] patrolPoints;
    private int currentPatrolPoint;

    [Header("Shooting")]
    // Shooting
    public bool shouldShoot;

    public GameObject bullet;
    public Transform firePoint;
    public float fireRate;
    private float fireCounter;

    public float shootRange;

    [Header("Variables")]
    // Animations
    public Animator anim;

    // Health
    public int health = 150;

    // Getting hit, dying
    public GameObject[] deathSplatters;
    public GameObject hitEffect;

    // Drop items
    public bool shouldDropItems;
    public GameObject[] itemsToDrop;
    public float itemDropPercentage;

    public SpriteRenderer theBody;

    // Start is called before the first frame update
    void Start()
    {
        if (shouldWander)
        {
            pauseCounter = Random.Range(pauseLength * 0.75f, pauseLength * 1.25f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (theBody.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            moveDirection = Vector3.zero;

            // If player in aggro range, chase
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= agroRadius && shouldChasePlayer)
            {
                if(Vector3.Distance(transform.position, PlayerController.instance.transform.position) < 0.3f)
                {
                    moveDirection = Vector3.zero;
                } else
                {
                    moveDirection = PlayerController.instance.transform.position - transform.position;
                }
            } else
            {
                if (shouldWander)
                {
                    if(wanderCounter > 0)
                    {
                        wanderCounter -= Time.deltaTime;

                        moveDirection = wanderDirection;

                        if(wanderCounter <= 0)
                        {
                            pauseCounter = Random.Range(pauseLength * 0.75f, pauseLength * 1.25f);
                        }
                    }
                    
                    if(pauseCounter > 0)
                    {
                        pauseCounter -= Time.deltaTime;

                        if(pauseCounter <= 0)
                        {
                            wanderCounter = Random.Range(wanderLength * 0.75f, wanderLength * 1.25f);

                            wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                        }
                    }
                }

                if (shouldPatrol)
                {
                    moveDirection = patrolPoints[currentPatrolPoint].position - transform.position;

                    if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < 0.2f)
                    {
                        currentPatrolPoint++;
                        if(currentPatrolPoint >= patrolPoints.Length)
                        {
                            currentPatrolPoint = 0;
                        }
                    }
                }
            }

            // If player in range, run away
            if(shouldRunAway && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < runAwayRange)
            {
                moveDirection = transform.position - PlayerController.instance.transform.position;
            }

            moveDirection.Normalize();

            theRB.velocity = moveDirection * moveSpeed;

            // Shooting
            if (shouldShoot && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < shootRange)
            {
                fireCounter -= Time.deltaTime;

                if (fireCounter <= 0)
                {
                    fireCounter = fireRate;

                    Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);

                    AudioManager.instance.PlaySFX(13);
                }
            }
        } else
        {
            theRB.velocity = Vector2.zero;
        }

        // Animations
        if (moveDirection != Vector3.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        
    }

    public void DamageEnemey(int damage)
    {
        health -= damage;

        AudioManager.instance.PlaySFX(2);

        Instantiate(hitEffect, transform.position, transform.rotation);

        if(health <= 0)
        {
            Destroy(gameObject);

            AudioManager.instance.PlaySFX(1);

            int selectedSplatter = Random.Range(0, deathSplatters.Length);

            int rotation = Random.Range(0, 4);

            Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, rotation * 90f));

            if (shouldDropItems)
            {
                float dropChance = Random.Range(0f, 100f);

                if (dropChance < itemDropPercentage)
                {
                    int randomItem = Random.Range(0, itemsToDrop.Length);

                    Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
                }
            }
        }
    }
}
