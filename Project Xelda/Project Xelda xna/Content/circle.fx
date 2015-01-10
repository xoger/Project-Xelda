uniform extern texture ScreenTexture;
sampler screen = sampler_state { Texture = <ScreenTexture>;};
extern float4 InColour;
extern float time;
//float4 incolour;
float4 PixelShaderFunction(float2 inCoord: TEXCOORD0) : COLOR0
{
	float4 incolour = InColour;
	float4 color = tex2D(screen, inCoord);
    float dx = inCoord.x - 0.5f;
    float dy = inCoord.y - 0.5f;
    if(sqrt(dx * dx + dy * dy) <= time){
		incolour.r *= color.r;
		incolour.g *= color.g;
		incolour.b *= color.b;
		incolour.a *= color.a;
		
		if(sqrt(dx * dx + dy * dy) >= time-0.06){
			float alpha = lerp(float(incolour.a), float(-20), saturate((sqrt(dx * dx + dy * dy)) - (time - 0.06)));
			incolour.a = alpha;
		}
		return incolour;
	}
    else{
        return float4(color.r, color.g, color.b, 0.0f);
        }
}

technique
{
	pass P0
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}


// thank selkathguy