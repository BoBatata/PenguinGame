using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private Animator animator;
    private Collider2D collider;

    private InputManager inputManager;

    private bool isWalking;

    private int isWalkingAnimationHash;

    private Vector2 moveDirection;
    [SerializeField] private float velocity = 10f;

    private void Awake()
    {

        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();

        SetInputParameter();
    }

    private void Update()
    {
        Move();
        GetAnimationParametersHash();   
        PlayerAnimation();
    }

    private void Move()
    {
        if (moveDirection.x > 0)
        {
            transform.rotation = quaternion.identity;
        }
        else if (moveDirection.x < 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        moveDirection.x = inputManager.Player.Move.ReadValue<float>();
        rigidbody.velocity = new Vector2(moveDirection.x * velocity, rigidbody.velocity.y);
        isWalking = moveDirection.x != 0;
    }

    private void SetInputParameter()
    {
        inputManager = new InputManager();

    }

    private void PlayerAnimation()
    {
        if (isWalking && animator.GetBool(isWalkingAnimationHash) == false)
        {
            animator.SetBool(isWalkingAnimationHash, true);
        }
        else if (isWalking == false && animator.GetBool(isWalkingAnimationHash) == true)
        {
            animator.SetBool(isWalkingAnimationHash, false);
        }
    }

    private void GetAnimationParametersHash()
    {
        isWalkingAnimationHash = Animator.StringToHash("isWalking");
    }

    #region Enable/Disable
    private void OnEnable()
    {
        inputManager.Enable();
    }

    private void OnDisable()
    {
        inputManager.Disable();
    }
    #endregion


}
