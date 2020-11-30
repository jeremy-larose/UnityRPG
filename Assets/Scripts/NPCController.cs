using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NPCController : MonoBehaviour
{
    private Vector3 _startingPosition;
    private Vector3 _roamPosition;
    private NavMeshAgent _navMesh;
    private State _state;
    private MeshRenderer _meshRenderer;
    private Animator _animator;

    [SerializeField] private GameObject aggroIndicator;
    private Camera _camera;
    private Character _player;
    public float aggroRadius;
    public float moveSpeed;
    private static readonly int Moving = Animator.StringToHash("moving");
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveZ = Animator.StringToHash("moveZ");

    private enum State
    {
        Roaming,
        ChasingPlayer
    }

    private void Awake()
    {
        _navMesh = GetComponent< NavMeshAgent>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _camera = Camera.main;
    }

// Start is called before the first frame update
    void Start()
    {
        _startingPosition = transform.position;
        _roamPosition = GetRoamingPosition();
        _player = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        HandleNPCState();
    }

    private void FixedUpdate()
    {
        AnimateAndMoveCharacter();
    }

    void LateUpdate()
    {
        transform.LookAt( _camera.transform );
    }
    
    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized *
            Random.Range(10f, 10f);
    }

    private void FindTarget()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < aggroRadius)
        {
            aggroIndicator.gameObject.SetActive( true );
            _state = State.ChasingPlayer;
        }
    }

    private void AnimateAndMoveCharacter()
    {
        if ( _navMesh.velocity != Vector3.zero )
        {
            _animator.SetBool( Moving, true );
            Vector3 velocity;
            (velocity = _navMesh.velocity).Normalize();
            _animator.SetFloat( MoveX, velocity.x );
            _animator.SetFloat( MoveZ, velocity.z );
        }
        else
        {
            _animator.SetBool( Moving, false );
        }
    }

    private void HandleNPCState()
    {
        switch (_state)
        {
            case State.Roaming:
                _navMesh.destination = _roamPosition;
                var reachedPositionDistance = 4f;

                if (Vector3.Distance(transform.position, _roamPosition) < reachedPositionDistance)
                {
                    _roamPosition = GetRoamingPosition();
                }

                FindTarget();
                break;
            
            case State.ChasingPlayer:
                _navMesh.destination = _player.transform.position;

                if (Vector3.Distance(transform.position, _navMesh.destination) > aggroRadius)
                {
                    aggroIndicator.gameObject.SetActive( false );
                    _roamPosition = _startingPosition;
                    _state = State.Roaming;
                }
                break;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere( transform.position, aggroRadius );
    }
}
