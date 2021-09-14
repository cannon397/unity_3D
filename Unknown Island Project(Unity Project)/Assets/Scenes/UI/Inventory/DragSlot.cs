using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{

    static public DragSlot instance;

    public Slot drag_slot;

    [SerializeField]
    public Image image_item;

    void Start()
    {
        instance = this;    
    }

    public void DragSetImage(Image image_item)
    {
        this.image_item.sprite = image_item.sprite;
        SetColor(1);
    }
    public void SetColor(float alpha)
    {
        Color color = image_item.color;
        color.a = alpha;
        image_item.color = color;
    }

    
}
