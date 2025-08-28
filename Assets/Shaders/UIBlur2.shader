Shader "UI/MaskedUIBlur_NoGrab"
{
    Properties
    {
        _SourceTex ("Source (captured RT)", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 10)) = 1
        _TintAdd ("Additive Tint", Color) = (0,0,0,0)
        _TintMul ("Multiply Tint", Color) = (1,1,1,1)
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
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _SourceTex;
            float4    _SourceTex_TexelSize;
            float     _BlurSize;

            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; float4 color:COLOR; };
            struct v2f { float4 pos:SV_POSITION; float2 uv:TEXCOORD0; float4 color:COLOR; };

            v2f vert(appdata v){
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = v.uv;
                o.color = v.color; // RawImage color/alpha as mask
                return o;
            }

            float4 _TintAdd, _TintMul;

            fixed4 frag(v2f i):SV_Target
            {
                float2 off = _SourceTex_TexelSize.xy * _BlurSize;

                // 3x3 box blur on _SourceTex
                fixed4 c = 0;
                [unroll] for(int x=-1;x<=1;x++)
                [unroll] for(int y=-1;y<=1;y++)
                    c += tex2D(_SourceTex, i.uv + float2(x,y)*off);

                c /= 9.0;

                // tint & mask by RawImage vertex color (alpha drives opacity)
                c.rgb = c.rgb * _TintMul.rgb + _TintAdd.rgb;
                c.a   = i.color.a; // use RawImage alpha as the popup bg opacity
                return c;
            }
            ENDHLSL
        }
    }
}
