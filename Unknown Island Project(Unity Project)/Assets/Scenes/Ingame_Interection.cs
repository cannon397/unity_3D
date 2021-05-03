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

    public Ingame_Interection()
    {

    }

    void Start()
    {

    }

    void Update()
    {
        
    }
    //유저가 나무를 향해 화면을 보고 있는지 판단해서 press E 이미지를 띄우고 그 상태에서 E 누르면 나무를 캐고(비활성화) 하위 아이템을 해당 좌표에 활성화 후 순간이동 시키는 함수
    public void RayCastTree(Transform camera, GameObject press_some_button_image, string[] key_custom_arry, GameObject tree_log, GameObject tree_fruit, LayerMask laymask_tree)
    {
        RaycastHit hitinfo;
        Debug.DrawRay(camera.position, camera.forward.normalized * 8f, Color.red);
        if (Physics.Raycast(camera.position, camera.forward.normalized, out hitinfo, 8f, laymask_tree))
        {
            press_some_button_image.SetActive(true);
            if(Input.GetKeyDown(key_custom_arry[1]))
            {
                tree_log.SetActive(true);
                tree_fruit.SetActive(true);
                tree_log.transform.localPosition = hitinfo.collider.gameObject.transform.localPosition;
                tree_fruit.transform.localPosition = hitinfo.collider.gameObject.transform.localPosition + new Vector3(Random.Range(-1.0f, 1.0f), 1.0f, Random.Range(-1.0f, 1.0f));

                hitinfo.collider.gameObject.SetActive(false);
            }
        }
        else if(press_some_button_image.activeSelf == true)
        {
            press_some_button_image.SetActive(false);
        }
    }
    // 나무 재생성 하는 함수
    public IEnumerator ResetTree(List<GameObject> tree_list)
    {
        for(int i = 0; i < tree_list.Count; i++)
        {
            if (tree_list[i].activeSelf) { }
            else
            {
                yield return new WaitForSeconds(300.0f);//300초 뒤에 나무 생성 한다는 뜻
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
}
