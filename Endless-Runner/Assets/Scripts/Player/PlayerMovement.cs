using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class PlayerMovement : Player
{
    float lerpLanesRate = 0.45f;
    float jumpForce = 100f;

    bool canPress = true;
    bool canDetect = false;
    private void MoveLeft(InputAction.CallbackContext context)
    {
        boolStates &= ~canJump;

        if ((boolStates & isOnGround) == isOnGround && canPress)
        {
            canPress = false;
            if (!isOnTurn)
            {
                if (onLane >= 1)
                {
                    StartCoroutine(LerpLanes(lerpLanesRate, new Vector3(-3, playerSideMoveGameObj.transform.position.y, playerSideMoveGameObj.transform.position.z)));
                    playerAnimator.SetTrigger(hasMovedLeft);

                    onLane -= 1;
                }
            }

            else if (isOnTurn && transform.rotation != Quaternion.Euler(0, -90, 0))
            {
                turnDirection = 0;
            }
        }

        StartCoroutine(PauseWaitor(0.4f));
    }

    private void MoveRight(InputAction.CallbackContext context)
    {
        boolStates &= ~canJump;

        if ((boolStates & isOnGround) == isOnGround && canPress)
        {
            canPress = false;
            if (!isOnTurn)
            {
                if (onLane <= 1)
                {
                    StartCoroutine(LerpLanes(lerpLanesRate, new Vector3(3, playerSideMoveGameObj.transform.position.y, playerSideMoveGameObj.transform.position.z)));
                    playerAnimator.SetTrigger(hasMovedRight);

                    onLane += 1;
                }
            }

            else if (isOnTurn && transform.rotation != Quaternion.Euler(0, -90, 0))
            {
                turnDirection = 2;

            }
        }
        StartCoroutine(PauseWaitor(0.4f));
    }
    private void Jump(InputAction.CallbackContext context)
    {
        if ((boolStates & (canJump | isOnGround)) == (canJump | isOnGround) && (boolStates & isGamePaused) == 0)
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            playerAnimator.SetTrigger(hasJumped);

            boolStates &= ~isOnGround;
            boolStates &= ~canJump;
        }
    }

    private void PlayerGameStarted()
    {
        playerAnimator.Rebind();
        playerAnimator.Update(0f);
        playerAnimator.SetBool(hasGameStarted, true);
    }

/*    private void Slide(InputAction.CallbackContext context)
    {
        if ((boolStates & isOnGround) == isOnGround && (boolStates & isGamePaused) == 0 && canPress)
        {
            playerCapsule.direction = 2;
            playerCapsule.center = new Vector3(0, 0.125f, 0);
            playerAnimator.SetTrigger(hasSlid); StartCoroutine(WaitorWithMehtod(1f, ResetCapsuleTransform));           
        }
        StartCoroutine(PauseWaitor(0.5f));
    }*/

    private IEnumerator LerpLanes(float seconds, Vector3 end)
    {
        sumLaneDef = sumLaneDef + end;

        float elapsedTime = 0;
        Vector3 startingPos = playerSideMoveGameObj.transform.position;
        while (elapsedTime < seconds)
        {
            playerSideMoveGameObj.transform.position = new Vector3(Mathf.Lerp(startingPos.x, sumLaneDef.x, (elapsedTime / seconds)), playerSideMoveGameObj.transform.position.y, playerSideMoveGameObj.transform.position.z);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        playerSideMoveGameObj.transform.position = new Vector3(sumLaneDef.x, playerSideMoveGameObj.transform.position.y, playerSideMoveGameObj.transform.position.z);

        boolStates |= canJump;
    }

    IEnumerator PauseWaitor(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        canPress = true;
    }

   
    protected override void OnEnable()
    {
        base.OnEnable();

        playerInputActions.Player.MoveRight.started += MoveRight;
        playerInputActions.Player.MoveLeft.started += MoveLeft;
        playerInputActions.Player.Jump.started += Jump;
      //  playerInputActions.Player.Slide.started += Slide;

        MenuManager.NewGameStarted += PlayerGameStarted;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        playerInputActions.Player.MoveRight.started -= MoveRight;
        playerInputActions.Player.MoveLeft.started -= MoveLeft;
        playerInputActions.Player.Jump.started -= Jump;
       // playerInputActions.Player.Slide.started -= Slide;

        MenuManager.NewGameStarted -= PlayerGameStarted;
    }
}
