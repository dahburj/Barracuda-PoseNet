// Copyright (c) 2017 Doozy Entertainment / Marlink Trading SRL and Ez Entertainment / Ez Entertainment SRL. All Rights Reserved.
// This code is a collaboration between Doozy Entertainment and Ez Entertainment and is not to be used in any other assets other then the ones created by their respective companies.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System.Collections;
using UnityEngine;

namespace QuickEngine.Extensions
{
    public static class AudioExtensions
    {
        public static IEnumerator PlayOneShotDelayed(this AudioSource anAudioSource, AudioClip anAudioClip, float aDelay)
        {
            while (aDelay > 0)
            {
                yield return null;
                aDelay -= Time.deltaTime;
            }
            anAudioSource.PlayOneShot(anAudioClip);
        }

        public static AudioType PlatformAudioType()
        {
#if UNITY_EDITOR && UNITY_IOS
			return AudioType.MPEG;
#elif UNITY_IOS
			return AudioType.AUDIOQUEUE;
#elif UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WSA
            return AudioType.OGGVORBIS;
#elif UNITY_ANDROID
			return AudioType.MPEG;
#else
			return AudioType.OGGVORBIS;
#endif
        }

        public static string PlatformAudioExtension()
        {
#if UNITY_EDITOR && UNITY_IOS
			return ".mp3";
#elif UNITY_IOS
			return ".mp3";
#elif UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WSA
            return ".ogg";
#elif UNITY_ANDROID
			return ".mp3";
#else
			return ".ogg";
#endif
        }

        public static string PlatformFileProtocol()
        {
#if UNITY_EDITOR_OSX
			return "file://";
#elif UNITY_EDITOR_WIN
            return "file:///";
#elif UNITY_STANDALONE_WIN || UNITY_WSA
			return "file:///";
#else
			return "file://";
#endif
        }

        public static float ToDecibel(this float linear)
        {
            float dB;

            if (linear != 0)
                dB = 20.0f * Mathf.Log10(linear);
            else
                dB = -144.0f;

            return dB;
        }

        public static float ToLinear(this float dB)
        {
            float linear = Mathf.Pow(10.0f, dB / 20.0f);

            return linear;
        }

    }
}
