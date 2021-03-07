Barracuda-PoseNet
Using Unity 2019.1.0b8

Real-time WebCam test scene is located in Assests/POSE-NET/Scenes/Test

Video https://youtu.be/MievgkYDquc

Getting 30-50FPS on a Macbook Pro Mid2015 Intel Core i7, 16GB memory DDR3

I got to this point with major help from these repos and alot of code here is re-used or based on theirs:-

https://github.com/keijiro/AsyncCaptureTest.git
https://github.com/infocom-tpo/PoseNet-Unity.git
https://github.com/rwightman/posenet-python.git
I’m using this Texture resize package https://assetstore.unity.com/packages/tools/utilities/resize-pro-61344

If you don’t want to buy it and have your own way of resizing just comment out the lines using the package.

I feel like the code and performance could be made so much better, I began trying to see if I can make use of Unity’s new ECS system but it’s my first time experiementing with parallel jobs and could not get it to work with what I know. I’ll update as I learn more about using custom jobs and Barracuda as there isn’t alot of information on the topic out there. Contribs/Help suggested and appreciated!
