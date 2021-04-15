using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scenes
{
    class Setting_header
    {
        private int monitor_dropdown_value = 2;
        private bool fullscreen_bool = true;
        private float sound_master_volume = 0;
        private float mouse_dpi = 1;
        private string key_biding_point;
        private string[] key_biding;
        /* 키 바인딩
         * 0 : 인칭 변환 (기본값 : V)
         */

        //모니터 해상도값
        public int GetMonitorDV() { return(monitor_dropdown_value); }
        public void SetMonitorDV(int i) { monitor_dropdown_value = i; }
        //전체화면 유무 값
        public bool GetFullscreenBool() { return (fullscreen_bool); }
        public void SetFullscreenBool(bool b) { fullscreen_bool = b; }
        //마스터 볼륨 값
        public float GetSoundMasterVolume() { return (sound_master_volume); }
        public void SetSoundMasterVolume(float f) { sound_master_volume = f; }
        //마우스 감도 값
        public float GetMouseDpi() { return (mouse_dpi); }
        public void SetMouseDpi(float f) { mouse_dpi = f; }
        //키 바인딩 배열
        public string GetKeyBiding(int i) { return key_biding[i]; }
        public void SetKeyBiding(string st, int i) { key_biding[i] = st; }
        //키 바인딩 입력값
        public string GetKeyBidingPoint() { return key_biding_point; }
        public void SetKeyBidingPoint(string st) { key_biding_point = st; }

        static void Main(string[] args)
        {
            
        }
    }
}
