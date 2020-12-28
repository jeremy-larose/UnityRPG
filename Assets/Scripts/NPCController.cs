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
    [SerializeField] private State _state;
    private MeshRenderer _meshRenderer;
    private Animator _animator;

    [SerializeField] private GameObject aggroIndicator;
    private Camera _camera;
    private Character _player;
    public float aggroRadius;
    public float attackRange;
    public float moveSpeed;
    public float attackSpeed;
    
    private static readonly int Moving = Animator.StringToHash("moving");
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveZ = Animator.StringToHash("moveZ");
    private static readonly int Attacking = Animator.StringToHash("attacking");

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

                if (Vector3.Distance(transform.position, _navMesh.destination) < attackRange)
                {
                    if (!GetComponent<Character>().Traits.ContainsKey(Character.CharacterFlags.Pacifist))
                    {
                        Debug.Log( "NPCController: Would have attacked, but is set to pacifist!");
                    }
                    else
                    {
                        StartCoroutine(Attack());
                    }
                }
                break;
        }
    }

    private IEnumerator Attack()
    {
        _animator.SetBool(Attacking, true);
        aggroIndicator.gameObject.SetActive( false );
        yield return new WaitForSeconds(attackSpeed);
        _animator.SetBool(Attacking, false);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere( transform.position, aggroRadius );
    }
}
/*
Insignificant Other: 
    <p>Relationships with Narcissists/Emotional Abusers are not normal relationships. They are highly toxic and very emotionally painful experiences. Even when you walk away from an emotional abuser, life doesn't magically return to normal. Your wounds are invisible. Unfortunately, the healing journey from emotional abuse is not normal either. Relationships with emotional abusers are not real relationships. They are memberships. You received an "à la carte" subscription in exchange for your heart. I know, I've been there.</p>

    <p>I wrote this book because I didn't understand. I didn't understand why I worked so hard to save a relationship with a person who treated me so terribly. I didn't understand why it felt impossible to walk away from this toxic person. I didn't understand why he said he loved me as he continued to intentionally inflict emotional pain on me. I didn't understand why he lied, pathologically. I didn't understand why he cheated, repeatedly. I didn't understand why he enjoyed twisting my mind into a pretzel with his drama. I didn't understand why the relationship was so confusing. But... I certainly do now. I want to share my experiences and knowledge to help you understand too.</p>
    

"emotional abuse", "abuse recovery", "narcissist", "abusive relationship", "relationship help"

<p>Toni LaRose lives in Spring Lake, Michigan. She is a proud mother of two incredibly talented sons, Jason Gagnon and Jeremy LaRose. She is a Registered Nurse and Life Coach with a passion for psychology. She has worked in inpatient psychiatric settings, assisting people who struggle with mental illness. She has coached
    many individuals on their healing journey from Narcissistic/Emotional Abuse. She is a firm believer that unless you have personally walked in someone’s shoes, you cannot effectively assist them in their recovery. She has spent years researching Narcissistic/Emotional Abuse while on her own path to recovery.</p>

    <p>She is a firm believer that with the right tools and the right mindset, anyone can overcome their past traumas and live the life God intended them to live. She is the author of four books: 
    <br>
    Insignificant Other
    The Journey to Significance
    I Am Significant Recovery Journal
I Am Significant Fitness Journal.</p>

    <p>When she is not working on a new book creation, she enjoys reading, working out at the gym, hiking, biking, snow skiing, being close to nature, and walking on the beach. She is a spiritual warrior who is on a quest with her higher self to run her own race. She chases her own dreams with passion and curiosity. She strongly believes that if you are not committed to the journey of becoming a life-long learner, you are missing out on the true essence of life. She prefers deep, philosophical conversations over small talk with
    small minds. She believes strongly in the hero’s journey, which simply means we all need to learn the lessons life has to teach us… so that we can assist others along on their journey.</p>
*/