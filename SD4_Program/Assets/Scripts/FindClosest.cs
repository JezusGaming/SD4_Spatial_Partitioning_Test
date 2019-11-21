using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FindClosest : MonoBehaviour
{
    public GameObject WhiteAIPrefab;
    public GameObject BlackAIPrefab;
    public int m_nCountBlack;
    public int m_nCountWhite;
    protected KdTree<AI_Move> m_WhiteAgents = new KdTree<AI_Move>();
    protected KdTree<AI_Move> m_BlackAgents = new KdTree<AI_Move>();

    public bool m_bKdTree = false;
    public bool m_bOctree = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_nCountWhite; i++)
        {
            m_WhiteAgents.Add(Instantiate(WhiteAIPrefab).GetComponent<AI_Move>());
        }
        for (int i = 0; i < m_nCountBlack; i++)
        {
            m_BlackAgents.Add(Instantiate(BlackAIPrefab).GetComponent<AI_Move>());
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (m_bKdTree)
        {
            m_BlackAgents.UpdatePositions();

            foreach (var m_WhiteAgent in m_WhiteAgents)
            {
                AI_Move nearestObject = m_BlackAgents.FindClosest(m_WhiteAgent.transform.position);

                Debug.DrawLine(m_WhiteAgent.transform.position, nearestObject.transform.position, Color.red);
            }
        }
        else if(m_bOctree)
        {
            
        }
        else if(!m_bKdTree && !m_bOctree)
        {
            foreach (var m_WhiteAgent in m_WhiteAgents)
            {
                var nearestDist = float.MaxValue;
                AI_Move nearestObj = null;

                foreach (var m_BlackAgent in m_BlackAgents)
                {
                    nearestDist = Vector3.Distance(m_WhiteAgent.transform.position, m_BlackAgent.transform.position);
                    nearestObj = m_BlackAgent;
                }
                Debug.DrawLine(m_WhiteAgent.transform.position, nearestObj.transform.position, Color.red);
            }
        }
    }
}
