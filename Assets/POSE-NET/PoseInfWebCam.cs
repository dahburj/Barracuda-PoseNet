using System.IO;
using Unity.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Barracuda;
using UnityEngine.UI;
using VacuumShaders.TextureExtensions;

using Unity.Jobs;
using UnityEngine.Jobs;

#if UNITY_2018_3_OR_NEWER
using UnityEngine.Rendering;
#else
using UnityEngine.Experimental.Rendering;
#endif

//using TensorFlow;

public class PoseInfWebCam : BaseJobObjectExample
{

    public string modelName;

    public int Width;
    public int Height;
    public int FPS;

    public RawImage rawImage;
    public Text fpsText;
    public Text debugText;

    WebCamTexture webcamTexture;
    public GLRenderer gl;

    public RawImage m_Display;
    //    int ImageSize = 512;
    //    PoseNet posenet = new PoseNet();
    static PoseNet posenet;
    static PoseNet.Pose[] poses;
    static bool isPosing;
    Queue<AsyncGPUReadbackRequest> _requests = new Queue<AsyncGPUReadbackRequest>();


    ProcessTensorJob m_processTesnorJob;

    JobHandle m_TensorJobHandle;
    JobHandle m_PoseJobHandle;

    NativeArray<Color32> m_NativeColors;
    Color32[] m_Data;
    static Texture2D m_Texture;


    Model model;
    static Barracuda.IWorker worker;

    static Tensor m_tensor;

    // Use this for initialization
    void Start()
    {

        if (posenet == null)
        {

            posenet = new PoseNet();
            debugText.text = "Model Loaded";

        }

        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length > 1)
            webcamTexture = new WebCamTexture(devices[1].name, Screen.width, (Screen.height / 2), FPS);
        else
        {
            webcamTexture = new WebCamTexture(devices[0].name, Screen.width, (Screen.height / 2), FPS);

        }

        m_Display.material.mainTexture = webcamTexture;

        webcamTexture.Play();


        gl = GameObject.FindGameObjectWithTag("GLRenderer").GetComponent<GLRenderer>();

        //    StartCoroutine(PoseUpdateFromStart());

    //    Debug.Log("Made it to the end of start");

   //     Debug.Log("offset max is: " + rawImage.GetComponent<RectTransform>().offsetMax + "  offsetMin is: " + rawImage.GetComponent<RectTransform>().offsetMin);

        m_Data = new Color32[Width * Height];
        m_NativeColors = new NativeArray<Color32>(m_Data, Allocator.Persistent);

        m_Texture = new Texture2D(Width, Height);

