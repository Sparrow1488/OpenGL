#version 330 core

layout (location = 0) in vec3 aPos; // input position
// layout (location = 1) in vec3 aColor; // input color

uniform vec4 uniColor;

out vec4 vertexColor;

void main() {
	gl_Position = vec4(aPos.x + 0.2, aPos.y + 0.2, aPos.z, 1.0);
	vertexColor = uniColor;
}