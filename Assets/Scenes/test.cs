using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : Player
{
   
    // Start is called before the first frame update
     void Awake()
    {
       
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        // Forward to the parent (or just deal with it here).
        // Let's say it has a script called "PlayerCollisionHelper" on it:
        Player parentScript = transform.parent.GetComponent<Player>();

        // Let it know a collision happened:
        parentScript.CollisionFromChild(collision);
    }
}
