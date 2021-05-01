using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using Mono.Data.Sqlite;
using Assets.Scenes;

public class pause_menu : MonoBehaviour
{
    public GameObject pause_panel;
    public GameObject setting_panel;
    public Slider mousedpi_slider;
    public Slider gamemaster_sound_slider;
    public AudioMixer master_mixer;
    public GameObject keycustom_panel;
    public GameObject keycustom_check_panel;
    public Button key_custom_save_button;
    public Button save_setting_value_button;

    private string[] key_custom_arry;
    private static int key_adr;
    private DBAccess db;
    private Setting_header sh;
    private Player player;
    void Start()
    {
        db = new DBAccess();
        sh = new Setting_header(db);
        player = new Player();

        gamemaster_sound_slider.onValueChanged.AddListener(delegate
        {
            SoundVolumeMaster();
        });
        key_custom_save_button.onClick.AddListener(delegate
        {
            SetKeyCustom(sh);
        });
        save_setting_value_button.onClick.AddListener(delegate
        {
            SaveSettingValue(sh);
        });

        pause_panel = GameObject.Find("Pause_Panel");
        pause_panel.SetActive(false);
        setting_panel = GameObject.Find("Setting_Panel");
        setting_panel.SetActive(false);
        keycustom_panel = GameObject.Find("KeyCustom_Panel");
        keycustom_panel.SetActive(false);

        ImportSettingValue(sh);
    }

    void Update()
    {
        KeyCustomCheck(sh);
        CheckKeyControl();
    }

    private void CheckKeyControl()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause_panel.activeSelf)
            {
                pause_panel.SetActive(false);
                player.GamePause(false);
            }
            else
            {
                pause_panel.SetActive(true);
                player.GamePause(true);
            }
        }
    }
    public void ButtonClickGameContinue()
    {
        player = new Player();
        pause_panel.SetActive(false);
        player.GamePause(false);
    }
    public void ButtonClickMainMenu()
    {
        SceneManager.LoadScene("GameTitle");
    }
    public void ButtonClickOpenSetting()
    {
        setting_panel.SetActive(true);
        pause_panel.SetActive(false);
    }
    public void ButtonClickCloseSetting()
    {
        setting_panel.SetActive(false);
        pause_panel.SetActive(true);
    }
    void SoundVolumeMaster()
    {
        float volume = gamemaster_sound_slider.value;
        master_mixer.SetFloat("Master_Volume", volume);
    }
    //키 커스텀창 열기
    public void ButtonClickKeyCustomOpen()
    {
        setting_panel.SetActive(false);
        keycustom_panel.SetActive(true);
    }
    //키 커스텀창 닫기
    public void ButtonClickKeyCustomClose()
    {
        keycustom_panel.SetActive(false);
        setting_panel.SetActive(true);
    }

    //키 커스텀 인지 아닌지 확인 해서 바꾼 키 값 반환 하는 함수
    public void KeyCustomCheck(Setting_header sh)
    {
        if (keycustom_check_panel.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                keycustom_check_panel.SetActive(false);
            }
            if (sh.CheckKeyCustomAvble(Input.inputString))
            {
                switch (key_adr)
                {
                    case 0:
                        key_custom_arry[0] = Input.inputString;
                        break;
                    case 1:
                        key_custom_arry[1] = Input.inputString;
                        break;
                    case 2:
                        key_custom_arry[2] = Input.inputString;
                        break;
                }
                keycustom_check_panel.SetActive(false);
            }
        }
    }
    //키 커스텀 설정값 저장 함수
    public void SetKeyCustom(Setting_header sh)
    {
        sh.SetKeyCustom(key_custom_arry);
    }
    //키 커스텀 모함수
    public void KeyCustom(int i)
    {
        key_adr = i;
        keycustom_check_panel.SetActive(true);
    }

    public void KeyCustomPointOfViewKey() { KeyCustom(0); }//인칭변환키 변경
    public void KeyCustomInterectionKey() { KeyCustom(1); }//상호작용키 변경
    public void KeyCustomInventoryKey() { KeyCustom(2); }//인벤토리키 변경

    //설정값 동기화
    private void ImportSettingValue(Setting_header sh)
    {
        float volume = sh.GetSoundMasterVolume();
        gamemaster_sound_slider.value = volume;
        master_mixer.SetFloat("Master_Volume", volume);
        mousedpi_slider.value = sh.GetMouseDpi();
        key_custom_arry = sh.SyncKeyCustom();
    }
    //설정값 저장 함수
    public void SaveSettingValue(Setting_header sh)
    {
        sh.SetSoundMasterVolume(gamemaster_sound_slider.value);
        sh.SetMouseDpi(mousedpi_slider.value);
    }
}
