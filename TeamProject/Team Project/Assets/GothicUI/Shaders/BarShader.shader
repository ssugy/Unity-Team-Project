Shader "Custom/BarShader"
{
    Properties
    {
        _NoiseSize ("Noise Size", Vector) = (1, 1, 1, 1)
        _NoiseSpeed ("Noise Speed", Vector) = (1, 1, 1, 1)
        _MaskTexture ("Mask texture", 2D) = "white" {}
        
        _FlashesMultiplier ("Flashes Multiplier", Float) = 2
        _WavesMultiplier ("Waves Multiplier", Float) = 0.5
        
        _HotLineColor ("Hot Line Color", Color) = (1, 1, 1, 1)
		_HotLineHeight ("Hot Line Height", Range(0, 0.1)) = 0.01
		_HotLineBrightness ("Hot Line Brightness", Range(0, 10)) = 1
		
		
		_FillLevel ("Fill Level", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
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
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 color : COLOR;
                float4 vertex : SV_POSITION;
            };
            
            float3 random3(float3 c) {
                float j = 4096.0*sin(dot(c,float3(17.0, 59.4, 15.0)));
                float3 r;
                r.z = frac(512.0*j);
                j *= .125;
                r.x = frac(512.0*j);
                j *= .125;
                r.y = frac(512.0*j);
                return r-0.5;
            }
            
            float simplex3d(float3 p) {
                const float F3 =  0.3333333;
                const float G3 =  0.1666667;
                 /* 1. find current tetrahedron T and it's four vertices */
                 /* s, s+i1, s+i2, s+1.0 - absolute skewed (integer) coordinates of T vertices */
                 /* x, x1, x2, x3 - unskewed coordinates of p relative to each of T vertices*/
                 
                 /* calculate s and x */
                 float3 s = floor(p + dot(p, float3(F3, F3, F3)));
                 float3 x = p - s + dot(s, float3(G3, G3, G3));
                 
                 /* calculate i1 and i2 */
                 float3 e = step(float3(0, 0, 0), x - x.yzx);
                 float3 i1 = e*(1.0 - e.zxy);
                 float3 i2 = 1.0 - e.zxy*(1.0 - e);
                    
                 /* x1, x2, x3 */
                 float3 x1 = x - i1 + G3;
                 float3 x2 = x - i2 + 2.0*G3;
                 float3 x3 = x - 1.0 + 3.0*G3;
                 
                 /* 2. find four surflets and store them in d */
                 float4 w, d;
                 
                 /* calculate surflet weights */
                 w.x = dot(x, x);
                 w.y = dot(x1, x1);
                 w.z = dot(x2, x2);
                 w.w = dot(x3, x3);
                 
                 /* w fades from 0.6 at the center of the surflet to 0.0 at the margin */
                 w = max(0.6 - w, 0.0);
                 
                 /* calculate surflet components */
                 d.x = dot(random3(s), x);
                 d.y = dot(random3(s + i1), x1);
                 d.z = dot(random3(s + i2), x2);
                 d.w = dot(random3(s + 1.0), x3);
                 
                 /* multiply d by w^4 */
                 w *= w;
                 w *= w;
                 d *= w;
                 
                 /* 3. return the sum of the four surflets */
                 return dot(d, float4(52, 52, 52, 52));
            }

            float4 _NoiseSize;
            float4 _NoiseSpeed;
            sampler2D _MaskTexture;
			float4 _MaskTexture_ST;
			float _FlashesMultiplier;
			float _WavesMultiplier;
			
			float _FillLevel;
			float _HotLineHeight;
			float _Brightness;
			float _HotLineBrightness;
			float4 _HotLineColor;
			
			fixed4 hotLineColor(v2f i)
			{
			    fixed4 result = _HotLineColor;
			    float hotLineAlpha = result.a;
			    float val = _FillLevel * (1 + _HotLineHeight) - (_HotLineHeight / 2) - i.uv.x;
			    result = lerp(0, result, 1 - smoothstep(0, _HotLineHeight, abs(val)));
			    result = result * hotLineAlpha;
			    result.a = hotLineAlpha;
			    return result * _HotLineBrightness;
			}
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                o.uv2 = TRANSFORM_TEX(v.uv2, _MaskTexture);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float flashNoiseValue = 0.5 + 0.5 * simplex3d(float3(float2(i.uv.x - _Time.x * _NoiseSpeed.x, i.uv.y) * _NoiseSize.xy, _Time.x * _NoiseSpeed.y) * 32.0);
                float totalNoiseValue = 0.5 + 0.5 * simplex3d(float3(float2(i.uv.x - _Time.x * _NoiseSpeed.z, i.uv.y) * _NoiseSize.zw, _Time.x * _NoiseSpeed.w) * 32.0);
                float maskValue = tex2D(_MaskTexture, i.uv2).r;
                float leftFlashes = flashNoiseValue * maskValue * _FlashesMultiplier;
                float totalWaves = totalNoiseValue * _WavesMultiplier;
                float4 resultColor = i.color * (1 + totalWaves) * (1 + leftFlashes) + hotLineColor(i);

                return resultColor * step(i.uv.x, _FillLevel * (1 + _HotLineHeight));
            }
            ENDCG
        }
    }
}
