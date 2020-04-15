using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class makeSmaller : MonoBehaviour
{
    public Vector2 shrinkFactor = Vector2.one * 0.05f;
    public float shrinkTime = 0.5f;
    public float rotateMagnitude = 1f;
    public void shrink()
    {
        var rect = GetComponent<RectTransform>();
        var newScale = rect.localScale;
        newScale = new Vector3(Mathf.Round(newScale.x / shrinkFactor.x) * shrinkFactor.x,
            Mathf.Round(newScale.y / shrinkFactor.y) * shrinkFactor.x);
        newScale -= (Vector3)shrinkFactor;
        rect.DOScale(newScale, shrinkTime);
        var randomQuat = rect.rotation *
            Quaternion.Euler(new Vector3(0,0,Random.Range(-1f, 1)*rotateMagnitude));
        rect.DORotateQuaternion(randomQuat, 0.6f);
        //rect.localScale -= (Vector3)shrinkFactor;
    }

}
