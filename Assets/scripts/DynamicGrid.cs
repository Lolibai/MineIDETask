﻿using UnityEngine;
using UnityEngine.UI;

public class DynamicGrid : MonoBehaviour
{

    public int col, row;

    void Start()
    {

        RectTransform parent = gameObject.GetComponent<RectTransform>();
        GridLayoutGroup grid = gameObject.GetComponent<GridLayoutGroup>();

        grid.cellSize = new Vector2(parent.rect.width / col, parent.rect.height / row);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
