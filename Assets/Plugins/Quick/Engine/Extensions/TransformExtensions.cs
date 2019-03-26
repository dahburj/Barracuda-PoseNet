// Copyright (c) 2017 Doozy Entertainment / Marlink Trading SRL and Ez Entertainment / Ez Entertainment SRL. All Rights Reserved.
// This code is a collaboration between Doozy Entertainment and Ez Entertainment and is not to be used in any other assets other then the ones created by their respective companies.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;

namespace QuickEngine.Extensions
{
    public static class TransformExtensions
    {
        #region Position
        /// <summary>
        /// Sets the X position of this transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="x">New position X.</param>
        public static void SetX(this Transform transform, float x)
        {
            var newPosition = new Vector3(x, transform.position.y, transform.position.z);
            transform.position = newPosition;
        }
        /// <summary>
        /// Sets the Y position of this transform.
        /// </summary>
        /// <param name="transform">The Transform.</param>
        /// <param name="y">New position Y.</param>
        public static void SetY(this Transform transform, float y)
        {
            var newPosition = new Vector3(transform.position.x, y, transform.position.z);

            transform.position = newPosition;
        }
        /// <summary>
        /// Sets the Z position of this transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="z">New position Z.</param>
        public static void SetZ(this Transform transform, float z)
        {
            var newPosition = new Vector3(transform.position.x, transform.position.y, z);

            transform.position = newPosition;
        }
        /// <summary>
        /// Sets the X and Y position of this transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="x">New position X.</param>
        /// <param name="y">New position Y.</param>
        public static void SetXY(this Transform transform, float x, float y)
        {
            var newPosition = new Vector3(x, y, transform.position.z);
            transform.position = newPosition;
        }
        /// <summary>
        /// Sets the X and Z position of this transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="x">New position X.</param>
        /// <param name="z">New position Y.</param>
        public static void SetXZ(this Transform transform, float x, float z)
        {
            var newPosition = new Vector3(x, transform.position.y, z);
            transform.position = newPosition;
        }
        /// <summary>
        /// Sets the Y and Z position of this transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="y">New position Y.</param>
        /// <param name="z">New position Z.</param>
        public static void SetYZ(this Transform transform, float y, float z)
        {
            var newPosition = new Vector3(transform.position.x, y, z);
            transform.position = newPosition;
        }
        /// <summary>
        /// Sets the X, Y and Z position of this transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="x">New position X.</param>
        /// <param name="y">New position Y.</param>
        /// <param name="z">New position Z.</param>
        public static void SetXYZ(this Transform transform, float x, float y, float z)
        {
            var newPosition = new Vector3(x, y, z);
            transform.position = newPosition;
        }

