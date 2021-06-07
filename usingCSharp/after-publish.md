# using After Publish On Visual Studio
## Publish ProfileにEnvironmentを追加する

↓らへんの予約語を避ければ、プロパティをPublish Profileから、MSBuildのAfter Publishへ伝搬することが可能。

https://docs.microsoft.com/ja-jp/visualstudio/msbuild/msbuild-reserved-and-well-known-properties?view=vs-2019
https://docs.microsoft.com/ja-jp/visualstudio/msbuild/common-msbuild-project-properties?view=vs-2019

## 例）Environment の PropをPublish Profileに追加する場合は以下
```
<?xml version="1.0" encoding="utf-8"?>
<!--
https://go.microsoft.com/fwlink/?LinkID=208121. 
-->
<Project ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <DeleteExistingFiles>True</DeleteExistingFiles>
    // 中略
    <SelfContained>false</SelfContained>
    <Environment>Development</Environment>
  </PropertyGroup>
</Project>
```

## Sdkスタイルの.csprojにTargetを追加する
注意：TargetNameはuniqueになるよう設定する。

```
  <Target Name="DeployToDevelopment" Condition="'$(Environment)' == 'Development'" AfterTargets="AfterPublish">
    <Exec WorkingDirectory="Deploy" Command="dev.bat" ContinueOnError="true">
      <Output TaskParameter="ExitCode" PropertyName="ErrorCode" />
    </Exec>
  </Target>

  <Target Name="DeployToStaging" Condition="'$(Environment)' == 'Staging'" AfterTargets="AfterPublish">
    <Exec WorkingDirectory="Deploy" Command="stage.bat" ContinueOnError="true">
      <Output TaskParameter="ExitCode" PropertyName="ErrorCode" />
    </Exec>
  </Target>


  <Target Name="DeployToProduction" Condition="'$(Environment)' == 'Production'" AfterTargets="AfterPublish">
    <Exec WorkingDirectory="Deploy" Command="prod.bat" ContinueOnError="true">
      <Output TaskParameter="ExitCode" PropertyName="ErrorCode" />
    </Exec>
  </Target>
```

