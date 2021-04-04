using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

public class WebcamManager : MonoBehaviour
{
    private WebCamTexture webCamTexture;
    private bool isPlay = false;
    private Color32[] frameData;
    public GameObject player;
    public GameObject[] emotionsUI;
    
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

        // Reading PYTHON Json
        if (File.Exists(Application.dataPath + "/test.json"))
        {
            
            string jsonStr = File.ReadAllText(Application.dataPath + "/test.json");
            List<Emotion> jsonEmotionDatas = new List<Emotion>();
            if (jsonStr!= null && jsonStr!="")jsonEmotionDatas = JsonUtility.FromJson<EmotionJson>(jsonStr).datas;

            // Draw UI Graph
            for (int i = 0; i < jsonEmotionDatas.Count; i++)
            {
                GameObject emotionSlider = emotionsUI[i];
                // Set Emotion Name
                Text text = emotionSlider.GetComponentInChildren<Text>();
                text.text = jsonEmotionDatas[i].name;

                // Set Emotion Ratio value
                Slider slider = emotionSlider.GetComponentInChildren<Slider>();
                slider.value = jsonEmotionDatas[i].value;
                Debug.Log(text.text + ":" + slider.value.ToString());

                PlayerController playerController = player.GetComponent<PlayerController>();

                if (text.text == "happy" && slider.value > 0.5f)
                {
                    playerController.Movement(slider.value);
                }
                if(text.text == "surprised" && slider.value > 0.5f)
                {
                    playerController.Jump();
                }
            }
        }
        //PlayerController playerController = player.GetComponent<PlayerController>();
        // TODO playerController
    }

    void OnGUI()
    {
        if(isPlay)
        {
            GUI.DrawTexture(new Rect(0, 0, 400, 300), webCamTexture, ScaleMode.ScaleToFit);
        }
    }
}
