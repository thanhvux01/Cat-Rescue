using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private float drag = 0.3f;

    private Vector3 dampingVelocity;
    private Vector3 impact;

    //switch state does't change gravity current speed
    private float verticalVelocity;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity; // 0,1,0
    private void Update()
    {
        //in case we are in ungravity environment so we have to check < 0 vertical velocity  
        if (verticalVelocity < 0f && controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

        if (impact.magnitude < 0.2f)
        {
            impact = Vector3.zero;
            if (agent != null)
                agent.enabled = true;
        }
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
        if (agent != null)
        {
            agent.enabled = false;
        }
    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }

    public void Reset()
    {
       verticalVelocity = 0f;
       impact = Vector3.zero;
    }
}
