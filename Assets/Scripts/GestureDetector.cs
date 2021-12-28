using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Gesture
{
    public string name;
    public List<Vector3> fingerDatas;
    public UnityEvent GestureEvent;
}

public class GestureDetector : MonoBehaviour
{
    /// <summary>
    /// 获取手的骨骼
    /// </summary>
    public OVRSkeleton skeleton;

    /// <summary>
    /// 记录的手势数据
    /// </summary>
    public List<Gesture> gestures;

    /// <summary>
    /// 手势识别的灵敏度，值越大，手势误差就越大
    /// </summary>
    public float threshold = 0.1f;

    public bool debugMode = true;

    private Gesture preGesture;

    private List<OVRBone> fingerBones;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetFingerBones());
        preGesture = new Gesture();
    }

    IEnumerator GetFingerBones()
    {
        do
        {
            fingerBones = new List<OVRBone>(skeleton.Bones);
            yield return null;
        }
        while (fingerBones.Count <= 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (debugMode && Input.GetKeyDown(KeyCode.Space))
        {
            fingerBones = new List<OVRBone>(skeleton.Bones);
            Save();
        }

        Gesture currentGesture = Recognize();

        bool hasRecognized = !currentGesture.Equals(new Gesture());

        if (hasRecognized && !currentGesture.Equals(preGesture))
        {
            Debug.Log("发现新手势 ：" + currentGesture.name);
            preGesture = currentGesture;
            currentGesture.GestureEvent.Invoke();
        }
    }

    void Save()
    {
        Gesture gesture = new Gesture();
        gesture.name = "New Gesture";
        List<Vector3> data = new List<Vector3>();
        foreach (var bone in fingerBones)
        {
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }
        gesture.fingerDatas = data;
        gestures.Add(gesture);
    }

    Gesture Recognize()
    {
        Gesture currentGesture = new Gesture();
        float currentMin = Mathf.Infinity;

        foreach (var gesture in gestures)
        {
            float sumDistance = 0;
            bool isDiscarded = false;
            for (int i = 0; i < fingerBones.Count; i++)
            {
                Vector3 curData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(curData, gesture.fingerDatas[i]);
                if (distance > threshold)
                {
                    isDiscarded = true;
                    break;
                }
                sumDistance += distance;
            }

            if (!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance;
                currentGesture = gesture;
            }

        }

        return currentGesture;
    }
}
