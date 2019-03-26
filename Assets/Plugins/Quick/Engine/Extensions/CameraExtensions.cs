// Copyright (c) 2017 Doozy Entertainment / Marlink Trading SRL and Ez Entertainment / Ez Entertainment SRL. All Rights Reserved.
// This code is a collaboration between Doozy Entertainment and Ez Entertainment and is not to be used in any other assets other then the ones created by their respective companies.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;

namespace QuickEngine.Extensions
{
    public static class CameraExtensions
    {
        public static Vector2 Pixel2Units2D(this Camera c)
        {
            if (!c.orthographic)
            {
                Debug.LogError("Pixel2Units2D works only with orthographics camera");
                return Vector2.zero;
            }

            Vector3 point = c.WorldToScreenPoint(c.transform.position + c.transform.forward);
            Vector3 centerPoint = c.ScreenToWorldPoint(point);
            Vector3 upPoint = c.ScreenToWorldPoint(point + new Vector3(0, 1, 0));
            Vector3 rightPoint = c.ScreenToWorldPoint(point + new Vector3(1, 0, 0));

            return new Vector2(Mathf.Abs(rightPoint.x - centerPoint.x), Mathf.Abs(upPoint.y - centerPoint.y));
        }

        public static Vector2 Unit2Pixels2D(this Camera c)
        {
            if (!c.orthographic)
            {
                Debug.LogError("Unit2Pixels2D works only with orthographics camera");
                return Vector2.zero;
            }

            Vector3 point = c.transform.position + c.transform.forward;
            Vector3 centerPoint = c.WorldToScreenPoint(point);
            Vector3 upPoint = c.WorldToScreenPoint(point + new Vector3(0, 1, 0));
            Vector3 rightPoint = c.WorldToScreenPoint(point + new Vector3(1, 0, 0));


            return new Vector2(Mathf.Abs(rightPoint.x - centerPoint.x), Mathf.Abs(upPoint.y - centerPoint.y));
        }

        public static Vector2 ToWorldSize(this Camera camera, Bounds bounds)
        {
            Vector3 origin = new Vector3(bounds.min.x, bounds.max.y);
            Vector3 extents = new Vector3(bounds.max.x, bounds.min.y);

            return new Vector2(extents.x - origin.x, origin.y - extents.y);
        }

        public static Vector2 ToScreenSize(this Camera camera, Bounds bounds)
        {
            Vector3 origin = camera.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y));
            Vector3 extents = camera.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y));

            return new Vector2(extents.x - origin.x, origin.y - extents.y);
        }

        public static Rect ToScreenRect(this Camera camera, Renderer renderer)
        {
            Bounds bounds = renderer.bounds;
            Vector3 origin = camera.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, renderer.transform.position.z));
            Vector3 extents = camera.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, renderer.transform.position.z));

            return new Rect(origin.x, origin.y, extents.x - origin.x, origin.y - extents.y);
        }

        public static Rect ToWorldRect(this Camera camera, Renderer renderer)
        {
            Bounds bounds = renderer.bounds;
            Vector3 origin = new Vector3(bounds.min.x, bounds.max.y, renderer.transform.position.z);
            Vector3 extents = new Vector3(bounds.max.x, bounds.min.y, renderer.transform.position.z);

            return new Rect(origin.x, origin.y, extents.x - origin.x, origin.y - extents.y);
        }

        public static Vector3 EdgePosition(this Camera camera, TextAnchor point, float distance)
        {
            Vector3 result = Vector3.zero;

            switch (point)
            {
                case TextAnchor.LowerCenter:
                    result = camera.ViewportToWorldPoint(new Vector3(0.5f, 0f, distance));
                    break;
                case TextAnchor.LowerLeft:
                    result = camera.ViewportToWorldPoint(new Vector3(0f, 0f, distance));
                    break;
                case TextAnchor.LowerRight:
                    result = camera.ViewportToWorldPoint(new Vector3(1f, 0f, distance));
                    break;
                case TextAnchor.MiddleCenter:
                    result = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distance));
                    break;
                case TextAnchor.MiddleLeft:
                    result = camera.ViewportToWorldPoint(new Vector3(0f, 0.5f, distance));
                    break;
                case TextAnchor.MiddleRight:
                    result = camera.ViewportToWorldPoint(new Vector3(1f, 0.5f, distance));
                    break;
                case TextAnchor.UpperCenter:
                    result = camera.ViewportToWorldPoint(new Vector3(0.5f, 1f, distance));
                    break;
                case TextAnchor.UpperLeft:
                    result = camera.ViewportToWorldPoint(new Vector3(0f, 1f, distance));
                    break;
                case TextAnchor.UpperRight:
                    result = camera.ViewportToWorldPoint(new Vector3(1f, 1f, distance));
                    break;
            }

            return result;
        }
    }
}
