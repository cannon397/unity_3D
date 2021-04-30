using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class testNPCRoarTEST {


    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator testNPCRoarTESTWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame

        SetupScene();

        yield return new WaitForSeconds(20);
    }

    void SetupScene()
    {
       MonoBehaviour.Instantiate(Resources.Load<GameObject>("Scenery"));
    }

}
