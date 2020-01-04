using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using Barracuda;
using System.IO;


public partial class PoseNet
{

    Vector2 GetOffsetPoint(int y,int x,int keypoint, float[,,,] offsets) {
        return new Vector2(
            offsets[0, y, x, keypoint + NUM_KEYPOINTS],
            offsets[0, y, x, keypoint]
        );
    }


    Vector2 GetOffsetPointOG(int y,int x,int keypoint, Tensor offsets) {
        return new Vector2(
            offsets[0, y, x, keypoint + NUM_KEYPOINTS],
            offsets[0, y, x, keypoint]
        );
    }

    float SquaredDistance(
        float y1, float x1, float y2, float x2) {
        var dy = y2 - y1;
        var dx = x2 - x1;
        return dy * dy + dx * dx;
    }

    Vector2 AddVectors(Vector2 a, Vector2 b) {
        return new Vector2(x: a.x + b.x, y: a.y + b.y);
    }

    Vector2 GetImageCoords(
        Part part,int outputStride,float[,,,] offsets) {
        var vec = GetOffsetPoint(part.heatmapY, part.heatmapX,
                                 part.id, offsets);
        return new Vector2(
            (float)(part.heatmapX * outputStride) + vec.x,
            (float)(part.heatmapY * outputStride) + vec.y
        );
    }

    Vector2 GetImageCoordsOG(
        Part part,int outputStride,Tensor offsets) {
        var vec = GetOffsetPointOG(part.heatmapY, part.heatmapX,
                                    part.id, offsets);
        return new Vector2(
            (float)(part.heatmapX * outputStride) + vec.x,
            (float)(part.heatmapY * outputStride) + vec.y
        );
    }

    public Tuple<Keypoint, Keypoint>[] GetAdjacentKeyPoints(
           Keypoint[] keypoints, float minConfidence)
    {

        return connectedPartIndices
            .Where(x => !EitherPointDoesntMeetConfidence(
                keypoints[x.Item1].score, keypoints[x.Item2].score, minConfidence))
           .Select(x => new Tuple<Keypoint, Keypoint>(keypoints[x.Item1], keypoints[x.Item2])).ToArray();

    }

    bool EitherPointDoesntMeetConfidence(
        float a, float b, float minConfidence)
    {
        return (a < minConfidence || b < minConfidence);
    }

    public static double mean(float[,,,] tensor)
    {
        double sum = 0f;
        var x = tensor.GetLength(1);
        var y = tensor.GetLength(2);
        var z = tensor.GetLength(3);
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                for (int k = 0; k < z; k++)
                {
                    sum += tensor[0, i, j, k];
                }
            }
        }
        var mean = sum / (x * y * z);
        return mean;
    }

    public static double meanOG(Tensor tensor)
    {
        double sum = 0f;
        var x = tensor.shape[1];
        var y = tensor.shape[2];
        var z = tensor.shape[3];

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                for (int k = 0; k < z; k++)
                {
                    sum += tensor[0, i, j, k];
                }
            }
        }
        var mean = sum / (x * y * z);
        return mean;
    }

    //Pose ScalePose(Pose pose, int scale) {

    //    var s = (float)scale;

    //    return new Pose(
    //        pose.keypoints.Select( x => 
    //            new Keypoint( 
    //                x.score,
    //                new Vector2(x.position.x * s, x.position.y * s),
    //                x.part)
    //         ).ToArray(),
    //         pose.score
    //     );
    //}

    //Pose[] ScalePoses(Pose[] poses, int scale) {
    //    if (scale == 1) {
    //        return poses;
    //    }
    //    return poses.Select(x => ScalePose(pose: x, scale: scale)).ToArray();
    //}

    //int GetValidResolution(float imageScaleFactor,
    //                       int inputDimension,
    //                       int outputStride) {
    //    var evenResolution = (int)(inputDimension * imageScaleFactor) - 1;
    //    return evenResolution - (evenResolution % outputStride) + 1;
    //}

    //int Half(int k)
    //{
    //    return (int)Mathf.Floor((float)(k / 2));
    //}

    //--------------------------------------------------------------------

    // A unility class with functions to scale Texture2D Data.
    //
    // Scale is performed on the GPU using RTT, so it's blazing fast.
    // Setting up and Getting back the texture data is the bottleneck. 
    // But Scaling itself costs only 1 draw call and 1 RTT State setup!
    // WARNING: This script override the RTT Setup! (It sets a RTT!)	 
    //
    // Note: This scaler does NOT support aspect ratio based scaling. You will have to do it yourself!
    // It supports Alpha, but you will have to divide by alpha in your shaders, 
    // because of premultiplied alpha effect. Or you should use blend modes.
    //
    // <summary>
    //	Returns a scaled copy of given texture. 
    // </summary>
    // <param name='text'>Source texure to scale</param>
    // <param name="width">Destination texture width</param>
    // <param name="height">Destination texture height</param>
    // <param name="mode">Filtering mode</param>

    public Texture2D scaled(Texture2D src, int width, int height, FilterMode mode = FilterMode.Trilinear)
    {
        Rect texR = new Rect(0, 0, width, height);
        _gpu_scale(src, width, height, mode);

        //Get rendered data back to a new texture
        Texture2D result = new Texture2D(width, height, TextureFormat.RGBA32, true);
        result.Resize(width, height);
        result.ReadPixels(texR, 0, 0, true);

        var randomInt = UnityEngine.Random.Range(0, 100000000000000000);
        File.WriteAllBytes(Application.persistentDataPath + "/pose-" + randomInt + ".jpg", result.EncodeToJPG());
        Debug.Log("Saved size converted image path: " + Application.persistentDataPath + "/pose-" + randomInt + ".jpg");
        return result;
    }

    // <summary>
    // Scales the texture data of the given texture.
    // </summary>
    // <param name="tex">Texure to scale</param>
    // <param name="width">New width</param>
    // <param name="height">New height</param>
    // <param name="mode">Filtering mode</param>
    public void scale(Texture2D tex, int width, int height, FilterMode mode = FilterMode.Trilinear)
    {
        Rect texR = new Rect(0, 0, width, height);
        _gpu_scale(tex, width, height, mode);

        // Update new texture
        tex.Resize(width, height);
        tex.ReadPixels(texR, 0, 0, true);
        tex.Apply(true);    //Remove this if you hate us applying textures for you :)
    }

    // Internal unility that renders the source texture into the RTT - the scaling method itself.
    static void _gpu_scale(Texture2D src, int width, int height, FilterMode fmode)
    {
        //We need the source texture in VRAM because we render with it
        src.filterMode = fmode;
        src.Apply(true);

        //Using RTT for best quality and performance. Thanks, Unity 5
        RenderTexture rtt = new RenderTexture(width, height, 32);

        //Set the RTT in order to render to it
        Graphics.SetRenderTarget(rtt);

        //Setup 2D matrix in range 0..1, so nobody needs to care about sized
        GL.LoadPixelMatrix(0, 1, 1, 0);

        //Then clear & draw the texture to fill the entire RTT.
        GL.Clear(true, true, new Color(0, 0, 0, 0));
        Graphics.DrawTexture(new Rect(0, 0, 1, 1), src);
    }

}