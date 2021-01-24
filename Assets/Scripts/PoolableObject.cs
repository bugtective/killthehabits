using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public bool IsTaken {get; private set;}

    public virtual void OnCreate(Transform parent)
    {
        transform.SetParent(parent);
        Reset();
    }

    public virtual void Take()
    {
        IsTaken = true;
        gameObject.SetActive(true);
    }

    public virtual void Reset()
    {
        transform.localPosition = Vector3.one;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        IsTaken = false;
        gameObject.SetActive(false);
    }
}