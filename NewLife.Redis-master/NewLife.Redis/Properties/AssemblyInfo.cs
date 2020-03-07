﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// 有关程序集的一般信息由以下
// 控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("新生命Redis缓存组件")]
[assembly: AssemblyDescription("Redis基础操作、列表结构、哈希结构、Set结构，经过日均100亿次调用量的项目验证")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("NewLife.Redis")]
[assembly: AssemblyCompany("新生命开发团队")]
[assembly: AssemblyCopyright("©2002-2018 新生命开发团队")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 会使此程序集中的类型
//对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型
//请将此类型的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("8a5716e7-468b-4338-a61c-bb6d7a371ed4")]

// 程序集的版本信息由下列四个值组成: 
//
//      主版本
//      次版本
//      生成号
//      修订号
//
// 可以指定所有值，也可以使用以下所示的 "*" 预置版本号和修订号
//通过使用 "*"，如下所示:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("2.1.*")]
[assembly: AssemblyFileVersion("2.1.2018.0922")]

/*
 * v2.1.2018.0922   重构Redis协议解析，支持管道
 * 
 * v2.0.2018.0829   全面扩充常用指令，日均80亿次调用量
 * 
 * v1.0.2017.0820   建立Redis缓存组件
 */
