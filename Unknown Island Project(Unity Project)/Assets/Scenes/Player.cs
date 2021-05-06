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
    
    public Player()
    {
        uki = new UKI_script();
        jumpStatus = false;
    }
    void Awake()
    {
        
    }
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        
        // rigid.AddForce(new Vector3(Input.GetAxis("Horizontal"),
        //0,
        //Input.GetAxis("Vertical")), ForceMode.Impulse);


    }
    void OnCollisionEnter(Collision collision)
    {



    }
    public void LookAround(Transform cameraArm, Transform camera, float camera_dstc, float mouse_dpi)
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X") * mouse_dpi, Input.GetAxis("Mouse Y") * mouse_dpi);
        Vector3 camAngle = cameraArm.rotation.eulerAngles;

        float x = camAngle.x - mouseDelta.y;

        if (x < 180f) {x = Mathf.Clamp(x, -1f, 70f);}
        else {x = Mathf.Clamp(x, 290f, 361f);}

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);

        RaycastHit hitinfo;
        if (Physics.Linecast(cameraArm.position, camera.position, out hitinfo))//레이케스트 성공시
        {
            //point로 옮긴다.
            camera.transform.position = hitinfo.point;
        }
        else
        {
            camera.localPosition = Vector3.Lerp(camera.localPosition, new Vector3(0, -1f, -4f).normalized * camera_dstc, Time.deltaTime * 3);
        }

    }
    //점프 하지 않았을때 move 함수
    public void Move(Transform cameraArm, Transform characterBody, CharacterController controller, Animator animator, float speed, Rigidbody rigid, float jumpPower, LayerMask floor)
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
        if (isMove)
        {

            //캐릭터 카메라 주시방향
            characterBody.forward = moveDir;
            moveDir = moveDir.normalized * speed * (wDown ? 1f : 0.3f);
        }
        animator.SetBool("isWalk", moveInput != Vector2.zero);
        animator.SetBool("isRun", wDown);
        // 캐릭터에 중력 적용.
        moveDir.y -= 2000f * Time.deltaTime;
        // 캐릭터 움직임.
        controller.Move(moveDir * Time.deltaTime);
    }
    void LateUpdate()
    {
        
    }
    private void PointOfView(bool viewPoint, string[] key_custom_arry, int viewPointFlag)
    {
        viewPoint = Input.GetButtonDown(key_custom_arry[0]);
        if (viewPoint)
        {
            if(viewPointFlag == 1)
            {
                
                viewPointFlag = 0;
            }
            else 
            {
                viewPointFlag = 1;
            }
            //viewPointFlag == 1 ? viewPointFlag = 0 : viewPointFlag = 1;
            
            Debug.Log("v 누름");
            Debug.Log(viewPointFlag);
        }
    }
    //점프 가능한 상태인지 판단하는 함수
    public void JumpStatusOn(Transform charactor, LayerMask floor)
    {
        RaycastHit hitinfo;
        Debug.DrawRay(charactor.position, -charactor.up * 1.5f, Color.yellow);
        if (Physics.Raycast(charactor.position, -charactor.up, out hitinfo, 1.5f, floor))//레이케스트 성공시
        {
            jumpStatus = false;
        }
        else
        {
            jumpStatus = true;
        }
    }
    //점프 할때 쓰는 move 함수
    public IEnumerator JumpAndMove(Transform cameraArm, Transform characterBody, CharacterController controller, Animator animator, float speed, Rigidbody rigid, float jumpPower, LayerMask floor, float jumpheight)
    {
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

                if (isMove)
                {
                    //캐릭터 카메라 주시방향
                    characterBody.forward = moveDir;
                    moveDir = moveDir.normalized * speed * (wDown ? 1f : 0.3f);
                }
                moveDir.y += jumpPower;
                // 캐릭터 움직임.
                controller.Move(moveDir * Time.deltaTime);
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}
