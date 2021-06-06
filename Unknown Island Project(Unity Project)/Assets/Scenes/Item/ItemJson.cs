using System.Collections;
using System.Collections.Generic;
using System;
using LitJson;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Item
{
    //1xxxx = 장비
    //2xxxx = 소비
    //3xxxx = 기타
    //4xxxx = 건축
    public int item_id; //아이템 기본키
    public string item_name; //아이템 이름
    public string item_description; //아이템 설명
    public string item_icon; //아이템 이미지
    public ItemType item_type; //아이템 종류
    public ItemStatus item_status;//아이템 상태
    public ItemCombinationFormula item_combiation_formula; //아이템 조합식

    public enum ItemType
    {

        equip = 1, //장비아이템
        use = 2, //소모아이템
        etc = 3, //기타아이템
        build = 4,//건축물

    }
    public Item(int _item_id, string _item_name, string _item_description, ItemType _item_type, ItemStatus _item_status, ItemCombinationFormula _item_combiation_formula)
    {
        item_id = _item_id;
        item_name = _item_name;
        item_description = _item_description;
        item_type = _item_type;
        item_status = _item_status;
        item_combiation_formula = _item_combiation_formula;

    }
    public Item(int _item_id, string _item_name, string _item_description, ItemType _item_type,  ItemCombinationFormula _item_combiation_formula)
    {
        item_id = _item_id;
        item_name = _item_name;
        item_description = _item_description;
        item_type = _item_type;
        
        item_combiation_formula = _item_combiation_formula;

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
public class ItemJson
{
    public List<Item> item_list = new List<Item>();
    public List<List<ItemStatus>> item_status_list = new List<List<ItemStatus>>();
    public List<List<ItemCombinationFormula>> item_combin_list = new List<List<ItemCombinationFormula>>();
    public JsonData item_data;
    public ItemJson()
    {
        
      
        //장비,소비,기타로 분류
        //장비아이템
        item_list.Add(new Item(10001, "도끼", "도끼다. 나무를 벨 수 있을 것 같다.", Item.ItemType.equip,new ItemStatus(10, 30),new ItemCombinationFormula(new Dictionary<string, int>() {{ "30001",1}, {"30002",1}, {"30003",1} })));
        item_list.Add(new Item(10002, "곡괭이", "곡괭이다. 나무를 벨 수 있을 것 같다.", Item.ItemType.equip, new ItemStatus(5, 30), new ItemCombinationFormula(new Dictionary<string, int>() { { "30001", 1 }, { "30002", 1 }, { "30003", 2 } })));
        item_list.Add(new Item(10003, "창", "창이다. 이제 싸울 수 있다.", Item.ItemType.equip, new ItemStatus(20, 30), new ItemCombinationFormula(new Dictionary<string, int>() { { "30001", 1 }, { "30002", 1 }, { "30003", 1 } })));
        item_list.Add(new Item(10004, "낚시대", "낚시대이다. 바다에서 사용해보자.", Item.ItemType.equip, new ItemStatus(0, 30), new ItemCombinationFormula(new Dictionary<string, int>() { { "30001", 1 }, { "30002", 1 } })));
        item_list.Add(new Item(10005, "통발", "통발이다. 해변가에 설치해보자.", Item.ItemType.equip, new ItemStatus(0, 15), new ItemCombinationFormula(new Dictionary<string, int>() { { "30002", 3 }, { "30005", 8 } })));
        item_list.Add(new Item(10006, "코코넛 그릇", "코코넛 그릇이다.여러가지를 담을 수 있을 것 같다.", Item.ItemType.equip, new ItemStatus(0, 0), new ItemCombinationFormula(new Dictionary<string, int>() { { "20003", 1 } })));
        item_list.Add(new Item(10007, "횃불", "횃불이다. 주변을 밝혀보자.", Item.ItemType.equip, new ItemStatus(0, 50), new ItemCombinationFormula(new Dictionary<string, int>() { { "30001", 1 }, { "30006", 1 } })));
        item_list.Add(new Item(10008, "가방", "", Item.ItemType.equip, new ItemStatus(0, 0), new ItemCombinationFormula(new Dictionary<string, int>() { { "30002", 2 }, { "30006", 4 } })));

        //소비아이템
        item_list.Add(new Item(20001, "해독제", "독을 치료할 수 있을 것 같다.", Item.ItemType.use, new ItemStatus(1, 0, 0, 0), new ItemCombinationFormula(new Dictionary<string, int>() { { "30002", 1 }, { "10006", 1 }, { "30004", 1 } })));
        item_list.Add(new Item(20002, "상처약", "상처를 치료할 수 있을 것 같다.", Item.ItemType.use, new ItemStatus(0, 30, 0, 0), new ItemCombinationFormula(new Dictionary<string, int>() { { "30006", 1 }, { "10006", 1 }, { "30004", 1 } })));
        item_list.Add(new Item(20003, "코코넛", "코코넛이다. 무인도의 좋은 식량.", Item.ItemType.use, new ItemStatus(0, 5, 10, 5), new ItemCombinationFormula()));
        item_list.Add(new Item(20004, "정어리", "정어리다. 작고 소중해.", Item.ItemType.use, new ItemStatus(0, 5, 0, 5), new ItemCombinationFormula()));
        item_list.Add(new Item(20005, "물이담긴나무그릇", "목이 말라도 문제없어.", Item.ItemType.use, new ItemStatus(0, 5, 30, 5), new ItemCombinationFormula()));
        item_list.Add(new Item(20006, "복어", "복어다. 독 조심!", Item.ItemType.use, new ItemStatus(1, -10, 0, 10), new ItemCombinationFormula()));
        item_list.Add(new Item(20007, "넙치", "넙치다. 못생겼어!", Item.ItemType.use, new ItemStatus(0, 10, 0, 20), new ItemCombinationFormula()));
        item_list.Add(new Item(20008, "도미", "도미다. 고급 생선!", Item.ItemType.use, new ItemStatus(0, 15, 0, 30), new ItemCombinationFormula()));
        item_list.Add(new Item(20009, "다랑어", "다랑어다. 이걸 잡았다고!?", Item.ItemType.use, new ItemStatus(0, 50, 0, 25), new ItemCombinationFormula()));
        item_list.Add(new Item(20010, "구운 정어리", "구운 정어리다. 작고 소중해.", Item.ItemType.use, new ItemStatus(0, 10, 0, 10), new ItemCombinationFormula(new Dictionary<string, int>() { { "20004", 1 }, { "10006", 1 } })));
        item_list.Add(new Item(20010, "구운 복어", "구운 복어다. 안심은 금물!", Item.ItemType.use, new ItemStatus(0, 10, 0, 15), new ItemCombinationFormula(new Dictionary<string, int>() { { "20006", 1 }, { "10006", 1 } })));
        item_list.Add(new Item(20012, "구운 넙치", "구운 넙치다. 못생긴건 여전해.", Item.ItemType.use, new ItemStatus(0, 25, 0, 15), new ItemCombinationFormula(new Dictionary<string, int>() { { "20007", 1 }, { "10006", 1 } })));
        item_list.Add(new Item(20013, "구운 도미", "구운 도미다. 더 고급져졌어.", Item.ItemType.use, new ItemStatus(0, 20, 0, 35), new ItemCombinationFormula(new Dictionary<string, int>() { { "20008", 1 }, { "10006", 1 } })));
        item_list.Add(new Item(20014, "구운 다랑어", "구운 다랑어다. 구워도 맛있어!", Item.ItemType.use, new ItemStatus(0, 30, 0, 55), new ItemCombinationFormula(new Dictionary<string, int>() { { "20009", 1 }, { "10006", 1 } })));

        //기타아이템
        item_list.Add(new Item(30001, "통나무", "통나무이다. 쓰임이 많을 것 같다.", Item.ItemType.etc,  new ItemCombinationFormula()));
        item_list.Add(new Item(30002, "덩굴", "덩굴이다. 묶을 때 용이하다.", Item.ItemType.etc, new ItemCombinationFormula()));
        item_list.Add(new Item(30003, "돌", "돌이다. 좋은 도구의 첫걸음이 될 것 같다.", Item.ItemType.etc,  new ItemCombinationFormula()));
        item_list.Add(new Item(30004, "꽃", "꽃이다. 좋은 효능을 가지고 있는 것 같다.", Item.ItemType.etc,  new ItemCombinationFormula()));
        item_list.Add(new Item(30005, "나뭇가지", "나뭇가지다. 있으면 좋지.", Item.ItemType.etc,  new ItemCombinationFormula()));
        item_list.Add(new Item(30006, "야자수 잎", "야자수 잎이다. 덮고자면 따듯할 것 같다.", Item.ItemType.etc,  new ItemCombinationFormula()));

        //건축아이템
        item_list.Add(new Item(40001, "상자", "5X5로 총 25칸의 아이템칸을 더 사용할 수 있다.", Item.ItemType.build, new ItemCombinationFormula(new Dictionary<string, int>() { { "30001", 3 }, { "30002", 1 } })));
        item_list.Add(new Item(40002, "집", "", Item.ItemType.build, new ItemCombinationFormula(new Dictionary<string, int>() { { "30001", 6 }, { "30002", 4 }, { "30006", 4 } })));
        item_list.Add(new Item(40003, "모닥불", "", Item.ItemType.build,  new ItemCombinationFormula(new Dictionary<string, int>() { { "30001", 2 }, { "30006", 1 }, { "30003", 3 } })));




        string item_json = JsonConvert.SerializeObject(item_list,Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/Scenes/Item/ItemData.json", item_json);
        
        
        string json_string = File.ReadAllText(Application.dataPath + "/Scenes/Item/ItemData.json");

         item_data = JsonMapper.ToObject(json_string);


    }

}
