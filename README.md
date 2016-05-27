# 2048
在运行的时候，可能会遇到由于没有引用一些库而导致的问题。解决方法如下：
1.添加SQLite扩展
• 工具>>扩展和更新>>点击左侧栏“联机”>>搜索框输入sqlite>>下载
SQLite for Universal Windows Platform
2.添加SQLite引用
• 右击“引用”>>选择“添加引用”>>在弹出的窗口左侧栏选择
“Universal Windows”“扩展”>>选择“SQLite for Universal Windows
Platform后确定
3.添加 SQLite.Net 的引用
• 右键“引用”>>选择“管理GuGet程序包”>>在页面的“浏览”中搜
索“sqlite.net-pcl”<<选择“SQLitePCL”下载安装

添加了以上引用之后应该就可以正常运行了~
