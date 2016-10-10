Shader "Unlit/VertReproject"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform float4x4 _eyeProjection;

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float2 WorldToViewport(float4x4 camVP, float3 worldPoint)
			{
				float2 result;
				result.x = camVP[0][0] * worldPoint.x + camVP[0][1] * worldPoint.y + camVP[0][2] * worldPoint.z + camVP[0][3];
				result.y = camVP[1][0] * worldPoint.x + camVP[1][1] * worldPoint.y + camVP[1][2] * worldPoint.z + camVP[1][3];
				float num = camVP[3][0] * worldPoint.x + camVP[3][1] * worldPoint.y + camVP[3][2] * worldPoint.z + camVP[3][3];
				num = 1.0 / num;
				result.x *= num;
				result.y *= num;

				result.x = (result.x * 0.5 + 0.5);
				result.y = (result.y * 0.5 + 0.5);

				return result;
			}

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = WorldToViewport(_eyeProjection, v.color.xyz);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}