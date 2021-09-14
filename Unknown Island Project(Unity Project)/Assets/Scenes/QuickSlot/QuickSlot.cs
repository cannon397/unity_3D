using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot : MonoBehaviour
{

    // 슬롯들.
    private Slot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        slots = this.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
