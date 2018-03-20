// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/HatchShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Hatch01 ("Hatch01 (RGB)", 2D) = "white" {}
		_Hatch02 ("Hatch02 (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
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

            v2f vert (
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
            
            fixed4 frag (v2f i) : SV_Target
            {
				float diffuse = dot(_WorldSpaceLightPos0, normalize(i.normal));
				float3 color;
				if (diffuse > .5f) {
					color = tex2D(_Hatch01,i.uv);
				} else {
					color = tex2D(_Hatch02,i.uv);
				}

                return fixed4(color, 0);
            }
            ENDCG
        }
    }
	FallBack "Diffuse"
}
