Function CreateNodeItemGroupAnalyzer([xml]$xml, $version){
	$nodeItemGroup = $xml.CreateElement("ItemGroup", $xml.Project.xmlns)
	$xmlNone = $xml.CreateElement("Analyzer", $xml.Project.xmlns)
	$xmlNoneCondition = $xml.CreateAttribute("Condition")
	$xmlNoneCondition.value = "'`$(BuildingInsideVisualStudio)' == 'true'"
	$xmlNone.Attributes.Append($xmlNoneCondition)
	$xmlNoneInclude = $xml.CreateAttribute("Include")
	$xmlNoneInclude.value = $level+"packages\Sistran.Core.Analyzer."+$version+"\analyzers\Sistran.Core.Analyzer.dll"
	$xmlNone.Attributes.Append($xmlNoneInclude)
	$nodeItemGroup.AppendChild($xmlNone)
	$parent = $xml.Project
	$sibling = $xml.Project.ItemGroup 
	$parent.InsertAfter($nodeItemGroup,$sibling[$sibling.Count - 1])
}

function UpdateVersionAnalyzer([xml]$xml,$version){
	$nodes = $xml.Project.ItemGroup.Analyzer
	
	$nodes | where {$_.Include -like "*Sistran.Core.Analyzer.dll"} | foreach{
		$xmlNoneCondition = $xml.CreateAttribute("Condition")
		$xmlNoneCondition.value = "'`$(BuildingInsideVisualStudio)' == 'true'"
		$_.Attributes.Append($xmlNoneCondition)
		
		$newVersion = "sistran.core.analyzer." + $version
		$include = $_.Include
		$include = $include -replace "sistran.core.analyzer.[0-9\.]+",$newVersion
		$_.SetAttribute("Include", $include)
	}
}

function CreateNodeAnalyzer([xml]$xml,$version){
	$xmlNone = $xml.CreateElement("Analyzer", $xml.Project.xmlns)
	$xmlNoneCondition = $xml.CreateAttribute("Condition")
	$xmlNoneCondition.value = "'`$(BuildingInsideVisualStudio)' == 'true'"
	$xmlNone.Attributes.Append($xmlNoneCondition)
	$xmlNoneInclude = $xml.CreateAttribute("Include")
	$xmlNoneInclude.value = $level+"packages\Sistran.Core.Analyzer."+$version+"\analyzers\Sistran.Core.Analyzer.dll"
	$xmlNone.Attributes.Append($xmlNoneInclude)
	$parent = $xml.Project.ItemGroup | where { $_.Analyzer }
	$parent.AppendChild($xmlNone)
}

function CreateNodeItemGroup([xml]$xml){
	$nodeItemGroup = $xml.CreateElement("ItemGroup", $xml.Project.xmlns)
	$xmlNone = $xml.CreateElement("None", $xml.Project.xmlns)
	$xmlNoneInclude = $xml.CreateAttribute("Include")
	$xmlNoneInclude.value = "packages.config"
	$xmlNone.Attributes.Append($xmlNoneInclude)
	$nodeItemGroup.AppendChild($xmlNone)
	$parent = $xml.Project
	$sibling = $xml.Project.ItemGroup 
	$parent.InsertAfter($nodeItemGroup,$sibling[$sibling.Count - 1])
}

function CreateNodePackageConfig([xml]$xml){
	$xmlNone = $xml.CreateElement("None", $xml.Project.xmlns)
	$xmlNoneInclude = $xml.CreateAttribute("Include")
	$xmlNoneInclude.value = "packages.config"
	$xmlNone.Attributes.Append($xmlNoneInclude)
	$parent = $xml.Project.ItemGroup | where { $_.None }
	$parent.AppendChild($xmlNone)
}

function ValidateItemGroupNone([xml]$xml){
	$nodes = $xml.Project.ItemGroup.None
	if(-Not ($nodes | where {$_.Include -like "packages.config"})){	
		if(-Not ($nodes | where {$_.Include})){
			CreateNodeItemGroup -xml $xml
		}else{
			CreateNodePackageConfig -xml $xml
		}	
	}
}

function ValidateItemGroupAnalyzer([xml]$xml,$version){
	$nodes = $xml.Project.ItemGroup.Analyzer
	if(-Not ($nodes | where {$_.Include -like "*Sistran.Core.Analyzer.dll"})){	
		$directory = (get-item $project).Directory.Parent.parent.fullname
		
		if((Get-ChildItem -Path $directory\*.sln)){
			$level = "..\..\"
		}else{
			$level = "..\"
		}
		$level
		
		if(-Not ($nodes | where {$_.Include})){
			CreateNodeItemGroupAnalyzer -xml $xml -version $version -level $level
		}else{
			CreateNodeAnalyzer -xml $xml -version $version -level $level
		}	
	}else{
		UpdateVersionAnalyzer -xml $xml -version $version
	}
}

