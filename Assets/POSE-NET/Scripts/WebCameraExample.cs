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

    Queue<AsyncGPUReadbackRequest> _requests = new Queue<AsyncGPUReadbackRequest>();
 

    // Use this for initialization
    void Start () {

        if(posenet == null){

            posenet = new PoseNet();
            debugText.text = "Model Loaded";

        }

        WebCamDevice[] devices = WebCamTexture.devices;

        if(devices.Length > 1)
            webcamTexture = new WebCamTexture(devices[1].name, Screen.width, (Screen.height / 2), FPS);
        else
        {
            webcamTexture = new WebCamTexture(devices[0].name, Screen.width, (Screen.height / 2), FPS);

        }
        
    //    GetComponent<Renderer>().material.mainTexture = webcamTexture;

        m_Display.material.mainTexture = webcamTexture;

        model = ModelLoader.LoadFromStreamingAssets(modelName + ".bytes");

        worker = BarracudaWorkerFactory.CreateWorker(BarracudaWorkerFactory.Type.ComputeFast, model);
        
        webcamTexture.Play();

    //    gl = GameObject.Find("GLRenderer").GetComponent<GLRenderer>();

        gl = GameObject.FindGameObjectWithTag("GLRenderer").GetComponent<GLRenderer>();

    //    StartCoroutine(PoseUpdateFromStart());

        Debug.Log("Made it to the end of start");
        
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
                    var _camera = GetComponent<Camera>();
                    //Create Texture2D()

                    StartCoroutine(PoseUpdateNoTex(buffer, _camera.pixelWidth, _camera.pixelHeight, .001f));
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

        //        var _model = ModelLoader.LoadFromStreamingAssets(modelName + ".bytes");

        //        var _worker = BarracudaWorkerFactory.CreateWorker(BarracudaWorkerFactory.Type.Compute, _model);

        var _model = model;
        var _worker = worker;

        var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        texture.SetPixels32(buffer.ToArray());
        texture.Apply();

        yield return new WaitForSeconds(
        secondsToWait);

        texture.ResizePro(Width, Height, false, false);
        var inputs = new Dictionary<string, Tensor>();

        var tensor = new Tensor(texture, 3);
        inputs.Add("image", tensor);

        _worker.ExecuteAndWaitForCompletion(inputs);

        yield return new WaitForSeconds(secondsToWait);
 
        var Heatmap = _worker.Fetch("heatmap");

        yield return new WaitForSeconds(secondsToWait);

        var Offset = _worker.Fetch("offset_2");

        yield return new WaitForSeconds(secondsToWait);

        var Dis_fwd = _worker.Fetch("displacement_fwd_2");

        yield return new WaitForSeconds(secondsToWait);

        var Dis_bwd = _worker.Fetch("displacement_bwd_2");

        yield return new WaitForSeconds(secondsToWait);

        poses = posenet.DecodeMultiplePosesOG(Heatmap, Offset, Dis_fwd, Dis_bwd, 
            outputStride: 16, maxPoseDetections: 1, scoreThreshold: 0.8f, nmsRadius: 30);

     //   yield return new WaitForSeconds(1f);

        Offset.Dispose();
        Dis_fwd.Dispose();
        Dis_bwd.Dispose();
        Heatmap.Dispose();
        _worker.Dispose();

    //    yield return new WaitForEndOfFrame();

    //    yield return new WaitForSeconds(1f);

        isPosing = false;

        texture = null;
    //    _worker = null;
        inputs = null;
        
    //    Resources.UnloadUnusedAssets(); 


        yield return null;
    }


     

}
