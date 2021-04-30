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
    public GameObject tree;
    public GameObject press_some_button_image;
    public GameObject tree_log;
    public GameObject tree_fruit;

    [SerializeField]
    private new Transform camera;

    public LayerMask LayerMask;

    private string[] key_custom_arry;
    private DBAccess db;
    private Setting_header sh;

    void Start()
    {
        tree = GameObject.FindWithTag("Tree");
        press_some_button_image = GameObject.FindWithTag("Press_Button_Image");
        press_some_button_image.SetActive(false);
        tree_log = GameObject.FindWithTag("Tree_Log");
        tree_log.SetActive(false);
        tree_fruit = GameObject.FindWithTag("Tree_Fruit");
        tree_fruit.SetActive(false);

        db = new DBAccess();
        sh = new Setting_header(db);
        key_custom_arry = sh.SyncKeyCustom();
    }

    void Update()
    {
        RayCastTree();
    }

    private void RayCastTree()
    {
        RaycastHit hitinfo;
        Debug.DrawRay(camera.position, camera.forward.normalized * 8f, Color.red);
        if (Physics.Raycast(camera.position, camera.forward.normalized, out hitinfo, 8f, LayerMask))
        {
            press_some_button_image.SetActive(true);
            if(Input.GetKeyDown(key_custom_arry[1]))
            {
                tree_log.SetActive(true);
                tree_fruit.SetActive(true);
                //transform_tree_log.position = transform_tree.position;
                //transform_tree_fruit.position = transform_tree.position + new Vector3(Random.Range(-1.0f, 1.0f), 1.0f, Random.Range(-1.0f, 1.0f));
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
