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
        private bool fullscreen_bool = true;
        private string[] key_custom_array = new string[60];
        private string[] key_custom_availble_arry = new string[] { "Alpha1", "Alpha2", "Alpha3", "Alpha4", "Alpha5", "Alpha6", "Alpha7", "Alpha8", "Alpha9", "Alpha0", "Comma", "Period", "Slash",
            "Semicolon", "BackQuote", "LeftCurlyBracket", "RightCurlyBracket", "Mouse3", "Mouse4", "Quote", "Equals", "Minus", "LeftShift", "RightShift", "RightControl", "LeftControl",
            "RightAlt", "LeftAlt", "Tab", "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "k", "l", "z", "x", "c", "v", "b", "n", "m"};
        /* 키 바인딩
         * 0 : 인칭 변환         (기본값 : v)
         * 1 : 상호작용          (기본값 : e)
         * 2 : 인벤토리          (기본값 : i)
         */
        private DBAccess db;
        private String settings_table = "settings";
        private String keycustom_table = "key_custom";

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
        public int GetMonitorDV()
        {

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
        }
        //전체화면 유무 값
        public bool GetFullscreenBool()
        {
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
            if (b == true)
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
        public float GetSoundMasterVolume()
        {

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
        }
        //마우스 감도 값
        public float GetMouseDpi()
        {
            String[] db_cols = { "mouse_dpi" };
            String[] emptyStringArray = new string[0];
            String[] operation = { "=" };



            m_reader = db.SelectWhere(settings_table, db_cols, where, operation, where_value);
            m_reader.Read();

            return m_reader.GetFloat(0);

        }
        public void SetMouseDpi(float f)
        {
            String[] db_mouse_dp_value = { f.ToString() };
            String[] db_cols = { "mouse_dpi" };
            db.UpdateInto(settings_table, db_cols, db_mouse_dp_value, "no", "1");
        }
        //키 커스텀 
        public void SetKeyCustom(string[] st_arry)
        {
            string st = "'";
            for (int i = 0; i < st_arry.Length; i++)
            {
                if (i == st_arry.Length - 1)
                {
                    st += st_arry[i];
                }
                else
                {
                    st += st_arry[i] + "_";
                }
            }
            st += "'";
            String[] db_key_custom = { st };
            String[] db_cols = { "key_custom_value_arry" };
            db.UpdateInto(keycustom_table, db_cols, db_key_custom, "no", "1");
        }
        //키 커스텀 배열 동기화
        public string[] SyncKeyCustom()
        {
            String[] db_cols = { "key_custom_value_arry" };
            String[] operation = { "=" };
            m_reader = db.SelectWhere(keycustom_table, db_cols, where, operation, where_value);
            m_reader.Read();

            int i = 0;
            int a = 0;
            string st = null;
            foreach (char c in m_reader.GetString(0))
            {
                if (c == '_')
                {
                    key_custom_array[i] = st;
                    i++;
                    st = null;
                }
                else
                {
                    st += c;
                    if(a == m_reader.GetString(0).Length - 1)
                    {
                        key_custom_array[i] = st;
                    }
                }
                a++;
            }
            key_custom_array = ZeroErase(key_custom_array);
            return key_custom_array;
        }
        //키 커스텀 가능한 키 입력인지 확인
        public bool CheckKeyCustomAvble(string st)
        {
            for (int i = 0; i < key_custom_availble_arry.Length; i++)
            {
                if (st == key_custom_availble_arry[i])
                {
                    return true;
                }
            }
            return false;
        }
        //string[] 에서 null값 없애주는 함수
        private string[] ZeroErase(string[] ary)
        {
            var temp = new List<string>();
            foreach (var s in ary)
            {
                if (!string.IsNullOrEmpty(s))
                    temp.Add(s);
            }
            return temp.ToArray();
        }
    }
}
