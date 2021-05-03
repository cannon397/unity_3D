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

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void RayCastTree(Transform camera, GameObject press_some_button_image, string[] key_custom_arry, GameObject tree, GameObject tree_log, GameObject tree_fruit, LayerMask laymask_tree)
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
                tree_log.transform.localPosition = tree.transform.localPosition;
                tree_fruit.transform.localPosition = tree.transform.localPosition + new Vector3(Random.Range(-1.0f, 1.0f), 1.0f, Random.Range(-1.0f, 1.0f));
                tree.SetActive(false);
            }
        }
        else if(press_some_button_image.activeSelf == true)
        {
            press_some_button_image.SetActive(false);
        }
    }
}
