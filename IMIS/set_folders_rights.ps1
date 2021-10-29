$folders = @("Extracts","FromPhone","Images", "Archive", "Workspace")

foreach ($i in $folders){
   Write-Host "Setting the rights for" $i 
   
   $ACL = Get-ACL -Path $i
   $AccessRule = New-Object System.Security.AccessControl.FileSystemAccessRule("IIS_IUSRS","FullControl", "ContainerInherit,ObjectInherit", "None","Allow")
   $ACL.SetAccessRule($AccessRule)
   $ACL | Set-Acl -Path $i
   
}