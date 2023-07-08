using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class player_jump : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody rb;
    private bool isJumping;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isJumping = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isJumping == false)
        {
            rb.velocity = new Vector3(0, 5, 0);
            isJumping = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        isJumping = false;
    }
}