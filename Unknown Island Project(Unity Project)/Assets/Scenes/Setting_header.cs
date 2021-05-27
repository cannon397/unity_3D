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
        private string[] key_custom_array = new string[60];
        private float[] volume_arry = new float[10];
        private string[] key_custom_availble_arry = new string[] { "Alpha1", "Alpha2", "Alpha3", "Alpha4", "Alpha5", "Alpha6", "Alpha7", "Alpha8", "Alpha9", "Alpha0", "Comma", "Period", "Slash",
            "Semicolon", "BackQuote", "LeftBracket", "RightBracket", "Mouse3", "Mouse4", "Quote", "Equals", "Minus", "LeftShift", "RightShift", "RightControl", "LeftControl", "Backslash", 
            "RightAlt", "LeftAlt", "Tab", "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "k", "l", "z", "x", "c", "v", "b", "n", "m"};
        /* 키 바인딩
         * 0 : 인칭 변환         (기본값 : v)
         * 1 : 아이템 줍기       (기본값 : e)
         * 2 : 아이템 버리기     (기본값 : q)
         * 3 : 시계 확인         (기본값 : C)
         * 4 : 가방/제작         (기본값 : Tab)
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
        /// <summary>
        /// 모니터 해상도 Get
        /// </summary>
        /// <returns></returns>
        public int GetMonitorDV()
        {

            String[] db_cols = { "monitor_dropdown_value" };
            String[] operation = { "=" };
            m_reader = db.SelectWhere(settings_table, db_cols, where, operation, where_value);
            m_reader.Read();

            return m_reader.GetInt16(0);
        }
        /// <summary>
        /// 모니터 해상도 Set
        /// </summary>
        /// <param name="i"></param>
        public void SetMonitorDV(int i)
        {
            String[] db_monitor_dv_value = { i.ToString() };
            String[] db_cols = { "monitor_dropdown_value" };
            db.UpdateInto(settings_table, db_cols, db_monitor_dv_value, "no", "1");
        }
        /// <summary>
        /// 전체화면 값 Get
        /// </summary>
        /// <returns></returns>
        public int GetFullscreen()
        {
            String[] db_cols = { "fullscreen" };
            String[] operation = { "=" };
            m_reader = db.SelectWhere(settings_table, db_cols, where, operation, where_value);
            m_reader.Read();
            return m_reader.GetInt32(0);
        }
        /// <summary>
        /// 전체화면 값 Set
        /// </summary>
        /// <param name="b"></param>
        public void SetFullscreen(int i)
        {
            string st;
            st = i.ToString();
            String[] db_fullscreen_bool = { st };
            String[] db_cols = { "fullscreen" };
            db.UpdateInto(settings_table, db_cols, db_fullscreen_bool, "no", "1");
        }
        /// <summary>
        /// 마스터 볼륨 값 Get
        /// </summary>
        /// <returns></returns>
        public float[] GetSoundMasterVolume()
        {
            String[] db_cols = { "sound_master_volume" };
            String[] operation = { "=" };
            m_reader = db.SelectWhere(settings_table, db_cols, where, operation, where_value);
            m_reader.Read();
            string st = null;
            int i = 0;
            int a = 0;
            foreach (char c in m_reader.GetString(0))
            {
                if (c == '_')
                {
                    volume_arry[i] = float.Parse(st);
                    st = null;
                }
                else
                {
                    st += c;
                    if (a == m_reader.GetString(0).Length - 1)
                    {
                        volume_arry[i] = float.Parse(st);
                    }
                }
                a++;
            }
            volume_arry = ZeroErase(volume_arry);

            return volume_arry;
        }
        /// <summary>
        /// 마스터 볼륨 값 Set
        /// </summary>
        /// <param name="f"></param>
        public void SetSoundMasterVolume(float[] arr)
        {
            string st = "'";
            for (int i = 0; i < arr.Length; i++)
            {
                if (i == arr.Length - 1)
                {
                    st += arr[i].ToString();
                }
                else
                {
                    st += arr[i].ToString() + "_";
                }
            }
            st += "'";
            String[] db_master_volume_value = { st };
            String[] db_cols = { "sound_master_volume" };
            db.UpdateInto(settings_table, db_cols, db_master_volume_value, "no", "1");
        }
        /// <summary>
        /// 마우스 감도 값 Get
        /// </summary>
        /// <returns></returns>
        public float GetMouseDpi()
        {
            String[] db_cols = { "mouse_dpi" };
            String[] emptyStringArray = new string[0];
            String[] operation = { "=" };



            m_reader = db.SelectWhere(settings_table, db_cols, where, operation, where_value);
            m_reader.Read();

            return m_reader.GetFloat(0);

        }
        /// <summary>
        /// 마우스 감도 값 Set
        /// </summary>
        /// <param name="f"></param>
        public void SetMouseDpi(float f)
        {
            String[] db_mouse_dp_value = { f.ToString() };
            String[] db_cols = { "mouse_dpi" };
            db.UpdateInto(settings_table, db_cols, db_mouse_dp_value, "no", "1");
        }
        /// <summary>
        /// 키 커스텀 데이터 베이스에 문자열로 저장
        /// </summary>
        /// <param name="st_arry"></param>
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
        /// <summary>
        /// 키 커스텀 배열 데이터 베이스에서 받아와서 string[] 로 반환
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 키 커스텀 가능한 키 입력인지 확인
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 배열에서 null값 확인해서 없애주는 함수
        /// </summary>
        /// <param name="ary"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 배열에서 null값 확인해서 없애주는 함수
        /// </summary>
        /// <param name="ary"></param>
        /// <returns></returns>
        private float[] ZeroErase(float[] ary)
        {
            var temp = new List<float>();
            foreach (var s in ary)
            {
                if (!string.IsNullOrEmpty(s.ToString()))
                    temp.Add(s);
            }
            return temp.ToArray();
        }
    }
}
