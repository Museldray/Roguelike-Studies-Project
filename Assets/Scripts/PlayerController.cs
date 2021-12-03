 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement
    public float moveSpeed;
    private Vector2 moveInput;

    public Rigidbody2D theRB;

    public Transform gunArm;

    private Camera mainCam;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Move inputs
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        //transform.position += new Vector3(moveInput.x * Time.deltaTime * moveSpeed, moveInput.y * Time.deltaTime * moveSpeed, 0f);

        theRB.velocity = moveInput * moveSpeed;

        Vector3 mousePosition = Input.mousePosition;
        Vector3 screenPoint = mainCam.WorldToScreenPoint(transform.localPosition);

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

        if(moveInput != Vector2.zero)
        {
            anim.SetBool("isMoving", true);
        } else
        {
            anim.SetBool("isMoving", false);
        }
    }
}
