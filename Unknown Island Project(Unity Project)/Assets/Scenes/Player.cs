using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using Assets.Scenes;
using Mono.Data.Sqlite;
using System;

public class Player : MonoBehaviour
{
    UKI_script uki;

    //키보드
    float hAxis;
    float vAxis;
    bool wDown;

    public static bool jumpStatus;
    private static bool viewpoint_bool;//ture = 3rd

    private static float gravity;
    private static float jump;

    Camera Camera;
    
    public Player()
    {
        uki = new UKI_script();
        jumpStatus = false;
        gravity = 1000f;
        viewpoint_bool = true;
        Camera = FindObjectOfType<Camera>();
    }
    public IEnumerator LookAround(Transform cameraArm, Transform camera, float camera_dstc, float mouse_dpi, WaitForEndOfFrame wait)
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X") * mouse_dpi, Input.GetAxis("Mouse Y") * mouse_dpi);
        Vector3 camAngle = cameraArm.rotation.eulerAngles;

        float x = camAngle.x - mouseDelta.y;

        if (x < 180f) { x = Mathf.Clamp(x, -1f, 70f); }
        else { x = Mathf.Clamp(x, 300f, 361f); }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);

        if (viewpoint_bool == true)
        {
            Camera.fieldOfView = 60f;
            RaycastHit hitinfo;
            if (Physics.Linecast(cameraArm.position, camera.position, out hitinfo))//레이케스트 성공시
            {
                //point로 옮긴다.
                yield return wait;
                camera.position = Vector3.Lerp(camera.position, hitinfo.point, Time.deltaTime * 15);
            }
            else
            {
                yield return wait;
                camera.localPosition = Vector3.Lerp(camera.localPosition, new Vector3(0, 0, -4f).normalized * camera_dstc, Time.deltaTime * 2);
            }
            yield return wait;
        }
        else
        {
            Camera.fieldOfView = 50f;
            camera.position = cameraArm.position + cameraArm.up.normalized * 0.2f + cameraArm.forward.normalized * 0.1f;
        }
    }
    //점프 하지 않았을때 move 함수
    public void Move(Transform cameraArm, Transform characterBody, CharacterController controller, Animator animator, float speed)
    {
        Debug.DrawRay(cameraArm.position, new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized, Color.red);

        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Run");

        

        Vector2 moveInput = new Vector2(hAxis, vAxis);
        bool isMove = moveInput.magnitude != 0;
        //카메라 전면
        Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
        Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
        Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
        Vector3 moveDir_1 = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;

        if (isMove)
        {

            //캐릭터 카메라 주시방향
            if (viewpoint_bool == true)
            {
                characterBody.forward = moveDir;
            }
            else
            {
                characterBody.forward = moveDir_1;
            }
            moveDir = moveDir.normalized * speed * (wDown ? 0.8f : 0.3f);
        }
        animator.SetBool("isWalk", moveInput != Vector2.zero);
        animator.SetBool("isRun", wDown && moveInput != Vector2.zero);
        // 캐릭터에 중력 적용.
        moveDir.y += -gravity * Time.deltaTime;
        if (!jumpStatus)
        {
            gravity = 1000f;
        }
        else
        {
            gravity += 100f;
        }
        // 캐릭터 움직임.
        controller.Move(moveDir * Time.deltaTime);
    }
    public void PointOfView(string[] key_custom_arry)
    {
        if (Input.GetKeyDown(key_custom_arry[0]))
        {
            if(viewpoint_bool == true)
            {
                viewpoint_bool = false;
            }
            else 
            {
                viewpoint_bool = true;
            }
        }
    }
    //점프 가능한 상태인지 판단하는 함수
    public void JumpStatusOn(Transform charactor, LayerMask floor, LayerMask rock)
    {
        RaycastHit hitinfo;
        if (Physics.Raycast(charactor.position + charactor.up * 5f, -charactor.up, out hitinfo, 5.2f, floor))//레이케스트 성공시
        {
            jumpStatus = false;
        }
        else if (Physics.Raycast(charactor.position + charactor.up * 5f, -charactor.up, out hitinfo, 5.2f, rock))
        {
            jumpStatus = false;
        }
        else
        {
            jumpStatus = true;
        }
    }
    //점프 할때 쓰는 move 함수
    public IEnumerator JumpAndMove(Transform cameraArm, Transform characterBody, CharacterController controller, Animator animator, float speed, float jumpPower, WaitForFixedUpdate wait)
    {
        jump = jumpPower;
        if (Input.GetButtonDown("Jump") && !jumpStatus)
        {
            float time = 0f;
            while(time < 0.15f)
            {
                hAxis = Input.GetAxisRaw("Horizontal");
                vAxis = Input.GetAxisRaw("Vertical");
                wDown = Input.GetButton("Run");



                Vector2 moveInput = new Vector2(hAxis, vAxis);
                bool isMove = moveInput.magnitude != 0;
                //카메라 전면
                Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
                Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
                Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
                Vector3 moveDir_1 = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;

                if (isMove)
                {
                    //캐릭터 카메라 주시방향
                    if (viewpoint_bool == true)
                    {
                        characterBody.forward = moveDir;
                    }
                    else
                    {
                        characterBody.forward = moveDir_1;
                    }
                    moveDir = moveDir.normalized * speed / jumpPower;
                }
                moveDir.y += jump * Time.deltaTime;
                if (jumpStatus)
                {
                    jump -= 1f;
                }
                else
                {
                    jump = jumpPower;
                }
                // 캐릭터 움직임.
                controller.Move(moveDir * Time.deltaTime);
                time += Time.deltaTime;
                yield return wait;
            }
        }
    }
}
