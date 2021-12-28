using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureEventCtrl : MonoBehaviour
{
    public GameObject[] GameObjectArray;

    public void RecognizeOK()
    {
        GameObject go = GameObject.Instantiate(GameObjectArray[2]);
        Destroy(go, 1f);
    }

    public void RecognizeThumbs()
    {
        GameObject go = GameObject.Instantiate(GameObjectArray[0]);
        Destroy(go, 1f);
    }

    public void RecognizeYeah()
    {
        GameObject go = GameObject.Instantiate(GameObjectArray[1]);
        Destroy(go, 1f);
    }

    public void RecognizeDiss()
    {
        GameObject go = GameObject.Instantiate(GameObjectArray[3]);
        Destroy(go, 1f);
    }
}
