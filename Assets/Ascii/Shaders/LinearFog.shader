
// http://unity3d.com/support/documentation/Components/SL-Shader.html
Shader "Custom/Camera/Linear Fog Image Effect"
{
	// http://unity3d.com/support/documentation/Components/SL-Properties.html
	Properties
	{
	  _MainTex("Base (RGB)", 2D) = "white" {}
	}

		CGINCLUDE
#include "UnityCG.cginc"

		uniform sampler2D _MainTex;

	float useFog = 1.0f;
	float fogDensity = 1.0f;
	float4 fogColor;
	sampler2D _CameraDepthNormalsTexture;



	struct VertexShaderInput
	{
		float4 Position : POSITION;  // already transformed position.
		float2 UV : TEXCOORD0; // texture coordinates passed in
	};

	struct VertexShaderOutput
	{
		float4 Position : POSITION; // just need to copy the position of the VS input in this one.
		float2 UV : TEXCOORD0; // first set of tex coords.
	};

	VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
	{
		VertexShaderOutput output = (VertexShaderOutput)0;

		// copy the existing position into the output.
		output.Position = input.Position;
		output.UV = input.UV;

		return output;
	}

	float4 PixelShaderFunction(v2f_img i) : COLOR // was float2 texCoord : TEXCOORD0) : COLOR0
	{
		float4 tex1 = tex2D(_MainTex, i.uv); // input.UV

		//Fog Depth
		float4 NormalDepth;
		DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv), NormalDepth.w, NormalDepth.xyz);
		fixed4 fog = (NormalDepth.w * fogDensity) * useFog;
		fixed4 inverseFog = 1 - fog;

		//Fog
		fog *= fogColor;
		tex1 *= inverseFog;
		tex1 += fog;

		return tex1;
	}

ENDCG

	// Techniques (http://unity3d.com/support/documentation/Components/SL-SubShader.html).
	SubShader
	{
		// Tags (http://docs.unity3d.com/Manual/SL-CullAndDepth.html).
		ZTest Always
		Cull Off
		ZWrite Off
		Fog { Mode off }

		// Pass 0: Normal
		Pass
		{
		  CGPROGRAM
		  #pragma glsl
		  #pragma fragmentoption ARB_precision_hint_fastest
		  #pragma target 3.0
		  #pragma vertex vert_img
		  #pragma fragment PixelShaderFunction
		  ENDCG
		}

	}

	Fallback off
}