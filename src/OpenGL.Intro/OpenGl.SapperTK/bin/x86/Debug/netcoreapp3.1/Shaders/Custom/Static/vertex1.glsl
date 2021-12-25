#version 330 core

layout (location = 0) in vec3 aPos; // input
out vec4 vertexColor; // output to the fragment shader

void main() {
	gl_Position = vec4(aPos, 1.0);
	vertexColor = vec4(0.2, 0.5, 0.7, 1.0);
}