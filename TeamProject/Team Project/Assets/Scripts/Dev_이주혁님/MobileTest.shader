Shader "Custom/MobileTesst"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        // 문제2. 0에서 1까지의 범위를 갖는 프로퍼티를 선언하시오. 단, 프로퍼티 이름은 _Alpha이다.
        _Alpha ("Alpha", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        // 문제3. 2번의 문제를 연산에 사용하기 위한 선언을 하시오.
        // SurfaceOutputStandard의 Alpha 프로퍼티는 fixed형임.
        fixed _Alpha;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            // 문제1. 조명연산을 고려하지 않은 Blue 값을 갖는 세이더 코드를 작성하시오.
            //o.Emission = fixed3(0,0,1);            
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            // 문제4. _Alpha의 값을 출력결과 구조체에 적용하시오.                
            o.Alpha = _Alpha;
           
        }
        ENDCG
    }
    FallBack "Diffuse"
}