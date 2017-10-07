using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class AIMovement : MonoBehaviour
{
    public Transform formation_pos;
    public Vector3 target;
    UnityEngine.AI.NavMeshAgent agent;

    private bool form_up;


    // BOID VARIABLES

    Vector3 position;
    Vector3 velocity;
    Vector3 acceleration;

    public float maxForce; // Maximum steering force
    public float maxSpeed; // Maximum speed

    public float neighbourDistance;
    public float desiredSeperation;

    Quaternion newRotation;


    // Use this for initialization
    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        target = this.gameObject.transform.position;

        acceleration = Vector3.zero;

        //float angle = Random (TWO_PI);
        float angle = Random.Range(0.0f, (Mathf.PI * 2));
        velocity = new Vector3(0.0f, 0.0f, 0.0f);

    }

    // Update is called once per frame
    void Update ()
    {
        if(form_up == true)
        {
            target = formation_pos.position;
        }

        if (Vector3.Distance(this.gameObject.transform.position, target) > 0.5f)
        {
            agent.SetDestination(target);
        }
	}


    public void MoveOrder(Vector3 _pos)
    {
        target = _pos;
        form_up = false;
    }



    public void FollowPlayer()
    {
        form_up = true;
    }



    public void Run(List<AIMovement> _squadies)
    {
        Squad(_squadies);
        SquadieUpdate();
    }



    private void Squad(List<AIMovement> _squadies)
    {
        Vector3 sep = Seperate(_squadies); // seperation
        Vector3 ali = Align(_squadies);    // alignment
        Vector3 coh = Cohesion(_squadies);  // cohesion

        // Arbitrarily weight these forces
        sep = (sep * 5.0f);
        ali = (ali * 1.0f);
        coh = (coh * 1.0f);
        ApplyForce(sep);
        //ApplyForce(ali);
        //ApplyForce(coh);

    }



    void SquadieUpdate()
    {
        // update velocity
        velocity = (velocity + acceleration);

        // limit speed
        //velocity.limit (maxSpeed); 
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        transform.position = (transform.position + velocity);

        transform.rotation = Quaternion.LookRotation(velocity);

        // reset acceleration to 0 each cycle
        acceleration = Vector3.zero;
    }



    void ApplyForce(Vector3 force)
    {
        // add force to acceleration?
        acceleration += force;
    }



    // Seperation
    // Method checks for nearby boids and steers away
    public Vector3 Seperate(List<AIMovement> _squadies)
    {
        Vector3 steer = Vector3.zero;
        int count = 0;

        // check through every other boid
        for (int i = 0; i < _squadies.Count; i++)
        {
            float d = Vector3.Distance(transform.position, _squadies[i].transform.position);
            // if boid is a neighbour
            if (d > 0 && d < desiredSeperation)
            {
                // Calculate vector pointing away from neighbour
                Vector3 diff = (transform.position - _squadies[i].transform.position);
                diff.Normalize();
                diff = (diff / d); // Weight by distance
                steer = (steer + diff);
                count++;
            }
        }

        // Average -- divided by how many
        if (count > 0)
        {
            steer = (steer / count);
        }

        // as long as the vector is greater than 0
        if (steer != Vector3.zero)  // if(steer.mag() > 0)
        {
            //steer.setMag (maxSpeed);
            steer = Vector3.ClampMagnitude(steer, maxSpeed);

            // implement Reynolds: steering = desired - velocity
            steer.Normalize();
            steer = (steer * maxSpeed);
            steer = (steer - velocity);
            //steer.limit (maxForce);
            steer = Vector3.ClampMagnitude(steer, maxForce);
        }
        return steer;
    }



    // Alignment
    // For every nearby boid in the system, calculate the average velocity
    public Vector3 Align(List<AIMovement> _squadies)
    {
        // The position the boid want to be
        Vector3 sum = Vector3.zero;
        int count = 0;

        // check through every other boid
        for (int i = 0; i < _squadies.Count; i++)
        {
            float d = Vector3.Distance(transform.position, _squadies[i].transform.position);
            // if boid is a neighbour
            if ((d > 0) && (d < neighbourDistance))
            {
                sum = (sum + _squadies[i].velocity);
                count++;
            }
        }

        if (count > 0)
        {
            sum = (sum / count);
            //sum.setMag (maxSpeed);
            sum = Vector3.ClampMagnitude(sum, maxSpeed);

            // Implement Reynolds: Steering = desired - velocity
            sum.Normalize();
            sum = (sum * maxSpeed);
            Vector3 steer = (sum - velocity);
            //steer.limit (maxForce);
            steer = Vector3.ClampMagnitude(steer, maxForce);
            return steer;
        }
        else
            return new Vector3();
    }



    // Cohesion
    // For the average position (I.E. center) of all nearby boids, calculate steering
    // vector towards that position
    public Vector3 Cohesion(List<AIMovement> _squadies)
    {
        Vector3 sum = Vector3.zero; // start with empty vector to accumulate all positions
        int count = 0;

        for (int i = 0; i < _squadies.Count; i++)
        {
            float d = Vector3.Distance(transform.position, _squadies[i].transform.position);
            if ((d > 0) && (d < neighbourDistance))
            {
                sum = (sum + _squadies[i].transform.position); // add position
                count++;
            }
        }

        if (count > 0)
        {
            sum = (sum / count);

            return Seek(sum); // Steer towards the position
        }

        else
        {
            return new Vector3();
        }
    }



    // STEER = DESIRED MINUS VELOCITY
    Vector3 Seek(Vector3 target)
    {
        Vector3 desired = (target - transform.position); //  a vector pointing from the position to the target
                                                         // scale to max speed
        desired.Normalize();
        desired = (desired * maxSpeed);

        //desired.setMag (maxSpeed);
        desired = Vector3.ClampMagnitude(desired, maxSpeed);

        // steering = desired minus velocity
        Vector3 steer = (desired - velocity);
        //steer.limit (maxForce); // Limit to maximum steering force
        steer = Vector3.ClampMagnitude(steer, maxForce);
        return steer;
    }

}