function ReadProject([string]$project, $version){	
	[Xml]$xml = Get-Content -Path $project -Encoding UTF8
	
	$nodes = $xml.SelectNodes("//*[count(@*) = 0 and count(child::*) = 0 and not(string-length(text())) > 0]")

	$nodes | %{
		$_.ParentNode.RemoveChild($_)
	}
		
	ValidateItemGroupNone -xml $xml
	ValidateItemGroupAnalyzer -xml $xml -version $version
	$xml.save($project)
}

function GetProjects(){
	return (Get-ChildItem -Path '..\..\Source\*.csproj' -Recurse).fullname
}

function CreateFilePackageConfig ($file, $version){
	[xml]$xml = New-Object System.Xml.XmlDocument
	$dec = $xml.CreateXmlDeclaration("1.0","UTF-8",$null)
	$xml.AppendChild($dec)
		
	$node = $xml.CreateElement("packages", $xml.Project.xmlns)
	$nodeItem = $xml.CreateElement("package", $xml.Project.xmlns)
	
	$nodeItem.SetAttribute("id","Sistran.Core.Analyzer")
	$nodeItem.SetAttribute("version",$version)
	$nodeItem.SetAttribute("targetFramework","net45")
	$nodeItem.SetAttribute("developmentDependency","true")
	
	$node.AppendChild($nodeItem)
	$xml.AppendChild($node)
	$xml.save($file)
}

function ValidateFilePackageConfig ($file, $version){
	[Xml]$xml = Get-Content -Path $file -Encoding UTF8
	
	$nodes = $xml.packages.package
	if($nodes | where {$_.id -like "Sistran.Core.Analyzer"}){	
		$nodes | where {$_.id -like "Sistran.Core.Analyzer"} | foreach{
			$_.SetAttribute("version",$version)
		}
	}else{
		$nodeItem = $xml.CreateElement("package", $xml.Project.xmlns)

		$nodeItem.SetAttribute("id","Sistran.Core.Analyzer")
		$nodeItem.SetAttribute("version",$version)
		$nodeItem.SetAttribute("targetFramework","net45")
		$nodeItem.SetAttribute("developmentDependency","true")
		$xml.SelectSingleNode('/packages').AppendChild($nodeItem)
	}
	$xml.save($file)
}

function ValidatePackageConfig ($directory, $version){
	$file = $directory+"\packages.config"
	If(-Not (Test-Path -Path $file)){
		CreateFilePackageConfig -file $file -version $version
	}else{
		ValidateFilePackageConfig -file $file -version $version
	}
}

Function GetFileRuleSet ($directory){
	$count = 0
	while (-Not $hasRuleSet){
		$file = $directory + "\.alm\Analyzer\sistran.ruleset"
			
		If(-Not (Test-Path -Path $file)){
			$directory = (get-item $directory).parent.fullname
			$count = $count + 1
		}else{
			$hasRuleSet = 1
		}	
	}
	
	$file = "";
	for ($i=0; $i -le ($count-1); $i++) {
		$file = $file + "..\"
    }
	$file = $file + ".alm\Analyzer\sistran.ruleset"
	return $file
}

Function ValidateRuleSet ($project, $directory){
	$ruleSet = GetFileRuleSet -directory $directory
	[Xml]$xml = Get-Content -Path $project -Encoding UTF8
	$xml.Project.PropertyGroup | where {$_.Condition -like "*(Platform)' == '*"} | foreach{
		if(-Not ($_.CodeAnalysisRuleSet)){
			$nodeItem = $xml.CreateElement("CodeAnalysisRuleSet", $xml.Project.xmlns)
			$nodeItem.InnerText = $ruleSet
			$_.AppendChild($nodeItem)
		}else{
			$_.CodeAnalysisRuleSet = $ruleSet
		}
		
		if(-Not ($_.RunCodeAnalysis)){
			$nodeItem = $xml.CreateElement("RunCodeAnalysis", $xml.Project.xmlns)
			$nodeItem.InnerText = "true"
			$_.AppendChild($nodeItem)
		}else{
			$_.RunCodeAnalysis = "true"
		}
		
		if(-Not ($_.CodeAnalysisIgnoreGeneratedCode)){
			$nodeItem = $xml.CreateElement("CodeAnalysisIgnoreGeneratedCode", $xml.Project.xmlns)
			$nodeItem.InnerText = "false"
			$_.AppendChild($nodeItem)
		}else{
			$_.CodeAnalysisIgnoreGeneratedCode = "false"
		}
	}
				
	$xml.save($project)		
}

function Main($version){
	$projects = GetProjects
	foreach($project in $projects){
		$project
		$directory = (get-item $project).Directory.fullname
		ValidatePackageConfig -directory $directory -version $version
		ReadProject -project $project -version $version
		ValidateRuleSet -project $project -directory $directory
	}
}

Main -version "1.0.1"