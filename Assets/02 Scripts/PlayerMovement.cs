using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 100f;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;
    public Camera mainCamera;
    Transform tr;
    public Transform gunTr;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        //Rotate();
        ViewRotate();
        Move();
        playerAnimator.SetFloat("Move", playerInput.move);

    }
    void Move() {
        Vector3 moveDistance = (playerInput.move * tr.forward) 
            //+ (playerInput.moveSide * tr.right))
            * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }
    void ViewRotate() {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, 1<<8)) {
            // Vector3 dir = new Vector3(hit.point.x - tr.position.x, 0f, hit.point.z - tr.position.z);
            //tr.rotation = Quaternion.LookRotation(dir);
            Vector3 dir = new Vector3(hit.point.x - gunTr.position.x, 0f, hit.point.z -gunTr.position.z);
            if(dir.sqrMagnitude > 1f)
                tr.rotation = Quaternion.LookRotation(dir);
        }
    }

    void Rotate() {
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0f); 
    }

}
