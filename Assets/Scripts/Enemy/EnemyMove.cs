using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] 
    private float _updateDelay = 2f;
    private float _time;
    private GameObject _target;
    private NavMeshAgent _agent;

    void Start()
    {
        _time = 0f;

        _agent = GetComponent<NavMeshAgent>();

        if (_agent == null)
            Debug.LogError("NaxMeshAgent missing");

        _target = FindNearestGameObject();
    }

    void Update()
    {
        //StartCoroutine(SetTargetCoroutine());
        SetTarget();

        if (_target != null && _agent != null)
            _agent.SetDestination(_target.transform.position);
    }

    private void SetTarget()
    {
        _time += Time.deltaTime;
        if (_time >= _updateDelay)
        {
            _target = FindNearestGameObject();
            _time = 0;
        }
    }


    private IEnumerator SetTargetCoroutine()
    {
        yield return new WaitForSeconds(_updateDelay);
        _target = FindNearestGameObject();
    }

    private GameObject FindNearestGameObject()
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        return allPlayers
            .OrderBy(x => (x.transform.position - transform.position).sqrMagnitude)
            .FirstOrDefault();
    }
}