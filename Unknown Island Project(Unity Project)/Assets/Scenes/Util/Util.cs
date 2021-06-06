using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using LitJson;
using UnityEngine.EventSystems;

public class Util
{
    public float m_DoubleClickSecond = 0.25f;
    private bool m_IsOneClick = false;
    private double m_Timer = 0;
    
    
    public Util()
    {
        
        
    }
    
    public static JsonData ItemMatch(string item_id)
    {
        JsonData item_json;
        string json_string = File.ReadAllText(Application.dataPath + "/Scenes/Item/ItemData.json");

        item_json = JsonMapper.ToObject(json_string);
        for (int i = 0; i < item_json.Count; i++)
        {
            if(item_json[i]["item_id"].ToString() == item_id)
            {
                return item_json[i];
            }
        }
        
        return null;
    }
    //public void DoubleClick()
    //{

    //    if (m_IsOneClick && ((Time.time - m_Timer) > m_DoubleClickSecond))
    //    {

    //        //Debug.Log("One Click");
    //        m_IsOneClick = false;
    //    }

    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        if (!m_IsOneClick)
    //        {
    //            m_Timer = Time.time;
    //            m_IsOneClick = true;
    //        }

    //        else if (m_IsOneClick && ((Time.time - m_Timer) < m_DoubleClickSecond))
    //        {

    //            //Debug.Log("Double Click");
    //            m_IsOneClick = false;
    //        }
    //    }
    //}
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Start");
        //throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Draging");
        //throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");
        // throw new System.NotImplementedException();
    }
    
}

