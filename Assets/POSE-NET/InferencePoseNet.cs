using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Barracuda;
using System.Linq;
//using MLAgents;

public class InferencePoseNet : MonoBehaviour
{
    public string modelName;

    public RawImage testImage;


    PoseNet posenet = new PoseNet();        //Not a good idea maybe cpu wise? 
    PoseNet.Pose[] poses;

    private GLRenderer gl;

    // Start is called before the first frame update
    void Start()
    {
        var model = ModelLoader.LoadFromStreamingAssets(modelName + ".bytes");

        var worker = BarracudaWorkerFactory.CreateWorker(BarracudaWorkerFactory.Type.Compute, model);

        foreach(var layer in model.layers)
            Debug.Log("Layer " + layer.name + " does: " + layer.inputs);

        var inputs = new Dictionary<string, Tensor>();

        Texture2D img = Resources.Load("tennis_in_crowd") as Texture2D;
    //    Texture2D img = testImage.mainTexture as Texture2D;

        var tensor = new Tensor(img, 3);

        inputs.Add("image", tensor);

        worker.ExecuteAndWaitForCompletion(inputs);
        var Heatmap = worker.Fetch("heatmap");
        var Offset = worker.Fetch("offset_2");
        var Dis_fwd = worker.Fetch("displacement_fwd_2");
        var Dis_bwd = worker.Fetch("displacement_bwd_2");
        
        poses = posenet.DecodeMultiplePosesOG(Heatmap, Offset, Dis_fwd, Dis_bwd, 
            outputStride: 16, maxPoseDetections: 2, scoreThreshold: 0.02f, nmsRadius: 20);

        gl = GameObject.Find("GLRenderer").GetComponent<GLRenderer>();

        Debug.Log(poses.Length);
        
//        Debug.Log(Heatmap.height);
       Heatmap.Dispose();
       Offset.Dispose();
       Dis_fwd.Dispose();
       Dis_bwd.Dispose();
       worker.Dispose();
       

    }


    public void OnRenderObject()
    {
        gl.DrawResults(poses);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
