Shader "Custom/Ripple"
{
    Properties
    {
        [PerRendererData] _BumpTex("Normal Texture", 2D) = "bump" {}
        [PerRendererData]_BumpStrength("Normal Map Strength", Float) = 1
        [PerRendererData]_Position("Origin Offset", Vector) = (0.0, 0.0, 0.0)
        [PerRendererData]_Distortion("Distortion", Float) = 1
        [PerRendererData]_OuterThick("Ring Thickness", Float) = 2
        [PerRendererData]_FadeAmount("Fade Amount", Range(0,1)) = 0.5
        [PerRendererData]_FalloffDistance("Falloff Distance", Float) = 1
        [PerRendererData]_Speed("Speed", Float) = 2
        [PerRendererData]_Frequency("Frequency", Float) = 2
        [PerRendererData]_Color("Color", Color) = (0,0,0,0)
        [PerRendererData]_BlendType("BlendType", int) = 0
        [PerRendererData]_RippleSpeedMultiplier("Ripple Speed Multiplier", Range(0, 1)) = 0.5
        [PerRendererData]_RippleIntensityMultiplier("Ripple Intensity Multiplier", Range(0, 1)) = 0.5
    }

        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
            LOD 100
            Cull Off

            GrabPass {}

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fog
                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 bumpUV : TEXCOORD0;
                    float4 grabPos : TEXCOORD1;
                    float2 uv :TEXCOORD2;
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                };

                sampler2D _BumpTex;
                sampler2D _GrabTexture;
                float4 _BumpTex_ST;
                uniform float _BumpStrength;
                uniform float _Distortion;
                uniform float _OuterThick;
                uniform float _FadeAmount;
                uniform float3 _Position;
                uniform float _FalloffDistance;
                uniform float _Speed;
                uniform float _Frequency;
                uniform float4 _Color;
                uniform float _BlendType;
                uniform float _RippleSpeedMultiplier;
                uniform float _RippleIntensityMultiplier;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.bumpUV = TRANSFORM_TEX(v.uv, _BumpTex);
                    o.grabPos = ComputeGrabScreenPos(o.vertex);
                    o.uv = v.uv;
                    UNITY_TRANSFER_FOG(o, o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // 1. 获取当前屏幕的颜色
                    fixed4 originalColor = tex2Dproj(_GrabTexture, i.grabPos);

                    // 2. 获取法线贴图的值并进行调整
                    float2 normalValue = tex2D(_BumpTex, i.bumpUV).rg * 2 - 1;

                    // 3. 计算UV坐标与指定位置的差值
                    float2 uvOffset = i.uv - _Position - 0.5;
                    float distanceFromOrigin = length(uvOffset);

                    // 4. 计算水波纹效果的强度
                    //float rippleIntensity = sin(distanceFromOrigin * _Frequency - _Time.y * _Speed) + normalValue * _BumpStrength;
                    float rippleIntensity = _RippleIntensityMultiplier * (sin(distanceFromOrigin * _Frequency - _Time.y * _Speed * _RippleSpeedMultiplier) + normalValue * _BumpStrength);
                    rippleIntensity = pow(abs(rippleIntensity), _OuterThick) - 0.5;

                    // 5. 计算扭曲效果的强度
                    float distortionStrength = clamp(_Distortion * lerp(1, 0, distanceFromOrigin / _FalloffDistance), 0, 1);

                    // 6. 应用扭曲效果
                    i.grabPos.xy += rippleIntensity * (uvOffset / _FalloffDistance) * distortionStrength;

                    // 7. 获取扭曲后的颜色
                    fixed4 distortedColor = tex2Dproj(_GrabTexture, i.grabPos);

                    // 8. 根据水波纹效果的强度调整颜色
                    float4 colorAdjustment = lerp(float4(0,0,0,0), _Color, clamp(rippleIntensity, 0, 1) * distortionStrength * 2);
                    distortedColor = lerp(distortedColor, lerp(distortedColor + colorAdjustment, distortedColor - colorAdjustment, step(2, _BlendType)), _BlendType);

                    // 9. 根据_FadeAmount混合原始颜色和扭曲后的颜色
                    fixed4 finalColor = lerp(distortedColor, originalColor, _FadeAmount);

                     // 10. 应用雾效果
                     UNITY_APPLY_FOG(i.fogCoord, finalColor);

                    return finalColor;
                }
            ENDCG
            }
        }
}