        /// <summary>
        /// Translates this transform along the X axis.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="x">Distance on the X axis.</param>
        public static void TranslateX(this Transform transform, float x)
        {
            var offset = new Vector3(x, 0, 0);
            transform.position += offset;
        }
        /// <summary>
        /// Translates this transform along the Y axis.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="y">Distance on the Y axis.</param>
        public static void TranslateY(this Transform transform, float y)
        {
            var offset = new Vector3(0, y, 0);

            transform.position += offset;
        }
        /// <summary>
        /// Translates this transform along the Z axis.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="z">Distance on the Z axis.</param>
        public static void TranslateZ(this Transform transform, float z)
        {
            var offset = new Vector3(0, 0, z);
            transform.position += offset;
        }
        /// <summary>
        /// Translates this transform along the X, Y and Z axis.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="x">Distance on the X axis.</param>
        /// <param name="y">Distance on the Y axis.</param>
        /// <param name="z">Distance on the Z axis.</param>
        public static void TranslateXYZ(this Transform transform, float x, float y, float z)
        {
            var offset = new Vector3(x, y, z);
            transform.position += offset;
        }
        /// <summary>
        /// Sets the local X position of this transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="x">New localPosition X.</param>
        public static void SetLocalX(this Transform transform, float x)
        {
            var newPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
            transform.localPosition = newPosition;
        }
        /// <summary>
        /// Sets the local Y position of this transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="y">New localPosition Y.</param>
        public static void SetLocalY(this Transform transform, float y)
        {
            var newPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
            transform.localPosition = newPosition;
        }
        /// <summary>
        /// Sets the local Z position of this transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="z">New localPosition Z.</param>
        public static void SetLocalZ(this Transform transform, float z)
        {
            var newPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
            transform.localPosition = newPosition;
        }
        /// <summary>
        /// Sets the local X and Y position of this transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="x">New localPosition X.</param>
        /// <param name="y">New localPosition Y.</param>
        public static void SetLocalXY(this Transform transform, float x, float y)
        {
            var newPosition = new Vector3(x, y, transform.localPosition.z);
            transform.localPosition = newPosition;
        }
        /// <summary>
        /// Sets the local X and Z position of this transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="x">New localPosition X.</param>
        /// <param name="z">New localPosition Z.</param>
        public static void SetLocalXZ(this Transform transform, float x, float z)
        {
            var newPosition = new Vector3(x, transform.localPosition.z, z);
            transform.localPosition = newPosition;
        }
        /// <summary>
        /// Sets the local Y and Z position of this transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="y">New localPosition Y.</param>
        /// <param name="z">New localPosition Z.</param>
        public static void SetLocalYZ(this Transform transform, float y, float z)
        {
            var newPosition = new Vector3(transform.localPosition.x, y, z);
            transform.localPosition = newPosition;
        }
        /// <summary>
        /// Sets the local X, Y and Z position of this transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="x">New localPosition X.</param>
        /// <param name="y">New localPosition Y.</param>
        /// <param name="z">New localPosition Z.</param>
        public static void SetLocalXYZ(this Transform transform, float x, float y, float z)
        {
            var newPosition = new Vector3(x, y, z);
            transform.localPosition = newPosition;
        }

        /// <summary>
        /// Sets the position to 0, 0, 0.
        /// </summary>
        /// <param name="transform">The transform.</param>
        public static void ResetPosition(this Transform transform)
        {
            transform.position = Vector3.zero;
        }
        /// <summary>
        /// Sets the local position to 0, 0, 0.
        /// </summary>
        /// <param name="transform">The transform.</param>
        public static void ResetLocalPosition(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
        }
        #endregion

        #region Rotation
        /// <summary>
        /// Rotates the transform around the X axis.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="angle">Rotation angle.</param>
        public static void RotateAroundX(this Transform transform, float angle)
        {
            var rotation = new Vector3(angle, 0, 0);
            transform.Rotate(rotation);
        }
        /// <summary>
        /// Rotates the transform around the Y axis.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="angle">Rotation angle.</param>
        public static void RotateAroundY(this Transform transform, float angle)
        {
            var rotation = new Vector3(0, angle, 0);
            transform.Rotate(rotation);
        }
        /// <summary>
        /// Rotates the transform around the Z axis.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="angle">Rotation angle.</param>
        public static void RotateAroundZ(this Transform transform, float angle)
        {
            var rotation = new Vector3(0, 0, angle);
            transform.Rotate(rotation);
        }
        /// <summary>
        /// Sets the X rotation.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="angle">Euler angle X.</param>
        public static void SetRotationX(this Transform transform, float angle)
        {
            transform.eulerAngles = new Vector3(angle, 0, 0);
        }
        /// <summary>
        /// Sets the Y rotation.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="angle">Euler angle Y.</param>
        public static void SetRotationY(this Transform transform, float angle)
        {
            transform.eulerAngles = new Vector3(0, angle, 0);
        }
        /// <summary>
        /// Sets the Z rotation.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="angle">Euler angle Z.</param>
        public static void SetRotationZ(this Transform transform, float angle)
        {
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
        /// <summary>
        /// Sets the local X rotation.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="angle">Euler angle X.</param>
        public static void SetLocalRotationX(this Transform transform, float angle)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(angle, 0, 0));
        }
        /// <summary>
        /// Sets the local Y rotation.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="angle">Euler angle Y.</param>
        public static void SetLocalRotationY(this Transform transform, float angle)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
        /// <summary>
        /// Sets the local Z rotation.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="angle">Euler angle Z.</param>
        public static void SetLocalRotationZ(this Transform transform, float angle)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        /// <summary>
        /// Resets the rotation to 0, 0, 0
        /// </summary>
        /// <param name="transform">The transform.</param>
        public static void ResetRotation(this Transform transform)
        {
            transform.rotation = Quaternion.identity;
        }
        /// <summary>
        /// Resets the local rotation to 0, 0, 0
        /// </summary>
        /// <param name="transform">The transform.</param>
        public static void ResetLocalRotation(this Transform transform)
        {
            transform.localRotation = Quaternion.identity;
        }
        #endregion

