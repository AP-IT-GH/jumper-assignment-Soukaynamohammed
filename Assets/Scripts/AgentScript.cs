using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


public class AgentScript : Agent
{
    private Vector3 enemy1;
    private Vector3 enemy2;

    public GameObject enemyObject1;
    public GameObject enemyObject2;



    public override void Initialize()
    {
        enemy1 = enemyObject1.GetComponent<Transform>().position;
        enemy2 = enemyObject2.GetComponent<Transform>().position;
    }

    public override void OnEpisodeBegin()
    {
        enemyObject1.GetComponent<Transform>().position = enemy1;
        enemyObject2.GetComponent<Transform>().position = enemy2;

        enemyObject1.GetComponent<Rigidbody>().velocity = Vector3.zero;
        enemyObject2.GetComponent<Rigidbody>().velocity = Vector3.zero;

        enemyTouch = 0;
        int randomSpeed = Random.Range(-8, -3);
        enemyObject1.GetComponent<Rigidbody>().AddForce(new Vector3(-7,0,0),ForceMode.Impulse);

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);
    }

    private bool canJump = true;
    public override void OnActionReceived(ActionBuffers actions)
    {
        if (canJump && actions.ContinuousActions[0] > 0)
        {
            canJump = false;
            this.GetComponent<Rigidbody>()
                  .AddForce(
                  new Vector3(0, 7, 0),
                  ForceMode.Impulse
              );
            AddReward(-0.05f);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Contains("Floor"))
        {
            canJump = true;
        }

        if (collision.collider.tag.Contains("Enemy"))
        {
            AddReward(-0.5f);
            EndEpisode();
        }
    }

    private int enemyTouch;

    public void TouchWall()
    {
        
        enemyTouch++;
        if (enemyTouch == 1)
        {
            AddReward(1f);
            int randomSpeed = Random.Range(-8, -3);
            enemyObject2.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -7), ForceMode.Impulse);
        }
        if (enemyTouch >= 2)
        {
            AddReward(1f);
            EndEpisode();
        }
    }


}
