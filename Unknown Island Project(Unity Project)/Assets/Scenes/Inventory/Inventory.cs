using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;
using LitJson;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory
{
    ItemInfor item_Infor;
    ItemJson item_json;
    GameObject[] slot;
    ToggleGroup toggle;
    Dictionary<int, ItemInfor> slots = new Dictionary<int, ItemInfor>();
    string string_data;
    Text[] text;
    Sprite[] arr;

    string Untagged = "Untagged";

    int durability;
    int item_type;
    GraphicRaycaster gr;
    PointerEventData m_ped;

    //더블클릭 함수
    public float m_DoubleClickSecond = 0.25f;
    private bool m_IsOneClick = false;
    private double m_Timer = 0;
    
    public Inventory(int slots, ItemJson item_json)
    {
         gr = GameObject.Find("Inventory_Contents(ToggleGroup)").GetComponent<GraphicRaycaster>();
        m_ped = new PointerEventData(null);

        this.item_json = item_json;
        slot = new GameObject[20];
        arr = Resources.LoadAll<Sprite>("ItemIcon_Atlas");

        text = GameObject.Find("Inventory_ItemInfor").GetComponentsInChildren<Text>();
        GameObject.Find("SelectItemIcon").GetComponent<Image>().sprite = LoadSprite(Application.dataPath + "/Imgame_UI/Inventory_SelectedItemIcon.png");

        for (int i = 0; i < slots; i++)
        {
            //아이템 아이콘 찾기
            //
            // if(아이템 있을 경우 (세이브 파일 불러오는 경우) )
            slot[i] = GameObject.Find("Inventory_Contents(ToggleGroup)").transform.GetChild(i).gameObject;
            slot[i].transform.Find("ItemIcon").gameObject.SetActive(false);
            
            this.slots.Add(i, new ItemInfor());
            
        }
        
        toggle = GameObject.Find("Inventory_Contents(ToggleGroup)").GetComponent<ToggleGroup>();
        //Debug.Log(toggle.ActiveToggles().FirstOrDefault());
        toggle.ActiveToggles().FirstOrDefault().onValueChanged.AddListener(delegate {
            string item_name = toggle.ActiveToggles().FirstOrDefault().transform.Find("ItemIcon").transform.tag;
            //Debug.Log(item_name);
            if (item_name != "Untagged")
            {
                //아이템 데이터
                JsonData item_data = Util.ItemMatch(item_name);
                //아이템 이름
                GameObject.Find("Inventory_SelectItemName(Text)").GetComponent<Text>().text = item_data["item_name"].ToString();
                //아이템 설명
                GameObject.Find("Inventory_SelectItemInfo(Text)").GetComponent<Text>().text = item_data["item_description"].ToString();

                //배고픔
                if(int.Parse(item_data["item_status"]["hunger"].ToString()) < 0)
                {
                    GameObject.Find("Inventory_HungerStat(Text)").GetComponent<Text>().text = "-" + item_data["item_status"]["hunger"].ToString();
                }
                else
                {
                    GameObject.Find("Inventory_HungerStat(Text)").GetComponent<Text>().text = "+" + item_data["item_status"]["hunger"].ToString();
                }
                //목마름
                if (int.Parse(item_data["item_status"]["misture"].ToString()) < 0)
                {
                    GameObject.Find("Inventory_ThirstStat(Text)").GetComponent<Text>().text = "-" + item_data["item_status"]["misture"].ToString();
                }
                else
                {
                    GameObject.Find("Inventory_ThirstStat(Text)").GetComponent<Text>().text = "+" + item_data["item_status"]["misture"].ToString();
                }

                
                GameObject.Find("SelectItemIcon").GetComponent<Image>().sprite = arr[0];
                


            }
            else
            {
                

                GameObject.Find("SelectItemIcon").GetComponent<Image>().sprite = LoadSprite(Application.dataPath + "/Imgame_UI/Inventory_SelectedItemIcon.png");
                foreach (Text text in text)
                {
                    text.text="";
                }
                

            }
            //Debug.Log(Util.ItemMatch(item_name)["item_id"]);



        });



        string_data = JsonConvert.SerializeObject(this.slots, Formatting.Indented);    
    }


    public void Add(int item_id, int item_amount)
    {
        int count = item_json.item_data.Count;    
        //아이템 매칭후 찾은데이터 삽입
        for (int i = 0; i < count; i++)
        {
            if (item_json.item_data[i]["item_id"].ToString() == item_id.ToString())
            {
                //Debug.Log("아이템 데이터 매칭");
               durability = int.Parse(item_json.item_data[i]["item_status"]["durability"].ToString());
               item_type = int.Parse(item_json.item_data[i]["item_type"].ToString());
               item_Infor = new ItemInfor(item_id, item_amount, durability, item_type);
            }
        }

        //슬릇 빈칸인지 확인 빈칸이면 넣고 빈칸아니면 아이템 아이디 확인

        for (int i = 0; i < slots.Count; i++)
        {
            //겹칠때
            if (slots[i].item_id.ToString() == item_id.ToString())
            {
                //Debug.Log("기존 아이템과 겹침");
                if (item_type == 1)
                {
                    if (i == 19 && slots[19].item_id != 0)
                    {
                        //Debug.Log("가방이 꽉참");
                        break;
                    }
                    //Debug.Log("장비아이템");
                    continue;
                }
                int slot_item_amount = slots[i].amount;
                //수량체크
                if (slot_item_amount < 40)
                {
                    //Debug.Log("수량 40 미만");
                    //기존수량 + 획득수량이 40 초과할시
                    if (slot_item_amount + item_amount > 40)
                    {
                        Debug.Log("기존 수량과 더했을때 40을 넘길경우");
                        int remain = slot_item_amount + item_amount - 40;
                        slots[i].amount = 40;
                        for (int a = i+1; a < slots.Count; a++)
                        {
                            if(slots[a].item_id.ToString() == item_id.ToString())
                            {
                                slots[a].amount = slots[a].amount + remain;
                            }
                            else
                            {
                                //Debug.Log("가방이 꽉참");
                            }
                            //Debug.Log("나머지 버림");
                        }
                    }
                    //수량 40 미만 안할시
                    else
                    {
                        slots[i].amount = slot_item_amount + item_amount;
                    }
                    
                    break;
                }
                else
                {
                    if (i == 19 && slots[19].item_id !=0)
                    {
                        //Debug.Log("가방이 꽉참");
                        break;
                    }
                    continue;
                }

                //수량 제한 검사
            }
            //겹치지 않을때
            else
            {
                if (i == 19 && slots[19].item_id != 0)
                {
                    //Debug.Log("가방이 꽉참");
                    break;
                }
                if (slots[i].item_id != 0)
                {
                    //Debug.Log("겹치지 않음, 값이 있음");
                    continue;
                }
                else
                {
                   
                     
                    //Debug.Log("겹치지 않음, 값이 없음");
                    slot[i].transform.Find("ItemIcon").gameObject.gameObject.GetComponent<Image>().sprite = arr[0];
                    slots[i] = item_Infor;
                    slot[i].transform.Find("ItemIcon").gameObject.SetActive(true);
                    slot[i].transform.Find("ItemIcon").gameObject.tag = slots[i].item_id.ToString();
                    
                    break;
                }          
            }
        }
        //Debug.Log(JsonConvert.SerializeObject(slots, Formatting.Indented));
    }
    public void remove()
    {
        //버리기 기능
    }
    public void use(int item_type)
    {
        //사용
        //item_type
    }
    protected class ItemInfor
    {
        public int amount;
        public int durability;
        public int item_id;
        public int item_type;
        public ItemInfor(int item_id, int amount, int durability, int item_type)
        {
            this.item_id = item_id;
            this.amount = amount;
            this.durability = durability;
            this.item_type = item_type;
        }
        public ItemInfor()
        {
           
        }

    }


    private Sprite LoadSprite(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        if (System.IO.File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;
    }

    public void InventoryDoubleClick()
    {

        if (m_IsOneClick && ((Time.time - m_Timer) > m_DoubleClickSecond))
        {

            //Debug.Log("One Click");
            m_IsOneClick = false;
        }

        if (Input.GetMouseButtonDown(0))
        {


            if (!m_IsOneClick)
            {
                m_Timer = Time.time;
                m_IsOneClick = true;
            }

            else if (m_IsOneClick && ((Time.time - m_Timer) < m_DoubleClickSecond))
            {

                m_ped.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                gr.Raycast(m_ped, results);

                if (results.Count > 0)
                {

                    Debug.Log("오브젝트 : " + results[0].gameObject.transform.parent.gameObject.name);
                    string item_id = toggle.ActiveToggles().FirstOrDefault().gameObject.transform.Find("ItemIcon").tag;
                    if (results[0].gameObject.transform.parent.name == toggle.ActiveToggles().FirstOrDefault().name)
                    {
                        Debug.Log("같음");
                        if (item_id != Untagged)
                        {
                            string item_type = item_id.Substring(0, 1);
                            Debug.Log("item_type : " + item_type);
                            switch (item_type)
                            {
                                case "1":
                                    break;
                                case "2":
                                    UseConsumableItem();
                                    break;
                                case "3":
                                    break;
                            }

                        }
                    }
                }



                
                //Debug.Log(JsonConvert.SerializeObject(slots, Formatting.Indented));
                //Debug.Log(slot[0]);
                //Debug.Log(toggle.ActiveToggles().FirstOrDefault().name);
                //Debug.Log(toggle.ActiveToggles().FirstOrDefault().gameObject.transform.Find("ItemIcon").tag);

                //Debug.Log("Double Click");
                m_IsOneClick = false;
            }
        }
    }

    //소비아이템
    private void UseConsumableItem()
    {
        //아이템 사용 로직
        for (int i = 0; i < slot.Length; i++)
        {
            if (slot[i].name == toggle.ActiveToggles().FirstOrDefault().name)
            {
                slots[i].amount = slots[i].amount - 1;
                if(slots[i].amount == 0)
                {
                    slot[i].transform.Find("ItemIcon").tag = Untagged;
                    slot[i].transform.Find("ItemIcon").GetComponent<Image>().sprite = null;
                    slot[i].transform.Find("ItemIcon").gameObject.SetActive(false);
                    slots[i] = new ItemInfor();
                }
            }
        }
    }
   public void drag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("마우스 클릭중");
        }
        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("마우스 뗏음");
        }
    }
}

