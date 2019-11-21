using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Move : MonoBehaviour
{
    public Transform m_walkTowardsPoint;
    private NavMeshAgent m_agent;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3 (transform.position.x,transform.position.y, Random.Range(30, -30));
        m_agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        m_agent.destination = m_walkTowardsPoint.position;
    }
}
