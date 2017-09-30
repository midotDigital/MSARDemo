using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ModelController : MonoBehaviour {

    public bool CanController = false;//是否可以缩放,旋转等
    public bool IsUpdatePosition = false;//是否时时去刷新位置

    private Transform middlePosition;

    private Touch oldTouch1;  //上次触摸点1(手指1)
    private Touch oldTouch2;  //上次触摸点2(手指2)

    Vector3 initPosition;
    Vector3 initScale;
    Quaternion initRotation;

    private void Start()
    {
        middlePosition = GameObject.FindGameObjectWithTag("MiddlePosition").transform;
        initPosition = transform.position;
        initScale = transform.localScale;
        initRotation = transform.rotation;
    }

    void Update()
    {
        if (!CanController)
            return;
        RotaWithTouch();
        TouchZoom();
        UpdatePosition();
    }

    public void ResetModel() {
        CanController = false;
        //IsUpdatePosition = false;
        transform.position = initPosition;
        transform.localScale = initScale;
        transform.rotation = initRotation;
    }

    /// <summary>
    /// 时时刷新位置,位于屏幕中间,当丢失识别图时调用
    /// </summary>
    void UpdatePosition() {
        //if (transform.localScale.x <= 0)
        //    return;
        if (!IsUpdatePosition)
            return;
        if (middlePosition == null)
            return;
        transform.position = middlePosition.position;
    }

    void RotaWithTouch() {
        //单点触摸， 水平上下旋转           
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 deltaPos = touch.deltaPosition;
			transform.Rotate(Vector3.back * deltaPos.x * Time.deltaTime * 10,Space.World);
			transform.Rotate(Vector3.right * deltaPos.y * Time.deltaTime * 10,Space.World);

        }
    }

    /// <summary>
    /// 缩放
    /// </summary>
    void TouchZoom() {
        //没有触摸，就是触摸点为0
        if (Input.touchCount <= 0)
        {
            return;
        }
        //多点触摸, 放大缩小
        Touch newTouch1 = Input.GetTouch(0);
        Touch newTouch2 = Input.GetTouch(1);
        //第2点刚开始接触屏幕, 只记录，不做处理
        if (newTouch2.phase == TouchPhase.Began)
        {
            oldTouch2 = newTouch2;
            oldTouch1 = newTouch1;
            return;
        }
        //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型
        float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
        float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);
        //两个距离之差，为正表示放大手势， 为负表示缩小手势
        float offset = newDistance - oldDistance;
        //放大因子， 一个像素按 0.01倍来算(100可调整)
        float scaleFactor = offset / 10f;
        Vector3 localScale = transform.localScale;
        Vector3 scale = new Vector3(localScale.x + scaleFactor,
            localScale.y + scaleFactor,
            localScale.z + scaleFactor);
        //在什么情况下进行缩放
        if (scale.x >= 0.01f && scale.y >= 0.01f && scale.z >= 0.01f)
        {
			transform.DOScale (scale, 0.5f);
        }
        //记住最新的触摸点，下次使用
        oldTouch1 = newTouch1;
        oldTouch2 = newTouch2;
    }
}
