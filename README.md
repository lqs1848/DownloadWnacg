C# 下载wnacg的本子 自动解析列表页面 添加下载任务

使用前请确认自己的网络是否能访问 https://www.wnacg.org<br>
>这个网站时不时会抽风 偶尔会被墙 偶尔又不会被墙<br>
>可以多试几次<br>
>实在不行就扶梯吧<br>

使用时尽量选择比较空闲的时间段（白天）<br>
怕把别人网站爬挂了<br>
网页单线程爬<br>
下载限了2个线程<br>

程序中的 本子类型都是汉化板块下的<br>
总共有多少页 需要自己访问wnacg去看<br>

![image](https://github.com/lqs1848/DownloadWnacg/blob/master/info/layout.png)
![image](https://github.com/lqs1848/DownloadWnacg/blob/master/info/main.png)
![image](https://github.com/lqs1848/DownloadWnacg/blob/master/info/download.png)


下载地址：<br>
https://github.com/lqs1848/DownloadWnacg/releases/download/1.01/wnacg.zip<br>
解压后启动即可<br>


下载完成后在程序目录下的<br>
download目录中<br>
download目录中的history目录用于记录文件是否下载过<br>

原来是调用迅雷下载<br>
但后来不知道是不是网站封杀了迅雷<br>
迅雷无法下载<br>

如果想用第三方下载器下载<br>
可以解析之后<br>
在日志目录下找到<br>
downloadUrl_zip.txt<br>
复制内容<br>
粘贴到迅雷或者其他软件中即可<br>


downloadUrl_jpg.txt  中是封面信息<br>
这个可以用迅雷下载<br>
喜好连封面一起收藏的可以手动复制到迅雷中下载（会自动重命名）<br>



如果下载卡死了或进度一直没有变化
----------------------------------- 
>可以直接关掉程序<br>
>复制downloadUrl_zip.txt中的内容<br>
>粘贴到下载按钮左边的文本框中<br>
>点击下载按钮（会自动跳过卡死前已完成的任务）<br>


某些任务无法下载
----------------------------------- 
>有部分任务本身就是死链（可以尝试将下载地址用浏览器打开 来判断是否死链）<br>
>实在想看 可以用名称搜索 会有不同汉化组的资源（或者去搜种子）<br>