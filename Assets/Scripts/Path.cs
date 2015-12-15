using UnityEngine;
using System.Collections;

public class Path : MonoBehaviour {

    private float distance;
    private float pheromone;
    private GameObject particle;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public float getDistance()
    {
        return distance;
    }

    public void setDistance(float _distance)
    {
        this.distance = _distance;
    }

    public float getPheromone()
    {
        return pheromone;
    }

    public void setPheromone(float _pheromone)
    {
        this.pheromone = _pheromone;
    }
}
