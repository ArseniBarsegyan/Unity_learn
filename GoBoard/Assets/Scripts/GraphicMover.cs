﻿using UnityEngine;

public class GraphicMover : MonoBehaviour
{
    public GraphicMoverMode mode;
    public Transform startXform;
    public Transform endXform;

    public float moveTime = 1f;
    public float delay = 0f;
    public iTween.LoopType loopType = iTween.LoopType.none;
    public iTween.EaseType easeType = iTween.EaseType.easeOutExpo;

    void Awake()
    {
        if (endXform == null)
        {
            endXform = new GameObject(gameObject.name + "XformEnd").transform;
            endXform.position = transform.position;
            endXform.rotation = transform.rotation;
            endXform.localScale = transform.localScale;
        }

        if (startXform == null)
        {
            startXform = new GameObject(gameObject.name + "XformStart").transform;
            startXform.position = transform.position;
            startXform.rotation = transform.rotation;
            startXform.localScale = transform.localScale;
        }
        Reset();
    }

    public void Reset()
    {
        switch (mode) 
        {
            case GraphicMoverMode.MoveTo:
                if (startXform != null)
                {
                    transform.position = startXform.position;
                }                
                break;
            case GraphicMoverMode.MoveFrom:
                if (endXform != null)
                {
                    transform.position = endXform.position;
                }                
                break;
            case GraphicMoverMode.ScaleTo:
                if (endXform != null)
                {
                    transform.localScale = endXform.localScale;
                }                
                break;
        }
    }

    public void Move()
    {
        switch (mode) 
        {
            case GraphicMoverMode.MoveTo:
                iTween.MoveTo(gameObject, iTween.Hash(
                    "position", endXform.position,
                    "time", moveTime,
                    "delay", delay,
                    "easetype", easeType,
                    "looptype", loopType
                    ));
                break;

            case GraphicMoverMode.ScaleTo:
                iTween.MoveTo(gameObject, iTween.Hash(
                    "scale", endXform.localScale,
                    "time", moveTime,
                    "delay", delay,
                    "easetype", easeType,
                    "looptype", loopType
                    ));
                break;

            case GraphicMoverMode.MoveFrom:
                iTween.MoveFrom(gameObject, iTween.Hash(
                    "position", startXform.position,
                    "time", moveTime,
                    "delay", delay,
                    "easetype", easeType,
                    "looptype", loopType
                    ));
                break;
        }
    }
}

public enum GraphicMoverMode
{
    MoveTo,
    ScaleTo,
    MoveFrom
}
