using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using Mono.Data.Sqlite;
using Assets.Scenes;

public class UKI_script : MonoBehaviour
{
    public GameObject tree;
    public GameObject press_tree_image;
    public GameObject press_treeitem_image;
    public GameObject tree_log;
    public GameObject tree_fruit;
    public GameObject pause_panel;
    public GameObject setting_panel;
    public Slider mousedpi_slider;
    public Slider gamemaster_sound_slider;
    public AudioMixer master_mixer;
    public GameObject keycustom_panel;
    public GameObject keycustom_check_panel;
    public Button key_custom_save_button;
    public Button save_setting_value_button;

    Animator animator;
    //public Transform characterTrasform;
    Rigidbody rigid;
    [SerializeField]
    private new Transform camera; 
    [SerializeField]
    private Transform cameraArm;
    [SerializeField]
    private Transform characterBody;
    private CharacterController controller;

    public LayerMask laymask_tree;
    public LayerMask laymask_tree_item;
    public LayerMask laymask_floor;

    public float speed;
    public float jumpPower;
    public float jumpheight;
    private float camera_dstc;
    private static bool game_puase_bool;
    public static bool jumpStatus;
    public List<GameObject> tree_list;
    bool viewPoint;
    private float mouse_dpi;
    //flag
    // 1 = 3인칭 0 = 1인칭
    int viewPointFlag = 1;
    private string[] key_custom_arry;
    private static int key_adr; 
    public void SetKeyADR(int i) { key_adr = i; }
    static WaitForSeconds wait;
    private DBAccess db;
    private Setting_header sh;
    private Player pl;
    private Ingame_Interection ii;
    private pause_menu pm;
    private Tree_Dispancer td;
    void Awake()
    {
        animator = GameObject.Find("Player").GetComponentInChildren<Animator>();
        controller = GameObject.Find("Player").GetComponentInChildren<CharacterController>();
        camera_dstc = Mathf.Sqrt(4 * 4);
        game_puase_bool = false;
    }
    void Start()
    {
        db = new DBAccess();
        sh = new Setting_header(db);
        ii = new Ingame_Interection();
        pl = new Player();
        pm = new pause_menu();
        td = new Tree_Dispancer();

        key_adr = 9999;

        tree_list = new List<GameObject>();
        tree_list = td.TreeDispanceList();
        press_tree_image = GameObject.Find("Press_Tree_Image");
        press_tree_image.SetActive(false);
        press_treeitem_image = GameObject.Find("Press_TreeItem_Image");
        press_treeitem_image.SetActive(false);
        tree_log = GameObject.FindWithTag("Tree_Log");
        tree_log.SetActive(false);
        tree_fruit = GameObject.FindWithTag("Tree_Fruit");
        tree_fruit.SetActive(false);
        pause_panel = GameObject.Find("Pause_Panel");
        pause_panel.SetActive(false);
        setting_panel = GameObject.Find("Setting_Panel");
        setting_panel.SetActive(false);
        keycustom_check_panel = GameObject.Find("KeyCustomCheck_Panel");
        keycustom_check_panel.SetActive(false);
        keycustom_panel = GameObject.Find("KeyCustom_Panel");
        keycustom_panel.SetActive(false);


        gamemaster_sound_slider.onValueChanged.AddListener(delegate
        {
            pm.SoundVolumeMaster(gamemaster_sound_slider, master_mixer);
        });
        key_custom_save_button.onClick.AddListener(delegate
        {
            pm.SetKeyCustom(sh, key_custom_arry);
        });
        save_setting_value_button.onClick.AddListener(delegate
        {
            pm.SaveSettingValue(sh, gamemaster_sound_slider, mousedpi_slider);
        });
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 300;
        ImportSettingValue();
        wait = new WaitForSeconds(300.0f);//나무 리스폰 시간 조정
    }

    
    void Update()
    {
        if (game_puase_bool)
        {

        }
        else
        {
            pl.LookAround(cameraArm, camera, camera_dstc, mouse_dpi);
            pl.Move(cameraArm, characterBody, controller, animator, speed, rigid, jumpPower, laymask_floor);
            StartCoroutine(pl.JumpAndMove(cameraArm, characterBody, controller, animator, speed, rigid, jumpPower, laymask_floor, jumpheight));
            //PointOfView();
        }
        pl.JumpStatusOn(characterBody, laymask_floor);
        ii.RayCastTree(camera, press_tree_image, key_custom_arry, tree_log, tree_fruit, laymask_tree);
        ii.TayCastTreeItem(camera, press_treeitem_image, key_custom_arry, laymask_tree_item);
        pm.KeyCustomCheck(sh, keycustom_check_panel, key_adr, key_custom_arry);
        pm.CheckKeyControl(pause_panel);
        StartCoroutine(ii.ResetTree(tree_list, wait, 0));
    }
    void LateUpdate()
    {
        cameraArm.position = characterBody.position;
        Vector3 vector = characterBody.position;
        vector.y = characterBody.position.y + 2.5f;
        if (viewPointFlag == 1)
        {
            cameraArm.position = vector;
        }
        else
        {
            camera.position = characterBody.position;

        }
    }

    public void ButtonClickGameContinue()
    {
        pm = new pause_menu();
        pm.ButtonClickGameContinue(pause_panel);
    }
    public void ButtonClickMainMenu()
    {
        pm = new pause_menu();
        pm.ButtonClickMainMenu();
    }
    public void ButtonClickOpenSetting()
    {
        pm = new pause_menu();
        pm.ButtonClickOpenSetting(setting_panel, pause_panel);
    }
    public void ButtonClickCloseSetting()
    {
        pm = new pause_menu();
        pm.ButtonClickCloseSetting(setting_panel, pause_panel);
    }
    public void ButtonClickKeyCustomOpen()
    {
        pm = new pause_menu();
        pm.ButtonClickKeyCustomOpen(setting_panel, keycustom_panel);
    }
    public void ButtonClickKeyCustomClose()
    {
        pm = new pause_menu();
        pm.ButtonClickKeyCustomClose(setting_panel, keycustom_panel);
    }
    public void KeyCustomPointOfViewKey() { pm = new pause_menu(); pm.KeyCustom(0, keycustom_check_panel); }//인칭변환키 변경
    public void KeyCustomInterectionKey() { pm = new pause_menu(); pm.KeyCustom(1, keycustom_check_panel); }//상호작용키 변경
    public void KeyCustomInventoryKey() { pm = new pause_menu(); pm.KeyCustom(2, keycustom_check_panel); }//인벤토리키 변경
    private void ImportSettingValue()
    {
        float volume = sh.GetSoundMasterVolume();
        gamemaster_sound_slider.value = volume;
        master_mixer.SetFloat("Master_Volume", volume);
        mousedpi_slider.value = sh.GetMouseDpi();
        key_custom_arry = sh.SyncKeyCustom();
        mouse_dpi = mousedpi_slider.value / 50;
        key_custom_arry = sh.SyncKeyCustom();
    }
    //플레이어 움직임 멈춤 상태인지 정해주는 함수
    public void GamePause(bool bl)
    {
        game_puase_bool = bl;
    }
}
