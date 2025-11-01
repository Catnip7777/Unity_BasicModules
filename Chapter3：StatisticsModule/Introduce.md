## 这是第三期的内容
一个存档模块，提供存档和UI更新功能

整体更新了打印调试信息的功能，后续会上传新版本模块

## 以下是文案
哈喽大家好，这里是猫薄荷！这一期的主题是“数据模块”。

在游戏中，数据的储存和管理至关重要。无论是存档系统、游戏设置，还是随进度变化的UI，都离不开数据的存储与读取。

这一次，我们来编写一个简单的数据模块，它将实现以下功能：

- 将存档保存到本地

- 从本地读取存档

- 允许其他模块方便地读取修改

- 修改时自动更新UI

除此之外，我们开始使用单例模式，防止多个同样的模块发生冲突

添加这段代码即可


我们的数据模块拥有以下数据结构：

- 一个限制存档上限的静态变量SAVE_NUM

- 一个标记选择存档的整数saveindex

- 一个记录保存地址的字符串savepath

- 一个存档类的数组GameData[] gameDataSet


我们的数据模块提供以下方法：

- 游戏开始时加载存档

- 保存选择的存档

- 删除选择的存档

- “存档选择事件”触发器x3


此外，为了便于其他模块读写，我们在游戏存档类GameData创建一个手动的字典，我们提供三个函数，分别用于：添加项、寻找项和改变项。

在改变项的函数中，我们触发UI更新事件，即可使所有UI更新数据。


这样我们就实现了一个简单易懂的数据管理模块，同样的，具体代码我放在了github，此外我还添加了事件的调试系统，可以在事件触发时打印具体信息，看起来很好看😋。


最后，如果这期视频对你有帮助的话，求一个star，或者关注，或者三连，都可以，你们的每一个互动都能让我这个小up开心很久！

## This is the content of the third episode  
An archive module that provides archiving and UI update functions.  

The print debugging function has been comprehensively updated, and a new version of the module will be uploaded later.  

## Below is the copy  
Hello everyone, this is Catnip! The theme of this episode is "Data Module."  

In games, data storage and management are crucial. Whether it's the save system, game settings, or UI that changes with progress, everything relies on the storage and retrieval of data.  

This time, we’ll write a simple data module that will implement the following functions:  

- Save archives locally  
- Read archives from local storage  
- Allow other modules to easily read and modify data  
- Automatically update the UI when modifications are made  

In addition, we’ll start using the singleton pattern to prevent conflicts between multiple instances of the same module.  

Just add this piece of code.  

Our data module has the following data structure:  

- A static variable `SAVE_NUM` that limits the maximum number of save files  
- An integer `saveindex` that marks the selected save file  
- A string `savepath` that records the save path  
- An array of save classes `GameData[] gameDataSet`  

Our data module provides the following methods:  

- Load archives when the game starts  
- Save the selected archive  
- Delete the selected archive  
- Three "archive selection event" triggers  

Additionally, to make it easier for other modules to read and write data, we’ll create a manual dictionary in the game archive class `GameData`. We provide three functions for: adding items, finding items, and changing items.  

In the function for changing items, we trigger the UI update event, which ensures all UIs update their data.  

This way, we’ve implemented a simple and easy-to-understand data management module. As always, the specific code is available on GitHub. I’ve also added a debugging system for events, which prints detailed information when events are triggered—it looks really nice 😋.  

Finally, if this video was helpful to you, I’d appreciate a star, a follow, or a triple interaction (like, comment, and share). Every interaction from you guys makes this small content creator very happy!


