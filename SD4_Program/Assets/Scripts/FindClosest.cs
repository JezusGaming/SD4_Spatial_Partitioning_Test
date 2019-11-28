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
    protected List<AI_Move> m_WhiteQuadAgent = new List<AI_Move>();
    private List<TestObject> m_QuadAgentsWhite;
    private List<TestObject> m_QuadAgentsBlack;

    public Vector2 recPos;
    public Vector2 recSize;
    private Rect m_rect;

    private TestObject newObject;
    float shortestDistance = Mathf.Infinity;
    public bool m_bKdTree = false;
    public bool m_bOctree = false;

    public class TestObject : IQuadTreeObject
    {
        private Vector3 m_vPosition;
        public TestObject(Vector3 position)
        {
            m_vPosition = position;
        }
        public Vector2 GetPosition()
        {
            //Ignore the Y position, Quad-trees operate on a 2D plane.
            return new Vector2(m_vPosition.x, m_vPosition.z);
        }
    }
    QuadTree<TestObject> quadTree;

    // Start is called before the first frame update
    void Start()
    {
        quadTree = new QuadTree<TestObject>(10, new Rect(-1000, -1000, 2000, 2000));
        for (int i = 0; i < m_nCountWhite; i++)
        {
            m_WhiteAgents.Add(Instantiate(WhiteAIPrefab).GetComponent<AI_Move>());
            //newObject = new TestObject(m_WhiteAgents[i].transform.position);
            //quadTree.Insert(newObject);
        }
        for (int i = 0; i < m_nCountBlack; i++)
        {
            m_BlackAgents.Add(Instantiate(BlackAIPrefab).GetComponent<AI_Move>());
            //newObject = new TestObject(m_BlackAgents[i].transform.position);
            //quadTree.Insert(newObject);
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
            
            GameObject whiteAgent;
            //GameObject BlackAgent;
            foreach (var m_WhiteAgent in m_WhiteAgents)
            {
                newObject = new TestObject(m_WhiteAgent.transform.position);
                quadTree.Insert(newObject);
                shortestDistance = Mathf.Infinity;
                m_rect = new Rect(newObject.GetPosition(), recSize);

                m_QuadAgentsWhite = quadTree.RetrieveObjectsInArea(m_rect);

                for (int i = 0; i < m_QuadAgentsWhite.Count; i++)
                {
                    var distance = Vector3.Distance(m_WhiteAgent.transform.position, m_QuadAgentsWhite[i].GetPosition());
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        whiteAgent = m_WhiteAgent.gameObject;
                        //BlackAgent = m_BlackAgents[j].gameObject;
                        //quadTree.Clear();
                        //Debug.DrawLine(m_WhiteAgent.transform.position, m_QuadAgentsWhite[i].GetPosition(), Color.red);
                    }
                    //}
                }
            }
        }
        else if(!m_bKdTree && !m_bOctree)
        {
            
            foreach (var m_WhiteAgent in m_WhiteAgents)
            {
                var nearestDist = float.MaxValue;
                AI_Move nearestObj = null;
                shortestDistance = Mathf.Infinity;

                foreach (var m_BlackAgent in m_BlackAgents)
                {
                    nearestDist = Vector3.Distance(m_WhiteAgent.transform.position, m_BlackAgent.transform.position);
                    
                    if (nearestDist < shortestDistance)
                    {
                        shortestDistance = nearestDist;
                        nearestObj = m_BlackAgent;
                    }

                }
                        Debug.DrawLine(m_WhiteAgent.transform.position, nearestObj.transform.position, Color.red);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (quadTree != null)
        {
            quadTree.DrawDebug();
        }
    }
}
