using System.Collections;
using System.Collections.Generic;
using System;
using LitJson;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    //1xxxx = 장비
    //2xxxx = 소비
    //3xxxx = 기타
    //4xxxx = 건축
    public int item_id; //아이템 기본키
    public string item_name; //아이템 이름
    public string item_description; //아이템 설명
    public Sprite item_icon; //아이템 이미지
    public ItemType item_type; //아이템 종류
    public ItemStatus item_status;//아이템 상태
    public ItemCombinationFormula item_combiation_formula; //아이템 조합식
    public GameObject item_prefab; //아이템 프리팹


    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        public List<TKey> g_InspectorKeys;
        public List<TValue> g_InspectorValues;

        public SerializableDictionary()
        {
            g_InspectorKeys = new List<TKey>();
            g_InspectorValues = new List<TValue>();
            SyncInspectorFromDictionary();
        }
        /// <summary>
        /// 새로운 KeyValuePair을 추가하며, 인스펙터도 업데이트
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            SyncInspectorFromDictionary();
        }
        /// <summary>
        /// KeyValuePair을 삭제하며, 인스펙터도 업데이트
        /// </summary>
        /// <param name="key"></param>
        public new void Remove(TKey key)
        {
            base.Remove(key);
            SyncInspectorFromDictionary();
        }

        public void OnBeforeSerialize()
        {
        }
        /// <summary>
        /// 인스펙터를 딕셔너리로 초기화
        /// </summary>
        public void SyncInspectorFromDictionary()
        {
            //인스펙터 키 밸류 리스트 초기화
            g_InspectorKeys.Clear();
            g_InspectorValues.Clear();

            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                g_InspectorKeys.Add(pair.Key); g_InspectorValues.Add(pair.Value);
            }
        }

        /// <summary>
        /// 딕셔너리를 인스펙터로 초기화
        /// </summary>
        public void SyncDictionaryFromInspector()
        {
            //딕셔너리 키 밸류 리스트 초기화
            foreach (var key in Keys.ToList())
            {
                base.Remove(key);
            }

            for (int i = 0; i < g_InspectorKeys.Count; i++)
            {
                //중복된 키가 있다면 에러 출력
                if (this.ContainsKey(g_InspectorKeys[i]))
                {
                    Debug.LogError("중복된 키가 있습니다.");
                    break;
                }
                base.Add(g_InspectorKeys[i], g_InspectorValues[i]);
            }
        }

        public void OnAfterDeserialize()
        {
            //Debug.Log(this + string.Format("인스펙터 키 수 : {0} 값 수 : {1}", g_InspectorKeys.Count, g_InspectorValues.Count));

            //인스펙터의 Key Value가 KeyValuePair 형태를 띌 경우
            if (g_InspectorKeys.Count == g_InspectorValues.Count)
            {
                SyncDictionaryFromInspector();
            }
        }
    }
    [System.Serializable]
    public class SerializeDicEntity : SerializableDictionary<GameObject, int>
    {

    }

    //Entitys 코드 분류에 따라, 엔티티 프리팹을 딕셔너리에 저장 
    public SerializeDicEntity item_combin_infor;




    public enum ItemType
    {

        equip = 1, //장비아이템
        use = 2, //소모아이템
        etc = 3, //기타아이템
        build = 4,//건축물

    }
    




}
public class ItemCombinationFormula
{
    public Dictionary<string, int> data;
   
    public ItemCombinationFormula(Dictionary<string, int> _data)
    {
        data = _data;
    }
    public ItemCombinationFormula()
    {

    }
}
public class ItemStatus
{
    public int atk;
    public int durability;
    public int decoding;
    public int hp;
    public int misture;
    public int hunger;
    public ItemStatus(int _atk, int _durability)
    {
        atk = _atk;
        durability = _durability;
        
    }
    public ItemStatus( int _decoding, int _hp, int _misture, int _hunger)
    {
        decoding = _decoding;
        hp = _hp;
        misture = _misture;
        hunger = _hunger;
    }
}

