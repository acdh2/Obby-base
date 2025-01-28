using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventTrigger : MonoBehaviour
{
    public UnityEvent OnTriggerEnterEvent;
    public UnityEvent OnTriggerExitEvent;

    void OnTriggerEnter() {
        if (OnTriggerEnterEvent != null) {
            OnTriggerEnterEvent.Invoke();
        }
    }

    void OnTriggerExit() {
        if (OnTriggerExitEvent != null) {
            OnTriggerExitEvent.Invoke();
        }
    }
}
