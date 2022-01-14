#version 330 core

in vec2 TexCoord;

out vec4 FragColor;
uniform sampler2D texture0;
uniform vec4 SelectColor;

void main() 
{
	if(SelectColor == 0){
		FragColor = texture(texture0, TexCoord);
	}
	else{
		FragColor = texture(texture0, TexCoord) * SelectColor;
	}
}