Shader "Custom/EdgeGlowing"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "red" {}
        _GlowColor ("Glow Color", Color) = (1,1,1,1)
        _GlowStrength ("Glow Strength", Range(0, 5)) = 1
        _EdgeSensitivity ("Edge Sensitivity", Range(0, 1)) = 0.1
    }

    SubShader 
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 svpos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; 
            float4 _GlowColor;
            float _GlowStrength;
            float _EdgeSensitivity;

            v2f vert (appdata i)
            {
                v2f o;
                o.svpos = UnityObjectToClipPos(i.pos);
                o.uv = i.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);
                
                // Calculate edges
                half2 offset = _EdgeSensitivity * _MainTex_TexelSize.xy;
                half sum = 0;
                sum += abs(tex2D(_MainTex, i.uv + half2(-offset.x, 0)).r - col.r);
                sum += abs(tex2D(_MainTex, i.uv + half2(offset.x, 0)).r - col.r);
                sum += abs(tex2D(_MainTex, i.uv + half2(0, offset.y)).r - col.r);
                sum += abs(tex2D(_MainTex, i.uv + half2(0, -offset.y)).r - col.r);

                // Blend the edge glow
                col.rgb = lerp(col.rgb, _GlowColor.rgb, sum * _GlowStrength);
                
                return col;
            }
            ENDCG
        }
    }
}
