/*
 * 场景排序
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ContainRendererSorter : MonoBehaviour {

    private int sortingOrderBase = 500;
    private int offset = 3;
    private float timer;
    private float timerMax = .1f;
    [SerializeField]
    private bool runOnly=true;
    private Renderer[] renders;

    private float positionY ;

    private void Awake()
    {
        renders = GetComponentsInChildren<Renderer>();
    }

    private void LateUpdate() {
        timer -= Time.deltaTime;
        if (timer <= 0f) {
            timer = timerMax;
            foreach (var render in renders)
            {
                positionY = render.gameObject.transform.position.y;
                render.sortingOrder = (int)(sortingOrderBase - positionY * 5 - offset);
            }
            if (runOnly) {
                Destroy(this);
            }
        }
    }

}