        model = ModelLoader.LoadFromStreamingAssets(modelName + ".bytes");
        worker = BarracudaWorkerFactory.CreateWorker(BarracudaWorkerFactory.Type.CSharpFast,
            model);
    }

    private void OnEnable()
    {
        m_Data = new Color32[Width * Height];
        m_NativeColors = new NativeArray<Color32>(m_Data, Allocator.Persistent);
    }



    struct ProcessTensorJob : IJob
    {
      //  [ReadOnly]
     //   public Tensor input_tensor;

        public float deltaTime;

     //   public NativeArray<Color32> _buffer;


        public int webCamWidth;
        public int webCamHeight;

     //   Texture2D texture;

     //   public Model _model; //= ModelLoader.LoadFromStreamingAssets("model-mobilenet_v1_050" + ".bytes");
     //   private Model _model;
     //   public IWorker _worker;

     //    public Tensor _inputTens;

     //  public IWorker _worker; //= BarracudaWorkerFactory.CreateWorker(BarracudaWorkerFactory.Type.Compute, _model);


        public void Execute()
        {


            Debug.Log("Started");

            //works on ios, not on android due to "unable to compute shaders on platform" error

            //    _model = ModelLoader.LoadFromStreamingAssets("model-mobilenet_v1_050" + ".bytes");

            //    _worker = BarracudaWorkerFactory.CreateWorker(BarracudaWorkerFactory.Type.ComputeFast, _model);     //works on ios, not on android due to "unable to compute shaders on platform" error
            //  var texture = new Texture2D(webCamWidth, webCamHeight, TextureFormat.RGBA32, false);

            //  texture.SetPixels32(_buffer.ToArray());

            // texture.Apply();

            // texture.ResizePro(224, 224, false, false);


            isPosing = true;

  
            var _inputs = new Dictionary<string, Tensor>();

            //  var texture = new Texture2D(webCamWidth, webCamHeight, TextureFormat.RGBA32, false);

            // var tensor = new Tensor(m_Texture, 3);

            _inputs.Add("image", m_tensor);

            worker.ExecuteAndWaitForCompletion(_inputs);

            isPosing = false;


            /*
                         
            var tensor = new Tensor(texture, 3);

            _inputs.Add("image", tensor);           

            */

            /*

            var Heatmap = _worker.Fetch("heatmap");
            var Offset = _worker.Fetch("offset_2");
            var Dis_fwd = _worker.Fetch("displacement_fwd_2");
            var Dis_bwd = _worker.Fetch("displacement_bwd_2");


            poses = posenet.DecodeMultiplePosesOG(Heatmap, Offset, Dis_fwd, Dis_bwd,
                outputStride: 16, maxPoseDetections: 1, scoreThreshold: 0.8f, nmsRadius: 30);

            Debug.Log(poses);


            Offset.Dispose();
            Dis_fwd.Dispose();
            Dis_bwd.Dispose();
            Heatmap.Dispose();
            _worker.Dispose();
            */

            isPosing = false;
        }
    }

    struct FinishTensorJob : IJob
    {
        Tensor _heatmap;
        Tensor _offset;
        Tensor _disfwd;
        Tensor _disbwd;
        Barracuda.IWorker _worker;

        public void Execute() 
        {
            _heatmap.Dispose();
            _offset.Dispose();
            _disfwd.Dispose();
            _disbwd.Dispose();
            _worker.Dispose();
        }
    }

    // Update is called once per frame
    void Update()
    {

        var fps = 1.0f / Time.smoothDeltaTime;
        
        fpsText.text = Mathf.RoundToInt(fps) + " FPS";;

        m_Data = new Color32[Width* Height];
     //   NativeArray<Color32> m_buffer = new NativeArray<Color32>();

        webcamTexture.GetPixels32(m_Data);
        m_NativeColors.CopyFrom(m_Data);

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
                Debug.Log("Is posing: " + isPosing);

                var new_buffer = req.GetData<Color32>();

                //    Debug.Log("Starting");

                if (!isPosing)
                {
                    var t_worker = BarracudaWorkerFactory.CreateWorker(
                    BarracudaWorkerFactory.Type.ComputeFast, model);

                    var texture = new Texture2D(Screen.height, Screen.width, TextureFormat.RGBA32, false);
                    texture.SetPixels32(new_buffer.ToArray());
                    texture.Apply();


                  //  texture.ResizePro(Width, Height, false, false);

                    m_Texture = texture;

                    var _tensor = new Tensor(m_Texture, 3);

                    m_tensor = _tensor;


                    m_processTesnorJob = new ProcessTensorJob()
                    {
                        deltaTime = Time.deltaTime,
                     //   _buffer = buffer,
                     //   _worker = t_worker,
                        webCamWidth = Width,
                        webCamHeight = Height,
                    };

                    m_TensorJobHandle = m_processTesnorJob.Schedule();

                    texture = null;
                }

                _requests.Dequeue();
            }
            else
            {
                break;
            }
        } 

        /*
        m_processTesnorJob = new ProcessTensorJob()
        {

            deltaTime = Time.deltaTime,
            _buffer = m_NativeColors,

        };

        m_TensorJobHandle = m_processTesnorJob.Schedule();
        */      

    }

    public void LateUpdate()
    {
        m_TensorJobHandle.Complete();
        m_NativeColors.CopyTo(m_Data);

        m_Texture.SetPixels32(0, 0, Width, Height, m_Data);
        m_Texture.Apply(false);
        //  debugText.text = poses.Length.ToString();

    }



    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {

        if (_requests.Count < 8)
            _requests.Enqueue(AsyncGPUReadback.Request(src));
        else
            Debug.Log("Too many requests.");

        Graphics.Blit(src, dest);
    }



    //On Render()
    void OnRenderObject()
    {


        if (poses != null)
            gl.DrawResults(poses);

    }



    IEnumerator PoseUpdateNoTex(NativeArray<Color32> buffer, int width, int height)
    {
        isPosing = true;

        var _model = ModelLoader.LoadFromStreamingAssets(modelName + ".bytes");

        var _worker = BarracudaWorkerFactory.CreateWorker(BarracudaWorkerFactory.Type.Compute, _model);     //works on ios, not on android due to "unable to compute shaders on platform" error


        var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        texture.SetPixels32(buffer.ToArray());
        texture.Apply();


        texture.ResizePro(Width, Height, false, false);
        var inputs = new Dictionary<string, Tensor>();

        var tensor = new Tensor(texture, 3);
        inputs.Add("image", tensor);

        _worker.ExecuteAndWaitForCompletion(inputs);



        var Heatmap = _worker.Fetch("heatmap");

        var Offset = _worker.Fetch("offset_2");
        var Dis_fwd = _worker.Fetch("displacement_fwd_2");
        var Dis_bwd = _worker.Fetch("displacement_bwd_2");


        poses = posenet.DecodeMultiplePosesOG(Heatmap, Offset, Dis_fwd, Dis_bwd,
            outputStride: 16, maxPoseDetections: 1, scoreThreshold: 0.8f, nmsRadius: 30);



        Offset.Dispose();
        Dis_fwd.Dispose();
        Dis_bwd.Dispose();
        Heatmap.Dispose();
        _worker.Dispose();


        isPosing = false;

        texture = null;
        inputs = null;

        //    Resources.UnloadUnusedAssets(); 



        yield return null;
    }


}
