using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using Assets.Scenes;

public class game_tile : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject lable_on;
    public GameObject lable_off;
    public Toggle fullscreen_toggle;
    public GameObject setting_panel;
    public Slider gamemaster_sound_slider;
    public AudioMixer master_mixer;
    public Dropdown monitorsize_dropdown;
    public Slider mousedpi_slider;
    public GameObject keycustom_panel;
    public GameObject keycustom_check_panel;
    public Button key_custom_save_button;
    public Button save_setting_value_button;

    private Setting_header sh;
    private DBAccess db;

    private string[] key_custom_arry;
    private static int key_adr;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        db = new DBAccess();
        sh = new Setting_header(db);


        gamemaster_sound_slider.onValueChanged.AddListener(delegate
        {
            SoundVolumeMaster(sh);
        });
        fullscreen_toggle.onValueChanged.AddListener(delegate
        {
            FullscreenBool(sh);
        });
        monitorsize_dropdown.onValueChanged.AddListener(delegate
        {
            MonitorSize(sh);
        });
        key_custom_save_button.onClick.AddListener(delegate
        {
            SetKeyCustom(sh);
        });
        save_setting_value_button.onClick.AddListener(delegate
        {
            SaveSettingValue(sh);
        });
        //설정 동기화
        ImportSettingValue(sh);
    }

    public void Awake()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        KeyCustomCheck(sh);
    }

    //게임 시작
    public void GameStart()
    {
        SceneManager.LoadScene("UKI_MainGameScene");
    }
    public void GameExit()
    {
        Application.Quit();
    }

    //설정창 열기
    public void OpenSetting()
    {
        setting_panel.SetActive(true);
        
    }
    //설정창 닫기
    public void CloseSetting()
    {
        setting_panel.SetActive(false);
    }

    //전체화면 키고 끄는 옵션
    void FullscreenBool(Setting_header sh)
    {
        if (fullscreen_toggle.isOn)
        {
            lable_on.SetActive(true);
            lable_off.SetActive(false);
        }
        else
        {
            lable_on.SetActive(false);
            lable_off.SetActive(true);
        }
        MonitorSize(sh);
    }
    //해상도 설정
    void MonitorSize(Setting_header sh)
    {
        if (fullscreen_toggle.isOn == true)
        {
            switch (monitorsize_dropdown.value)
            {
                case 0:
                    Screen.SetResolution(960, 540, true);
                    break;
                case 1:
                    Screen.SetResolution(1280, 720, true);
                    break;
                case 2:
                    Screen.SetResolution(1920, 1080, true);
                    break;
            }
        }
        else
        {
            switch (sh.GetMonitorDV())
            {
                case 0:
                    Screen.SetResolution(960, 540, false);
                    break;
                case 1:
                    Screen.SetResolution(1280, 720, false);
                    break;
                case 2:
                    Screen.SetResolution(1920, 1080, false);
                    break;
            }
        }
    }

    //볼륨 조절
    void SoundVolumeMaster(Setting_header sh)
    {
        master_mixer.SetFloat("Master_Volume", gamemaster_sound_slider.value);
    }

    //키 커스텀창 열기
    public void KeyCustomOpen()
    {
        setting_panel.SetActive(false);
        keycustom_panel.SetActive(true);
    }
    //키 커스텀창 닫기
    public void KeyCustomClose()
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
        monitorsize_dropdown.value = sh.GetMonitorDV();
        fullscreen_toggle.isOn = sh.GetFullscreenBool();
        MonitorSize(sh);
        mousedpi_slider.value = sh.GetMouseDpi();
        key_custom_arry = sh.SyncKeyCustom();
    }
    //설정값 저장 함수
    public void SaveSettingValue(Setting_header sh)
    {
        float volume = gamemaster_sound_slider.value;
        sh.SetSoundMasterVolume(volume);
        sh.SetMonitorDV(monitorsize_dropdown.value);
        sh.SetFullscreenBool(fullscreen_toggle.isOn);
        sh.SetMouseDpi(mousedpi_slider.value);
    }
     void OnDestroy()
    {
        //db.CloseSqlConnection();
    }
}
