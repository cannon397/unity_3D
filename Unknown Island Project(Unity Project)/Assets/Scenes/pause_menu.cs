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
    public pause_menu()
    {
        uki = new UKI_script();
    }
    void Start()
    {

    }

    void Update()
    {

    }

    public void CheckKeyControl(GameObject pause_panel)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause_panel.activeSelf)
            {
                pause_panel.SetActive(false);
                uki.GamePause(false);
            }
            else
            {
                pause_panel.SetActive(true);
                uki.GamePause(true);
            }
        }
    }
    public void ButtonClickGameContinue(GameObject pause_panel)
    {
        pause_panel.SetActive(false);
        uki.GamePause(false);
    }
    public void ButtonClickMainMenu()
    {
        SceneManager.LoadScene("GameTitle");
    }
    public void ButtonClickOpenSetting(GameObject setting_panel, GameObject pause_panel)
    {
        setting_panel.SetActive(true);
        pause_panel.SetActive(false);
    }
    public void ButtonClickCloseSetting(GameObject setting_panel, GameObject pause_panel)
    {
        setting_panel.SetActive(false);
        pause_panel.SetActive(true);
    }
    public void SoundVolumeMaster(Slider gamemaster_sound_slider, AudioMixer master_mixer)
    {
        float volume = gamemaster_sound_slider.value;
        master_mixer.SetFloat("Master_Volume", volume);
    }
    //키 커스텀창 열기
    public void ButtonClickKeyCustomOpen(GameObject setting_panel, GameObject keycustom_panel)
    {
        setting_panel.SetActive(false);
        keycustom_panel.SetActive(true);
    }
    //키 커스텀창 닫기
    public void ButtonClickKeyCustomClose(GameObject setting_panel, GameObject keycustom_panel)
    {
        keycustom_panel.SetActive(false);
        setting_panel.SetActive(true);
    }

    //키 커스텀 인지 아닌지 확인 해서 바꾼 키 값 반환 하는 함수
    public void KeyCustomCheck(Setting_header sh, GameObject keycustom_check_panel, int key_adr, string[] key_custom_arry)
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
    public void SetKeyCustom(Setting_header sh, string[] key_custom_arry)
    {
        sh.SetKeyCustom(key_custom_arry);
    }
    //키 커스텀 모함수
    public void KeyCustom(int i, GameObject keycustom_check_panel)
    {
        uki.SetKeyADR(i);
        keycustom_check_panel.SetActive(true);
    }
    //설정값 저장 함수
    public void SaveSettingValue(Setting_header sh, Slider gamemaster_sound_slider, Slider mousedpi_slider)
    {
        sh.SetSoundMasterVolume(gamemaster_sound_slider.value);
        sh.SetMouseDpi(mousedpi_slider.value);
    }
}
