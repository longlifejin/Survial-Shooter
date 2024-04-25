using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 5f;

    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    public Camera mainCamera;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(playerInput.moveHorizontal == 0 && playerInput.moveVertical == 0)
        {
            playerAnimator.SetBool("Move", false);
        }
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        if(playerInput.moveHorizontal != 0 || playerInput.moveVertical != 0)
        {
            Vector3 move = new Vector3(playerInput.moveHorizontal, 0f, playerInput.moveVertical) * moveSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);
            playerAnimator.SetBool("Move", true);
        }        
    }

    private void Rotate()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100))
        {
            Vector3 targetPos = hit.point;
            targetPos.y = transform.position.y;

            Vector3 dir = targetPos - transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);
            playerRigidbody.MoveRotation(rot);
        }
    }
}
