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
  
    Collision collision;
    public float speed;
    public float jumpPower;
    Rigidbody rigid;
    public bool jumpStatus;
    public Transform characterTrasform;
    private float camera_dstc;
    DBAccess db;
    Setting_header sh;
    //키보드
    float hAxis;
    float vAxis;
    bool wDown;
    bool viewPoint;

    //flag
    // 1 = 3인칭 0 = 1인칭
    int viewPointFlag = 1;



    Vector3 moveVec;

    Animator animator;
    [SerializeField]
    private Transform cameraArm;
    [SerializeField]
    private Transform characterBody;
    [SerializeField]
    private Transform camera;
 
    //// Start is called before the first frame update
    //void Start()
    //{

    //}
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        jumpStatus = false;
        rigid = GetComponentInChildren<Rigidbody>();
        //Debug.Log(jumpStatus);
        camera_dstc = Mathf.Sqrt(4 * 4 + 10 * 10);
    }
     void Start()
    {
        db = new DBAccess();
        sh = new Setting_header(db);
        SqliteDataReader m_Reader = db.ReadFullTable("settings");
        while (m_Reader.Read()) {
            Debug.Log("no : "+ m_Reader["no"] + "monitor_dropdown_value : " + m_Reader["monitor_dropdown_value"] + "fullscreen : " + m_Reader["fullscreen"]);
        }

        db.CloseSqlConnection();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 300;
    }
    // Update is called once per frame
    void Update()
    {
        LookAround();
        Move();
        PointOfView();
        //Debug.Log(jumpStatus);

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
    private void LookAround()
    {
        //지금 당장은 sh 안에 값들이 제대로 지정 되지 않아서 NULL이 뜨기 때문에 적용 하려면 DB 연동 필요함
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X") * sh.GetMouseDpi(), Input.GetAxis("Mouse Y") * sh.GetMouseDpi());
        //Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
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
            camera.localPosition = Vector3.Lerp(camera.localPosition, new Vector3(0, 4f, -10f).normalized * camera_dstc, Time.deltaTime * 3);
        }

    }
    private void Move()
    {
        Debug.DrawRay(cameraArm.position, new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized, Color.red);

        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Run");

        

        Vector2 moveInput = new Vector2(hAxis, vAxis);
        bool isMove = moveInput.magnitude != 0;
        if (isMove)
        {
            //카메라 전면
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            //캐릭터 카메라 주시방향
            characterBody.forward = moveDir;
            transform.position += moveDir * speed * (wDown ? 1f : 0.3f) * Time.deltaTime;
        }

        //moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        //transform.position += moveVec * speed * (wDown ? 1f : 0.3f) * Time.deltaTime;

        animator.SetBool("isWalk", moveInput != Vector2.zero);
        animator.SetBool("isRun", wDown);
        //카메라
        
        //transform.LookAt(transform.position + moveVec);

        if (Input.GetButtonDown("Jump") && !jumpStatus)
        {
            jumpStatus = true;
            rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }
    }
    //충돌
    public void CollisionFromChild(Collision collision)
    {
       
        //Debug.Log("호출여부");
        if (collision.gameObject.tag == "Floor")
        {
            jumpStatus = false;
        }
    }
    void LateUpdate()
    {
        //cameraArm.position = characterBody.position;
        Vector3 vector = characterBody.position;
        vector.y = characterBody.position.y + 2.5f;
        if(viewPointFlag == 1)
        {
            cameraArm.position = vector;
        }
        else
        {
            camera.position = characterBody.position;
            
        }
        
        //Debug.Log("카메라"+cameraArm.position);
        //Debug.Log("캐릭터"+ characterBody.position);
    }
    private void PointOfView()
    {
        viewPoint = Input.GetButtonDown(sh.GetKeyBiding(0));
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

}
