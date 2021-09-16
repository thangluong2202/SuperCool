using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class Character
{
    [SerializeField]
    GameObject handObject;

    public enum CharacterState
    {
        OnTheGroundNormal,
        Jumping,
        Crouching,
        StandingUp,
        Falling,
    }

    #region Speed state
    private float baseSpeed = 10;

    private float deltaSpeed;
    private float deltaPercentSpeed;
    #endregion

    private float currentSpeed = 10;
    public float jumpSpeed;
    float fallingSpeed;
    public CharacterState characterState;
    const float gravity = 15;

    List<Action> queueActionForFixedUpdate;

    public BoxCollider coliderGroundCheck;
    public LayerMask groundLayerMask;

    Vector3 direction;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Vector3 flatMovemoment = direction * baseSpeed;

        Vector3 gravityMovement = Vector3.zero;
        Vector3 jumpMovement = Vector3.zero;

        if (characterState != CharacterState.Jumping)
        {
            if (!OnTheGround())
            {
                gravityMovement = -Vector3.up * fallingSpeed;
                fallingSpeed += gravity * Time.fixedDeltaTime;

                characterState = CharacterState.Falling;
            }
            else if (characterState == CharacterState.Falling)
            {
                characterState = CharacterState.OnTheGroundNormal;
                fallingSpeed = 0;
            }
        }
        else
        {
            jumpMovement = Vector3.up * jumpSpeed;
            jumpSpeed -= gravity * Time.fixedDeltaTime;

            if (jumpSpeed <= 0)
            {
                characterState = CharacterState.Falling;
            }
        }
        Debug.Log("Gravity: " + gravityMovement);
        rigidBody.velocity = flatMovemoment + gravityMovement + jumpMovement;
    }


    private bool OnTheGround()
    {
        if (Physics.CheckBox(transform.TransformPoint(coliderGroundCheck.center), coliderGroundCheck.size / 2, transform.rotation, groundLayerMask))
        {
            var anything = Physics.OverlapBox(transform.TransformPoint(coliderGroundCheck.center), coliderGroundCheck.size / 2, transform.rotation, groundLayerMask);
            return true;
        }

        return false;
    }

    public void Move(Vector3 direction)
    {
        this.direction = direction.normalized;
    }

    public void Rotate(Vector3 angle)
    {
        var characterRotation = transform.rotation.eulerAngles;
        characterRotation.y += angle.x;
        transform.rotation = Quaternion.Euler(characterRotation);


        var upRotation = handObject.transform.rotation.eulerAngles;
        upRotation.x = MapAngleToSpecialCase(upRotation.x);
        upRotation.x -= angle.y;
        upRotation.x = Mathf.Clamp(upRotation.x, -90, 90);
        handObject.transform.rotation = Quaternion.Euler(upRotation);
    }

    /// <summary>
    /// (0 -> 180) |-> (0 -> 180)
    /// (180 -> 360) |-> (-180 -> 0)
    /// </summary>
    /// <returns></returns>
    public float MapAngleToSpecialCase(float angle)
    {
        angle %= 360;
        if(angle >= 0 && angle <= 180)
        {
            return angle;
        }
        else
        {
            return angle - 360;
        }
    }

    public bool Jump()
    {
        if(characterState == CharacterState.OnTheGroundNormal)
        {
            characterState = CharacterState.Jumping;
            jumpSpeed = 8;
            return true;
        }

        return false;
    }

    public bool Crouch()
    {
        if(characterState == CharacterState.OnTheGroundNormal)
        {
            characterState = CharacterState.Crouching;
            return true;
        }
        return false;
    }


    public void StandUp()
    {
        if (characterState == CharacterState.Crouching)
        {
            characterState = CharacterState.StandingUp;
        }
    }

    public void ModifySpeedValue(float value)
    {
        deltaSpeed += value;
        RecalculateCurrentSpeed();
    }

    public void ModifySpeedPercent(float value)
    {
        deltaPercentSpeed += value;
        RecalculateCurrentSpeed();
    }

    private void RecalculateCurrentSpeed()
    {
        currentSpeed = (baseSpeed + deltaSpeed) * (1 + deltaPercentSpeed);
    }
}
