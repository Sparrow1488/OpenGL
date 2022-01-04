#version 330 core

in vec2 aTexture;
out vec4 FragColor;

uniform sampler2D texture14;
uniform sampler2D texture88;

void main() 
{
	FragColor = mix(texture(texture14, aTexture), texture(texture88, aTexture), 0.6);
}