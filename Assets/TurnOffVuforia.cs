using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TurnOffVuforia : MonoBehaviour {

    void Start()
    {
        setVuforiaOff();
    }

    private void setVuforiaOff()
    {
        //Turn off Vuforia
        VuforiaBehaviour.Instance.enabled = false;
    }
    
}
