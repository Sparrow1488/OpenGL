#version 330 core

out vec4 FragColor;
in vec2 aTexture;

uniform vec4 UniColor;
uniform sampler2D texture0;
uniform sampler2D texture1;

void main() 
{
    if(UniColor == 0){
		FragColor = mix(texture(texture0, aTexture), texture(texture1, aTexture), 0.6);
	}
	else{
		FragColor = mix(texture(texture0, aTexture)  * UniColor, texture(texture1, aTexture), 0.6);
	}
}