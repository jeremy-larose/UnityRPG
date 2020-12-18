using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _myRigidbody;
    private Animator _animator;
    private Vector3 _change = Vector3.zero;
    public float attackSpeed;
    public float moveSpeed;
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveZ = Animator.StringToHash("moveZ");
    private static readonly int Moving = Animator.StringToHash("moving");
    private static readonly int Attacking = Animator.StringToHash("attacking");

    // Start is called before the first frame update
    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _change.x = Input.GetAxisRaw("Horizontal");
        _change.z = Input.GetAxisRaw("Vertical");
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log( "Attacking!");
            StartCoroutine( Attack() );
        } 

    }

    void FixedUpdate()
    {
        AnimateAndMoveCharacter();

    }

    private IEnumerator Attack()
    {
        _animator.SetBool( Attacking, true );
        yield return new WaitForSeconds( attackSpeed );
        _animator.SetBool( Attacking, false );
    }

    private void AnimateAndMoveCharacter()
    {
        _change.Normalize();

        if (_change != Vector3.zero)
        {

            _animator.SetFloat(MoveX, _change.x );
            _animator.SetFloat(MoveZ, _change.z );
            _animator.SetBool( Moving, true );
            _myRigidbody.MovePosition( transform.position + _change * (moveSpeed * Time.deltaTime) );
        }
        else
        {
            _animator.SetBool( Moving, false );
        }
    }
}
