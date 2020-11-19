using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is for movement only. For other characters, see PlayerInput.

public class PlayerController : MonoBehaviour
{
    Movement movement;
    EntityPosition position;
    public int moveTime; //TEMPORARY!!! SHOULD BE IN AN EntityStats SCRIPT!
    PlayerQueue pq;

    private float inputDelayTimer = 0f;
    private float inputDelayTime = .1f;
    private float initialDelayTime = .4f;
    private bool isInputOnDelay = false;
    private bool firstPress = false;

    private void Start()
    {
        movement = GetComponent<Movement>();
        position = GetComponent<EntityPosition>();
        pq = GetComponent<PlayerQueue>();
    }
    private void Update()
    {
        {
            if (isInputOnDelay)
            {
                inputDelayTimer -= Time.deltaTime;
                if (inputDelayTimer <= 0f)
                {
                    isInputOnDelay = false;
                    inputDelayTimer = inputDelayTime;
                }
            }

            if (!firstPress)
            {
                if (Input.GetKeyDown("up") || Input.GetKeyDown("w"))
                {
                    if (!pq.DoAction(moveTime)) return;
                    movement.AttemptMovement(Direction.NORTH);
                    inputDelayTimer = initialDelayTime;
                    isInputOnDelay = true;
                    firstPress = true;
                }
                if (Input.GetKeyDown("down") || Input.GetKeyDown("s"))
                {
                    if (!pq.DoAction(moveTime)) return;
                    movement.AttemptMovement(Direction.SOUTH);
                    inputDelayTimer = initialDelayTime;
                    isInputOnDelay = true;
                    firstPress = true;
                }
                if (Input.GetKeyDown("left") || Input.GetKeyDown("a"))
                {
                    if (!pq.DoAction(moveTime)) return;
                    movement.AttemptMovement(Direction.WEST);
                    inputDelayTimer = initialDelayTime;
                    isInputOnDelay = true;
                    firstPress = true;
                }
                if (Input.GetKeyDown("right") || Input.GetKeyDown("d"))
                {
                    if (!pq.DoAction(moveTime)) return;
                    movement.AttemptMovement(Direction.EAST);
                    inputDelayTimer = initialDelayTime;
                    isInputOnDelay = true;
                    firstPress = true;
                }
            }
            if (!isInputOnDelay)
            {
                if (Input.GetKey("up") || Input.GetKey("w"))
                {
                    if (!pq.DoAction(moveTime)) return;
                    movement.AttemptMovement(Direction.NORTH);
                    isInputOnDelay = true;
                }
                if (Input.GetKey("down") || Input.GetKey("s"))
                {
                    if (!pq.DoAction(moveTime)) return;
                    movement.AttemptMovement(Direction.SOUTH);
                    isInputOnDelay = true;
                }
                if (Input.GetKey("left") || Input.GetKey("a"))
                {
                    if (!pq.DoAction(moveTime)) return;
                    movement.AttemptMovement(Direction.WEST);
                    isInputOnDelay = true;
                }
                if (Input.GetKey("right") || Input.GetKey("d"))
                {
                    if (!pq.DoAction(moveTime)) return;
                    movement.AttemptMovement(Direction.EAST);
                    isInputOnDelay = true;
                }
            }
            if (Input.GetKeyUp("up") || Input.GetKeyUp("w") || Input.GetKeyUp("left") || Input.GetKeyUp("a")
                || Input.GetKeyUp("right") || Input.GetKeyUp("d") || Input.GetKeyUp("down") || Input.GetKeyUp("s"))
            {
                isInputOnDelay = false;
                if (!(Input.GetKey("up") || Input.GetKey("w") || Input.GetKey("left") || Input.GetKey("a")
                || Input.GetKey("right") || Input.GetKey("d") || Input.GetKey("down") || Input.GetKey("s")))
                {
                    firstPress = false;
                }
            }
        }
    }
}
