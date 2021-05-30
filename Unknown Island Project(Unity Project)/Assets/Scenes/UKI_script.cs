using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using Assets.Scenes;
using System;

public class UKI_script : MonoBehaviour
{
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

    public Button open_setting_button;
    public Button save_game_button;
    public Button exit_tomain_button;
    public Button quit_game_button;
    public Button resume_game_button;

    public AudioMixer master_mixer;

    public GameObject setting_window_panel;
    public GameObject setting_gameplay_panel;
    public GameObject setting_sound_panel;
    public GameObject setting_control_panel;
    public GameObject setting_confirm_panel;

    public Button save_setting_button;
    public Button exit_setting_button;
    public Toggle setting_gameplay_toggle;
    public Toggle setting_sound_toggle;
    public Toggle setting_control_toggle;
    public Button quit_setting_button;
    public Button setting_confirm_button;
    public Button setting_cancle_button;

    public Button reset_graphic_button;
    public Dropdown screenmod_dropdown;
    public Dropdown resolution_dropdown;
    public Slider mouse_dpi_slider;
    public Text mouse_dpi_text;

    public Button reset_sound_button;
    public Slider master_volume_slider;
    public Slider bgm_volume_slider;
    public Slider fx_volume_slider;
    public Toggle master_volume_toggle;
    public Toggle bgm_volume_toggle;
    public Toggle fx_volume_toggle;

    public Button reset_keycustom_button;
    public Button keycustom_switchingview_button;
    public Button keycustom_getitem_button;
    public Button keycustom_dropitem_button;
    public Button keycustom_checkwatch_button;
    public Button keycustom_openinventory_button;
    public GameObject keycustom_check_panel;

    public GameObject nowon_setting_panel;




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
    public static float mouse_dpi;
    static private bool isinwater;

    public List<GameObject> tree_list;
    public Sprite[] keycab_list;
    //flag
    // 1 = 3인칭 0 = 1인칭
    int viewPointFlag = 1;
    public static string[] key_custom_arry;
    public static float[] volume_arry;
    private static int key_adr; 
    public void SetKeyADR(int i) { key_adr = i; }

    static WaitForSeconds wait_treereset;
    static AsyncOperation wait_settingtrap;
    static WaitForSeconds wait_fishtrap;
    static WaitForSeconds wait_fishtrapisfull;
    static WaitForFixedUpdate wait_fix;
    static WaitForEndOfFrame wait_end;
    static WaitForSeconds wait_hungthir;
    static WaitForSeconds wait_poison;
    static WaitForSeconds wait_bleed;
    static WaitForSeconds wait_cold;

