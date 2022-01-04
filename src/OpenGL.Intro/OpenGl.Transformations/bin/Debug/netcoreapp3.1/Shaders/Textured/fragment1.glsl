#version 330 core

in vec2 aTexture;
in vec4 aColor;
out vec4 FragColor;

uniform sampler2D texture14;
uniform sampler2D texture88;
uniform vec4 UniColor;

void main() 
{
//	FragColor = mix(texture(texture14, aTexture), texture(texture88, aTexture), 0.6);
	FragColor = UniColor;
}