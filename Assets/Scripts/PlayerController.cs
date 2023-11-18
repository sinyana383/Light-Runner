using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Lines
{
    Left, Middle, Right
}
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravity;

    private int curLine = (int)Lines.Middle;
    [SerializeField] private float distBetweenLines;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // TODO: place in function
        if (SwipeController.swipeRight)
        {
            if (curLine < (int)Lines.Right)
                ++curLine;
        }
        else if (SwipeController.swipeLeft)
        {
            if (curLine > (int)Lines.Left)
                --curLine;
        }

        if (SwipeController.swipeUp)
        {
            if (controller.isGrounded)
                Jump();
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (curLine == (int)Lines.Left)
            targetPosition += Vector3.left * distBetweenLines;
        else if (curLine == (int)Lines.Right)
            targetPosition += Vector3.right * distBetweenLines;

        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);
    }

    private void Jump()
    {
        direction.y = jumpHeight;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        direction.z = speed;
        direction.y += gravity * Time.fixedDeltaTime;
        var flags = controller.Move(direction * Time.fixedDeltaTime);
    }
}
