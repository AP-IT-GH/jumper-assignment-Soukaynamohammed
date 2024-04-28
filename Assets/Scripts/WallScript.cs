using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    public AgentScript agent;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Contains("Enemy"))
        {
           
            agent.TouchWall();
        }
    }
}
