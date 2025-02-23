using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public NavMeshAgent agent;
    public float wanderTime = 5f;
    public float wanderRadius = 20f;
    private float timeTillWander = 0f;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        timeTillWander -= Time.deltaTime;

        if (timeTillWander < 0f || agent.remainingDistance == 0f)
        {
            Vector3 randomPosition = Random.insideUnitSphere * wanderRadius;
            randomPosition += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, wanderRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }

            timeTillWander = wanderTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Destroy(collision.gameObject);
            Application.Quit();
        }
    }
}
