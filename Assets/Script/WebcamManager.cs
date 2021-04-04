using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WebcamManager : MonoBehaviour
{
    private WebCamTexture webCamTexture;
    private bool isPlay = false;
    private Color32[] frameData;
    
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 1; i<WebCamTexture.devices.Length; i++)
        {
            if(WebCamTexture.devices[i].isFrontFacing)
            {
                Debug.Log(WebCamTexture.devices[i].name);
                webCamTexture = new WebCamTexture(WebCamTexture.devices[i].name, 600, 360, 30);
                webCamTexture.Play();
                isPlay = true;
                break;
            }
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        frameData = new Color32[webCamTexture.height * webCamTexture.width];
        webCamTexture.GetPixels32(frameData);
        Debug.Log(frameData.Length);

        //JSON
        //Frame frame = new Frame();
        //for (int i = 0; i < frameData.Length; i++)
        //{
        //   frame.pixels.Add(frameData[i]);
        //}
        //string json = JsonUtility.ToJson(frame);

        //PNG
        Texture2D t = new Texture2D(webCamTexture.width,webCamTexture.height);
        t.SetPixels32(frameData);
        t.Apply();

        byte[] img = t.EncodeToPNG();
        string filePath = Application.dataPath+"/test.png";
        Debug.Log(filePath);
        File.WriteAllBytes(filePath,img);

        // TODO PYTHON
    }

    void OnGUI()
    {
        if(isPlay)
        {
            GUI.DrawTexture(new Rect(0, 0, 400, 300), webCamTexture, ScaleMode.ScaleToFit);
        }
    }
}
