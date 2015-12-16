using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogicManager : MonoBehaviour {

    private GameManager gameManager;

    public float alpha;
    public float beta;
    public float rho;
    public float Q;

    public float maxAlpha = 20;
    public float maxBeta = 20;
    public float maxRho = 1;
    public float maxQ = 50;

    private float currentMaxPheromone;

    [HideInInspector]
    public static LogicManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private static LogicManager _instance;

    void Awake()
    {
        _instance = this;
        currentMaxPheromone = 0;
    }

	// Use this for initialization
	void Start () 
    {
        gameManager = GameManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ResetPheromone()
    {
        for(int i = 0; i < gameManager.getNodeListSize(); i++)
        {
            for(int j = 0; j < i; j++)
            {
                Path path = gameManager.getPath(i, j);
                path.setPheromone(1 / path.getDistance());
                path.setLineAlpha(0);
            }
        }
    }

    public float getPheromoneUpdate(float distance)
    {
        //Respect to ant id = k :D
        return Q / distance;
    }

    public void addDeltaPheromoneToPaths(List<Node> prevNodes)
    {
        float distance = getTotalDistance(prevNodes);
        // Add delta T to tour path only
        for (int i = 0; i < prevNodes.Count - 1; i++)
        {
            Node nodeI = prevNodes[i];
            Node nodeJ = prevNodes[i + 1];

            float deltaT = getPheromoneUpdate(distance);

            Path path = gameManager.getPath(nodeI.getNodeId(), nodeJ.getNodeId());

            path.addDeltaPheromone(deltaT);
        }
    }

    public void pheromoneDecay(List<Node> nodeList)
    {
        float distance = getTotalDistance(nodeList);
        int allNSize = gameManager.getNodeListSize();;

        // Decay pheromone in all path with rho
        for (int i = 0; i < allNSize - 1; i++)
        {
            for(int j = 0; j < i; j++)
            {
                Path path = gameManager.getPath(i, j);
                path.setPheromone((1 - rho) * path.getPheromone());
            }
        }

        // Add delta T to tour path only
        for (int i = 0; i < nodeList.Count - 1; i++)
        {
            Node nodeI = nodeList[i];
            Node nodeJ = nodeList[i + 1];

            float deltaT = getPheromoneUpdate(distance);

            Path path = gameManager.getPath(nodeI.getNodeId(), nodeJ.getNodeId());
           
            path.setPheromone(path.getPheromone() + deltaT);

            Debug.Log("Delta T = " + deltaT + " New P of path " + nodeI.getNodeId() + " " + nodeJ.getNodeId() + " = " + path.getPheromone());
        }

        //gameManager.updatePheromoneLine();
    }

    public Node getNextNode(List<Node> prevNodes, Node currentNode)
    {
        int nodeListSize = gameManager.getNodeListSize();

        if (prevNodes.Count >= gameManager.getNodeListSize())return prevNodes[0];

        List<Node> possibleNodes = new List<Node>();

        for (int i = 0; i < nodeListSize; i++)
        {
            Node tempNode = gameManager.getNode(i);
            if(!prevNodes.Contains(tempNode) && tempNode.getNodeId() != currentNode.getNodeId())
            {
                possibleNodes.Add(tempNode);
            }
        }

        // Back to home case
        if (possibleNodes.Count == 0) return prevNodes[0];

        // There are possible paths left

        // Calculate probability of each path
        List<float> probs = new List<float>();
        List<Path> paths = new List<Path>();
        float sum = 0;
        foreach (Node destNode in possibleNodes)
        {
            Path path = gameManager.getPath(currentNode, destNode);
            paths.Add(path);

            float p = getPathProb(path);
            probs.Add(p);

            sum += p;
        }
        
        if(sum < 0.00000001f) // Too close to zero
        {
            int rnd = Random.Range(0, possibleNodes.Count);
            for(int i = 0; i < paths.Count; i++)
            {
                Path path = paths[i];
            }
            return possibleNodes[rnd];
        }

        float rndF = Random.Range(0.0f, 1.0f);
        float cumuSum = 0;
        Node retNode = currentNode;
        bool foundNode = false;

        for (int i = 0; i < possibleNodes.Count; i++ )
        {
            float p = probs[i] / sum;
            cumuSum += p;
            if (!foundNode & rndF < cumuSum)
            {
                retNode = possibleNodes[i];
                foundNode = true;
            }
            paths[i].setLineAlpha(p);

        }
        return retNode;
    }

    public float getPathProb(Path path)
    {
        float t = path.getPheromone();
        float neta = 1.0f / path.getDistance();
        float p = Mathf.Pow((float)t, alpha) * Mathf.Pow((float)neta, beta);
        return p;
    }

    public float getTotalDistance(List<Node> nodeList)
    {
        float totalDistance = 0;
        int nodeListSize = nodeList.Count;
        for(int i = 0; i < nodeListSize - 1; i++)
        {
            totalDistance += gameManager.getPath(nodeList[i], nodeList[i+1]).getDistance();
        }
        return totalDistance;
    }
}
