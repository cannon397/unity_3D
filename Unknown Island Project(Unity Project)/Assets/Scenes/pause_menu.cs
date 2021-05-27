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
    private UKI_script uki;
    private Setting_header sh;
    public static Sprite sprite;
    public static Button button;

    private Sprite[] keycab_list;

    static private bool issaved;
    public pause_menu(Setting_header sh)
    {
        uki = new UKI_script();
        this.sh = sh;
    }

    ///<summary>
    ///상시 키입력 받을 경우 여기다가 작성
    ///</summary>
    public void CheckKeyControl(GameObject pause_panel)
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (pause_panel.activeSelf)
            {
                pause_panel.SetActive(false);
                uki.GamePause(false);
            }
            else if(!pause_panel.activeSelf)
            {
                pause_panel.SetActive(true);
                uki.GamePause(true);
            }
        }
    }
    ///<summary>
    ///일시정지 화면 닫는 함수
    ///</summary>
    public void ButtonClickGameContinue(GameObject pause_panel)
    {
        pause_panel.SetActive(false);
        uki.GamePause(false);
    }
    ///<summary>
    ///메인화면으로 가는 함수
    ///</summary>
    public void ButtonClickMainMenu()
    {
        SceneManager.LoadScene("GameTitle");
    }
    ///<summary>
    ///설정창 닫는 함수
    ///</summary>
    public void ButtonClickCloseSetting(GameObject pause_panel, GameObject setting_panel, GameObject notsaved_panel)
    {
        if (issaved == true)
        {
            setting_panel.SetActive(false);
            pause_panel.SetActive(true);
        }
        else
        {
            notsaved_panel.SetActive(true);
        }
    }
    ///<summary>
    ///소리 조절 슬라이더 값 변동 되면 게임 볼륨 조절 하는 함수
    ///</summary>
    public void VolumeChange(string st, Slider sound_slider, AudioMixer master_mixer)
    {
        float volume = 0;
        switch (st)
        {
            case "Master":
                volume = sound_slider.value;
                master_mixer.SetFloat("Master_Volume", volume);
                issaved = false;
                break;
            case "BGM":
                volume = sound_slider.value;
                master_mixer.SetFloat("BGM_Volume", volume);
                issaved = false;
                break;
            case "FX":
                volume = sound_slider.value;
                master_mixer.SetFloat("FX_Volume", volume);
                issaved = false;
                break;
        }
        
    }
    /// <summary>
    /// 뮤트 토글로 사운드 뮤트 시키는 함수
    /// </summary>
    /// <param name="st">해당 사운드 이름</param>
    /// <param name="sound_slider">해당 사운드 슬라이더</param>
    /// <param name="mute_toggle">해당 사운드 뮤트 토글</param>
    /// <param name="master_mixer">오디오 믹서</param>
    public void MuteVolume(string st, Slider sound_slider, Toggle mute_toggle, AudioMixer master_mixer)
    {
        float volume = 0;
        if (mute_toggle.isOn)
        {
            switch (st)
            {
                case "Master":
                    master_mixer.SetFloat("Master_Volume", -80);
                    issaved = false;
                    break;
                case "BGM":
                    master_mixer.SetFloat("BGM_Volume", -80);
                    issaved = false;
                    break;
                case "FX":
                    master_mixer.SetFloat("FX_Volume", -80);
                    issaved = false;
                    break;
            }
        }
        else
        {
            switch (st)
            {
                case "Master":
                    volume = sound_slider.value;
                    master_mixer.SetFloat("Master_Volume", volume);
                    issaved = false;
                    break;
                case "BGM":
                    volume = sound_slider.value;
                    master_mixer.SetFloat("BGM_Volume", volume);
                    issaved = false;
                    break;
                case "FX":
                    volume = sound_slider.value;
                    master_mixer.SetFloat("FX_Volume", volume);
                    issaved = false;
                    break;
            }
        }
    }
    ///<summary>
    ///키 커스텀창 열기
    ///</summary>
    public void ButtonClickKeyCustomOpen(GameObject setting_panel, GameObject keycustom_panel)
    {
        setting_panel.SetActive(false);
        keycustom_panel.SetActive(true);
    }
    ///<summary>
    ///키 커스텀창 닫기
    ///</summary>
    public void ButtonClickKeyCustomClose(GameObject setting_panel, GameObject keycustom_panel, GameObject notsaved_panel)
    {
        if (issaved == true)
        {
            keycustom_panel.SetActive(false);
            setting_panel.SetActive(true);
        }
        else
        {
            notsaved_panel.SetActive(true);
        }
    }

    ///<summary>
    ///키 커스텀 인지 아닌지 확인 해서 바꾼 키 값 반환 하는 함수
    ///</summary>
    public void KeyCustomCheck(GameObject keycustom_check_panel, int key_adr, string[] key_custom_arry)
    {
        if (keycustom_check_panel.activeSelf)
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
                KeyCustomChangeImage(button, Input.inputString);
                keycustom_check_panel.SetActive(false);
            }
        }
    }
    ///<summary>
    ///키 커스텀 설정값 저장 함수
    ///</summary>
    public void SetKeyCustom(string[] key_custom_arry)
    {
        sh.SetKeyCustom(key_custom_arry);
        issaved = true;
    }
    ///<summary>
    ///키 커스텀 모함수
    ///</summary>
    public void KeyCustom(GameObject keycustom_check_panel, Button button_, int i)
    {
        uki.SetKeyADR(i);
        button = button_;
        keycustom_check_panel.SetActive(true);
        issaved = false;
    }
    /// <summary>
    /// 설정값을 저장 했는지 안했는지 판단 하는 변수 조정 함수
    /// </summary>
    public void IsSaved()
    {
        issaved = true;
    }
    public void IsNotSaved()
    {
        issaved = false;
    }
    public void ButtonClickNotSavedYes(GameObject keycustom_panel, string[] key_custom_arry, GameObject setting_panel, Slider gamemaster_sound_slider, Slider mousedpi_slider, GameObject notsaved_panel, GameObject pause_panel)
    {
        if (keycustom_panel.activeSelf)
        {
            SetKeyCustom(key_custom_arry);
        }
        else if (setting_panel.activeSelf)
        {
            uki.SaveSetting();
        }
        IsSaved();
        notsaved_panel.SetActive(false);
        if (keycustom_panel.activeSelf)
        {
            ButtonClickKeyCustomClose(setting_panel, keycustom_panel, notsaved_panel);
        }
        else if (setting_panel.activeSelf)
        {
            ButtonClickCloseSetting(pause_panel, setting_panel, notsaved_panel);
        }
    }
    public void ButtonClickNotSavedNo(GameObject keycustom_panel, GameObject pause_panel, GameObject notsaved_panel, GameObject setting_panel, Slider gamemaster_sound_slider, Slider mousedpi_slider, AudioMixer master_mixer)
    {
        uki.ImportSettingValue();
        IsSaved();
        if (keycustom_panel.activeSelf)
        {
            ButtonClickKeyCustomClose(setting_panel, keycustom_panel, notsaved_panel);
        }
        else if (setting_panel.activeSelf)
        {
            ButtonClickCloseSetting(pause_panel, setting_panel, notsaved_panel);
        }
        notsaved_panel.SetActive(false);
    }
    /// <summary>
    /// 패널창 열고 닫는 함수
    /// </summary>
    /// <param name="panel0">닫힐 창</param>
    /// <param name="panel1">열릴 창asdasd</param>
    public void ChangePanel(GameObject panel0, GameObject panel1)
    {
        panel0.SetActive(false);
        panel1.SetActive(true);
    }
    /// <summary>
    /// 해상도나 전체화면 바뀌면 모니터에 띄워주는 함수
    /// </summary>
    /// <param name="fullscreen_dropdown"></param>
    /// <param name="resolution_dropdown"></param>
    public void MonitorSize(Dropdown fullscreen_dropdown, Dropdown resolution_dropdown)
    {
        if (fullscreen_dropdown.value == 0)
        {
            switch (resolution_dropdown.value)
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
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (fullscreen_dropdown.value == 1)
        {
            switch (resolution_dropdown.value)
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
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            switch (resolution_dropdown.value)
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
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void KeyCustomChangeImage(Button button, string st)
    {
        if (keycab_list == null)
        {
            keycab_list = uki.GetSpriteKeyCab();
        }
        for (int i = 0; i < keycab_list.Length; i++)
        {
            if (keycab_list[i].name == "Icon_Key(Atlas)_" + st)
            {
                button.transform.Find("Image").GetComponentInChildren<Image>().sprite = keycab_list[i];
                break;
            }
        }
    }
}