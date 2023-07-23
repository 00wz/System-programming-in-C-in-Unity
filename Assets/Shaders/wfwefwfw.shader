Shader "MyShader/13_Alpha"{

	Properties{
		_Single("Single Color",Color)=(1,1,1,1)
		_MainTex("Main Tex",2D)="white"{}
		_NormalMap("Normal Map",2D)="bump"{}
		_BumpScale("Bump Scale",Float) = 1
		_AlphaScale("Alpha Scale",Range(0,1)) = 1

	}

		SubShader{
			Tags{"Queue" = "TransParent""IngnorProjector" = "True""RenderType" = "TransParent"}// Установите очередь рендеринга
				Pass{
				Tags{"LightMode" = "ForwardBase"}
			
				ZWrite Off// Закрыть zwrite.
			Blend SrcAlpha OneMinusSrcAlpha// Укажите смешанную функцию
				CGPROGRAM
			#include "Lighting.cginc"
			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			sampler2D _NormalMap;
			float4 _Single;
			float _BumpScale;
			float4 _MainTex_ST;
			float4 _NormalMap_ST;
			half _AlphaScale;

			struct v2f {

			float4 pos:SV_POSITION;
			float3 lightDir:TEXCOORD0;
			float4 worldVertex:TEXCOORD1;
			float4 uv:TEXCOORD2;


				};

			v2f vert(appdata_full v) {
				v2f f;
				f.pos = UnityObjectToClipPos(v.vertex);

				f.worldVertex = mul( v.vertex, unity_WorldToObject);

				f.uv.xy = v.texcoord.xy*_MainTex_ST.xy + _MainTex_ST.zw;
				f.uv.zw = v.texcoord.xy*_NormalMap_ST.xy + _NormalMap_ST.zw;

				TANGENT_SPACE_ROTATION;
				f.lightDir = mul(rotation, ObjSpaceLightDir(v.vertex));

				return f;

				}

			fixed4 frag(v2f f) :SV_Target{
				// текстура
				fixed3 albedo=tex2D(_MainTex,f.uv.xy)*_Single.rgb;

				fixed3 ambient0 = UNITY_LIGHTMODEL_AMBIENT.rgb*albedo;

				// Французская карта
				fixed4 normalColor= tex2D(_NormalMap, f.uv.zw);

				fixed3 tangentNormal= UnpackNormal(normalColor);

				tangentNormal.xy = tangentNormal.xy*_BumpScale;

				tangentNormal = normalize(tangentNormal);

				fixed3 lightDir = normalize(f.lightDir);
				
				// Отражение
				fixed3 halfLambert = dot(lightDir,tangentNormal)*0.5 + 0.5;
				fixed3 Diffuse0 = _LightColor0.rgb*albedo*halfLambert;

				fixed3 tempColor = Diffuse0 + ambient0;
				return fixed4(tempColor,_AlphaScale);// Альфа-значение может быть отрегулировано на панели инспектора
				}
			ENDCG
			

			}

		}
			Fallback "Specular"

}