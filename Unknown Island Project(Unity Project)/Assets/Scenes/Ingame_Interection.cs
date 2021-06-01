using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using Mono.Data.Sqlite;
using Assets.Scenes;

public class Ingame_Interection : MonoBehaviour
{
    static private List<GameObject> tree_log_list;
    static private List<GameObject> tree_fruit_list;
    static private List<GameObject> fishtrap_list;
    static private List<int> fishtrap_waitcount_list;//통발이 기다린 시간에 따라 +1 씩 증가하니까 그에 따른 물고기 양도 확률 적으로 증가하게 하셈
    static private List<bool> fishtrap_wait_bool_list;
    private Animator animator;
    static private bool fishtrap_count_already;

    private float time;

    public GameObject fishtrap_text;
    public GameObject setting_point;
    public GameObject fishtrap_overlap;

    public static GameObject press_tree_image;
    public static GameObject press_treeitem_image;
    public static GameObject press_water_image;
    public static GameObject press_fishtrap_image;
    public static GameObject press_rock_image;
    public static GameObject press_stone_image;

    private UKI_script uki;
    public Ingame_Interection()
    {
        tree_log_list = new List<GameObject>();
        tree_fruit_list = new List<GameObject>();
        fishtrap_list = new List<GameObject>();
        fishtrap_waitcount_list = new List<int>();
        fishtrap_wait_bool_list = new List<bool>();

        animator = GameObject.Find("Player").GetComponentInChildren<Animator>();

        press_tree_image = GameObject.Find("Press_Tree_Image");
        press_tree_image.SetActive(false);
        press_treeitem_image = GameObject.Find("Press_TreeItem_Image");
        press_treeitem_image.SetActive(false);
        press_water_image = GameObject.Find("Press_Water_Image");
        press_water_image.SetActive(false);
        press_fishtrap_image = GameObject.Find("Press_Fishtrap_Image");
        press_fishtrap_image.SetActive(false);
        press_rock_image = GameObject.Find("Press_Rock_Image");
        press_rock_image.SetActive(false);
        press_stone_image = GameObject.Find("Press_Stone_Image");
        press_stone_image.SetActive(false);

        fishtrap_text = GameObject.Find("Fishtrap_isFull_Text");
        fishtrap_text.SetActive(false);
        setting_point = GameObject.Find("Setting_point");
        fishtrap_overlap = GameObject.Find("Fish_trap_overlap");
        fishtrap_overlap.SetActive(false);

        fishtrap_count_already = false;

        uki = new UKI_script();
    }

    void Start()
    {

    }

