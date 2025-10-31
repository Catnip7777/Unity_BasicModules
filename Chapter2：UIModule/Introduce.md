## 这是第二个视频的内容，提供了一个简单的界面设计思路。
这部分基本都在unity中完成，素材可以用自己手边有的进行替换，代码部分只是添加事件，因此文件夹里没有其他文件。
## 以下是文案
哈喽大家好，这里是猫薄荷！上一期，我们打造了一个低耦合的输入管理模块，将各种输入都转化成了事件。这一期我们要沿着相同的思路，以空洞骑士那极具氛围感的主界面为蓝本进行复刻，制作一个同样高度自治的UI模块。

我们的目标非常明确：让UI模块管理好自己的一切。它与其他模块的唯一交流方式就是我们的事件系统。这样做的好处很大：功能彻底分离，架构思路清晰，方便复用和替换。

在动手之前，我们先来拆解一下这个界面，它主要由以下几部分组成：

视觉层：带辉光的标题文字和边框，动态的背景粒子特效，选项文字和指示器，点击时的特效反馈

听觉上：循环的背景音乐音效，点击音效

我们的资源方案是：

文字使用unity自带字体

边框和指示器手绘

粒子使用unity粒子系统制作

音效和背景音乐来自unity商店和pixabay

我们的技术方案是：

辉光效果使用URP管线自带的后处理

选项导航使用画布自带的selectable组件

选项触发使用unity事件系统的触发器，在触发内部特效和音效的同时，向外触发我们自己的事件系统，完成模块间的通信。

最终效果是这样的。



制作起来其实很简单。只需要注意将背景，特效，文字按照z轴放置在摄像机前面，就可以得到正确的覆盖关系

接下来让我们进入unity，一步步实现它。

1.标题部分

首先，我们选择URP渲染管线的模板，它自带了方便的后处理功能

接着，在摄像机上打开后处理，添加volume的发光效果，这样就可以让画面中超过阈值的物体发光

再创建3d的tmp文字作为标题，调整发光亮度，就可以得到辉光效果

最后加入手绘的标题边框，复制文字的发光材质并赋予它，这一部分就完成了。

2.选项部分

我们先在画布上放置好所有选项文本的位置

为了方便，我们直接将选项的指示器放置在场景的对应位置

再为每个文本添加selectable组件，以便使用画布的自动导航功能

最后，为每个选项添加事件触发器，当点击时，让它能触发我们接下来要制作的点击特效和音效。

3.加一点点粒子和音乐

现在，我们来注入灵魂

创建两个粒子系统，分别模拟烟雾和虚空粒子

将它们的发射器形状设置为覆盖整个屏幕，并选择“内部”生成，这样粒子就会在屏幕边界内随机出现。通过调整颜色、持续时间、速度和生成数量，再微调它们颜色与大小随时间变化的曲线，我们就能得到非常相似的背景氛围效果。同时，我们再创建一个点击时瞬间飞散的小粒子效果。最后，加入循环的背景音乐和清脆的点击音效，听觉层面也就完成了。

4.连接到我们的事件系统

最后，也是最关键的一步，让这个UI模块融入我们的事件驱动架构。我们编写一个简单的脚本，用来注册我们的点击事件。我们将这个脚本添加到每个文本的触发列表上。这样当任何一个文本被点击时，不仅点击特效音效会自动播放，也会触发我们的事件系统。

如此一来，其他模块只需要监听这五个事件，执行相应的处理即可，不需要关心UI模块内部发生了什么，这就是模块化的魅力。同时这个UI脚本也可以监听其他模块注册的事件，完成协作。

好了，一个《空洞骑士》风格的、完全模块化的主界面就完成了！我们回顾一下：从静态UI搭建，到动态粒子效果和音频，最后将这个模块连接到事件系统。希望这期视频能让你体会到模块化开发带来的清晰与优雅。
别忘了一键三连，我们下期再见!
## This is the content of the second video, providing a simple UI design approach.
This part is mostly completed within Unity. You can replace the assets with whatever you have on hand. The coding part only involves adding events, so there are no additional files in the folder.

## Script Below
Hello everyone, this is Catnip! In the previous video, we built a low-coupling input management module that converts various inputs into events. This time, we'll follow the same philosophy, using the highly atmospheric main menu of *Hollow Knight* as a blueprint to create an equally self-sufficient UI module.

Our goal is very clear: let the UI module manage everything about itself. Its only way to communicate with other modules is through our event system. The benefits of this approach are significant: complete separation of functions, a clear architectural mindset, and ease of reuse and replacement.

Before we start, let's break down this interface. It mainly consists of the following parts:

**Visually:** Glowing title text and borders, dynamic background particle effects, option text and indicators, and click feedback effects.

**Audibly:** Looping background music/ambience and click sound effects.

Our resource plan is:
*   Text uses Unity's built-in font.
*   Borders and indicators are hand-drawn.
*   Particles are created using Unity's Particle System.
*   Sound effects and background music are from the Unity Asset Store and Pixabay.

Our technical plan is:
*   The glow effect uses the built-in post-processing in the URP pipeline.
*   Option navigation uses the Canvas' built-in `Selectable` components.
*   Option triggering uses the Unity Event System's `Event Triggers`. While triggering internal visual/audio effects, they also trigger our own event system to facilitate inter-module communication.

The final result looks like this.

The process is actually quite simple. Just pay attention to placing the background, effects, and text along the Z-axis in front of the camera to get the correct layering.

Now, let's go into Unity and implement it step by step.

**1. Title Section**
First, we select the URP rendering pipeline template, which comes with convenient post-processing features.
Next, enable post-processing on the camera and add the Bloom effect. This allows objects exceeding a brightness threshold in the scene to glow.
Then, create a 3D TextMeshPro object as the title. Adjust its emission intensity to get the glow effect.
Finally, add the hand-drawn title border, duplicate the text's emissive material, and assign it to the border. This part is now complete.

**2. Options Section**
First, position all the option texts on the Canvas.
For convenience, we place the option indicator directly at the corresponding position in the scene.
Then, add a `Selectable` component to each text object to utilize the Canvas' automatic navigation.
Finally, add an `Event Trigger` to each option, so it can trigger the click effects and sound we'll create next when clicked.

**3. Add a Touch of Particles and Music**
Now, let's inject the soul.
Create two particle systems to simulate mist and void particles respectively.
Set their emitter shape to cover the entire screen and choose 'Inside' for generation. This makes particles spawn randomly within the screen boundaries. By adjusting colors, duration, speed, spawn rate, and fine-tuning the curves for how their color and size change over time, we can achieve a very similar background atmosphere. Also, create a small particle effect that bursts outwards instantly upon clicking.
Finally, add looping background music and crisp click sound effects. The auditory layer is now complete.

**4. Connect to Our Event System**
Finally, the most crucial step: integrating this UI module into our event-driven architecture. We write a simple script to register our click events. We add this script to the trigger list of each text object. This way, when any text is clicked, not only will the click effects and sounds play automatically, but it will also trigger our event system.

Thus, other modules only need to listen for these specific events and execute the corresponding logic, without needing to know what happens inside the UI module. This is the charm of modularity. Conversely, this UI script can also listen for events registered by other modules to enable collaboration.

And there you have it! A *Hollow Knight*-style, fully modular main menu is complete! Let's recap: from building the static UI, to adding dynamic particles and audio, and finally connecting the module to the event system. I hope this video helps you appreciate the clarity and elegance that modular development brings.
Don't forget to like, subscribe, and hit the bell! See you in the next video!
