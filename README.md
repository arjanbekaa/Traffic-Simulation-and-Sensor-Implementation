Project Description
This initiative aimed to develop a comprehensive traffic simulation system with integrated sensor technologies, delivering a highly realistic driving experience. The core objective was to create an immersive, educational, and interactive environment where users can explore, test, and learn about sensor systems commonly used in autonomous vehicles.

Key Project Components
1. Traffic Simulation
We engineered a dynamic system to emulate vehicle behavior, traffic patterns, and interactions with the environment. Techniques like object pooling and realistic vehicle physics were employed to craft a vivid, lifelike simulation.

2. Sensor Implementations
The project included functional integration of various sensors to provide both visual and audio feedback, effectively simulating the detection systems used in modern vehicles:

Camera Sensor

Captures the surrounding environment to identify objects and obstacles.

Configuration:

Dimensions: X = 6.17, Y = 4.55

Gatefit: Overscan

Focal length: 1

Field of View: 144°

Proximity Sensors

Utilize raycasting to scan the immediate surroundings.

Specifications:

Effective distance: 3–5 meters

Scan angle: 50°–140°

Number of rays: 20–40 (adjustable based on required resolution)

Radar Sensor

A single raycast extending 50 meters ahead of the vehicle to detect objects and compute their distance and speed via reflection data.

Voice Recognition Sensor

Uses the microphone to detect keywords from a predefined list and trigger corresponding actions when recognized.

Voice Commands for Testing (Windows Build Only)
“Turn on the lights” / “Turn off the lights”

“Turn on 3D feedback sensors” / “Turn off 3D feedback sensors”

Technologies Used
Unity 3D – For creating the simulation environment and visuals

C# – For programming sensor logic and system behaviors

Unity Asset Store – For design assets and visual enhancement

ChatGPT – Assisted with research, design decisions, and development troubleshooting

Playable Build
You can explore a live version of the simulation here:

Traffic Simulation and Sensor Implementation by Arjan Beka
Available platforms: HTML5 (web-based) and Windows 
arjan-beka.itch.io
.

Source Code & Accessibility
GitHub Repository – The full source code is publicly available for inspection and contribution.

Playable Builds – Accessible via itch.io (web and Windows versions).

(Links will be inserted where appropriate.)

Conclusion
This project showcases a sophisticated simulation of traffic and sensor technologies using Unity 3D and C#. It demonstrates the interplay of camera imaging, proximity detection, radar scanning, and voice interaction, offering a compelling example of how AI, sensing, and simulation converge in the automotive domain. It serves as an inspiring technical demonstration for developers, researchers, and students interested in the future of intelligent vehicle systems.
