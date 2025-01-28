using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventTrigger : MonoBehaviour
{
    public UnityEvent OnTriggerEnterEvent;
    public UnityEvent OnTriggerExitEvent;

    bool isColliding = false;
    bool wasColliding = false;

    public void OnPlayerCollides() {
        isColliding = true;
    }

    void LateUpdate() {
        if (isColliding != wasColliding) {
            if (isColliding) OnEnter();
            else OnExit();
        }
        wasColliding = isColliding;
        isColliding = false;
    }

    void OnEnter() {
        if (OnTriggerEnterEvent != null) {
            OnTriggerEnterEvent.Invoke();
        }
    }

    void OnExit() {
        if (OnTriggerExitEvent != null) {
            OnTriggerExitEvent.Invoke();
        }
    }
}
