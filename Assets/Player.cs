using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{

    public event System.Action OnReachEndOfLevel;

    public float moveSpeed = 5;
    public float smoothMoveTime = .1f;
    public float turnSpeed = 8;

    float angle;
    float smoothInputPressed;
    float smoothMoveVelocity;
    Vector3 velocity;

    Rigidbody skeleton;
    bool CantMove;

    public int diamonds = 0;
    public UnityEvent<Player> OnDiamondCollected;

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;


    void Start()
    {
        skeleton = GetComponent<Rigidbody>();
        Guard.OnGuardHasSpottedPlayer += Disable;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputDirection = Vector3.zero;
        //Only moves if the player has not been spotted
        if (!CantMove)
        {
            //movement of player, "GetAxisRaw" makes the movement smoother
            inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        }
        //only move when there is an input
        float inputPressed = inputDirection.magnitude;
        //smooths the players movement, "ref" allows me to change the variable of smoothMoveVelocity on the fly
        smoothInputPressed = Mathf.SmoothDamp(smoothInputPressed, inputPressed, ref smoothMoveVelocity, smoothMoveTime);
        //direction player is facing
        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        //controls angle smoothing
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputPressed);

        velocity = transform.forward * moveSpeed * smoothInputPressed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }
    //trigger to see if the player has reached the end point, then disable movement
    void OnTriggerEnter(Collider hitCollider)
    {
        if (hitCollider.tag == "Finish")
        {
            Disable();
            if (OnReachEndOfLevel != null)
            {
                OnReachEndOfLevel();
            }
        }

        if (hitCollider.tag == "Diamond")
        {
            Debug.Log("Diamond collected!");
            diamonds = diamonds + 1;
            Destroy(hitCollider);
            OnDiamondCollected.Invoke(this);

            if (diamonds >= 5)
            {
                Disable();
                if (OnReachEndOfLevel != null)
                {
                    OnReachEndOfLevel();
                }
            }
        }

        if (hitCollider.tag == "Bomb")
        {
            Debug.Log("You bombed!");
            diamonds = diamonds - 1;
            Destroy(hitCollider);
            OnDiamondCollected.Invoke(this);
        }

    }

    public void Disable()
    {
        CantMove = true;
    }

    void FixedUpdate()
    {
        skeleton.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        skeleton.MovePosition(skeleton.position + velocity * Time.deltaTime);
    }

    void OnDestroy()
    {
        //calls this method when player is destroyed, for example if the scene has changed etc
        Guard.OnGuardHasSpottedPlayer -= Disable;
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }
}