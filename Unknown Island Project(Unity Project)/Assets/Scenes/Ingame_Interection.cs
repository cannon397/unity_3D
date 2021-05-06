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
    public Ingame_Interection()
    {
        tree_log_list = new List<GameObject>();
        tree_fruit_list = new List<GameObject>();
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
        Debug.DrawRay(camera.position, camera.forward.normalized * 8f, Color.red);
        if (Physics.Raycast(camera.position, camera.forward.normalized, out hitinfo, 8f, laymask_tree))
        {
            press_tree_image.SetActive(true);
            if(Input.GetKeyDown(key_custom_arry[1]))
            {
                //나무 패는 에니메이션
                tree_log.SetActive(true);
                tree_fruit.SetActive(true);
                tree_log.transform.localPosition = hitinfo.collider.gameObject.transform.localPosition;
                tree_fruit.transform.localPosition = hitinfo.collider.gameObject.transform.localPosition + new Vector3(Random.Range(-1.0f, 1.0f), 1.0f, Random.Range(-1.0f, 1.0f));
                TreeItemCreate(hitinfo.collider.gameObject.transform, tree_log, tree_fruit);
                tree_log.SetActive(false);
                tree_fruit.SetActive(false);


                hitinfo.collider.gameObject.SetActive(false);
            }
        }
        else if(press_tree_image.activeSelf == true)
        {
            press_tree_image.SetActive(false);
        }
    }
    //유저가 나무하위 아이템을 향해 화면을 보고 있는지 판단해서 press E 이미지를 띄우고 그 상태에서 E 누르면 아이템을 줍고(비활성화) 아이템을 저장하는 함수
    public void TayCastTreeItem(Transform camera, GameObject press_treeitem_image, string[] key_custom_arry, LayerMask laymask_tree_item)
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
                }
                else if (hitinfo.collider.gameObject.tag == "Tree_Fruit")
                {
                    //줍는 에니메이션
                    //인벤토리 데이터 베이스에 과일 저장
                }
                hitinfo.collider.gameObject.SetActive(false);
            }
        }
        else if (press_treeitem_image.activeSelf == true)
        {
            press_treeitem_image.SetActive(false);
        }
    }
    // 나무 재생성 하는 함수
    public IEnumerator ResetTree(List<GameObject> tree_list, WaitForSeconds wait, int i)
    {
        i = Random.Range(0, tree_list.Count);
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
        GameObject fruit = Instantiate(tree_fruit, target.position, Quaternion.identity);
        tree_log_list.Add(log);
        tree_fruit_list.Add(fruit);
        Destroy(log, 30f);
        Destroy(fruit, 30f);
    }
}
