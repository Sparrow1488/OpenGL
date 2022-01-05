#version 330 core

in vec2 TexCoord;
out vec4 FragColor;

uniform sampler2D texture0;

void main() 
{
//	FragColor = texture(texture0, TexCoord);
	FragColor = vec4(0.3, 0.2, 0.4, 1.0);
}