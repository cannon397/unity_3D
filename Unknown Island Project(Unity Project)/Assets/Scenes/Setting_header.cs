using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Mono.Data.Sqlite;

namespace Assets.Scenes
{
    public class Setting_header
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
        private DBAccess db;
        private String settings_table = "settings";

        String[] where = { "no" };
        String[] where_value = { "1" };

        SqliteDataReader m_reader;

        public Setting_header(DBAccess db)
        {
            this.db = db;
        }
        public Setting_header()
        {
           
        }

        //모니터 해상도값
        public int GetMonitorDV() {

            String[] db_cols = { "monitor_dropdown_value" };
            String[] operation = { "=" };
            m_reader = db.SelectWhere(settings_table, db_cols, where, operation, where_value);
            m_reader.Read();

            return m_reader.GetInt16(0);
        }
        public void SetMonitorDV(int i)
        {
            String[] db_monitor_dv_value = { i.ToString() };
            String[] db_cols = { "monitor_dropdown_value" };
            db.UpdateInto(settings_table, db_cols, db_monitor_dv_value, "no", "1");
            monitor_dropdown_value = i;
        }
        //전체화면 유무 값
        public bool GetFullscreenBool() {
            String[] db_cols = { "fullscreen" };
            String[] operation = { "=" };
            m_reader = db.SelectWhere(settings_table, db_cols, where, operation, where_value);
            m_reader.Read();
            if (m_reader.GetInt16(0) == 1)
            {
                fullscreen_bool = true;
            }
            else
            {
                fullscreen_bool = false;
            }
            return (fullscreen_bool);
        }
        public void SetFullscreenBool(bool b)
        {
            string i;
            if(b == true)
            {
                i = "1";
            }
            else
            {
                i = "0";
            }
            String[] db_fullscreen_bool = { i };
            String[] db_cols = { "fullscreen" };
            db.UpdateInto(settings_table, db_cols, db_fullscreen_bool, "no", "1");
            fullscreen_bool = b; 
        }
        //마스터 볼륨 값
        public float GetSoundMasterVolume() {

            String[] db_cols = { "sound_master_volume" };
            String[] operation = { "=" };
            m_reader = db.SelectWhere(settings_table, db_cols, where, operation, where_value);
            m_reader.Read();

            return m_reader.GetFloat(0);
        }
        public void SetSoundMasterVolume(float f)
        {
            
            String[] db_master_volume_value = { f.ToString() };
            String[] db_cols = { "sound_master_volume" };
            db.UpdateInto(settings_table, db_cols, db_master_volume_value, "no", "1");
            //db.CloseSqlConnection();
            Debug.Log(db);
        }
        //마우스 감도 값
        public float GetMouseDpi()
        {
            String[] db_cols = { "mouse_dpi" };
            String[] emptyStringArray = new string[0];
            String[] operation = { "=" };



            m_reader = db.SelectWhere(settings_table, db_cols, where, operation, where_value);
            m_reader.Read();
            
                Debug.Log("db mouse_dpi : " + m_reader.GetFloat(0));
                return m_reader.GetFloat(0);

        }
        public void SetMouseDpi(float f)
        {
            
            String[] db_mouse_dp_value = { f.ToString() };
            String[] db_cols = { "mouse_dpi" };
            db.UpdateInto(settings_table, db_cols, db_mouse_dp_value, "no", "1");
            
            mouse_dpi = f;
        }
        //키 바인딩 배열
        public string GetKeyBiding(int i) { return "V"; /*key_biding[i]*/ }
        public void SetKeyBiding(string st, int i) { key_biding[i] = st; }
        //키 바인딩 입력값
        public string GetKeyBidingPoint() { return key_biding_point; }
        public void SetKeyBidingPoint(string st) { key_biding_point = st; }

        static void Main(string[] args)
        {
            
        }
    }
}
