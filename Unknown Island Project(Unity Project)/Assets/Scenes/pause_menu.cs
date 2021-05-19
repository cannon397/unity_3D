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
    ///설정창 여는 함수
    ///</summary>
    public void ButtonClickOpenSetting(GameObject pause_panel, GameObject setting_panel)
    {
        setting_panel.SetActive(true);
        pause_panel.SetActive(false);
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
    public void SoundVolumeMaster(Slider gamemaster_sound_slider, AudioMixer master_mixer)
    {
        float volume = gamemaster_sound_slider.value;
        master_mixer.SetFloat("Master_Volume", volume);
        issaved = false;
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
                keycustom_check_panel.SetActive(false);
            }
        }
    }
    ///<summary>
    ///키 커스텀 설정값 저장 함수
    ///</summary>
    public void SetKeyCustom(string[] key_custom_arry)
    {
        Debug.Log("키 커스텀 배열 : " + key_custom_arry[0] + key_custom_arry[1] + key_custom_arry[2]);
        sh.SetKeyCustom(key_custom_arry);
        issaved = true;
    }
    ///<summary>
    ///키 커스텀 모함수
    ///</summary>
    public void KeyCustom(GameObject keycustom_check_panel, int i)
    {
        uki.SetKeyADR(i);
        keycustom_check_panel.SetActive(true);
        issaved = false;
    }
    ///<summary>
    ///설정값 저장 함수
    ///</summary>
    public void SaveSettingValue(Slider gamemaster_sound_slider, Slider mousedpi_slider)
    {
        sh.SetSoundMasterVolume(gamemaster_sound_slider.value);
        sh.SetMouseDpi(mousedpi_slider.value);
        uki.SetDPI(mousedpi_slider.value / 50);
        issaved = true;
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
            SaveSettingValue(gamemaster_sound_slider, mousedpi_slider);
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
        float volume = sh.GetSoundMasterVolume();
        gamemaster_sound_slider.value = volume;
        master_mixer.SetFloat("Master_Volume", volume);
        mousedpi_slider.value = sh.GetMouseDpi();
        uki.SetDPI(mousedpi_slider.value / 50);
        uki.SetKeyArr(sh.SyncKeyCustom());
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
}
