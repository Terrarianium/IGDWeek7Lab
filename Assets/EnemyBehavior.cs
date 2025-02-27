using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    public GameObject[] patrolPoints;
    private NavMeshAgent agent;
    private Vector3 lastKnownPosition;
    private float wanderRadius = 10f;
    private float sawPlayer = 0f;

    
    void Start()
    {
        agent = enemy.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(sawPlayer);

        if (Vector3.Distance(enemy.transform.position, player.transform.position) < 10)
        {
            lastKnownPosition = player.transform.position;
            agent.SetDestination(lastKnownPosition);
            enemy.GetComponent<Renderer>().material.color = Color.yellow;
            sawPlayer = 3f;
        }
        else if (sawPlayer > 0f && agent.remainingDistance < 1)
        {
            enemy.GetComponent<Renderer>().material.color = Color.red;

            Vector3 randomPosition = Random.insideUnitSphere * wanderRadius;
            randomPosition += lastKnownPosition;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, wanderRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }

            sawPlayer -= 1f;
        }
        else if (sawPlayer == 0f && agent.remainingDistance < 1)
        {
            agent.SetDestination(patrolPoints[Random.Range(0, 4)].transform.position);
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
