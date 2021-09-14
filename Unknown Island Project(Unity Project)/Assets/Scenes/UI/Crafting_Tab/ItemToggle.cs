using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemToggle : MonoBehaviour, IPointerClickHandler
{
    public Item item;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if(item != null)
            {
                GameObject.Find("Crafting_Tab").SendMessage("CrantingInfor",item);
            }
        }
    }
}