    private DBAccess db;
    public Setting_header sh;
    private Player pl;
    private Ingame_Interection ii;
    private pause_menu pm;
    private Dispancer ds;
    private Setting_Status ss;
    private ItemJson ij;
    void Start()
    {
        db = new DBAccess();
        sh = new Setting_header(db);
        ii = new Ingame_Interection();
        pl = new Player();
        pm = new pause_menu(sh);
        ds = new Dispancer();
        ss = new Setting_Status();
        ij = new ItemJson();
        animator = GameObject.Find("Player").GetComponentInChildren<Animator>();
        controller = GameObject.Find("Player").GetComponentInChildren<CharacterController>();
        camera_dstc = Mathf.Sqrt(4 * 4);
        game_puase_bool = false;
        key_adr = 9999;

        tree_list = new List<GameObject>();
        tree_list = ds.TreeDispanceList();
        keycab_list = ds.KeyDispanceList();

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
        open_setting_button = GameObject.Find("Option_Button").GetComponent<Button>();
        save_game_button = GameObject.Find("Save_Button").GetComponent<Button>();
        exit_tomain_button = GameObject.Find("Exit_Button").GetComponent<Button>();
        quit_game_button = GameObject.Find("Quit_Button").GetComponent<Button>();
        resume_game_button = GameObject.Find("Resume_Button").GetComponent<Button>();
        pause_panel.SetActive(false);

        setting_window_panel = GameObject.Find("Setting_Window(Image)");
        {
            setting_gameplay_panel = setting_window_panel.transform.Find("SettingContent(Panel)").Find("Setting_GamePlay(Panel)").gameObject;
            {
                reset_graphic_button = setting_gameplay_panel.transform.Find("ResetSetting_Gameplay(Button)").GetComponent<Button>();
                screenmod_dropdown = setting_gameplay_panel.transform.Find("ScreenMod_DropDown").GetComponent<Dropdown>();
                resolution_dropdown = setting_gameplay_panel.transform.Find("Resolution_DropDown").GetComponent<Dropdown>();
                mouse_dpi_slider = setting_gameplay_panel.transform.Find("MouseDpi_Slider").GetComponent<Slider>();
                mouse_dpi_text = setting_gameplay_panel.transform.Find("MouseDpi_Input(Imgae)").gameObject.GetComponentInChildren<Text>();
                setting_gameplay_panel.SetActive(false);
            }

            setting_sound_panel = setting_window_panel.transform.Find("SettingContent(Panel)").Find("Setting_Sound(Panel)").gameObject;
            {
                reset_sound_button = setting_sound_panel.transform.Find("ResetSetting_Gameplay(Button)").GetComponent<Button>();
                master_volume_slider = setting_sound_panel.transform.Find("Master_Volume(Slider)").GetComponent<Slider>();
                bgm_volume_slider = setting_sound_panel.transform.Find("Backgounrd_Volume(Slider)").GetComponent<Slider>();
                fx_volume_slider = setting_sound_panel.transform.Find("SoundEffect_Volume(Slider)").GetComponent<Slider>();
                master_volume_toggle = setting_sound_panel.transform.Find("Master_Volume_Mute(Toggle)").GetComponent<Toggle>();
                bgm_volume_toggle = setting_sound_panel.transform.Find("Background_Volume_Mute(Toggle)").GetComponent<Toggle>();
                fx_volume_toggle = setting_sound_panel.transform.Find("SoundEffect_Volume_Mute(Toggle)").GetComponent<Toggle>();
                setting_sound_panel.SetActive(false);
            }

            setting_control_panel = setting_window_panel.transform.Find("SettingContent(Panel)").Find("Setting_Control(Panel)").gameObject;
            {
                keycustom_check_panel = setting_control_panel.transform.Find("KeyCustom_Check_Panel").gameObject;
                keycustom_check_panel.SetActive(false);
                reset_keycustom_button = setting_control_panel.transform.Find("ResetSetting_Gameplay(Button)").GetComponent<Button>();
                keycustom_switchingview_button = GameObject.Find("Key_SwitchingView(Text)").GetComponentInChildren<Button>();
                keycustom_getitem_button = GameObject.Find("Key_GetItem(Text)").GetComponentInChildren<Button>();
                keycustom_dropitem_button = GameObject.Find("Key_DropItem(Text)").GetComponentInChildren<Button>();
                keycustom_checkwatch_button = GameObject.Find("Key_CheckWatch(Text)").GetComponentInChildren<Button>();
                keycustom_openinventory_button = GameObject.Find("Key_OpenInventory(Text)").GetComponentInChildren<Button>();
                setting_control_panel.SetActive(false);
            }

            save_setting_button = setting_window_panel.transform.Find("SaveSetting_Button").GetComponent<Button>();
            exit_setting_button = setting_window_panel.transform.Find("ExitSetting_Button").GetComponent<Button>();
            setting_gameplay_toggle = setting_window_panel.transform.Find("Subject_Button(Group)").Find("Setting_GamePlay(Button)").GetComponent<Toggle>();
            setting_sound_toggle = setting_window_panel.transform.Find("Subject_Button(Group)").Find("Setting_Sound(Button)").GetComponent<Toggle>();
            setting_control_toggle = setting_window_panel.transform.Find("Subject_Button(Group)").Find("Setting_Control(Button)").GetComponent<Toggle>();
            quit_setting_button = setting_window_panel.transform.Find("Quit(Button)").GetComponent<Button>();

            setting_confirm_panel = GameObject.Find("Confirm_Background(Image)");
            {
                setting_cancle_button = setting_confirm_panel.transform.Find("Cancel(Button)").GetComponent<Button>();
                setting_confirm_button = setting_confirm_panel.transform.Find("Confirm(Button)").GetComponent<Button>();
                setting_confirm_panel.SetActive(false);
            }

            setting_window_panel.SetActive(false);
        }

        

        open_setting_button.onClick.AddListener(delegate
        {
            pm.ChangePanel(pause_panel, setting_window_panel);
            setting_gameplay_panel.SetActive(true);
            nowon_setting_panel = setting_gameplay_panel;
        });
        save_game_button.onClick.AddListener(delegate
        {
            //게임 저장
        });
        exit_tomain_button.onClick.AddListener(delegate
        {
            pm.ButtonClickMainMenu();
        });
        quit_game_button.onClick.AddListener(delegate
        {
            Application.Quit();
        });
        resume_game_button.onClick.AddListener(delegate
        {
            pm.ButtonClickGameContinue(pause_panel);
        });
        setting_gameplay_toggle.onValueChanged.AddListener(delegate
        {
            pm.ChangePanel(nowon_setting_panel, setting_gameplay_panel);
            nowon_setting_panel = setting_gameplay_panel;
        });
        setting_sound_toggle.onValueChanged.AddListener(delegate
        {
            pm.ChangePanel(nowon_setting_panel, setting_sound_panel);
            nowon_setting_panel = setting_sound_panel;
        });
        setting_control_toggle.onValueChanged.AddListener(delegate
        {
            pm.ChangePanel(nowon_setting_panel, setting_control_panel);
            nowon_setting_panel = setting_control_panel;
        });
        save_setting_button.onClick.AddListener(delegate
        {
            SaveSetting();
        });
        exit_setting_button.onClick.AddListener(delegate
        {
            pm.ButtonClickCloseSetting(pause_panel, setting_window_panel, setting_confirm_panel);
            if (nowon_setting_panel.activeSelf)
            {
                nowon_setting_panel.SetActive(false);
            }
        });
        quit_setting_button.onClick.AddListener(delegate
        {
            pm.ChangePanel(setting_window_panel, pause_panel);
            if (nowon_setting_panel.activeSelf)
            {
                nowon_setting_panel.SetActive(false);
            }
        });
        screenmod_dropdown.onValueChanged.AddListener(delegate
        {
            pm.MonitorSize(screenmod_dropdown, resolution_dropdown);
        });
        resolution_dropdown.onValueChanged.AddListener(delegate
        {
            pm.MonitorSize(screenmod_dropdown, resolution_dropdown);
        });
        mouse_dpi_slider.onValueChanged.AddListener(delegate
        {
            mouse_dpi_text.text = Mathf.Round(mouse_dpi_slider.value).ToString();
        });
        reset_graphic_button.onClick.AddListener(delegate
        {
            ImportSettingValue(sh, master_volume_slider, bgm_volume_slider, fx_volume_slider, master_mixer, mouse_dpi_slider, resolution_dropdown, screenmod_dropdown);
        });
        reset_sound_button.onClick.AddListener(delegate
        {
            ImportSettingValue(sh, master_volume_slider, bgm_volume_slider, fx_volume_slider, master_mixer, mouse_dpi_slider, resolution_dropdown, screenmod_dropdown);
        }); 
        reset_keycustom_button.onClick.AddListener(delegate
        {
            ImportSettingValue(sh, master_volume_slider, bgm_volume_slider, fx_volume_slider, master_mixer, mouse_dpi_slider, resolution_dropdown, screenmod_dropdown);
        });
        master_volume_slider.onValueChanged.AddListener(delegate
        {
            pm.VolumeChange("Master", master_volume_slider, master_mixer);
        });
        bgm_volume_slider.onValueChanged.AddListener(delegate
        {
            pm.VolumeChange("BGM", bgm_volume_slider, master_mixer);
        });
        fx_volume_slider.onValueChanged.AddListener(delegate
        {
            pm.VolumeChange("FX", fx_volume_slider, master_mixer);
        });
        master_volume_toggle.onValueChanged.AddListener(delegate
        {
            pm.MuteVolume("Master", master_volume_slider, master_volume_toggle, master_mixer);
        });
        bgm_volume_toggle.onValueChanged.AddListener(delegate
        {
            pm.MuteVolume("BGM", bgm_volume_slider, bgm_volume_toggle, master_mixer);
        });
        fx_volume_toggle.onValueChanged.AddListener(delegate
        {
            pm.MuteVolume("FX", fx_volume_slider, fx_volume_toggle, master_mixer);
        });
        keycustom_switchingview_button.onClick.AddListener(delegate
        {
            pm.KeyCustom(keycustom_check_panel, keycustom_switchingview_button, 0);
        });
        keycustom_getitem_button.onClick.AddListener(delegate
        {
            pm.KeyCustom(keycustom_check_panel, keycustom_getitem_button, 1);
        });
        keycustom_dropitem_button.onClick.AddListener(delegate
        {
            pm.KeyCustom(keycustom_check_panel, keycustom_dropitem_button, 2);
        });
        keycustom_checkwatch_button.onClick.AddListener(delegate
        {
            pm.KeyCustom(keycustom_check_panel, keycustom_checkwatch_button, 3);
        });
        keycustom_openinventory_button.onClick.AddListener(delegate
        {
            pm.KeyCustom(keycustom_check_panel, keycustom_openinventory_button, 4);
        });
        setting_confirm_button.onClick.AddListener(delegate
        {
            pm.ButtonClickNotSavedYes(setting_control_panel, key_custom_arry, setting_window_panel, setting_confirm_panel, pause_panel);
        });
        setting_cancle_button.onClick.AddListener(delegate
        {
            pm.ButtonClickNotSavedNo(setting_control_panel, setting_window_panel, setting_confirm_panel, pause_panel, sh, master_volume_slider, 
                bgm_volume_slider, fx_volume_slider, master_mixer, mouse_dpi_slider, resolution_dropdown, screenmod_dropdown);
        });



        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 300;
        ImportSettingValue(sh, master_volume_slider, bgm_volume_slider, fx_volume_slider, master_mixer, mouse_dpi_slider, resolution_dropdown, screenmod_dropdown);
        wait_treereset = new WaitForSeconds(4.0f);//나무 리스폰 시간 조정
        wait_settingtrap = new AsyncOperation();
        wait_fishtrap = new WaitForSeconds(120f);//통발에 물고기 잡히길 기다리는 시간 조정
        wait_fishtrapisfull = new WaitForSeconds(5f);
        wait_fix = new WaitForFixedUpdate();
        wait_hungthir = new WaitForSeconds(60f);//배고픔이랑 목마름 수치 내려가는 주기
        wait_poison = new WaitForSeconds(10f);//중독시 채력 닳는 주기
        wait_bleed = new WaitForSeconds(10f);//출혈시 채력 닳는 주기
        wait_cold = new WaitForSeconds(10f);//저체온증시 채력 닳는 주기

        StartCoroutine(ss.HungerAndThirst(wait_hungthir, 1, 2));
        StartCoroutine(ss.StatusPoison(wait_poison, wait_fix, 5, 10));
        StartCoroutine(ss.StatusBleed(wait_bleed, wait_fix, 7, 10));
        StartCoroutine(ss.StatusCold(wait_cold, wait_fix, 3, 10));
        StartCoroutine(ii.ResetTree(tree_list, wait_treereset, wait_fix));
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
        pm.KeyCustomCheck(keycustom_check_panel, key_adr, key_custom_arry);
        pm.CheckKeyControl(pause_panel);
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
    public void ImportSettingValue(Setting_header sh, Slider master_volume_slider, Slider bgm_volume_slider, Slider fx_volume_slider, AudioMixer master_mixer, 
        Slider mouse_dpi_slider, Dropdown resolution_dropdown, Dropdown screenmod_dropdown)
    {
        volume_arry = sh.GetSoundMasterVolume();
        float volume = volume_arry[0];
        master_volume_slider.value = volume;
        master_mixer.SetFloat("Master_Volume", volume);
        volume = volume_arry[1];
        bgm_volume_slider.value = volume;
        master_mixer.SetFloat("BGM_Volume", volume);
        volume = volume_arry[2];
        fx_volume_slider.value = volume;
        master_mixer.SetFloat("FX_Volume", volume);
        mouse_dpi_slider.value = sh.GetMouseDpi();
        mouse_dpi = mouse_dpi_slider.value / 50;
        key_custom_arry = sh.SyncKeyCustom();
        resolution_dropdown.value = sh.GetMonitorDV();
        screenmod_dropdown.value = sh.GetFullscreen();
    }
    //플레이어 움직임 멈춤 상태인지 정해주는 함수
    public void GamePause(bool bl)
    {
        game_puase_bool = bl;
    }
    public void SetDPI(float f)
    {
        mouse_dpi = f;
    }
    public void SetKeyArr(string[] st_arr)
    {
        key_custom_arry = st_arr;
    }
    public float[] GetVolumeArry()
    {
        return volume_arry;
    }
    public Sprite[] GetSpriteKeyCab()
    {
        return keycab_list;
    }
    public void SaveSetting()
    {
        sh.SetFullscreen(screenmod_dropdown.value);
        sh.SetKeyCustom(key_custom_arry);
        sh.SetMonitorDV(resolution_dropdown.value);
        sh.SetMouseDpi(mouse_dpi_slider.value);
        sh.SetSoundMasterVolume(volume_arry);
        pm.IsSaved();
    }
}
