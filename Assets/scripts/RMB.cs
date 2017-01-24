using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RMB : MonoBehaviour,IPointerClickHandler
{
    public Sprite flag;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            gameObject.GetComponent<Image>().sprite = flag;
    }
}
