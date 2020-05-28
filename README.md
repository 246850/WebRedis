# WebRedis
基于asp.net core 3.1框架体系和StackExchange.Redis调用Redis命令编辑的Web客户端（注：尚在优化中，是个半成品）
### 配置
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "MySQL": "server=localhost;user id=root;password=123456; database=webredisdb; charset=utf8"
  }
}
```
### 数据库MySql执行文件(webredisdb.sql)
# 演示站点
[http://101.132.140.8:3611](http://101.132.140.8:3611)
![](https://images.cnblogs.com/cnblogs_com/GodX/572372/o_200522073610049525F9-CE00-4237-87C2-8AC13859EB40.png)
