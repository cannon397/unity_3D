using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private Vector3 origin_pos;
    
    public Item item;
    public int item_count;
    public Image itemImage;
    public Text item_amount_text;
    Color color; 
    void Start()
    {
        
        color  = itemImage.GetComponent<Image>().color;
        origin_pos = transform.position;
        item_count = 0;
        item_amount_text.text = null;

    }

    //아이템 획득
    public void AddItem(Item item, int item_amount=1)
    {
        this.item = item;
        item_count = item_count + item_amount;
        color.r = 1;
        color.g = 1;
        color.b = 1;
        color.a = 1;
        itemImage.color = color;
        itemImage.sprite = item.item_icon;
        item_amount_text.text = "x" + item_count.ToString();
        
    }



    // 아이템 개수 조정.
    public void SetSlotCount(int _count)
    {
        item_count += _count;
        item_amount_text.text = "x"+item_count.ToString();

        if (item_count <= 0)
            ClearSlot();
    }
    // 슬롯 초기화.
    private void ClearSlot()
    {
        item = null;
        item_count = 0;
        itemImage.sprite = null;
        item_amount_text.text = null;


    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(item != null)
            {
                if(item.item_type == Item.ItemType.equip)
                {
                    //장착
                }
                else if(item.item_type == Item.ItemType.use)
                {
                    //소비
                    SetSlotCount(-1);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        if (item != null)
        {


            DragSlot.instance.drag_slot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            //Debug.Log("OnDrag");
            
            this.itemImage.sprite = null;
            color.a = 0;
            this.itemImage.color = color;
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
            

        if (item != null)
        {
            this.itemImage.sprite = item.item_icon;
            color.a = 1;
            this.itemImage.color = color;
            Debug.Log("아이템 비어있지 않을때");

        }
        else
        {
            this.itemImage.sprite = null;
            color.a = 0;
            this.itemImage.color = color;
            
            
        }
        DragSlot.instance.SetColor(0);
        DragSlot.instance.drag_slot = null;
        DragSlot.instance.image_item.sprite = null;
        

    }

    public void OnDrop(PointerEventData eventData)
    {
        
        if(DragSlot.instance.drag_slot != null)
        {
            ChangeSlot();
        }
        
    }
    private void ChangeSlot()
    {
        Item temp_item = item;
        int temp_item_count = item_count;

        AddItem(DragSlot.instance.drag_slot.item, DragSlot.instance.drag_slot.item_count);
        
        if(temp_item != null)
        {
            DragSlot.instance.drag_slot.AddItem(temp_item, temp_item_count);
            
        }
        else
        {
            DragSlot.instance.drag_slot.ClearSlot();
        }
    }
}
