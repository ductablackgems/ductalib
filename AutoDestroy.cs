using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float timeDestroy = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Deactive", timeDestroy);
    }

    void Deactive() {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
