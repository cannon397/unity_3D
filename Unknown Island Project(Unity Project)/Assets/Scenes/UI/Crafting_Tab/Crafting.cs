using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    public static bool CraftingActivated = false;

    [SerializeField]
    private GameObject crafting_obj;
    [SerializeField]
    private GameObject crafting_tab;

    ItemToggle[] item_toggle;
    [SerializeField]
    private GameObject[] array_tab;
    //인벤토리정보
    [SerializeField]
    private GameObject inventory;

    //조합정보창
    public Image Tab_MakingIcon;
    public Text Tab_Making_Name;
    public Text Tab_Making_Info;

    //제작버튼
    public Button crafting_button;

    private Item item;
    // Start is called before the first frame update
    void Start()
    {
        item_toggle = crafting_obj.GetComponentsInChildren<ItemToggle>(true);
        SelectCraftingTab();
        ItemCombination();

    }

    // Update is called once per frame
    void Update()
    {
        TryOpenCraftingTab();
    }
    //조합창 열기,닫기
    private void TryOpenCraftingTab()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CraftingActivated = !CraftingActivated;

            if (CraftingActivated)
                OpenCraftingTab();
            else
                CloseCraftingTab();
        }
    }
    //조합창 열기
    private void OpenCraftingTab()
    {
        crafting_obj.SetActive(true);
    }
    //조합창 닫기
    private void CloseCraftingTab()
    {
        crafting_obj.SetActive(false);
    }
    //조합창 탭변경
    private void SelectCraftingTab()
    {
        for (int i = 0; i < crafting_tab.transform.childCount; i++)
        {
            int tempVar = i;
            crafting_tab.transform.GetChild(i).GetComponentInChildren<Toggle>().onValueChanged.AddListener((tog) =>{
                if (tog == true)
                {

                    
                    for(int a = 0; a < array_tab.Length; a++)
                    {
                        
                        if (crafting_tab.transform.GetChild(tempVar).gameObject.name.Contains(array_tab[a].tag))
                        {
                            array_tab[a].gameObject.SetActive(true);
                        }
                        else
                        {
                            array_tab[a].gameObject.SetActive(false);
                        }
                    }

                }


            });
               
         }
    }
    private void CrantingInfor(Item _item)
    {
        Tab_MakingIcon.sprite = _item.item_icon;
        Tab_Making_Info.text = _item.item_description;
        Tab_Making_Name.text = _item.item_name;
        int i = 0;
        item = _item;

        Color color;
        for (int x = 0; x < GameObject.Find("Tab_Making_Content(Image)").transform.childCount; x++)
        {
            try
            {
                if (_item.item_combin_infor.g_InspectorKeys[x].GetComponent<ItemPickUp>() != null)
                {
                    color = GameObject.Find("Tab_Making_Content(Image)").transform.GetChild(x).GetComponent<Image>().color;
                    color.a = 1;
                    GameObject.Find("Tab_Making_Content(Image)").transform.GetChild(x).GetComponent<Image>().color = color;
                    GameObject.Find("Tab_Making_Content(Image)").transform.GetChild(x).gameObject.GetComponent<Image>().sprite = _item.item_combin_infor.g_InspectorKeys[x].GetComponent<ItemPickUp>().item.item_icon;
                    for (int a = 0; a < inventory.GetComponent<Inventory>().slots.Length; a++)
                    {

                        if (inventory.GetComponent<Inventory>().slots[a].item != null)
                        {
                           
                            if (inventory.GetComponent<Inventory>().slots[a].item.item_id == _item.item_combin_infor.g_InspectorKeys[x].GetComponent<ItemPickUp>().item.item_id)
                            {
                                
                                GameObject.Find("Tab_Making_Content(Image)").transform.GetChild(x).Find("Recipe").GetComponent<Text>().text = inventory.GetComponent<Inventory>().slots[a].item_count + "/" + _item.item_combin_infor.g_InspectorValues[x];
                                break;
                            }

                        }
                        else
                        {
                            GameObject.Find("Tab_Making_Content(Image)").transform.GetChild(x).Find("Recipe").GetComponent<Text>().text = "0/" + _item.item_combin_infor.g_InspectorValues[x];
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                GameObject.Find("Tab_Making_Content(Image)").transform.GetChild(x).Find("Recipe").GetComponent<Text>().text = "";
                GameObject.Find("Tab_Making_Content(Image)").transform.GetChild(x).GetComponent<Image>().sprite = null;

                color = GameObject.Find("Tab_Making_Content(Image)").transform.GetChild(x).GetComponent<Image>().color;
                color.a = 0;
                GameObject.Find("Tab_Making_Content(Image)").transform.GetChild(x).GetComponent<Image>().color = color;
            }

        }


    }
    private void ItemCombination()
    {
        bool[] check_item; 
        crafting_button.onClick.AddListener(delegate
        {
            Debug.Log("버튼 클릭!");
            if (item != null)
            {
                check_item = new bool[item.item_combin_infor.Count];
                int x = 0;
                foreach(KeyValuePair<GameObject,int> item in item.item_combin_infor)
                {
                    for(int i = 0; i < inventory.GetComponent<Inventory>().slots.Length; i++)
                    {
                        if(inventory.GetComponent<Inventory>().slots[i].item != null)
                        {
                            if (item.Key.GetComponent<ItemPickUp>().item.item_id == inventory.GetComponent<Inventory>().slots[i].item.item_id)
                            {
                                if (item.Value <= inventory.GetComponent<Inventory>().slots[i].item_count)
                                {
                                    //아이템 제작 가능 상태
                                    check_item[x] = true;
                                    Debug.Log("제작 조건 만족 : " + inventory.GetComponent<Inventory>().slots[i].item.name);
                                }
                                else
                                {
                                    //아이템 제작 불가 상태
                                    check_item[x] = false;
                                    Debug.Log("제작 조건 불만족 : " + inventory.GetComponent<Inventory>().slots[i].item.name);
                                }
                            }
                        }

                    }
                    x++;
                }
                bool possible = true;
                for (int i = 0; i < check_item.Length; i++)
                {
                    if(check_item[i] == false)
                    {
                        possible = false;
                    }

                }
                if(possible == true)
                {
                    foreach (KeyValuePair<GameObject, int> item in item.item_combin_infor)
                    {
                        for (int i = 0; i < inventory.GetComponent<Inventory>().slots.Length; i++)
                        {
                            if (inventory.GetComponent<Inventory>().slots[i].item != null)
                            {
                                if (item.Key.GetComponent<ItemPickUp>().item.item_id == inventory.GetComponent<Inventory>().slots[i].item.item_id)
                                {
                                    inventory.GetComponent<Inventory>().slots[i].SetSlotCount(-item.Value);
                                }
                            }

                        }
                       
                    }
                    inventory.GetComponent<Inventory>().AcquireItem(item);
                }
                else
                {
                    Debug.Log("아이템 제작  실패");
                }
            }
        });
    }
}
