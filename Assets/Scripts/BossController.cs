using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public static BossController instance;

    public BossAction[] actions;
    private int currentAction;
    private float actionCounter;

    private float shotCounter;
    private Vector2 moveDirection;
    public Rigidbody2D theRB;
    public GameObject firePoints;

    public int currentHealth;

    public GameObject deathEffect, levelExit, rewardCharacterCage, hitEffect;

    public BossSequence[] sequences;
    public int currentSequence;

    private float startRotation;
    private float currentAngle;
    private float endRotation;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        startRotation = firePoints.transform.eulerAngles.z;
        endRotation = startRotation + 360f;
        currentAngle = startRotation;

        actions = sequences[currentSequence].actions;

        actionCounter = actions[currentAction].actionLength;

        UIController.instance.bossHealthBar.gameObject.SetActive(true);
        UIController.instance.bossHealthBar.maxValue = currentHealth;
        UIController.instance.bossHealthBar.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(actionCounter > 0)
        {
            actionCounter -= Time.deltaTime;

            // Movement
            moveDirection = Vector2.zero;

            if (actions[currentAction].shouldMove)
            {
                if (actions[currentAction].shouldChasePlayer)
                {
                    moveDirection = PlayerController.instance.transform.position - transform.position;
                    moveDirection.Normalize();
                }

                if (actions[currentAction].moveToPoint && Vector3.Distance(transform.position, actions[currentAction].pointToMoveTo.position) > 0.5f)
                {
                    moveDirection = actions[currentAction].pointToMoveTo.position - transform.position;
                    moveDirection.Normalize();
                }
            }

            theRB.velocity = moveDirection * actions[currentAction].moveSpeed;            

            // Shooting
            if (actions[currentAction].shouldShoot)
            {
                shotCounter -= Time.deltaTime;

                if (actions[currentAction].shouldRotate)
                {
                    currentAngle = currentAngle + actions[currentAction].rotationSpeed * Time.deltaTime;
                    currentAngle %= 360.0f;
                    firePoints.transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
                } else
                {
                    firePoints.transform.rotation = Quaternion.identity;
                }

                if (shotCounter <= 0)
                {
                    shotCounter = actions[currentAction].timeBetweenShots;

                    foreach (Transform firePoint in actions[currentAction].shotPoints)
                    {
                        Instantiate(actions[currentAction].bulletToShoot, firePoint.position, firePoint.rotation);
                    }
                }
            }

        } else
        {
            currentAction++;

            if(currentAction >= actions.Length)
            {
                currentAction = 0;
            }

            actionCounter = actions[currentAction].actionLength;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if(currentHealth <= 0)
        {
            gameObject.SetActive(false);

            Instantiate(deathEffect, transform.position, transform.rotation);

            if (Vector3.Distance(PlayerController.instance.transform.position, rewardCharacterCage.transform.position) < 2f)
            {
                rewardCharacterCage.transform.position += new Vector3(0f, 4f, 0f);
            }

            rewardCharacterCage.SetActive(true);

            if (Vector3.Distance(PlayerController.instance.transform.position, levelExit.transform.position) < 2f)
            {
                levelExit.transform.position += new Vector3(-4f, 0f, 0f);
            }

            levelExit.SetActive(true);

            UIController.instance.bossHealthBar.gameObject.SetActive(false);
        } else
        {
            if(currentHealth <= sequences[currentSequence].endSequenceHealth && currentSequence < sequences.Length - 1)
            {
                currentSequence++;
                actions = sequences[currentSequence].actions;
                currentAction = 0;
                actionCounter = actions[currentAction].actionLength;
            }
        }

        UIController.instance.bossHealthBar.value = currentHealth;
    }
}

[System.Serializable]
public class BossAction
{
    [Header("Action")]
    public float actionLength;

    public bool shouldMove;
    public bool shouldChasePlayer;
    public bool shouldPatrol;
    public float moveSpeed;
    public bool moveToPoint;
    public Transform pointToMoveTo;
    public bool shouldRotate;
    public float rotationSpeed;

    public bool shouldShoot;
    public GameObject bulletToShoot;
    public float timeBetweenShots;
    public Transform[] shotPoints;
}

[System.Serializable]
public class BossSequence
{
    [Header("Sequence")]
    public BossAction[] actions;

    public int endSequenceHealth;
}
