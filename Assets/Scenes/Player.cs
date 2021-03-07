using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Collision collision;
    public float speed;
    public float jumpPower;
    Rigidbody rigid;
    public bool jumpStatus;

    float hAxis;
    float vAxis;
    bool wDown;

    Vector3 moveVec;

    Animator animator;
    [SerializeField]
    private Transform cameraArm;
    [SerializeField]
    private Transform characterBody;
    //// Start is called before the first frame update
    //void Start()
    //{

    //}
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        jumpStatus = false;
        rigid = GetComponentInChildren<Rigidbody>();
        Debug.Log(jumpStatus);

    }
    // Update is called once per frame
    void Update()
    {
        LookAround();
        Move();
        Debug.Log(jumpStatus);
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
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if( x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);

        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
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

        //transform.LookAt(transform.position + moveVec);

        if (Input.GetButtonDown("Jump") && !jumpStatus)
        {
            jumpStatus = true;
            rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }
    }

    public void CollisionFromChild(Collision collision)
    {
       
        Debug.Log("호출여부");
        if (collision.gameObject.tag == "Floor")
        {
            jumpStatus = false;
        }
    }
}
