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
    public GameObject press_water_image;
    public GameObject press_fishtrap_image;
    public GameObject press_rock_image;
    public GameObject press_stone_image;

    public GameObject tree_log;
    public GameObject tree_fruit;
    public GameObject fish_trap;
    public GameObject rock_stone;

    public GameObject pause_panel;
    public GameObject setting_panel;
    public GameObject keycustom_panel;
    public GameObject keycustom_check_panel;

    public Slider mousedpi_slider;
    public Slider gamemaster_sound_slider;
    public AudioMixer master_mixer;

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
    public LayerMask laymask_water;
    public LayerMask laymask_fishtrap;
    public LayerMask laymask_rock;
    public LayerMask laymask_stone;

    public float speed;
    public float jumpPower;
    public float jumpheight;
    private float camera_dstc;
    private static bool game_puase_bool;
    public static bool jumpStatus;
    bool viewPoint;
    private float mouse_dpi;
    static private bool isinwater;

    public List<GameObject> tree_list;
    //flag
    // 1 = 3인칭 0 = 1인칭
    int viewPointFlag = 1;
    private string[] key_custom_arry;
    private static int key_adr; 
    public void SetKeyADR(int i) { key_adr = i; }

    static WaitForSeconds wait_treereset;
    static AsyncOperation wait_settingtrap;
    static WaitForSeconds wait_fishtrap;
    static WaitForSeconds wait_fishtrapisfull;
    static WaitForFixedUpdate wait_fix;
    static WaitForEndOfFrame wait_end;

    private DBAccess db;
    private Setting_header sh;
    private Player pl;
    private Ingame_Interection ii;
    private pause_menu pm;
    private Dispancer ds;
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
        ds = new Dispancer();

        key_adr = 9999;

        tree_list = new List<GameObject>();
        tree_list = ds.TreeDispanceList();

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

        tree_log = GameObject.FindWithTag("Tree_Log");
        tree_fruit = GameObject.FindWithTag("Tree_Fruit");
        fish_trap = GameObject.Find("Fish_trap");
        rock_stone = GameObject.Find("Rock_stone");

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
        wait_treereset = new WaitForSeconds(300.0f);//나무 리스폰 시간 조정
        wait_settingtrap = new AsyncOperation();
        wait_fishtrap = new WaitForSeconds(120f);//통발에 물고기 잡히길 기다리는 시간 조정
        wait_fishtrapisfull = new WaitForSeconds(5f);
        wait_fix = new WaitForFixedUpdate();
    }

    
    void Update()
    {
        if (game_puase_bool)
        {

        }
        else
        {
            StartCoroutine(pl.JumpAndMove(cameraArm, characterBody, controller, animator, speed, jumpPower, wait_fix));
            StartCoroutine(pl.LookAround(cameraArm, camera, camera_dstc, mouse_dpi, wait_end));
            pl.PointOfView(key_custom_arry);
        }
        pl.JumpStatusOn(characterBody, laymask_floor, laymask_rock);
        ii.RayCastTree(characterBody, press_tree_image, key_custom_arry, tree_log, tree_fruit, laymask_tree);
        ii.RayCastTreeItem(characterBody, press_treeitem_image, key_custom_arry, laymask_tree_item);
        ii.RayCastWaterFishTrap(characterBody, press_water_image, press_fishtrap_image, key_custom_arry, fish_trap, laymask_water, laymask_fishtrap);
        ii.RayCastRock(characterBody, press_rock_image, press_stone_image, key_custom_arry, rock_stone, laymask_rock, laymask_stone);
        pm.KeyCustomCheck(sh, keycustom_check_panel, key_adr, key_custom_arry);
        pm.CheckKeyControl(pause_panel);
        StartCoroutine(ii.ResetTree(tree_list, wait_treereset, 0));
        StartCoroutine(ii.CountFishTrap(wait_fishtrap, wait_settingtrap, 0));
        StartCoroutine(ii.FishtrapIsfull(wait_fishtrapisfull, wait_fix));
    }
    private void FixedUpdate()
    {
        if (game_puase_bool)
        {

        }
        else
        {
            //pl.LookAround(cameraArm, camera, camera_dstc, mouse_dpi);
            pl.Move(cameraArm, characterBody, controller, animator, speed);
            //PointOfView();
        }
    }
    void LateUpdate()
    {
        Vector3 vector = characterBody.position;
        vector.y = characterBody.position.y + 1.5f;
        if (viewPointFlag == 1)
        {
            cameraArm.position = vector;
        }
        else
        {
            camera.position = cameraArm.position;
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
    public void IsinWater(bool _bool)
    {
        isinwater = _bool;
    }
}