        #region Scale
        /// <summary>
        /// Sets the local X scale of this transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="x">New X scale.</param>
        public static void SetScaleX(this Transform transform, float x)
        {
            var newScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
            transform.localScale = newScale;
        }
        /// <summary>
        /// Sets the local Y scale of this transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="y">New Y scale.</param>
        public static void SetScaleY(this Transform transform, float y)
        {
            var newScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
            transform.localScale = newScale;
        }
        /// <summary>
        /// Sets the local Z scale of this transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="z">New Z scale.</param>
        public static void SetScaleZ(this Transform transform, float z)
        {
            var newScale = new Vector3(transform.localScale.x, transform.localScale.y, z);
            transform.localScale = newScale;
        }
        /// <summary>
        /// Sets the local X and Y scale of this transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="x">New X scale.</param>
        /// <param name="y">New Y scale.</param>
        public static void SetScaleXY(this Transform transform, float x, float y)
        {
            var newScale = new Vector3(x, y, transform.localScale.z);
            transform.localScale = newScale;
        }
        /// <summary>
        /// Sets the local X and Z scale of this transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="x">New X scale.</param>
        /// <param name="z">New Z scale.</param>
        public static void SetScaleXZ(this Transform transform, float x, float z)
        {
            var newScale = new Vector3(x, transform.localScale.y, z);
            transform.localScale = newScale;
        }
        /// <summary>
        /// Sets the local Y and Z scale of this transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="y">New Y scale.</param>
        /// <param name="z">New Z scale.</param>
        public static void SetScaleYZ(this Transform transform, float y, float z)
        {
            var newScale = new Vector3(transform.localScale.x, y, z);
            transform.localScale = newScale;
        }
        /// <summary>
        /// Sets the local X, Y and Z scale of this transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="x">New X scale.</param>
        /// <param name="y">New Y scale.</param>
        /// <param name="z">New Z scale.</param>
        public static void SetScaleXYZ(this Transform transform, float x, float y, float z)
        {
            var newScale = new Vector3(x, y, z);
            transform.localScale = newScale;
        }
        /// <summary>
        /// Multiply the transform's X scale by the given amout.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="x">X scale multiplier.</param>
        public static void ScaleByX(this Transform transform, float x)
        {
            transform.localScale = new Vector3(transform.localScale.x * x, transform.localScale.y, transform.localScale.z);
        }
        /// <summary>
        /// Scale this transform in the Y direction.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="y">Y scale multiplier.</param>
        public static void ScaleByY(this Transform transform, float y)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * y, transform.localScale.z);
        }
        /// <summary>
        /// Scale this transform in the Z direction.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="z">Z scale multiplier.</param>
        public static void ScaleByZ(this Transform transform, float z)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * z);
        }
        /// <summary>
        /// Scale this transform in the X, Y direction.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="x">X scale multiplier.</param>
        /// <param name="y">Y scale multiplier.</param>
        public static void ScaleByXY(this Transform transform, float x, float y)
        {
            transform.localScale = new Vector3(transform.localScale.x * x, transform.localScale.y * y, transform.localScale.z);
        }
        /// <summary>
        /// Scale this transform in the X, Z directions.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="x">X scale multiplier.</param>
        /// <param name="z">Z scale multiplier.</param>
        public static void ScaleByXZ(this Transform transform, float x, float z)
        {
            transform.localScale = new Vector3(transform.localScale.x * x, transform.localScale.y, transform.localScale.z * z);
        }
        /// <summary>
        /// Scale this transform in the Y and Z directions.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="y">Y scale multiplier.</param>
        /// <param name="z">Z scale multiplier.</param>
        public static void ScaleByYZ(this Transform transform, float y, float z)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * y, transform.localScale.z * z);
        }
        /// <summary>
        /// Scale this transform in the X and Y directions.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="r">X and Y scale multiplier.</param>
        public static void ScaleByXY(this Transform transform, float r)
        {
            transform.ScaleByXY(r, r);
        }
        /// <summary>
        /// Scale this transform in the X and Z directions.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="r">X and Z scale multiplier.</param>
        public static void ScaleByXZ(this Transform transform, float r)
        {
            transform.ScaleByXZ(r, r);
        }
        /// <summary>
        /// Scale this transform in the Y and Z directions.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="r">Y and Z scale multiplier.</param>
        public static void ScaleByYZ(this Transform transform, float r)
        {
            transform.ScaleByYZ(r, r);
        }
        /// <summary>
        /// Scale this transform in the X, Y and Z directions.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="x">X scale multiplier.</param>
        /// <param name="y">Y scale multiplier.</param>
        /// <param name="z">Z scale multiplier.</param>
        public static void ScaleByXYZ(this Transform transform, float x, float y, float z)
        {
            transform.localScale = new Vector3(x, y, z);
        }
        /// <summary>
        /// Scale this transform in the X, Y and Z directions.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="r">X, Y and Z scale multiplier.</param>
        public static void ScaleByXYZ(this Transform transform, float r)
        {
            transform.ScaleByXYZ(r, r, r);
        }
        /// <summary>
        /// Resets the local scale of this transform in to Vector3.one.
        /// </summary>
        /// <param name="transform">The transform.</param>
        public static void ResetScale(this Transform transform)
        {
            transform.localScale = Vector3.one;
        }
        #endregion

        #region FlipScale
        /// <summary>
        /// Negates the X scale.
        /// </summary>
        /// <param name="transform">The transform.</param>
        public static void FlipX(this Transform transform)
        {
            transform.SetScaleX(-transform.localScale.x);
        }
        /// <summary>
        /// Negates the Y scale.
        /// </summary>
        /// <param name="transform">The transform.</param>
        public static void FlipY(this Transform transform)
        {
            transform.SetScaleY(-transform.localScale.y);
        }
        /// <summary>
        /// Negates the Z scale.
        /// </summary>
        /// <param name="transform">The transform.</param>
        public static void FlipZ(this Transform transform)
        {
            transform.SetScaleZ(-transform.localScale.z);
        }
        /// <summary>
        /// Negates the X and Y scale.
        /// </summary>
        /// <param name="transform">The transform.</param>
        public static void FlipXY(this Transform transform)
        {
            transform.SetScaleXY(-transform.localScale.x, -transform.localScale.y);
        }
        /// <summary>
        /// Negates the X and Z scale.
        /// </summary>
        /// <param name="transform">The transform.</param>
        public static void FlipXZ(this Transform transform)
        {
            transform.SetScaleXZ(-transform.localScale.x, -transform.localScale.z);
        }
        /// <summary>
        /// Negates the Y and Z scale.
        /// </summary>
        /// <param name="transform">The transform.</param>
        public static void FlipYZ(this Transform transform)
        {
            transform.SetScaleYZ(-transform.localScale.y, -transform.localScale.z);
        }
        /// <summary>
        /// Negates the X, Y and Z scale.
        /// </summary>
        /// <param name="transform">The transform.</param>
        public static void FlipXYZ(this Transform transform)
        {
            transform.SetScaleXYZ(-transform.localScale.z, -transform.localScale.y, -transform.localScale.z);
        }
        /// <summary>
        /// Sets all scale values to the absolute values.
        /// </summary>
        /// <param name="transform">The transform.</param>
        public static void FlipPostive(this Transform transform)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                Mathf.Abs(transform.localScale.y),
                Mathf.Abs(transform.localScale.z));
        }
        #endregion

        #region Reset & ResetLocal - Position Rotation Scale
        /// <summary>
        /// Resets position, rotation and scale.
        /// </summary>
        /// <param name="transform">The transform.</param>
        public static void Reset(this Transform transform)
        {
            transform.ResetRotation();
            transform.ResetPosition();
            transform.ResetScale();
        }
        /// <summary>
        /// Resets localPosition, localRotation and localScale.
        /// </summary>
        /// <param name="transform">The transform.</param>
        public static void ResetLocal(this Transform transform)
        {
            transform.ResetLocalRotation();
            transform.ResetLocalPosition();
            transform.ResetScale();
        }
        #endregion
    }
}
