Shader "Custom/UnlitTextureMix"
{
    Properties
    {
        _Tex1 ("Texture1", 2D) = "white" {}
        _Tex2 ("Texture2", 2D) = "white" {}
        _MixValue("MixValue", Range(0,1)) = 0.5
        _Color("Main Color", COLOR) = (1,1,1,1)
        _Height("Height", Range(0,2)) = 0.5 // сила изгиба

    }
    SubShader{
			Tags{"Queue" = "TransParent""IngnorProjector" = "True""RenderType" = "TransParent"}// Установите очередь рендеринга
				Pass{
				Tags{"LightMode" = "ForwardBase"}
			
				ZWrite Off// Закрыть zwrite.
			Blend SrcAlpha OneMinusSrcAlpha// Укажите смешанную функцию
            CGPROGRAM
            #pragma vertex vert // директива для обработки вершин
            #pragma fragment frag // директива для обработки фрагментов
            #include "UnityCG.cginc" // библиотека с полезными функциями
            sampler2D _Tex1; // текстура1
            float4 _Tex1_ST;
            sampler2D _Tex2; // текстура2
            float4 _Tex2_ST;
            float _MixValue; // параметр смешивания
            float4 _Color; // цвет, которым будет окрашиваться изображение
            float _Height; // сила изгиба

            // структура, которая помогает преобразовать данные вершины в данные фрагмента
            struct v2f
            {
                float2 uv : TEXCOORD0; // UV-координаты вершины
                float4 vertex : SV_POSITION; // координаты вершины
            };

            //здесь происходит обработка вершин
            v2f vert (appdata_full v)
            {
                v2f result;
                //v.vertex.xyz -= v.normal * _Height * v.vertex.x * v.vertex.x;
                result.vertex = UnityObjectToClipPos(v.vertex);
                result.uv = TRANSFORM_TEX(v.texcoord, _Tex1);
                return result;
            }

            //здесь происходит обработка пикселей, цвет пикселей умножается на цвет материала
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color;
                color = tex2D(_Tex1, i.uv) * _MixValue;
                color += tex2D(_Tex2, i.uv) * (1 - _MixValue);
                color = color * _Color;
                color.w*=_Height;
                return color;
            }
            ENDCG
        }


    }
    FallBack "Diffuse"
}
