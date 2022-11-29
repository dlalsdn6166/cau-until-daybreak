Shader "Shader/Outline" {
    Properties
    {
		_Color("Color", Color) = (1,0,0,1)
        _Width("Width", Float) = .1
    }

    SubShader{
        Pass {
            Cull Front

            CGPROGRAM

            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag

            float4 _Color;
            half _Width;

            float4 vert(float4 pos : POSITION) : POSITION
            {
                return UnityObjectToClipPos(pos * (1 + _Width));
            }

            half4 frag() : COLOR
            {
                return _Color;
            }
            ENDCG
        }
    }
}