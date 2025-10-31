## 这里是第一个视频的内容
注意这个文件夹内的两个模块是1.0版本，后续将随章节更新。
更新的时候我会标出1.0，2.0等编号。
章节顺序是我学习和编写的顺序，从最简单的方式开始逐步添加功能😋。

## 以下是文案
哈喽大家好，这里是猫薄荷！作为游戏开发的初学者，你是不是也遇到过这些问题？

看着一大堆api不知如何下手

有游戏设计的创意，但制作起来又不知道该用到哪些功能

做demo的时陷入代码细节，写着写着思路就乱了？


别担心，这些问题我都经历过！今天分享一个超实用的心得：用模块化思维搭建你的游戏框架。这就像在“游戏创意”和“复杂API”之间搭起一座桥梁，用可复用的“功能积木”来构建游戏。

我们只需要编写好一些基本功能，比如输入管理，数据管理，游戏状态，演出，就可以用一套代码制作不同的游戏。独特的功能只需要单独编写即可。

作为这个系列的第一讲，我们来攻克最基础也最关键的——输入管理模块

我们的目标是设计一个能满足以下需求的系统：

+灵活绑定：轻松配置按键

+实时改键：让玩家自定义操作

+连招组合：支持复杂输入判断

+事件驱动：让输入与其他模块解耦


我们知道unity提供三种输入方式，分别是input类和两个输入管理系统。我们使用input类构建我们自己的输入管理系统。

使用方法很简单：

首先我们先创建一个空物体，叫eventsystem，然后挂上一个脚本，这样事件系统就可以运行了。然后再创建一个空物体，挂上输入系统的脚本，修改输入系统的按键绑定连招和组合键预设。最后让任意物体在事件系统加入事件，这样在输入触发时就可以收到通知，运行我们写好的函数了。

具体原理也很简单：

事件系统其实就是一个委托字典。

输入系统则是调用了Input类轮询我们绑定好的按键，再通过缓存数组存储按键历史，就可以实现连招和组合键了。

具体的代码我会放在github，大家可以随意修改

你学会了吗？

## This is the content of the first video
Note: The two modules in this folder are version 1.0 and will be updated with subsequent chapters.
When updating, I will mark the version number like 1.0, 2.0, etc.
The chapter order follows my learning and development sequence, starting from the simplest methods and gradually adding features 😋.

## Script Below
Hello everyone, this is Catnip! As a beginner in game development, have you ever encountered these problems?

*   Feeling overwhelmed by a massive list of APIs and not knowing where to start.
*   Having game design ideas but not knowing which functions to use to build them.
*   Getting bogged down in code details while making a demo, losing your train of thought.

Don't worry, I've been through all of this too! Today, I'm sharing a super useful insight: **build your game framework using a modular approach**. It's like building a bridge between your "game ideas" and "complex APIs," constructing your game with reusable "functional building blocks."

We just need to write some basic functionalities, like **Input Management, Data Management, Game State, and Presentation**. Then, we can use the same codebase to create different games. Unique features only need to be written separately.

For the first part of this series, let's tackle the most fundamental and crucial module—**the Input Management Module**.

Our goal is to design a system that meets the following requirements:

*   **Flexible Binding:** Easily configure key bindings.
*   **Real-time Rebinding:** Allow players to customize controls.
*   **Combo & Combination Support:** Handle complex input detection like combos and key combinations.
*   **Event-Driven:** Decouple input from other modules.

We know Unity offers three input methods: the `Input` class and two Input Management systems. We will use the `Input` class to build our own input management system.

How to use it is very simple:

1.  First, create an empty GameObject called `EventSystem` and attach a script to it. This gets the event system running.
2.  Then, create another empty GameObject and attach the input system script to it. Modify the key bindings, combo, and key combination presets in the input system.
3.  Finally, have any object subscribe to events in the event system. This way, when an input is triggered, it will receive a notification and run the functions we've written.

The core principle is also quite simple:

*   The **Event System** is essentially a dictionary of delegates.
*   The **Input System** uses the `Input` class to poll our bound keys and implements combos/key combinations by storing input history in a cached array.

The specific code will be available on GitHub, and you are free to modify it.

Did you find this helpful?
