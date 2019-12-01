using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Move : MonoBehaviour
{
	// The position the AI will walk to
    public Transform m_walkTowardsPoint;
    private NavMeshAgent m_agent;

	//---------------------------------------------------------------------
	// Start is called before the first frame update.
	//---------------------------------------------------------------------
	void Start()
    {
		// Used to spawn the AI in a random location on the z so they spawn all in a line
        transform.position = new Vector3 (transform.position.x,transform.position.y, Random.Range(30, -30));
        m_agent = GetComponent<NavMeshAgent>();
    }

	//---------------------------------------------------------------------
	// Update is called once per frame.
	//---------------------------------------------------------------------
	void Update()
    {
		// Moves the AI to the position set in the prefab
        m_agent.destination = m_walkTowardsPoint.position;
    }
}
