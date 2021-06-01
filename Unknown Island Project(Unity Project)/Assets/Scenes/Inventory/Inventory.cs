using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;
using LitJson;
using System.IO;
using UnityEngine.UI;

public class Inventory
{



    ItemInfor item_Infor;
    ItemJson item_json;
    GameObject[] slot;
    
    Dictionary<int, ItemInfor> slots = new Dictionary<int, ItemInfor>();
    string string_data;

    int durability;
    int item_type;
    public Inventory(int slots, ItemJson item_json)
    {
        this.item_json = item_json;
        slot = new GameObject[20];

       
        
        for (int i = 0; i < slots; i++)
        {

            //아이템 아이콘 찾기
            //
            // if(아이템 있을 경우 (세이브 파일 불러오는 경우) )
            slot[i] = GameObject.Find("Inventory_Contents(ToggleGroup)").transform.GetChild(i).gameObject.transform.Find("ItemIcon").gameObject;
            slot[i].SetActive(false);
            this.slots.Add(i, new ItemInfor());


        }
       
        
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
                    //수량 초과 안할시
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
                        //Debug.Log("가방이 꽉참1");
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
                    //Debug.Log("가방이 꽉참1");
                    break;
                }
                if (slots[i].item_id != 0)
                {
                    //Debug.Log("겹치지 않을때 값이 있음");
                    continue;
                }
                else
                {
                    //Debug.Log("겹치지 않을때 값이 없음 넣어야함");
                    slots[i] = item_Infor;
                    slot[i].SetActive(true);
                    
                    break;
                }
                
            }

        }
        Debug.Log(JsonConvert.SerializeObject(slots, Formatting.Indented));
       

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



}

