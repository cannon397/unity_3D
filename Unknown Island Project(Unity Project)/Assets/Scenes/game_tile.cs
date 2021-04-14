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
    public InputField mousedpi_inputfield;
    public Slider mousedpi_slider;
    Setting_header sh;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
    }

    public void Awake()
    {
        sh = new Setting_header();
        //ImportSettingValue(); //설정 동기화
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
    public void FullscreenBool(bool _bool)
    {
        Debug.Log("FullscreenBool = " + _bool);
        if (fullscreen_toggle.isOn)
        {
            lable_on.SetActive(true);
            lable_off.SetActive(false);
            Debug.Log("토글 온");
        }
        else
        {
            lable_on.SetActive(false);
            lable_off.SetActive(true);
            Debug.Log("토글 오프");
        }
        MonitorSize();
        sh.SetFullscreenBool(fullscreen_toggle.isOn);
    }
    //해상도 설정
    public void MonitorSize()
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
    public void SoundVolumeMaster()
    {
        float volume = gamemaster_sound.value;
        master_mixer.SetFloat("Master", volume);
        sh.SetSoundMasterVolume(volume);
    }

    //마우스 감도 조절
    public void MouseDpiControlInputField(string st)
    {
        float f = float.Parse(st);
        sh.SetMouseDpi(f);
        mousedpi_inputfield.text = st;
    }

    private void ImportSettingValue()
    {
        gamemaster_sound.value = sh.GetSoundMasterVolume();
        monitorsize_dropdown.value = sh.GetMonitorDV();
        fullscreen_toggle.isOn = sh.GetFullscreenBool();
    }
}
