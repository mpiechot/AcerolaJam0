using UnityEngine;
using UnityEngine.AI;

public class DEMO_Navigator : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private NavMeshAgent agent;

    private bool targetSet = false;

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }

        if (!targetSet)
        {
            agent.SetDestination(target.position);
            targetSet = true;
        }
    }
}
