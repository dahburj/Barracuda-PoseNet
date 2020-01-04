using System.IO;
using Unity.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Barracuda;
using UnityEngine.UI;
using VacuumShaders.TextureExtensions;

#if UNITY_2018_3_OR_NEWER
using UnityEngine.Rendering;
#else
using UnityEngine.Experimental.Rendering;
#endif

//using TensorFlow;

public class WebCameraExample : MonoBehaviour {

    public string modelName;
    
    public int Width = 256;
    public int Height = 256;
    public int FPS = 30;

    public Text fpsText;
    public Text debugText;
    WebCamTexture webcamTexture;
    public GLRenderer gl;

    public RawImage m_Display;

    static PoseNet posenet;
    PoseNet.Pose[] poses;
    bool isPosing;

    Model model;
    Barracuda.IWorker worker;
    Camera _camera;

    Queue<AsyncGPUReadbackRequest> _requests = new Queue<AsyncGPUReadbackRequest>();
 

    // Use this for initialization
    void Start () {

        _camera = GetComponent<Camera>();


        WebCamDevice[] devices = WebCamTexture.devices;

        if(devices.Length > 1)
            webcamTexture = new WebCamTexture(devices[1].name, Screen.width, (Screen.height / 2), FPS);
        else
        {
            webcamTexture = new WebCamTexture(devices[0].name, Screen.width, (Screen.height / 2), FPS);

        }

        if (posenet == null)
        {

            posenet = new PoseNet();
            debugText.text = "Model Loaded";

        }

        //    GetComponent<Renderer>().material.mainTexture = webcamTexture;

        m_Display.material.mainTexture = webcamTexture;

    //    model = ModelLoader.LoadFromStreamingAssets(modelName + ".bytes");

     //   worker = BarracudaWorkerFactory.CreateWorker(BarracudaWorkerFactory.Type.Compute, model);
        
        webcamTexture.Play();

    //    gl = GameObject.Find("GLRenderer").GetComponent<GLRenderer>();

        gl = GameObject.FindGameObjectWithTag("GLRenderer").GetComponent<GLRenderer>();

    //    StartCoroutine(PoseUpdateFromStart());

        Debug.Log("Made it to the end of start");

    //    worker.Dispose();
        
    }
	
	// Update is called once per frame
	void Update () {

        var fps = 1.0f / Time.smoothDeltaTime;

        fpsText.text = Mathf.RoundToInt(fps) + " FPS";

        while (_requests.Count > 0)
        {
            var req = _requests.Peek();

            if (req.hasError)
            {
                Debug.Log("GPU readback error");
                _requests.Dequeue();
            }
            else if (req.done)
            {
                var buffer = req.GetData<Color32>();

                if (Time.frameCount % 1 == 0 && !isPosing)
                {
                    //Create Texture2D()

                    StartCoroutine(PoseUpdateNoTex(buffer, _camera.scaledPixelWidth, _camera.scaledPixelHeight, .001f));
                    isPosing = true;
                    // var result = new AsyncCompletionSource<PoseNet.Pose[]>();
                    // StartCoroutine(PoseAsync(result, buffer, GetComponent<Camera>().pixelWidth, GetComponent<Camera>().pixelHeight));
                }

                _requests.Dequeue();
            }
            else
            {
                break;
            }
        }
        
    }


    void OnRenderImage(RenderTexture src, RenderTexture dest) {

        if(_requests.Count < 8)
            _requests.Enqueue(AsyncGPUReadback.Request(src));
        else
            Debug.Log("Too many requests take it easy boy");
        
        Graphics.Blit(src, dest);

    }
        
    //On Render()
    void OnRenderObject() {


        if(poses != null)
            gl.DrawResults(poses);

    }

    IEnumerator PoseUpdateNoTex(NativeArray<Color32> buffer, int width, int height, float secondsToWait)
    {
        //    isPosing = true;

        var _model = ModelLoader.LoadFromStreamingAssets(modelName + ".bytes");

        //var _model = model;

        var _worker = BarracudaWorkerFactory.CreateWorker(BarracudaWorkerFactory.Type.Compute, _model);
        //var _worker = worker;

        var frame = new Texture2D(width, height, TextureFormat.RGB24, false);
        frame.SetPixels32(buffer.ToArray());
        frame.Apply();

        //yield return new WaitForSeconds(secondsToWait);

        yield return new WaitForEndOfFrame();

        //frame.ResizePro(Width, Height, false, false);
        posenet.scale(frame, Width, Height, FilterMode.Bilinear);

        // Save frame image jpg to disk for debugging
        /// var randomInt = UnityEngine.Random.Range(0, 100000000000000000);
        /// File.WriteAllBytes(Application.persistentDataPath + "/pose-" + randomInt + ".jpg", frame.EncodeToJPG());
        /// Debug.Log("Saved size converted image path: " + Application.persistentDataPath + "/pose-" + randomInt + ".jpg");

        var inputs = new Dictionary<string, Tensor>();

        var tensor = new Tensor(frame, 3);
        inputs.Add("image", tensor);

        _worker.ExecuteAndWaitForCompletion(inputs);

        //yield return new WaitForSeconds(secondsToWait);
        yield return new WaitForEndOfFrame();
 
        var Heatmap = _worker.Fetch("heatmap");

        //yield return new WaitForSeconds(secondsToWait);
        yield return new WaitForEndOfFrame();

        var Offset = _worker.Fetch("offset_2");

        //yield return new WaitForSeconds(secondsToWait);
        yield return new WaitForEndOfFrame();

        var Dis_fwd = _worker.Fetch("displacement_fwd_2");

        //yield return new WaitForSeconds(secondsToWait);
        yield return new WaitForEndOfFrame();

        var Dis_bwd = _worker.Fetch("displacement_bwd_2");

        //yield return new WaitForSeconds(secondsToWait);
        yield return new WaitForEndOfFrame();

        poses = posenet.DecodeMultiplePosesOG(Heatmap, Offset, Dis_fwd, Dis_bwd, 
            outputStride: 16, maxPoseDetections: 1, scoreThreshold: 0.8f, nmsRadius: 30);


        Offset.Dispose();
        Dis_fwd.Dispose();
        Dis_bwd.Dispose();
        Heatmap.Dispose();
        _worker.Dispose();


        isPosing = false;

        frame = null;
        inputs = null;
        
    //    Resources.UnloadUnusedAssets(); 


        yield return null;
    }

}
