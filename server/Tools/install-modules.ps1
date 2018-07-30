 param (
    [string]$channel = "Debug"
 )

function Get-ScriptDirectory
{
  $Invocation = ((Get-Variable MyInvocation -Scope 1).Value).MyCommand.Path
  Split-Path $Invocation
}

Push-Location $(Get-ScriptDirectory)
Set-Location "..\Modules"

#-notcontains "Release"
Get-ChildItem -Recurse "HaleModule*.dll" | 
Where-Object { $_.Directory.Parent.Name  -notcontains 'obj' -contains $channel } |
ForEach { 
  $module = $_.BaseName
  "Installing " + $module + ": "
  $dir = [Environment]::GetFolderPath("CommonApplicationData") + "\Hale\Core\Modules\" + $_.BaseName
  
  if(!(Test-Path $dir)){
    " - Creating directory " + $dir
    New-Item $dir -type Directory | Out-Null
  }

  $_.Directory.GetFiles("*.dll") | ForEach { 
    " - " + $_.Name + " => " + ($dir + "\" + ($_).Name)
    Copy-Item $_.FullName ($dir + "\" + ($_).Name)
    #($dir + "\" + ($_).Name)
  }
  [Environment]::NewLine
}

Pop-Location