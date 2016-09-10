rm *.nupkg
nuget pack .\AverageBuddy.nuspec -IncludeReferencedProjects -Prop Configuration=Release
nuget push *.nupkg