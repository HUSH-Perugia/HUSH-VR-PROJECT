using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TurnOnVuforia : MonoBehaviour {

    void Start()
    {
        setVuforiaOn();
    }

    private void setVuforiaOn ()
    {
        //Turn on Vuforia
        VuforiaBehaviour.Instance.enabled = true;
    }

}
