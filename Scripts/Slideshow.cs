using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Slideshow : MonoBehaviour
{
    public Texture[] imageArray;
    private int currentImage;

    float deltaTime = 0.0f;

    public float timer1 = 1.0f;
    public float timer1Remaining = 0.3f;
    public bool timer1IsRunning = true;
    public string timer1Text;

    private void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        Rect imageRect = new Rect(0, 0, Screen.width, Screen.height);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
