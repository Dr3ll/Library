float4x4 xViewProjection;

float2 marks[50];
int max;


// TODO: add effect parameters here.

struct VertexToPixel
{
    float4 Position     : POSITION;
    float4 Color        : COLOR0;
};

struct VertexShaderOutput
{
    float4 Color : COLOR0;
};

VertexToPixel VertexShaderFunction(float4 inPos : POSITION, float4 inColor : COLOR0)
{
    VertexToPixel output = (VertexToPixel)0;
     
     output.Position = mul(inPos, xViewProjection);
     output.Color = inColor;

    return output;
}

float4 PixelShaderFunction(VertexToPixel input, float4 pos : VPOS) : COLOR0
{


	for(int i=0; i<max; i++)
	{
		vector <float, 2> blub = marks[i].xy;

		float dist = abs(marks[i].x * pos.x + marks[i].y * pos.y) / length(blub);


		if(dist < 10)
			return float4(1, 0, 0, 1);

		//float mult = ((starts[i].x - pos.x) * dirs[i].x + (starts[i].y - pos.y) * dirs[i].y);

		//vector <float, 2> dist = 
		//{	starts[i].x - pos.x - dirs[i].x * mult,
		//	starts[i].y - pos.y - dirs[i].y * mult };

		//mark.xy = marks[i].zw;

		//if(distance(pos, mark) < 50)
			//return float4(1, 0, 0, 1);
	}

    return float4(1, 1, 1, 1);
}

technique Technique1
{
    pass Pass1
    {


        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
