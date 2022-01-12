using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if ENABLE_WINMD_SUPPORT
    Debug.Log("Windows Runtime Support enabled");
    // Put calls to your custom .winmd API here
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
