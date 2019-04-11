using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
public class PlayerMovement : MonoBehaviour {

    public Animator         animator;
    public NavMeshAgent     agent;
    public float            inputHoldDelay      = 0.5f;
    public float            turnSpeedThreshold  = 0.5f;
    public float            speedDampTime       = 0.1f;
    public float            slowingSpeed        = 0.175f;
    public float            turnSmoothing       = 15f;

    private WaitForSeconds  inputHoldWait;
    private Vector3         desPostion;

    private const float     stopDisanceProportion   = 0.1f;
    private const float     navMeshSampleDistace    = 4f;
    private readonly int    hashSpeedPara           = Animator.StringToHash("Speed");





    private void Start()
    {
        agent.updateRotation    = false;
        inputHoldWait           = new WaitForSeconds(inputHoldDelay);
        desPostion              = transform.position;

    }


    private void OnAnimatorMove()
    {
        agent.velocity = animator.deltaPosition / Time.deltaTime;
    }

    private void Update()
    {
        if (agent.pathPending)
        {
            return;
        }

        float speed = agent.desiredVelocity.magnitude;
        if(agent.remainingDistance <= agent.stoppingDistance * stopDisanceProportion)
        {
            Stopping(out speed);
        }
        else if(agent.remainingDistance <= agent.stoppingDistance)
        {
            Slowing(out speed,agent.remainingDistance);
        }
        else if(speed > turnSpeedThreshold)
        {
            Moving();
        }

        animator.SetFloat(hashSpeedPara, speed, speedDampTime, Time.deltaTime);


    }


    private void Stopping(out float speed)
    {
        agent.isStopped     = true;
        transform.position  = desPostion;
        speed               = 0;
    }

    private void Slowing(out float speed, float distanceToDes)
    {
        agent.isStopped     = true;
        transform.position  = Vector3.MoveTowards(transform.position, desPostion, slowingSpeed * Time.deltaTime);
        float propotionDistance = 1f - distanceToDes / agent.stoppingDistance;
        speed = Mathf.Lerp(slowingSpeed, 0, propotionDistance);

    }

    private void Moving()
    {
        Quaternion targetRotation   = Quaternion.LookRotation(agent.desiredVelocity);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
    }


    public void OnGroundClick(BaseEventData data)
    {
        PointerEventData pData = (PointerEventData)data;
        NavMeshHit hit;
        if(NavMesh.SamplePosition
            (pData.pointerCurrentRaycast.worldPosition, out hit, navMeshSampleDistace, NavMesh.AllAreas)){
            desPostion = hit.position;
        }
        else
        {
            desPostion = pData.pointerCurrentRaycast.worldPosition;
        }

        agent.SetDestination(desPostion);
        agent.isStopped = false;
    }

    
}
