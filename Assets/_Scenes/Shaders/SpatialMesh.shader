Shader "Holo2FirstHand/SpatialMesh"
{
	Properties
	{
		_BaseColor("Base color", Color) = (0.0, 0.0, 0.0, 1.0)
		_MainTex("Color (RGB)", 2D) = "white" {}
		_Min("Min", Range(0, 1)) = 0
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			float4 _BaseColor;
			uniform sampler2D	_MainTex;
			uniform half4		_MainTex_ST;
			float _Min;

			struct v2f
			{
				fixed4 vertex : SV_POSITION;
				fixed3 normal : NORMAL;
				fixed4 world : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert(appdata_base v)
			{
				UNITY_SETUP_INSTANCE_ID(v);
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = mul(unity_ObjectToWorld, fixed4(v.normal, 0.0)).xyz;
				o.world = mul(unity_ObjectToWorld, v.vertex);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				float s = smoothstep(_Min, 1, dot(normalize(i.normal), half3(0,1,0)));
				return s * tex2D(_MainTex, i.world.xz * _MainTex_ST.xy + _MainTex_ST.zw) * _BaseColor;
			}
			ENDCG
		}
	}
}
