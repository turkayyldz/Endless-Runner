using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
public abstract class Player : PlayerManager
{
    protected static readonly int hasMovedLeft = Animator.StringToHash("hasMovedLeft");
    protected static readonly int hasMovedRight = Animator.StringToHash("hasMovedRight");
    protected static readonly int hasJumped = Animator.StringToHash("hasJumped");
    protected static readonly int hasGameStarted = Animator.StringToHash("hasGameStarted");
    //protected static readonly int hasSlid = Animator.StringToHash("hasSlid");
    protected static readonly int hasDied = Animator.StringToHash("hasDied");
    protected static readonly int restart = Animator.StringToHash("restart");

    protected static readonly int hasTurnedRight = Animator.StringToHash("hasTurnedRight");
    protected static readonly int hasTurnedLeft = Animator.StringToHash("hasTurnedLeft");

    [SerializeField] protected GameObject playerGameObj;
    [SerializeField] protected GameObject playerSideMoveGameObj;
    
    [SerializeField] protected Animator playerAnimator;
    [SerializeField] protected Animator playerRotAnimator;

    protected Rigidbody playerRigidbody;
    protected CapsuleCollider playerCapsule;

    protected Vector3 capsulePos;
    protected float capsuleHeight;

    protected override void Awake()
    {
        base.Awake();

        currentHunger = defTotalHunger;
        hungerDropRate = defHungerInvokeRate;
        hungerDropInvokeDelay = 2f;

        playerAnimator = playerSideMoveGameObj.GetComponent<Animator>();
        playerRigidbody = playerSideMoveGameObj.GetComponent<Rigidbody>();
        playerCapsule = playerSideMoveGameObj.GetComponent<CapsuleCollider>();

        capsuleHeight = playerCapsule.height;
        capsulePos = playerCapsule.center;

        playerDefPos = playerGameObj.transform.position;
        playerSideDefPos = playerSideMoveGameObj.transform.position;
        playerDefRotation = playerGameObj.transform.rotation;
    }

    protected void ResetPlayerPos()
    {
        playerAnimator.enabled = true;
        playerGameObj.transform.position = playerDefPos;
        playerSideMoveGameObj.transform.position = playerSideDefPos;
        playerSideMoveGameObj.transform.rotation = playerDefRotation;
        ResetCapsuleTransform();
    }

    protected void ResetCapsuleTransform()
    {
        playerCapsule.radius = 0.25f;
        playerCapsule.direction = 1;
        playerCapsule.center = capsulePos;
    }

    protected void PlayHungerAnim()
    {
        playerAnimator.SetInteger(hasDied, 0);
    }

    protected void OnTurnPlayAnim(string animVal)
    {
        playerRotAnimator.SetTrigger(animVal);
    }

    public void OnDiePlayAnim(byte causeOfDeath)
    {
        playerAnimator.SetInteger(hasDied, causeOfDeath);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        PlaneMovement.isTurningAnim += OnTurnPlayAnim;
        PlayerStats.PlayerDied += PlayHungerAnim;
        MenuManager.NewGameStarted += ResetPlayerPos;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        PlaneMovement.isTurningAnim -= OnTurnPlayAnim;
        PlayerStats.PlayerDied -= PlayHungerAnim;
        MenuManager.NewGameStarted -= ResetPlayerPos;
    }



}
