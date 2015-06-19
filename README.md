# RoslynTester

A library that will help you unit test your Roslyn analyzers. This package contains the default test helpers provided with the Diagnostics + CodeFix solution template but updated for the latest version of Roslyn and with a few enhancements.

**NuGet**

https://www.nuget.org/packages/RoslynTester/

[![Build status](https://ci.appveyor.com/api/projects/status/3x918k5jre5imjjn?svg=true)](https://ci.appveyor.com/project/Vannevelj/roslyntester)
[![Test status](http://teststatusbadge.azurewebsites.net/api/status/Vannevelj/roslyntester)](https://ci.appveyor.com/project/Vannevelj/roslyntester)


## Why should I use this?

The CodeFix + Diagnostics solution template is not updated at the same speed as Roslyn is. This means that if you want to test your analyzers, you are stuck with the older version of Roslyn which might (will?) contain bugs that have been fixed in later versions.

By providing these classes as a NuGet package I achieve two solutions:

* When you have an existing analyzer and want to update to a new version of Roslyn, you don't have to worry about backwards-compatibility issues introduced by a newer version.
* When you start a new project you can get started right away with the latest version of Roslyn by removing the default test files and including this package.

## What guarantee is there that this is constantly updated?

There is none. I will try to keep up with new Roslyn releases and add features when I feel them to be necessary but in case I ever fall behind you're free to let me know (or send a PR) and I'll get right on it.

## Can I contribute?

Yes, definitely. Create an issue or look for an open issue and provide me with a pull request. As always: if you're intending to make a big change, let's discuss it first to avoid unnecessary work.
