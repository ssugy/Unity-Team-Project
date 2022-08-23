Shader "Custom/FluideDouble"
{
	Properties
	{
		_MainTex ("First Texture", 2D) = "white" {}
		_Speed1 ("Speed", Vector) = (0, 0, 0, 0)

		_SecondTex ("Second Texture", 2D) = "white" {}
		_Speed2 ("Speed", Vector) = (0, 0, 0, 0)

		_ThirdTex ("Third Texture", 2D) = "white" {}
		_Speed3 ("Speed", Vector) = (0, 0, 0, 0)
		
		_MainColor ("Color", Color) = (1, 1, 1, 1)
		_Brightness ("Brightness", Range(1, 100)) = 1
		
		_HotLineColor ("Hot Line Color", Color) = (1, 1, 1, 1)
		_HotLineHeight ("Hot Line Height", Range(0, 0.1)) = 0.01
		_HotLineBrightness ("Hot Line Brightness", Range(0, 10)) = 1
		
		_AlphaColor ("Unfilled Part", Color) = (0, 0, 0, 0)
		_FillLevel ("Fill Level", Range(0, 1)) = 1
		_FadeAreaHeight ("Fade Area Height", Range(0, 0.2)) = 0.05
		//------------------------------------------------
		_MainTex1 ("First Texture", 2D) = "white" {}
		_Speed11 ("Speed", Vector) = (0, 0, 0, 0)

		_SecondTex1 ("Second Texture", 2D) = "white" {}
		_Speed21 ("Speed", Vector) = (0, 0, 0, 0)

		_ThirdTex1 ("Third Texture", 2D) = "white" {}
		_Speed31 ("Speed", Vector) = (0, 0, 0, 0)
		
		_MainColor1 ("Color", Color) = (1, 1, 1, 1)
		_Brightness1 ("Brightness", Range(1, 100)) = 1
		
		_HotLineColor1 ("Hot Line Color", Color) = (1, 1, 1, 1)
		_HotLineHeight1 ("Hot Line Height", Range(0, 0.1)) = 0.01
		_HotLineBrightness1 ("Hot Line Brightness", Range(0, 10)) = 1
		
		_AlphaColor1 ("Unfilled Part", Color) = (0, 0, 0, 0)
		_FillLevel1 ("Fill Level", Range(0, 1)) = 1
		_FadeAreaHeight1 ("Fade Area Height", Range(0, 0.2)) = 0.05
		
		_VerticalSeparator ("Vertical Separator", Range(0, 1)) = 0.5
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True"}
		LOD 100

        ZWrite Off
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float2 uv3 : TEXCOORD2;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float2 _Speed1;
			
			sampler2D _SecondTex;
			float4 _SecondTex_ST;
			float2 _Speed2;
			
			sampler2D _ThirdTex;
			float4 _ThirdTex_ST;
			float2 _Speed3;
			
			float4 _AlphaColor;
			float4 _HotLineColor;
			float4 _MainColor;
			float _FillLevel;
			float _FadeAreaHeight;
			float _HotLineHeight;
			float _Brightness;
			float _HotLineBrightness;
			
			float _VerticalSeparator;
			
			//---------------------------
			sampler2D _MainTex1;
			float4 _MainTex1_ST;
			float2 _Speed11;
			
			sampler2D _SecondTex1;
			float4 _SecondTex1_ST;
			float2 _Speed21;
			
			sampler2D _ThirdTex1;
			float4 _ThirdTex1_ST;
			float2 _Speed31;
			
			float4 _AlphaColor1;
			float4 _HotLineColor1;
			float4 _MainColor1;
			float _FillLevel1;
			float _FadeAreaHeight1;
			float _HotLineHeight1;
			float _Brightness1;
			float _HotLineBrightness1;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.uv, _SecondTex);
				o.uv3 = TRANSFORM_TEX(v.uv, _ThirdTex);
				return o;
			}
			
			fixed4 resolveMainColor(v2f i)
			{
			    _FillLevel = lerp(-_FadeAreaHeight, 1, _FillLevel);
			    _FillLevel = _FillLevel * _MainTex_ST.y;
			    float filledColor = smoothstep(_FillLevel, _FillLevel + _FadeAreaHeight, i.uv.y - _MainTex_ST.w);
			    return lerp(_MainColor, _AlphaColor, filledColor);
			}
			
			fixed4 resolveMainColor1(v2f i)
			{
			    _FillLevel1 = lerp(-_FadeAreaHeight1, 1, _FillLevel1);
			    _FillLevel1 = _FillLevel1 * _MainTex1_ST.y;
			    float filledColor = smoothstep(_FillLevel1, _FillLevel1 + _FadeAreaHeight1, i.uv.y - _MainTex1_ST.w);
			    return lerp(_MainColor1, _AlphaColor1, filledColor);
			}

			fixed4 proccessTexture(float2 uv, sampler2D txt, float4 offsets, float2 animationOffsets)
            {
                float2 coordinates = uv;
                float2 p = 2 * (coordinates - offsets.zw) / offsets.xy - 1;
                float r = dot(p, p);
                int stp = step(0, 1 - r);
                float f = (1 - pow(1.00 - r, 0.5)) / r;
                coordinates.x = offsets.x * p.x * f + offsets.z;
                coordinates.y = offsets.y * p.y * f + offsets.w;
                coordinates = coordinates / 2 - 0.5 + animationOffsets;
                return stp * float4(tex2D(txt, coordinates).rgb, 1.0) + (1 - stp) * float4(0, 0, 0, 0);
            }
			
			fixed4 setupTextures(v2f i) 
			{
				fixed4 texture1 = proccessTexture(i.uv, _MainTex, _MainTex_ST, _Speed1 * _Time.x);
 				fixed4 texture2 = proccessTexture(i.uv2, _SecondTex, _SecondTex_ST, _Speed2 * _Time.x);
 				fixed4 texture3 = proccessTexture(i.uv3, _ThirdTex, _ThirdTex_ST, _Speed3 * _Time.x);
			    return texture1 * texture2 * texture3;
			}
			
			fixed4 setupTextures1(v2f i) 
			{
				fixed4 texture1 = proccessTexture(i.uv, _MainTex1, _MainTex1_ST, _Speed11 * _Time.x);
 				fixed4 texture2 = proccessTexture(i.uv2, _SecondTex1, _SecondTex1_ST, _Speed21 * _Time.x);
 				fixed4 texture3 = proccessTexture(i.uv3, _ThirdTex1, _ThirdTex1_ST, _Speed31 * _Time.x);
			    return texture1 * texture2 * texture3;
			}
			
			fixed4 hotLineColor(v2f i)
			{
			    fixed4 result = _HotLineColor;
			    float hotLineAlpha = result.a;
			    result = lerp(0, result, 1 - smoothstep(0, _HotLineHeight, abs(_FillLevel + _HotLineHeight - i.uv.y + _MainTex_ST.w)));
			    result = result * hotLineAlpha;
			    result.a = hotLineAlpha;
			    return result * _HotLineBrightness;
			}
			
			fixed4 hotLineColor1(v2f i)
			{
			    fixed4 result = _HotLineColor1;
			    float hotLineAlpha = result.a;
			    result = lerp(0, result, 1 - smoothstep(0, _HotLineHeight1, abs(_FillLevel1 + _HotLineHeight1 - i.uv.y + _MainTex1_ST.w)));
			    result = result * hotLineAlpha;
			    result.a = hotLineAlpha;
			    return result * _HotLineBrightness1;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			    fixed4 appliableColor = resolveMainColor(i);
			    fixed4 appliableColor1 = resolveMainColor1(i);
			    
			    fixed4 multipliedTextures = setupTextures(i);
			    fixed4 multipliedTextures1 = setupTextures1(i);
			    
				float resultAlpha = appliableColor.a;
				float hotLineValue = (appliableColor.r + appliableColor.g + appliableColor.b) / 3;
				appliableColor = appliableColor + hotLineValue * hotLineColor(i);
				fixed4 result = multipliedTextures * appliableColor;
                result = result * _Brightness;
                result.a = resultAlpha * multipliedTextures.a;
                
                float resultAlpha1 = appliableColor1.a;
				float hotLineValue1 = (appliableColor1.r + appliableColor1.g + appliableColor1.b) / 3;
				appliableColor1 = appliableColor1 + hotLineValue1 * hotLineColor1(i);
				fixed4 result1 = multipliedTextures1 * appliableColor1;
                result1 = result1 * _Brightness1;
                result1.a = resultAlpha1 * multipliedTextures1.a;
                
                
                float separator = step(_VerticalSeparator, i.uv.x);
                
                
				return result * separator + result1 * (1 - separator);
			}
			
			
			
			ENDCG
		}
	}
}
