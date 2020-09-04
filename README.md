# Dance it Out - LimunalVR UG (T2 2020)
This project is a partnership program between Deakin University and Liminal VR to create short virtual reality experiences designed to empower people to consciously choose how they feel and perform. <br/>
There are currently 4 categories of experience on Liminal VR platform: Calm, Energy, AWE & Pain Relief.

## Introductions
Dance it out is a 5-10 minutes virtual reality experience project that aimed to be fun and engaging, leaving the user feeling psyched up, motivated and on a high, with greater feelings of personal power and confidence.
The scene is setup in a nightclub with high motivated music, attractive design and realistic interactions (dancing, lighting, etc.)
This experience is developed based on psychology research documents (Psych Docs) provided by Liminal VR.
* For more information about Liminal VR application & program, visit the official page: https://liminalvr.com/

## Requirements
1. Unity game engine: version 2019.1.10f1
2. Android Build Support platform module for Unity
3. BitBucket & Sourcetree
4. Liminal SDK
* For more informations about liminal SDK, visit the page [Liminal-SDK-Unity-Package](https://github.com/LiminalVR/LiminalSdk-UnityPackage/blob/develop/README.md#setup-git)

## Installation
After get all requirements ready, now you can clone install the project on your device.
1. Install sourcetree and login to Deakin BitBucket server
- Root URL: https://bitbucket-students.deakin.edu.au/
- Username: "your-deakin-username-here"

2. Clone the repository
- Copy reposiroty link: https://bitbucket-students.deakin.edu.au/scm/dimm-ug/liminalvr-ug-squad_2020t2.git
- Paste the link in 'source path' in Sourcetree and select Clone

3. Open project on Unity
- Open Unity Hub --> select Unity Version (2019.1.10f1)
- Add project --> browse to downloaded folder (When cloning project)
- Open project 'liminalvr-ug-squad-2020t2'

4. Open 'DanceitOff' scene in project file.

* Installation video: [Here](https://youtu.be/j3MVVTp1MHQ)

## Project Settings
- Android ready
- Oculus VR (Oculus Go/Quest) Ready
- Graphics & Quality settings to Liminal's standards

## Project Builds & Testing
1. Build .apk file for Oculus Go/Quest (Unity)
- File > Build Settings > platform (Android)

2. Select 'Scene In Build'
- Drag and drop 'DanceitOff' scene in the scene in build

3. Player Settings
- Company name: Liminal VR
- Product name: DanceitOffApp

4. Build and Run
- Note: it may take up to 25-30 minutes to build the project

## Troubleshooting
- Unable to open Unity due to Package Manager Manifest issues
  - Chances are your git is not setup properly.

## Notes
- There are two prefabs which rely on a default layer to not reflect lighting as the lights in the scene has been set to cull layer 1:TransparentFX (Tile & Smoke Particles), you might need to change these to the layers you have already established for the limapp to work properly.
