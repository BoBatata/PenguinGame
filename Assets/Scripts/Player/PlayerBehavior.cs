using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D rigidbodyPlayer;
    private Animator animator;
    private Collider2D colliderPlayer;

    private InputManager inputManager;

    private bool isWalking;
    private bool isSliding;
    private bool slidingCooldownIsOver;

    private int isWalkingAnimationHash;
    private int isSlidingAnimationHash;

    private Vector2 moveDirection;
    [SerializeField] private float velocity = 10f;
    [SerializeField] private float slideVelocity;
    [SerializeField] private float slideDuration;
    [SerializeField] private float slideCooldown;

    private void Awake()
    {

        rigidbodyPlayer = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colliderPlayer = GetComponent<Collider2D>();

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
        rigidbodyPlayer.velocity = new Vector2(moveDirection.x * velocity, rigidbodyPlayer.velocity.y);
        isWalking = moveDirection.x != 0;
    }

    private void Slide(InputAction.CallbackContext inputContext)
    {
        isSliding = inputContext.ReadValueAsButton();
        if (isSliding == true)
        {
            StartCoroutine(SlidingAction());
        }
    }

    IEnumerator SlidingAction()
    {
        Debug.Log("COMEÇOU");
        animator.SetBool(isSlidingAnimationHash, true);
        yield return new WaitForSeconds(1000f);
        Debug.Log("TERMINOU");
        animator.SetBool(isSlidingAnimationHash, false);

    }

    private void SlideAnimationEnd()
    {
        animator.SetTrigger(isSlidingAnimationHash);
    }

    private void SetInputParameter()
    {
        inputManager = new InputManager();

        inputManager.Player.Slide.started += Slide;
        inputManager.Player.Slide.canceled += Slide;

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

        if (isSliding && animator.GetBool(isSlidingAnimationHash) == false)
        {
            animator.SetBool(isSlidingAnimationHash, true);
        }
        else if (isSliding == false && animator.GetBool(isSlidingAnimationHash) == true)
        {
            animator.SetBool(isSlidingAnimationHash, false);
        }
    }

    private void GetAnimationParametersHash()
    {
        isSlidingAnimationHash = Animator.StringToHash("isSliding");
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
