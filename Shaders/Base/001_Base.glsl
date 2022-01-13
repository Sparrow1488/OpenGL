#ifdef GL_ES
precision mediump float;
#endif

uniform vec2 u_resolution;
uniform float u_time;

float plot(vec2 st) {    
    return smoothstep(0.005, 0.0, abs(st.x - st.y));
}

void main() {
	vec2 st = gl_FragCoord.xy/u_resolution;
    float y = st.x;
    vec3 color = vec3(y);
    float pct = plot(st);
    color = (1.0-pct) * color + pct * vec3(1.0,sin(u_time),0.5);

	gl_FragColor = vec4(color,1.0);
}