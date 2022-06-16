using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleInGame : MonoBehaviour
{
    Renderer myRenderer;
    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<MeshRenderer>();
        myRenderer.enabled = false;
    }
}
