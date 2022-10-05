Shader "Custom/WorldShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpTex ("Normal Map", 2D) = "bump" {}
         _MainTex2 ("Albedo (RGB)", 2D) = "white" {}
        _BumpTex2 ("Normal Map", 2D) = "bump" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpTex;
        sampler2D _MainTex2;
        sampler2D _BumpTex2;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_MainTex2;
            float2 uv_BumpTex;
            float2 uv_BumpTex2;
            float4 color:Color;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c1 = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            fixed4 c2 = tex2D (_MainTex2, IN.uv_MainTex2) * _Color;
            fixed3 n1 = UnpackNormal(tex2D (_BumpTex, IN.uv_BumpTex));
            fixed3 n2 = UnpackNormal(tex2D (_BumpTex2, IN.uv_BumpTex2));
            o.Albedo = lerp(c1.rgb, c2.rgb, IN.color.r);
            o.Normal= lerp(n1, n2, IN.color.r);
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c1.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
