Shader "Hidden/VacuumShaders/ResizePro/BlitCopy" 
{
	Properties 
	{
		_MainTex ("Texture", any) = "" {}
	}
	
	CGINCLUDE
	#include "UnityCG.cginc" 
		 
sampler2D _MainTex;	 



#ifdef UNITY_COLORSPACE_GAMMA
	#define ADJUST_COLOR_BY_COLOR_SPACE(value) 
#else
	//#define ADJUST_COLOR_BY_COLOR_SPACE(value) value = LinearToGammaSpace(value);

	//No need for Linear color conversion in Unity 2018.1 ???
	#define ADJUST_COLOR_BY_COLOR_SPACE(value)
#endif

	inline float4 LinearToGammaSpace(float4 linRGB)
	{
		linRGB.rgb = max(linRGB.rgb, half3(0.h, 0.h, 0.h));
		// An almost-perfect approximation from http://chilliant.blogspot.com.au/2012/08/srgb-approximations-for-hlsl.html?m=1
		return float4(max(1.055h * pow(linRGB.rgb, 0.416666667h) - 0.055h, 0.h).rgb, linRGB.a);
	}


	float4 frag_adj (v2f_img i) : SV_Target     
	{           		 
		float4 c = tex2D(_MainTex, i.uv);

		ADJUST_COLOR_BY_COLOR_SPACE(c)

		return c;
	}     

	ENDCG 
	 
	Subshader 
	{
	    ZTest Always Cull Off ZWrite Off
	    Fog { Mode off } 
		 
		Pass 
		{    
		    CGPROGRAM
		    #pragma vertex vert_img
		    #pragma fragment frag_adj 

		    ENDCG
		}  
	}

	Fallback off
	
} // shader