    void Update()
    {

    }
    ///<summary>
    ///유저가 나무를 향해 화면을 보고 있는지 판단해서 press E 이미지를 띄우고 그 상태에서 E 누르면 나무를 캐고(비활성화) 하위 아이템을 해당 좌표에 활성화 후 순간이동 시키는 함수
    ///</summary>
    public void RayCastTree(Transform charactor, GameObject tree_log, GameObject tree_fruit, LayerMask laymask_tree)
    {
        //if(도끼를 장착 했는지 확인)
        RaycastHit hitinfo;
        if (Physics.SphereCast(charactor.position - charactor.forward, 1f, charactor.forward, out hitinfo, 2f, laymask_tree))
        {
            press_tree_image.SetActive(true);
            if (Input.GetButtonDown("Fire1"))
            {
                //나무 패는 에니메이션
                animator.SetTrigger("doLogging");
                TreeItemCreate(hitinfo.collider.gameObject.transform, tree_log, tree_fruit);


                hitinfo.collider.gameObject.SetActive(false);
            }
        }
        else if(press_tree_image.activeSelf)
        {
            press_tree_image.SetActive(false);
        }
    }
    ///<summary>
    ///나무 재생성 하는 함수
    ///</summary>
    public IEnumerator ResetTree(List<GameObject> tree_list, WaitForSeconds wait, WaitForFixedUpdate wait_fix)
    {
        while (true)
        {
            yield return wait_fix;
            int i = UnityEngine.Random.Range(0, tree_list.Count);
            if (tree_list[i].activeSelf) { }
            else
            {
                yield return wait;
                if (CheckScreenOut(tree_list[i]))
                {
                    tree_list[i].transform.localScale = new Vector3(0, 0, 0);
                    tree_list[i].SetActive(true);
                    time = 0f;
                    while (time < 3f)
                    {
                        yield return wait_fix;
                        tree_list[i].transform.localScale += new Vector3(Time.deltaTime/3, Time.deltaTime/3, Time.deltaTime/3);
                        time += Time.deltaTime;
                    }
                }
                else
                {
                    tree_list[i].SetActive(true);
                }
            }
        }
    }
    ///<summary>
    ///나무가 카메라에 보이는지 확인 하는 함수 보이면 true
    ///</summary>
    private bool CheckScreenOut(GameObject tree)
    {
        Vector3 targetScreenPos = Camera.main.WorldToViewportPoint(tree.transform.position);
        if (targetScreenPos.z >0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    ///<summary>
    ///나무 하위 아이템 생성 하는 함수
    ///</summary>
    private void TreeItemCreate(Transform target, GameObject tree_log, GameObject tree_fruit)
    {
        GameObject log = Instantiate(tree_log, target.position + new Vector3(0, 2.5f, 0), Quaternion.identity);
        GameObject fruit = Instantiate(tree_fruit, target.position + new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 3.0f, UnityEngine.Random.Range(-1.0f, 1.0f)), Quaternion.identity);
        tree_log_list.Add(log);
        tree_fruit_list.Add(fruit);
        Destroy(log, 30f);
        Destroy(fruit, 30f);
    }
    ///<summary>
    ///물인지 판단해서 통발 설치 및 회수
    ///</summary>
    public void RayCastWaterFishTrap(Transform charactor, GameObject fish_trap, LayerMask laymask_water)
    {
        RaycastHit hitinfo_trap;
        //통발 설치
        if (!press_fishtrap_image.activeSelf)
        {
            //if(통발을 장착 했는지 확인)
            if (Physics.Raycast(charactor.position + charactor.up * 5f, -charactor.up, 5.2f, laymask_water))
            {
                press_water_image.SetActive(true);
                if (fishtrap_list.Count < 4)
                {
                    fishtrap_overlap.SetActive(true);
                    fishtrap_overlap.transform.position = setting_point.transform.position;
                }
                else
                {
                    fishtrap_overlap.SetActive(false);
                }
                if (Input.GetButtonDown("Fire1"))
                {
                    if (fishtrap_list.Count < 4)
                    {
                        fishtrap_overlap.SetActive(false);
                        FishTrapCreate(setting_point.transform.position, fish_trap);
                    }
                    else
                    {
                        if (!fishtrap_text.activeSelf)
                        {
                            fishtrap_count_already = true;
                        }
                    }
                }
            }
            else if (press_water_image.activeSelf)
            {
                press_water_image.SetActive(false);
                fishtrap_overlap.SetActive(false);
            }
        }
    }
    ///<summary>
    ///통발 최대 갯수 텍스트 체크 해서 시간 지나면 해제 하는 함수
    ///</summary>
    public IEnumerator FishtrapIsfull(WaitForSeconds wait, WaitForFixedUpdate wait_fix)
    {
        yield return wait_fix;
        if (fishtrap_count_already)
        {
            fishtrap_count_already = false;
            fishtrap_text.SetActive(true);
            yield return wait;
            fishtrap_text.SetActive(false);
        }
    }
    ///<summary>
    ///통발 클론 생성 함수
    ///</summary>
    private void FishTrapCreate(Vector3 tr_point, GameObject fish_trap)
    {
        GameObject ft = Instantiate(fish_trap, tr_point, Quaternion.identity);
        fishtrap_list.Add(ft);
        fishtrap_waitcount_list.Add(0);
        fishtrap_wait_bool_list.Add(false);
    }
    ///<summary>
    ///통발 설치 한지 얼마나 지났는지 확인 해주는 함수
    ///</summary>
    public IEnumerator CountFishTrap(WaitForSeconds wait, AsyncOperation wait_settingtrap, int i)
    {
        yield return wait_settingtrap;
        if (fishtrap_list.Count > 0)
        {
            i = UnityEngine.Random.Range(0, fishtrap_list.Count);
            if (fishtrap_list[i] == null) { }
            else if(fishtrap_wait_bool_list[i] == false)
            {
                fishtrap_wait_bool_list[i] = true;
                yield return wait;
                try
                {
                    fishtrap_waitcount_list[i]++;
                    fishtrap_wait_bool_list[i] = false;
                }
                catch(Exception ex)
                {

                }
            }
        }
    }
    ///<summary>
    ///돌 캐는 함수
    ///</summary>
    public void RayCastRock(Transform charactor, GameObject rock_stone, LayerMask laymask_rock)
    {
        //if(곡괭이를 장착 했는지 확인)
        RaycastHit hitinfo;
        if (Physics.SphereCast(charactor.position - charactor.forward, 1f, charactor.forward, out hitinfo, 2f, laymask_rock))
        {
            press_rock_image.SetActive(true);
            if (Input.GetButtonDown("Fire1"))
            {
                //곡괭이질 애니메이션
                animator.SetTrigger("doMining");
                RockItemCreate(hitinfo.point, charactor.position - hitinfo.point + charactor.up, rock_stone);
            }
        }
        else if (press_rock_image.activeSelf)
        {
            press_rock_image.SetActive(false);
        }
    }
    ///<summary>
    ///돌 하위 아이템 생성 함수
    ///</summary>
    private void RockItemCreate(Vector3 item_point, Vector3  tr_point, GameObject rock_stone)
    {
        Destroy(Instantiate(rock_stone, item_point + tr_point * 0.3f, Quaternion.identity), 30f);
    }
    /// <summary>
    /// 아이템 줍기 함수
    /// </summary>
    /// <param name="charactor">Player Transform</param>
    /// <param name="layermask">PickUp_Item LayerMask</param>
    public void PickUpItem(Transform charactor, LayerMask layermask, string[] key_custom_arry)
    {
        RaycastHit hitinfo;
        if (Physics.SphereCast(charactor.position - charactor.forward, 1f, charactor.forward, out hitinfo, 2f, layermask))
        {//여기는 해당 아이템을 집는 기능 구현 부분
            if(hitinfo.collider.gameObject.tag == "10001")//도끼
            {

            }
            else if (hitinfo.collider.gameObject.tag == "10002")//곡괭이
            {

            }
            else if (hitinfo.collider.gameObject.tag == "10003")//창
            {

            }
            else if (hitinfo.collider.gameObject.tag == "10004")//낚시대
            {

            }
            else if (hitinfo.collider.gameObject.tag == "10005")//통발
            {
                press_fishtrap_image.SetActive(true);
                press_water_image.SetActive(false);
                fishtrap_overlap.SetActive(false);
                if (Input.GetKeyDown(key_custom_arry[1]))
                {
                    //통발 줍는 애니메이션
                    animator.SetTrigger("doPickup");
                    //인벤토리에 통발과 물고기 저장
                    int rnd = UnityEngine.Random.Range(1, 101);
                    switch (fishtrap_waitcount_list[fishtrap_list.IndexOf(hitinfo.collider.gameObject)])
                    {//통발 시간에 따른 물고기 회수량 확률 적용
                        case 0:
                            if (rnd < 95)
                            {
                                //물고기 0개
                            }
                            else
                            {
                                //물고기 1개
                            }
                            break;
                        case 1:
                            if (rnd < 50)
                            {
                                //물고기 1개
                            }
                            else
                            {
                                //물고기 2개
                            }
                            break;
                        case 2:
                            if (rnd < 15)
                            {
                                //물고기 1개
                            }
                            else if (rnd < 50)
                            {
                                //물고기 2개
                            }
                            else
                            {
                                //물고기 3개
                            }
                            break;
                        case 3:
                            if (rnd < 5)
                            {
                                //물고기 1개
                            }
                            else if (rnd < 30)
                            {
                                //물고기 2개
                            }
                            else if (rnd < 60)
                            {
                                //물고기 3개
                            }
                            else
                            {
                                //물고기 4개
                            }
                            break;
                        case 4:
                            if (rnd < 5)
                            {
                                //물고기 2개
                            }
                            else if (rnd < 30)
                            {
                                //물고기 3개
                            }
                            else if (rnd < 60)
                            {
                                //물고기 4개
                            }
                            else
                            {
                                //물고기 5개
                            }
                            break;
                        default:
                            if (rnd < 5)
                            {
                                //물고기 3개
                            }
                            else if (rnd < 30)
                            {
                                //물고기 4개
                            }
                            else if (rnd < 60)
                            {
                                //물고기 5개
                            }
                            else
                            {
                                //물고기 6개
                            }
                            break;
                    }
                    fishtrap_waitcount_list.RemoveAt(fishtrap_list.IndexOf(hitinfo.collider.gameObject));
                    fishtrap_wait_bool_list.RemoveAt(fishtrap_list.IndexOf(hitinfo.collider.gameObject));
                    fishtrap_list.Remove(hitinfo.collider.gameObject);
                    Destroy(hitinfo.collider.gameObject);
                }
            }
            else if (hitinfo.collider.gameObject.tag == "10006")//코코넛 그릇
            {

            }
            else if (hitinfo.collider.gameObject.tag == "10007")//횃불
            {

            }
            else if (hitinfo.collider.gameObject.tag == "10008")//가방
            {

            }
            else if (hitinfo.collider.gameObject.tag == "20001")//해독제
            {

            }
            else if (hitinfo.collider.gameObject.tag == "20002")//상처약
            {

            }
            else if (hitinfo.collider.gameObject.tag == "20003")//코코넛
            {
                press_treeitem_image.SetActive(true);
                if (Input.GetKeyDown(key_custom_arry[1]))
                {
                    //줍는 에니메이션
                    animator.SetTrigger("doPickup");
                    //인벤토리 데이터 베이스에 코코넛 저장
                    Destroy(hitinfo.collider.gameObject);
                }
            }
            else if (hitinfo.collider.gameObject.tag == "20004")//정어리
            {

            }
            else if (hitinfo.collider.gameObject.tag == "20005")//물이담긴 나무그릇
            {

            }
            else if (hitinfo.collider.gameObject.tag == "20006")//복어
            {

            }
            else if (hitinfo.collider.gameObject.tag == "20007")//넙치
            {

            }
            else if (hitinfo.collider.gameObject.tag == "20008")//도미
            {

            }
            else if (hitinfo.collider.gameObject.tag == "20009")//다랑어
            {

            }
            else if (hitinfo.collider.gameObject.tag == "20010")//구운 정어리
            {

            }
            else if (hitinfo.collider.gameObject.tag == "20011")//구운 복어
            {

            }
            else if (hitinfo.collider.gameObject.tag == "20012")//구운 넙치
            {

            }
            else if (hitinfo.collider.gameObject.tag == "20013")//구운 도미
            {

            }
            else if (hitinfo.collider.gameObject.tag == "20014")//구운 다랑어
            {

            }
            else if (hitinfo.collider.gameObject.tag == "30001")//통나무
            {

                press_treeitem_image.SetActive(true);
                if (Input.GetKeyDown(key_custom_arry[1]))
                {
                    //줍는 에니메이션
                    animator.SetTrigger("doPickup");
                    //인벤토리 데이터 베이스에 통나무 저장
                    Destroy(hitinfo.collider.gameObject);
                }
            }
            else if (hitinfo.collider.gameObject.tag == "30002")//덩굴
            {

            }
            else if (hitinfo.collider.gameObject.tag == "30003")//돌
            {
                press_stone_image.SetActive(true);
                if (Input.GetKeyDown(key_custom_arry[1]))
                {
                    //줍는 애니메이션 및 줍기 구현
                    animator.SetTrigger("doPickup");
                    //인벤토리에 돌 저장
                    Destroy(hitinfo.collider.gameObject);
                }
            }
            else if (hitinfo.collider.gameObject.tag == "30004")//꽃
            {

            }
            else if (hitinfo.collider.gameObject.tag == "30005")//나뭇가지
            {

            }
            else if (hitinfo.collider.gameObject.tag == "30006")//야자수 잎
            {

            }
            else if (hitinfo.collider.gameObject.tag == "40001")//상자
            {

            }
            else if (hitinfo.collider.gameObject.tag == "40002")//집
            {

            }
            else if (hitinfo.collider.gameObject.tag == "40003")//모닥불
            {

            }
        }
        else
        {//여기는 줍기 UI, 애니메이션 Bool 총괄로 꺼주는 파트
            press_treeitem_image.SetActive(false);
            press_fishtrap_image.SetActive(false);
            press_stone_image.SetActive(false);
        }
    }
}
