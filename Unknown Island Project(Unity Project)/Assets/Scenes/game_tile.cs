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
    public Slider gamemaster_sound;
    public AudioMixer master_mixer;
    public Dropdown monitorsize_dropdown;
    public Slider mousedpi_slider;
    public GameObject keybiding_panel;
    public GameObject keybiding_check_panel;
    private Setting_header sh;
    private DBAccess db;
    List<string> monitor_dropdown_options = new List<string>();


    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        //setting_panel.SetActive(true);
        db = new DBAccess();
        sh = new Setting_header(db);

        generate_moniotr_dropdown_list();

        gamemaster_sound.onValueChanged.AddListener(delegate
        {
            SoundVolumeMaster(sh);
        });
        fullscreen_toggle.onValueChanged.AddListener(delegate
        {
            FullscreenBool(sh);
        });
        mousedpi_slider.onValueChanged.AddListener(delegate {
            MouseDpiControlSlider(sh);
        });
        monitorsize_dropdown.onValueChanged.AddListener(delegate
        {
            MonitorSize(sh);
        });
         //설정 동기화
        ImportSettingValue();
        //setting_panel.SetActive(false);
    }

    public void Awake()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
    }

    //게임 시작
    public void GameStart()
    {
        SceneManager.LoadScene("SampleScene");
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
        sh.SetFullscreenBool(fullscreen_toggle.isOn);
    }
    //해상도 설정
     void MonitorSize(Setting_header sh)
    {
        sh.SetMonitorDV(monitorsize_dropdown.value);//해상도 값 내보내기
        if (fullscreen_toggle.isOn == true)
        {
            switch (sh.GetMonitorDV())
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
        
        float volume = gamemaster_sound.value;
        master_mixer.SetFloat("Master_Volume", volume);
        //db = new DBAccess();
        //sh = new Setting_header();
        sh.SetSoundMasterVolume(volume);
    }

    //마우스 감도 조절
     void MouseDpiControlSlider(Setting_header sh)
    {
        float f = mousedpi_slider.value;
        sh.SetMouseDpi(f);
    }

    //키 바인딩창 열기
    public void KeyBidingOpen()
    {
        setting_panel.SetActive(false);
        keybiding_panel.SetActive(true);
    }
    //키 바인딩창 닫기
    public void KeyBidingClose()
    {
        keybiding_panel.SetActive(false);
        setting_panel.SetActive(true);
    }

    //키 바인딩 인지 아닌지 확인 해서 바꾼 키 값 반환 하는 함수
    public void KeyBidingCheck()
    {
        if (keybiding_check_panel.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                keybiding_check_panel.SetActive(false);
            }
            else if(Input.inputString != null)
            {
                sh.SetKeyBidingPoint(Input.inputString);
            }
        }
    }
    //인칭변환키 변경
    public void KeyBidingPointOfViewKey()
    {
        keybiding_check_panel.SetActive(true);
        string st;
        while(sh.GetKeyBidingPoint() == null)
        {
            st = sh.GetKeyBidingPoint();
            sh.SetKeyBiding(st, 0);
            sh.SetKeyBidingPoint(null);
            keybiding_check_panel.SetActive(false);
            break;
        }
    }

    private void ImportSettingValue()
    {
        float volume = sh.GetSoundMasterVolume();
        gamemaster_sound.value = volume;
        master_mixer.SetFloat("Master_Volume", volume);
        //monitorsize_dropdown.value = sh.GetMonitorDV();
        //fullscreen_toggle.isOn = sh.GetFullscreenBool();
        //MonitorSize();
        //float f = sh.GetMouseDpi();
        //mousedpi_slider.value = f;
    }
     void OnDestroy()
    {
        //db.CloseSqlConnection();
    }
    private void generate_moniotr_dropdown_list()
    {
        monitor_dropdown_options.Add("1920 x 1080");
        monitor_dropdown_options.Add("1280 x 720");
        monitor_dropdown_options.Add("720 x 540");
        monitorsize_dropdown.AddOptions(monitor_dropdown_options);


        monitorsize_dropdown.value = 0;


    }
}
