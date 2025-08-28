Shader "UI/SimpleBlurTint"
{
    Properties
    {
        _MainTex    ("Texture", 2D) = "white" {}
        _BlurSize   ("Blur Size", Range(0, 10)) = 1
        _MultiplyColor ("Multiply Color (darken/tint)", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4    _MainTex_TexelSize;
            float     _BlurSize;
            float4    _MultiplyColor; // rgb=tint/darken, a unused here

            struct appdata {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                float4 color  : COLOR;     // RawImage's color/alpha
            };

            struct v2f {
                float4 pos   : SV_POSITION;
                float2 uv    : TEXCOORD0;
                float4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos   = UnityObjectToClipPos(v.vertex);
                o.uv    = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 offset = _MainTex_TexelSize.xy * _BlurSize;
                fixed4 sum = 0;

                // 3x3 box blur
                [unroll] for (int x = -1; x <= 1; x++)
                {
                    [unroll] for (int y = -1; y <= 1; y++)
                    {
                        sum += tex2D(_MainTex, uv + float2(x, y) * offset);
                    }
                }

                fixed4 col = sum / 9.0;

                // Multiply tint (use <1.0 to darken, or tint color)
                col.rgb *= _MultiplyColor.rgb;

                // Use RawImage's color alpha as final opacity (so you can fade in/out)
                col.a = i.color.a;

                return col;
            }
            ENDHLSL
        }
    }
}
