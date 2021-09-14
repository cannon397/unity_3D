using LitJson;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;


    // 필요한 컴포넌트
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotsParent;

    //아이템 정보
    public Text ItemInfor_ItemName;
    public Text ItemInfor_ItemDescription;
    public Text ItemInfor_ThirstStat;
    public Text ItemInfor_HungerStat;
    public Image ItemInfor_Image;

    // 슬롯들.
    [HideInInspector]
    public Slot[] slots;



    // Start is called before the first frame update
    void Start()
    {
        
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
        ItemInfor();
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
        
    }
    //키보드 인벤토리 열기
    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }
    }
    //인벤토리 열기
    private void OpenInventory()
    {

        go_InventoryBase.SetActive(true);
    }

    //인벤토리 닫기
    private void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
    }


    //아이템 습득
    public void AcquireItem(Item item, int _count = 1)
    {


        //장비아이템 제외 소비,기타,등등
        if (Item.ItemType.equip != item.item_type)
        {

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.item_id == item.item_id)
                    {
                        slots[i].SetSlotCount(_count);

                        return;
                    }
                }
            }
        }
        //장비아이템 습득
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(item, _count);
               
                return;
            }
        }
    }
    //아이템 정보
    private void ItemInfor()
    {
        for (int i = 0; i < go_SlotsParent.transform.childCount; i++)
        {
            int tempVar = i;
            go_SlotsParent.transform.GetChild(i).GetComponentInChildren<Toggle>().onValueChanged.AddListener((tog) => {
               
                if (tog == true)
                {
                    Color color = ItemInfor_Image.color;
                    if (slots[tempVar].item != null)
                    {
                        ItemInfor_ItemName.text = slots[tempVar].item.item_name;
                        ItemInfor_ItemDescription.text = slots[tempVar].item.item_description;
                        ItemInfor_Image.sprite = slots[tempVar].item.item_icon;
                        
                        color.a = 1;
                        ItemInfor_Image.color = color;
                    }
                    else
                    {
                        ItemInfor_ItemName.text = "";
                        ItemInfor_ItemDescription.text = "";
                        ItemInfor_Image.sprite = null;

                        color.a = 0;
                        ItemInfor_Image.color = color;
                    }

                }
            });
        }
    }
}
