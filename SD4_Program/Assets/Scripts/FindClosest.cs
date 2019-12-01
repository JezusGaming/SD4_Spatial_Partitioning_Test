using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FindClosest : MonoBehaviour
{
	// Used to spawn the AI prefabs
    public GameObject WhiteAIPrefab;
    public GameObject BlackAIPrefab;
	// Used to count the amount of AI
    public int m_nCountBlack;
    public int m_nCountWhite;
	// Used to store the AI in the kdtree
    protected KdTree<AI_Move> m_WhiteAgents = new KdTree<AI_Move>();
    protected KdTree<AI_Move> m_BlackAgents = new KdTree<AI_Move>();
    protected List<AI_Move> m_WhiteQuadAgent = new List<AI_Move>();
	// Used to get the AI from the quad tree
    private List<TestObject> m_QuadAgentsWhite;
    private List<TestObject> m_QuadAgentsBlack;
	// Used to create the quad area in retrivve all
    private Vector2 recPos;
    private Vector2 recSize = new Vector2(0.1f,0.1f);
    private Rect m_rect;
	// Used to add objects to the quadtree
    private TestObject newObject;
	// Used to find the closest AI
    float shortestDistance = Mathf.Infinity;
	// Used to enable the data structures
    public bool m_bKdTree = false;
    public bool m_bQuadTree = false;
	// Used to create and store objects in the quad tree
    QuadTree<TestObject> quadTree;

	//---------------------------------------------------------------------
	// Find closest object to given position
	//
	// Param:
	//		position: The position of the object
	//
	// Returns:
	//		  returns the closest object
	//---------------------------------------------------------------------
	public class TestObject : IQuadTreeObject
    {
        private Vector3 m_vPosition;
		//---------------------------------------------------------------------
		// Sets the position of the object when initialised
		//
		// Param:
		//		position: The position of the object
		//
		//---------------------------------------------------------------------
		public TestObject(Vector3 position)
        {
            m_vPosition = position;
        }
		//---------------------------------------------------------------------
		// Used to get the position of the object
		//
		// Returns:
		//		  returns the position of the object
		//---------------------------------------------------------------------
		public Vector2 GetPosition()
        {
            //Ignore the Y position, Quad-trees operate on a 2D plane.
            return new Vector2(m_vPosition.x, m_vPosition.z);
        }
    }

	//---------------------------------------------------------------------
	// Start is called before the first frame update, Sets up the quad tree
	// and spawns the AI
	//---------------------------------------------------------------------
	void Start()
    {
        quadTree = new QuadTree<TestObject>(10, new Rect(-1000, -1000, 2000, 2000));
        for (int i = 0; i < m_nCountWhite; i++)
        {
            m_WhiteAgents.Add(Instantiate(WhiteAIPrefab).GetComponent<AI_Move>());
        }
        for (int i = 0; i < m_nCountBlack; i++)
        {
            m_BlackAgents.Add(Instantiate(BlackAIPrefab).GetComponent<AI_Move>());
        }
        
    }

	//---------------------------------------------------------------------
	// Update is called once per frame, Used to check for the closest 
	// object to the AI depending on which data structure is picked.
	//---------------------------------------------------------------------
	void Update()
    {
        if (m_bKdTree)
        {
			// Updates the AI agents that were added at the start
            m_BlackAgents.UpdatePositions();

            foreach (var m_WhiteAgent in m_WhiteAgents)
            {
				// Finds the closest AI
                AI_Move nearestObject = m_BlackAgents.FindClosest(m_WhiteAgent.transform.position);
				// Draws a debug line between them.
                Debug.DrawLine(m_WhiteAgent.transform.position, nearestObject.transform.position, Color.red);
            }
        }
        else if(m_bQuadTree)
        {
            GameObject whiteAgent;
            foreach (var m_WhiteAgent in m_WhiteAgents)
            {
				// Inserts the AI into the quad tree
                newObject = new TestObject(m_WhiteAgent.transform.position);
                quadTree.Insert(newObject);
				// Resets the shortest distance
                shortestDistance = Mathf.Infinity;
				// Sets the rect area to retrieve the objects
                m_rect = new Rect(newObject.GetPosition(), recSize);
				// retrieves all objects in the area
                m_QuadAgentsWhite = quadTree.RetrieveObjectsInArea(m_rect);

				// Gets the shortest distance between the AI
                for (int i = 0; i < m_QuadAgentsWhite.Count; i++)
                {
                    var distance = Vector3.Distance(m_WhiteAgent.transform.position, m_QuadAgentsWhite[i].GetPosition());
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        whiteAgent = m_WhiteAgent.gameObject;
                    }
                }
            }
        }
        else if(!m_bKdTree && !m_bQuadTree)
        {
            
            foreach (var m_WhiteAgent in m_WhiteAgents)
            {
				// Variables used to calculate the closest AI
                var nearestDist = float.MaxValue;
                AI_Move nearestObj = null;
                shortestDistance = Mathf.Infinity;

                foreach (var m_BlackAgent in m_BlackAgents)
                {
					// Gets the shortest distance between the AI
					nearestDist = Vector3.Distance(m_WhiteAgent.transform.position, m_BlackAgent.transform.position);
					   
                    if (nearestDist < shortestDistance)
                    {
                        shortestDistance = nearestDist;
						// Sets the closest AI
                        nearestObj = m_BlackAgent;
                    }

                }
				// Draws a debug line between the closest AI
                        Debug.DrawLine(m_WhiteAgent.transform.position, nearestObj.transform.position, Color.red);
            }
        }
    }
}
