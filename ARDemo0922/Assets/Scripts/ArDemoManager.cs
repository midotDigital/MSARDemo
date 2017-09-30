using EasyAR;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ArDemoManager : MonoBehaviour
{
    public Button CloseBtn, VoiceBtn;

    public AudioClip[] Clips;
    public GameObject[] Images;
    public ModelController[] Fishs;

    string[] targets = { "caoyu", "jiyu", "yongyu" };

    AudioSource _as;

    public static ArDemoManager Instance;

    int currentTarget = -1;
    public int CurrentTarget
    {
        get
        {
            return currentTarget;
        }
        set
        {
            if(value == -1)
            {
                HiddenAll();
            }
            if (currentTarget == value)
                return;
            currentTarget = value;
            SwitchTarget();
        }
    }

    void SwitchTarget() {
        if (currentTarget == -1)
            return;
        HiddenAll();
        playAudio(currentTarget);
        ShowItem(Images[currentTarget], true);
        ShowItem(Fishs[currentTarget], true);
    }

    void HiddenAll() {
        stopVoice();
        foreach (GameObject igo in Images) {
			if (igo.transform.localScale.x > 0) {
                ShowItem(igo, false);
            }
        }
        foreach (ModelController fish in Fishs) {
            fish.ResetModel();
            if (fish.transform.localScale.x > 0) {
                ShowItem(fish, false);
            }
        }
    }

    void ShowItem(ModelController item,bool flag) {
        if (flag)
        {
            item.transform.DOScale(10, 0.6f);
            item.CanController = true;
           // item.IsUpdatePosition = true;
        }
        else {
            item.transform.DOScale(0, 0.6f);
        }
    }

    void ShowItem(GameObject item, bool flag)
    {
        if (flag)
        {
			item.transform.DOScale(1, 0.6f);
			item.transform.parent.DOScale(1, 0.6f);

        }
        else
        {
			item.transform.DOScale(0, 0.6f);
			item.transform.parent.DOScale(0, 0.6f);
        }
    }

    private void Awake()
    {
        Instance = this;
        var EasyARBehaviour = FindObjectOfType<EasyARBehaviour>();
        EasyARBehaviour.Initialize();

        foreach (var behaviour in ARBuilder.Instance.ARCameraBehaviours)
        {
            behaviour.TargetFound += OnTargetFound;
            behaviour.TargetLost += OnTargetLost;
            behaviour.TextMessage += OnTextMessage;
        }
        foreach (var behaviour in ARBuilder.Instance.ImageTrackerBehaviours)
        {
            behaviour.TargetLoad += OnTargetLoad;
            behaviour.TargetUnload += OnTargetUnload;
        }
    }

    private void Start()
    {
        _as = GetComponent<AudioSource>();
        CloseBtn.onClick.AddListener(CloseAll);
        VoiceBtn.onClick.AddListener(DoBtnVoice);
    }

    void playAudio(int index) {
        _as.Stop();
        _as.clip = Clips[index];
        _as.Play();
    }

    void stopVoice() {
        _as.Stop();
    }

    public void CloseAll() {
        CurrentTarget = -1;
    }

    public void DoBtnVoice() {
        if (_as.isPlaying)
        {
            _as.Stop();
        }
        else {
			if(currentTarget != -1)
            	_as.Play();
        }
    }

    int findIndexOnTargets(string name) {
        for (int i = 0; i < targets.Length; i++) {
            if (targets[i].Equals(name)) {
                return i;
            }
        }
        return - 1;
    }

    #region 识别回调
    void OnTargetFound(ARCameraBaseBehaviour arcameraBehaviour, TargetAbstractBehaviour targetBehaviour, Target target)
    {
        CurrentTarget = findIndexOnTargets(target.Name);
    }

    void OnTargetLost(ARCameraBaseBehaviour arcameraBehaviour, TargetAbstractBehaviour targetBehaviour, Target target)
    {
        if (currentTarget == -1)
            return;
       // Fishs[currentTarget].IsUpdatePosition = true;
    }

    void OnTargetLoad(ImageTrackerBaseBehaviour trackerBehaviour, ImageTargetBaseBehaviour targetBehaviour, Target target, bool status)
    {
    }

    void OnTargetUnload(ImageTrackerBaseBehaviour trackerBehaviour, ImageTargetBaseBehaviour targetBehaviour, Target target, bool status)
    {
    }

    private void OnTextMessage(ARCameraBaseBehaviour arcameraBehaviour, string text)
    {
    }
#endregion

}
