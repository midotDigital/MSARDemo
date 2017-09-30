using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using EasyAR;

public class TargetController : MonoBehaviour {

    public GameObject Prefab;
    public float Height = 0.2f;

    string targetFile = "targets.json";

    [ContextMenu("创建ImageTarget")]
    public void CreateImageTarget() {
        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "\\" + targetFile);

        if (sr == null)
        {
            return;
        }

        string json = sr.ReadToEnd();

        Targets tars = JsonUtility.FromJson<Targets>(json);

        foreach (TargetModel model in tars.images) {
            GameObject go = Instantiate(Prefab);
            ArImageTarget imageTarget = go.GetComponent<ArImageTarget>();
            imageTarget.Path = model.image;
            imageTarget.Name = model.name;
            imageTarget.Desc = model.desc;
            go.name = model.name;
            GameObject pre = Instantiate(Resources.Load<GameObject>("TargetPrefabs/" + model.prefab));
            pre.transform.SetParent(go.transform);
            pre.transform.localPosition = new Vector3(0, Height, 0);
        }
    }
}

[Serializable]
public class Targets
{
    public List<TargetModel> images;
}

[Serializable]
public class TargetModel
{
    public string image;
    public string name;
    public string desc;
    public string prefab;
}
