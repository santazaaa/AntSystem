using UnityEngine;
using System.Collections;

public class Path : MonoBehaviour {

    private float distance;
    private float pheromone;
    private float deltaPheromone;
    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

	// Use this for initialization
	void Start () {
        //lineRenderer.material = new Material (Shader.Find("Particles/Additive"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setPosition(Vector3 startPos, Vector3 endPos)
    {
        lineRenderer.SetVertexCount(2);
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }

    public float getDistance()
    {
        return distance;
    }

    public void setDistance(float _distance)
    {
        this.distance = 100 * _distance;
    }

    public float getPheromone()
    {
        return pheromone;
    }

    public void setPheromone(float _pheromone)
    {
        this.pheromone = _pheromone;
    }

    public void setLineAlpha(float alpha)
    {
        Color tmp = lineRenderer.material.GetColor("_Color");
        lineRenderer.material.SetColor("_Color", new Color(tmp.r, tmp.g, tmp.b, alpha));
    }

    public void addDeltaPheromone(float amount)
    {
        deltaPheromone += amount;
    }

    public void updatePheromone(float rho)
    {
        pheromone = pheromone * (1.0f - rho) + deltaPheromone;
        deltaPheromone = 0;
    }

}
