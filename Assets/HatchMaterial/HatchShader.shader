
Shader "Custom/HatchShader" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_Hatch01("Hatch01 (RGB)", 2D) = "white" {}
		_Hatch02("Hatch02 (RGB)", 2D) = "white" {}
		_Hatch03("Hatch03 (RGB)", 2D) = "white" {}
		_Hatch04("Hatch04 (RGB)", 2D) = "white" {}
		_Hatch05("Hatch05 (RGB)", 2D) = "white" {}
		_Hatch06("Hatch06 (RGB)", 2D) = "white" {}
	}
		SubShader
	{
		Pass
	{
		CGPROGRAM
		// include file that contains UnityObjectToWorldNormal helper function
#include "UnityCG.cginc"
#pragma vertex vert
#pragma fragment frag

		struct v2f {
		float2 uv : TEXCOORD0;
		float4 pos : SV_POSITION;
		float3 normal : NORMAL;
	};

	v2f vert(
		float4 vertex : POSITION, // vertex position input
		float2 uv : TEXCOORD0, // first texture coordinate input
		float3 normal : NORMAL
	)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(vertex);
		o.uv = uv;
		o.normal = UnityObjectToWorldNormal(normal);
		return o;
	}

	sampler2D _Hatch01;
	sampler2D _Hatch02;
	sampler2D _Hatch03;
	sampler2D _Hatch04;
	sampler2D _Hatch05;
	sampler2D _Hatch06;

	fixed4 frag(v2f i) : SV_Target
	{
		float diffuse = dot(_WorldSpaceLightPos0, normalize(i.normal));
		float3 color;
	

		if (diffuse < .16f) {
			color = tex2D(_Hatch06, i.uv);
		}
		else if (diffuse < .32f) {
			color = tex2D(_Hatch05, i.uv);
		}
		else if (diffuse < .48f) {
			color = tex2D(_Hatch04, i.uv);
		}
		else if (diffuse < .64f) {
			color = tex2D(_Hatch03, i.uv);
		}
		else if (diffuse < .8f) {
			color = tex2D(_Hatch02, i.uv);
		}
		else {
			color = tex2D(_Hatch01, i.uv);
		}

		return fixed4(color, 0);
	}
		ENDCG
	}
	}
		FallBack "Diffuse"
}