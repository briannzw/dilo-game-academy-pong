using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WaitForSecond : MonoBehaviour
{
    [SerializeField] private UnityEvent OnComplete;

    public void Wait(float seconds)
    {
        StartCoroutine(IeWaitForSeconds(seconds));
    }

    private IEnumerator IeWaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if(OnComplete != null)
        {
            OnComplete.Invoke();
        }
    }
}
