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
    private UKI_script uki;
    public Ingame_Interection()
    {
        tree_log_list = new List<GameObject>();
        tree_fruit_list = new List<GameObject>();
        fishtrap_list = new List<GameObject>();
        fishtrap_waitcount_list = new List<int>();
        fishtrap_wait_bool_list = new List<bool>();
        uki = new UKI_script();
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
    //유저가 나무를 향해 화면을 보고 있는지 판단해서 press E 이미지를 띄우고 그 상태에서 E 누르면 나무를 캐고(비활성화) 하위 아이템을 해당 좌표에 활성화 후 순간이동 시키는 함수
    public void RayCastTree(Transform camera, GameObject press_tree_image, string[] key_custom_arry, GameObject tree_log, GameObject tree_fruit, LayerMask laymask_tree)
    {
        RaycastHit hitinfo;
        if (Physics.Raycast(camera.position, camera.forward.normalized, out hitinfo, 8f, laymask_tree))
        {
            //if(도끼를 장착 했는지 확인)
            {
                press_tree_image.SetActive(true);
                if (Input.GetKeyDown(key_custom_arry[1]))
                {
                    //나무 패는 에니메이션
                    TreeItemCreate(hitinfo.collider.gameObject.transform, tree_log, tree_fruit);


                    hitinfo.collider.gameObject.SetActive(false);
                }
            }
        }
        else if(press_tree_image.activeSelf)
        {
            press_tree_image.SetActive(false);
        }
    }
    //유저가 나무하위 아이템을 향해 화면을 보고 있는지 판단해서 press E 이미지를 띄우고 그 상태에서 E 누르면 아이템을 줍고(비활성화) 아이템을 저장하는 함수
    public void RayCastTreeItem(Transform camera, GameObject press_treeitem_image, string[] key_custom_arry, LayerMask laymask_tree_item)
    {
        RaycastHit hitinfo;
        if (Physics.Raycast(camera.position, camera.forward.normalized, out hitinfo, 8f, laymask_tree_item))
        {
            press_treeitem_image.SetActive(true);
            if (Input.GetKeyDown(key_custom_arry[1]))
            {
                if(hitinfo.collider.gameObject.tag == "Tree_Log")
                {
                    //줍는 에니메이션
                    //인벤토리 데이터 베이스에 통나무 저장
                    Destroy(hitinfo.collider.gameObject);
                }
                else if (hitinfo.collider.gameObject.tag == "Tree_Fruit")
                {
                    //줍는 에니메이션
                    //인벤토리 데이터 베이스에 과일 저장
                    Destroy(hitinfo.collider.gameObject);
                }
                hitinfo.collider.gameObject.SetActive(false);
            }
        }
        else if (press_treeitem_image.activeSelf)
        {
            press_treeitem_image.SetActive(false);
        }
    }
    // 나무 재생성 하는 함수
    public IEnumerator ResetTree(List<GameObject> tree_list, WaitForSeconds wait, int i)
    {
        i = UnityEngine.Random.Range(0, tree_list.Count);
        if (tree_list[i].activeSelf) { }
        else
        {
            yield return wait;
            if (CheckScreenOut(tree_list[i]))
            {
                //나무가 크는 에니메이션 넣고
                tree_list[i].SetActive(true);
            }
            else
            {
                tree_list[i].SetActive(true);
            }
        }
    }
    // 나무가 카메라에 보이는지 확인 하는 함수 보이면 true
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
    //나무 하위 아이템 생성 하는 함수
    private void TreeItemCreate(Transform target, GameObject tree_log, GameObject tree_fruit)
    {
        GameObject log = Instantiate(tree_log, target.position, Quaternion.identity);
        GameObject fruit = Instantiate(tree_fruit, target.position + new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 2.0f, UnityEngine.Random.Range(-1.0f, 1.0f)), Quaternion.identity);
        tree_log_list.Add(log);
        tree_fruit_list.Add(fruit);
        Destroy(log, 30f);
        Destroy(fruit, 30f);
    }
    //물인지 판단해서 통발 설치 및 회수
    public void RayCastWaterFishTrap(Transform camera, GameObject press_water_image, GameObject press_fishtrap_image, string[] key_custom_arry, GameObject fish_trap, LayerMask laymask_water, LayerMask laymask_fishtrap)
    {
        RaycastHit hitinfo_trap;
        RaycastHit hitinfo_water;
        //통발 회수
        if (Physics.Raycast(camera.position, camera.forward.normalized, out hitinfo_trap, 8f, laymask_fishtrap))
        {
            press_fishtrap_image.SetActive(true);
            if (Input.GetKeyDown(key_custom_arry[1]))
            {
                //통발 줍는 애니메이션
                //인벤토리에 통발과 물고기 저장
                //fishtrap_waitcount_list[fishtrap_list.IndexOf(hitinfo_trap.collider.gameObject)] 현재 회수한 통발의 기다린 시간에 따른 카운트 값
                fishtrap_waitcount_list.RemoveAt(fishtrap_list.IndexOf(hitinfo_trap.collider.gameObject));
                fishtrap_wait_bool_list.RemoveAt(fishtrap_list.IndexOf(hitinfo_trap.collider.gameObject));
                fishtrap_list.Remove(hitinfo_trap.collider.gameObject);
                Destroy(hitinfo_trap.collider.gameObject);
            }
        }
        else if (press_fishtrap_image.activeSelf)
        {
            press_fishtrap_image.SetActive(false);
        }
        //통발 설치
        else if (Physics.Raycast(camera.position, camera.forward.normalized, out hitinfo_water, 8f, laymask_water))
        {
            //if(통발을 장착 했는지 확인)
            {
                press_water_image.SetActive(true);
                if (Input.GetKeyDown(key_custom_arry[1]))
                {
                    FishTrapCreate(hitinfo_water.point, fish_trap);
                }
            }
        }
        else if (press_water_image.activeSelf)
        {
            press_water_image.SetActive(false);
        }
    }
    //통발 클론 생성 함수
    private void FishTrapCreate(Vector3 tr_point, GameObject fish_trap)
    {
        GameObject ft = Instantiate(fish_trap, tr_point, Quaternion.identity);
        fishtrap_list.Add(ft);
        fishtrap_waitcount_list.Add(0);
        fishtrap_wait_bool_list.Add(false);
    }
    //통발 설치 한지 얼마나 지났는지 확인 해주는 함수
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
    //돌 캐는 함수
    public void RayCastRock(Transform camera, GameObject press_rock_image, string[] key_custom_arry, GameObject rock_stone, LayerMask laymask_rock)
    {
        RaycastHit hitinfo;
        if (Physics.Raycast(camera.position, camera.forward.normalized, out hitinfo, 8f, laymask_rock))
        {
            //if(곡괭이를 장착 했는지 확인)
            {
                press_rock_image.SetActive(true);
                if (Input.GetKeyDown(key_custom_arry[1]))
                {
                    //나무 패는 에니메이션
                    RockItemCreate(camera, hitinfo.point, hitinfo.collider.gameObject.transform.position, rock_stone);
                }
            }
        }
        else if (press_rock_image.activeSelf)
        {
            press_rock_image.SetActive(false);
        }
    }
    private void RockItemCreate(Transform camera, Vector3 item_point, Vector3  tr_point, GameObject rock_stone)
    {
        Instantiate(rock_stone, item_point - tr_point.normalized, Quaternion.identity);
    }
}
