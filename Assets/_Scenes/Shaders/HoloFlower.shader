// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Holo2FirstHand/HoloFlower"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_Offset("Animation", Range(0, 1)) = 0.0
		_Scale("Scale", Range(0,5)) = 0.0
		_Attack("Attack", Range(0,20)) = 0.0
		_Release("Release", Range(0,2)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldNormal;
			float3 viewDir;
			INTERNAL_DATA
		};

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		half _Offset;
		half _Scale;
		half _Attack;
		half _Release;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

		float expSustainedImpulse(float x, float f, float k)
		{
			float s = max(x - f, 0.0);
			return min(x*x / (f*f), 1 + (2.0 / f)*s*exp(-k * s));
		}

		float gain(float x, float k)
		{
			const float a = 0.5*pow(2.0*((x < 0.5) ? x : 1.0 - x), k);
			return (x < 0.5) ? a : 1.0 - a;
		}

		void vert(inout appdata_full v) 
		{
			float2 world = mul(unity_ObjectToWorld, v.vertex);
			float scale =  expSustainedImpulse(max(0, -v.vertex.y * unity_ObjectToWorld[0] * _Scale + _Offset), _Release, _Attack);
			v.vertex.xyz *= float3(scale, 1, scale);
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float f = saturate(dot(normalize(IN.worldNormal), normalize(IN.viewDir)));
			float g = gain(_Offset, 2.);
			fixed4 c = lerp(_Color, tex2D(_MainTex, IN.uv_MainTex), g);
			o.Emission = lerp(_Color, 0, f) * (1-g);
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
