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
    [SerializeField] private GameManager penalsManager;

    private CharacterController controller;
    private Vector3 direction;
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravity;
    private int curLine = (int)Lines.Middle;
    [SerializeField] private float distBetweenLines;
    
    [SerializeField] private Animator animator;
    private Vector3 previousPosition;
    private bool isJumping = false;
    
    
    [SerializeField] private Light targetLight;
    [SerializeField] private float maxLightRange = 60;

    [SerializeField] private Material emissionMaterial;
    public delegate void LightRangeChanged(float newRange);
    public static event LightRangeChanged OnLightRangeChanged;
    // Start is called before the first frame update
    void Start()
    {
        previousPosition = transform.position;
        emissionMaterial.SetColor("_EmissionColor", Color.yellow);
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

        if (controller.isGrounded)
            animator.SetBool("isRunning", true);
        else
            animator.SetBool("isRunning", false);
        
        Vector3 velocity = (transform.position - previousPosition) / Time.deltaTime;
        Debug.Log($"{velocity.y} = {transform.position} - {previousPosition} / {Time.deltaTime}");
        Debug.Log($"{velocity.y} = {isJumping}");
        previousPosition = transform.position;
        if (isJumping && velocity.y <= 0)
        {
            animator.SetTrigger("fall");
            isJumping = false;
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
        isJumping = true;
        direction.y = jumpHeight;
        animator.SetTrigger("jump");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        direction.z = speed;
        direction.y += gravity * Time.fixedDeltaTime;
        var flags = controller.Move(direction * Time.fixedDeltaTime);
    }

    public void ChangeLight(float range)
    {
        float newRange = Mathf.Clamp(targetLight.range + range, 0, maxLightRange);
        
        targetLight.range = newRange;
        OnLightRangeChanged?.Invoke(newRange);
        if (targetLight.range < 10)
        {
            emissionMaterial.SetColor("_EmissionColor", Color.black);
            penalsManager.OnGameOver();
        }
    }
}
