using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<NavMeshAgent>().destination = player.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == player)
        {
            Debug.Log("Hit");
        }
    }
}
