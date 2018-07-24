''Copyright (c) 2016-2017 Swiss Agency for Development and Cooperation (SDC)
''
''The program users must agree to the following terms:
''
''Copyright notices
''This program is free software: you can redistribute it and/or modify it under the terms of the GNU AGPL v3 License as published by the 
''Free Software Foundation, version 3 of the License.
''This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of 
''MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU AGPL v3 License for more details www.gnu.org.
''
''Disclaimer of Warranty
''There is no warranty for the program, to the extent permitted by applicable law; except when otherwise stated in writing the copyright 
''holders and/or other parties provide the program "as is" without warranty of any kind, either expressed or implied, including, but not 
''limited to, the implied warranties of merchantability and fitness for a particular purpose. The entire risk as to the quality and 
''performance of the program is with you. Should the program prove defective, you assume the cost of all necessary servicing, repair or correction.
''
''Limitation of Liability 
''In no event unless required by applicable law or agreed to in writing will any copyright holder, or any other party who modifies and/or 
''conveys the program as permitted above, be liable to you for damages, including any general, special, incidental or consequential damages 
''arising out of the use or inability to use the program (including but not limited to loss of data or data being rendered inaccurate or losses 
''sustained by you or third parties or a failure of the program to operate with any other programs), even if such holder or other party has been 
''advised of the possibility of such damages.
''
''In case of dispute arising out or in relation to the use of the program, it is subject to the public law of Switzerland. The place of jurisdiction is Berne.
'
' 
'

Imports System.IO
Imports System.Text
Imports System.Security.Cryptography
Imports System.Data.SqlClient
Imports System.Data.SQLite
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Configuration
Imports System.Web
Imports System.Threading
Imports SevenZip


Public Class IMISExtractsBL


    Private Const DB3_PWD As String = "%^Klp)*3"
    Private Const DB_PWD As String = "%^Klp)*3"

    Private Const DESKEY As String = ":-+A7V@="
    Private Const RARPWD As String = ")(#$1HsD"
    
    Private dtblIn As New DataTable
    Private dtblOut As New DataTable
    Private Proc As New Process


    Public Function GetLastCreateExtractInfo(ByVal LocationId As Integer, ByVal ExtractType As Integer, Optional ByVal ExtractDirection As Integer = 0) As IMIS_EN.tblExtracts
        Dim Extract As New IMIS_DAL.IMISExtractsDAL
        Return Extract.GetLastCreateExtractInfo(LocationId, ExtractType)

    End Function

    Public Function GetDownLoadExtractInfo(Optional ByVal ExtractID As Integer = 0, Optional ByVal LocationId As Integer = 0, Optional ByVal PhotoExtract As Boolean = False, Optional ByVal ExtractFileName As String = "") As String
        Dim Extract As New IMIS_DAL.IMISExtractsDAL
        Dim dtExtract As DataTable
        Dim str As String
        Dim ExtractIDSearch As Integer
        str = ""
        GetDownLoadExtractInfo = ""

        If PhotoExtract = False Then

        End If

        If ExtractID = 0 Then
            'FULL Extract --> 
            ExtractIDSearch = Extract.GetLastFullExtractID(LocationId, ExtractFileName)
        Else
            ExtractIDSearch = ExtractID
        End If

        dtExtract = Extract.GetExtract(ExtractIDSearch)
        If dtExtract.Rows.Count = 0 Then
            Exit Function
        Else
            str = dtExtract.Rows(0)("ExtractFileName")
        End If

        If PhotoExtract = True Then
            str = Left(str, Len(str) - 4) & "_Photos" & Right(str, 4)
        End If

        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/" & str)) = True Then
            GetDownLoadExtractInfo = str
        End If

    End Function



    Public Function GetExtractList(ByVal LocationId As Integer, ByVal ExtractDirection As Integer, ByVal ExtractType As Integer) As DataTable
        Dim Extracts As New IMIS_DAL.IMISExtractsDAL
        Return Extracts.GetExtractList(LocationId, ExtractDirection, ExtractType)

    End Function


    Public Sub Zip(ByVal strDestFolder As String, ByVal strDestFileName As String, ByVal strSrcFolder As String, ByVal strSrcFilter As String)
        Dim cmd As String = ""
        'Folders must have the last '\' character
        'cmd = "a -p" & RARPWD & " """ & strDestFolder & strDestFileName & """ """ & strSrcFolder & strSrcFilter & """"
        'StartProcess(WinRarFolder, cmd)

        If IntPtr.Size = 8 Then 'If 64 bit
            SevenZipBase.SetLibraryPath(System.AppDomain.CurrentDomain.RelativeSearchPath & "\7z64.dll")
        Else
            SevenZipBase.SetLibraryPath(System.AppDomain.CurrentDomain.RelativeSearchPath & "\7z.dll")
        End If

        Dim Compressor As New SevenZipCompressor
        With Compressor
            .ArchiveFormat = OutArchiveFormat.SevenZip
            .CompressionMode = CompressionMode.Create
            .CompressionMethod = CompressionMethod.Default
            .DirectoryStructure = False
            .CompressionLevel = CompressionLevel.Normal
        End With


        'Compressor.CompressFilesEncrypted(strDestFolder & Path.DirectorySeparatorChar & strDestFileName, RARPWD, strSrcFolder)
        If Directory.EnumerateFiles(strSrcFolder, strSrcFilter).ToArray().Length > 0 Then
            Compressor.CompressFilesEncrypted(strDestFolder & Path.DirectorySeparatorChar & strDestFileName, RARPWD, Directory.EnumerateFiles(strSrcFolder, strSrcFilter).ToArray())
        End If


    End Sub


    Private Sub Unzip(ByVal strZippedFilename As String, ByVal strDestFolder As String)
        'Dim cmd As String = ""

        ''Folders must have the last '\' character
        ''cmd = "e -o+ -p" & RARPWD & " " & strZippedFilename & " " & strDestFolder
        'cmd = "e -o+ -p" & RARPWD & " """ & strZippedFilename & """ """ & strDestFolder & """"
        'StartProcess(WinRarFolder, cmd)

        Try

            If Not File.Exists(strZippedFilename) Then Exit Sub
            If IntPtr.Size = 8 Then 'If 64 bit
                SevenZipBase.SetLibraryPath(System.AppDomain.CurrentDomain.RelativeSearchPath & "\7z64.dll")
            Else
                SevenZipBase.SetLibraryPath(System.AppDomain.CurrentDomain.RelativeSearchPath & "\7z.dll")
            End If

            Dim Ext As SevenZipExtractor = New SevenZipExtractor(strZippedFilename, RARPWD)
            Ext.ExtractArchive(strDestFolder)

        Catch ex As Exception

            Throw ex
        End Try


    End Sub

    Private Sub StartProcess(ByVal WinRarFolder As String, ByVal cmd As String)
        Proc = New Process

        With Proc.StartInfo
            .FileName = WinRarFolder & "WinRAR.exe"
            .Arguments = cmd
            .UseShellExecute = False
            .RedirectStandardOutput = True
            .RedirectStandardError = False
            .CreateNoWindow = False
        End With

        Proc.EnableRaisingEvents = True
        Proc.Start()

        Dim output As String = Proc.StandardOutput.ReadToEnd

        Proc.WaitForExit()





    End Sub



    Private Function StrToBytes(ByVal str As String) As Byte()
        If str <> "" Then
            Dim byt(str.Length - 1) As Byte
            Dim int As Integer = 0
            For Each c As Char In str
                byt(int) = Asc(c)
            Next
            Return byt
        Else
            Return Nothing
        End If
    End Function

    Private Sub Encrypt(ByRef XMLFile As FileStream, ByVal key As String, ByVal outFile As String)
        Dim DESalg As New DESCryptoServiceProvider
        Dim outFs As New FileStream(outFile, FileMode.Create)
        Dim objEncod As Encoding = Encoding.ASCII
        DESalg.Key = objEncod.GetBytes(key)
        DESalg.IV = objEncod.GetBytes("11110000")
        Dim CryFile As New CryptoStream(outFs, DESalg.CreateEncryptor(DESalg.Key, DESalg.IV), CryptoStreamMode.Write)

        Dim BytesRead As Integer = 0
        Dim TransferChuckSize As Integer = 10485760   '10MB
        Dim numTotalBytes As Integer = CType(XMLFile.Length, Integer)
        Dim numTransferredBytes As Integer = 0
        Dim CurrentTransfer As Integer = 0
        ' Dim numBytesToRead As Integer = CType(XMLFile.Length, Integer)
        Dim numBytesRead As Integer = 0
        Dim bytes() As Byte = New Byte(10) {}

        ReDim bytes(0)
        XMLFile.Seek(0, SeekOrigin.Begin)
        'NOW NEED TO SPLIT
        While numTransferredBytes < numTotalBytes
            'chunks of 
            If numTotalBytes - numTransferredBytes > TransferChuckSize Then
                CurrentTransfer = TransferChuckSize
            Else
                CurrentTransfer = numTotalBytes - numTransferredBytes
            End If

            ReDim bytes(CurrentTransfer - 1)

            For i = 0 To CurrentTransfer - 1
                bytes(i) = XMLFile.ReadByte()
            Next
            'BytesRead = XMLFile.ReadByte((bytes, numTransferredBytes, _
            '            CurrentTransfer)

            Try
                CryFile.Write(bytes, 0, CurrentTransfer)
            Catch ex As Exception
                Try
                    CryFile.Write(bytes, 0, CurrentTransfer - 1)
                Catch ex1 As Exception
                    CryFile.Close()
                    outFs.Close()
                    Throw New Exception("Failed to encrypt an extract file...please verify available memory.")
                End Try

            End Try

            numTransferredBytes += CurrentTransfer
        End While

        CryFile.Close()
        outFs.Close()

    End Sub



    Private Function Decrypt(ByVal key As String, ByVal inFile As String) As String
        Dim DESalg As New DESCryptoServiceProvider
        Dim fs As New FileStream(inFile, FileMode.Open)
        Dim objEncod As Encoding = Encoding.ASCII
        DESalg.IV = objEncod.GetBytes("11110000")

        DESalg.Key = objEncod.GetBytes(key)
        Dim cryp As New CryptoStream(fs, DESalg.CreateDecryptor(DESalg.Key, DESalg.IV), CryptoStreamMode.Read)
        Dim fileInf As New IO.FileInfo(inFile)
        Dim strReader As New StreamReader(cryp)

        Dim str As String = strReader.ReadToEnd
        strReader.Close()
        cryp.Close()
        fs.Close()

        Return str
    End Function

    Private Sub EncryptData(ByVal filename As String, ByVal ExtractTableName As String, ByVal dtbl As DataTable)

        Dim ExtractFolder As String = Path.GetDirectoryName(filename) 'HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/")


        Dim strTempXMLFile As String = ExtractFolder & "\" & ExtractTableName & ".xml"

        ' If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/" & ExtractTableName & ".xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/" & ExtractTableName & ".xml"))
        If System.IO.File.Exists(strTempXMLFile) = True Then System.IO.File.Delete(strTempXMLFile)

        dtblIn = dtbl
        dtblIn.TableName = ExtractTableName
        ' Write the schema and data to XML in a memory stream.

        Dim XMLFile As New System.IO.FileStream(strTempXMLFile, System.IO.FileMode.Create)
        dtblIn.WriteXml(XMLFile, XmlWriteMode.WriteSchema)
        Encrypt(XMLFile, DESKEY, filename)

        XMLFile.Close()
        If System.IO.File.Exists(strTempXMLFile) = True Then System.IO.File.Delete(strTempXMLFile)

        ' If System.IO.File.Exists(strTempXMLFile) = True Then System.IO.File.Delete(strTempXMLFile)
        ' Dim xmlStream As New System.IO.MemoryStream()
        ' dtblIn.WriteXml(xmlStream, XmlWriteMode.WriteSchema)
        ' Dim buf As Byte() = xmlStream.ToArray()
        ' Encrypt(buf, DESKEY, filename)
    End Sub

    Private Function DecryptData(ByVal filename As String) As DataTable
        Dim dtblDecrypted As New DataTable

        Dim EncryptedBytes As Byte() = Encoding.UTF8.GetBytes(Decrypt(DESKEY, filename))
        Dim xmlStream2 As New System.IO.MemoryStream(EncryptedBytes)
        dtblDecrypted.ReadXml(xmlStream2)
        Return dtblDecrypted
    End Function

    Private Function ImageToBlob(ByVal id As String, ByVal filePath As String) As SQLiteParameter

        Dim SQLParam As New SQLiteParameter

        If Not System.IO.File.Exists(HttpContext.Current.Server.MapPath(filePath)) Then

            SQLParam = New SQLiteParameter("@Image", Nothing)
            SQLParam.DbType = DbType.Binary
            SQLParam.Value = Nothing

            Return SQLParam

        End If

        Dim fs As FileStream = New FileStream(HttpContext.Current.Server.MapPath(filePath), FileMode.Open, FileAccess.Read)
        Dim br As BinaryReader = New BinaryReader(fs)
        Dim bm() As Byte = br.ReadBytes(fs.Length)

        br.Close()
        fs.Close()


        Dim Photo() As Byte = bm
        SQLParam = New SQLiteParameter("@Image", Photo)
        SQLParam.DbType = DbType.Binary
        SQLParam.Value = Photo

        Return SQLParam


    End Function

    Private Sub DeleteWorkingFolder(WorkingFolder As String)
        If My.Computer.FileSystem.DirectoryExists(WorkingFolder) Then My.Computer.FileSystem.DeleteDirectory(WorkingFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
    End Sub

    Private Sub FlushWorkFolder()


        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xInsureePolicy.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xInsureePolicy.xml"))

        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xLocations.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xLocations.xml"))

        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xRegions.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xRegions.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xDistricts.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xDistricts.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xWards.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xWards.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xVillages.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xVillages.xml"))
        '2
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xItems.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xItems.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xServices.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xServices.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPLItems.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPLItems.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPLItemsDetails.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPLItemsDetails.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPLServices.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPLServices.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPLServicesDetails.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPLServicesDetails.xml"))
        '3

        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xICD.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xICD.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xHF.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xHF.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPayer.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPayer.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xOfficer.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xOfficer.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xProduct.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xProduct.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xProductItems.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xProductItems.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xProductServices.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xProductServices.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xRelDistr.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xRelDistr.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xClaimAdmin.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xClaimAdmin.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xOfficerVillage.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xOfficerVillage.xml"))

        '4
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xFamilies.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xFamilies.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xInsuree.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xInsuree.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPhotos.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPhotos.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPolicies.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPolicies.xml"))
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPremiums.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPremiums.xml"))
        '5
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xExtract.xml")) = True Then System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xExtract.xml"))

    End Sub

    Public Sub CreatePhoneExtracts(ByRef eExtractInfo As IMIS_EN.eExtractInfo, ByVal WithInsuree As Boolean)

        Dim eDefaults As New IMIS_EN.tblIMISDefaults
        Dim ExtractDAL As New IMIS_DAL.IMISExtractsDAL
        Dim DB_NAME As String
        Dim dtExtractSource As New DataTable
        Dim eExtractInfoLast As New IMIS_EN.tblExtracts

        eExtractInfoLast = ExtractDAL.GetLastCreateExtractInfo(If(eExtractInfo.DistrictId = 0, eExtractInfo.RegionId, eExtractInfo.DistrictId), eExtractInfo.ExtractType, 0)

        'Try

        Dim eExtract As New IMIS_EN.tblExtracts

        Dim Defaults As New IMIS_BL.IMISDefaultsBL
        Defaults.GetDefaults(eDefaults)

        DB_NAME = HttpContext.Current.Server.MapPath("Extracts\Phone") & "\ImisData" & eExtractInfo.LocationId & ".db3"

        If System.IO.File.Exists(DB_NAME) = True Then
            System.IO.File.Delete(DB_NAME)
        End If
        If WithInsuree = True Then dtExtractSource = ExtractDAL.GetPhoneExtractSource(eExtractInfo.LocationId)

        Dim con As New SQLite.SQLiteConnection
        Dim cmd As New SQLite.SQLiteCommand

        con.ConnectionString = "Data source = " & DB_NAME
        con.Open()
        'con.ChangePassword(DB_PWD)
        cmd = con.CreateCommand

        Dim sSQL As String = ""
        Dim strPhoto As String = ""
        sSQL = "CREATE TABLE tblPolicyInquiry(CHFID text,Photo BLOB, InsureeName Text, DOB Text, Gender Text, ProductCode Text, ProductName Text, ExpiryDate Text, Status Text, DedType Int, Ded1 Int, Ded2 Int, Ceiling1 Int, Ceiling2 Int)"
        cmd.CommandText = sSQL
        cmd.ExecuteNonQuery()


        If WithInsuree = True Then
            '  dtExtractSource = ExtractDAL.GetPhoneExtractSource(eExtractInfo.DistrictID)

            Dim i As Integer = 1

            Using InsertCmd = New SQLiteCommand(con)
                Using transaction = con.BeginTransaction
                    For Each row In dtExtractSource.Rows
                        sSQL = "INSERT INTO tblPolicyInquiry(CHFID ,Photo , InsureeName, DOB, Gender, ProductCode, ProductName, ExpiryDate, Status, DedType, Ded1, Ded2, Ceiling1, Ceiling2)" & _
                       " VALUES(@CHFID,@image,@InsureeName,@DOB,@Gender,@ProductCode,@ProductName,@ExpiryDate,@Status,@DedType,@Ded1,@Ded2,@Ceiling1,@Ceiling2)"
                        InsertCmd.CommandText = sSQL

                        InsertCmd.Parameters.AddWithValue("@CHFID", row("CHFID").ToString)
                        InsertCmd.Parameters.Add(ImageToBlob("@image", "\" & row("PhotoPath")))
                        InsertCmd.Parameters.AddWithValue("@InsureeName", row("InsureeName").ToString)
                        InsertCmd.Parameters.AddWithValue("@DOB", row("DOB"))
                        InsertCmd.Parameters.AddWithValue("@Gender", row("Gender").ToString)
                        InsertCmd.Parameters.AddWithValue("@ProductCode", row("ProductCode").ToString)
                        InsertCmd.Parameters.AddWithValue("@ProductName", row("ProductName").ToString)
                        InsertCmd.Parameters.AddWithValue("@ExpiryDate", row("ExpiryDate"))
                        InsertCmd.Parameters.AddWithValue("@Status", row("Status"))
                        InsertCmd.Parameters.AddWithValue("@DedType", row("DedType"))
                        InsertCmd.Parameters.AddWithValue("@Ded1", row("Ded1"))
                        InsertCmd.Parameters.AddWithValue("@Ded2", row("Ded2"))
                        InsertCmd.Parameters.AddWithValue("@Ceiling1", row("Ceiling1"))
                        InsertCmd.Parameters.AddWithValue("@Ceiling2", row("Ceiling2"))

                        InsertCmd.ExecuteNonQuery()
                        i = i + 1

                    Next
                    transaction.Commit()
                    InsertCmd.Dispose()
                End Using
            End Using

        End If
        cmd.Dispose()

        cmd = New SQLite.SQLiteCommand
        cmd = con.CreateCommand
        sSQL = "CREATE TABLE tblReferences([Code] Text, [Name] Text, [Type] Text, [Price] INT)"
        cmd.CommandText = sSQL
        cmd.ExecuteNonQuery()

        Dim dtReference As New DataTable
        dtReference = ExtractDAL.getReferences()



        Using InsertCmd = New SQLiteCommand(con)
            Using transaction = con.BeginTransaction
                For Each row In dtReference.Rows
                    sSQL = "INSERT INTO tblReferences([Code],[Name],[Type],[Price])" & _
                           " VALUES(@Code,@Name,@Type,@Price)"

                    InsertCmd.CommandText = sSQL

                    InsertCmd.Parameters.AddWithValue("@Code", row("Code").ToString)
                    InsertCmd.Parameters.AddWithValue("@Name", row("Name").ToString)
                    InsertCmd.Parameters.AddWithValue("@Type", row("Type").ToString)
                    InsertCmd.Parameters.AddWithValue("@Price", row("Price"))

                    InsertCmd.ExecuteNonQuery()

                Next
                transaction.Commit()
            End Using
        End Using

        cmd.Dispose()

        cmd = New SQLite.SQLiteCommand
        cmd = con.CreateCommand
        sSQL = "CREATE TABLE tblControls([FieldName] Text, [Adjustibility] Text, [Usage] Text)"
        cmd.CommandText = sSQL
        cmd.ExecuteNonQuery()

        Dim dtControls As New DataTable
        Dim ControlsDAL As New IMIS_DAL.ControlsDAL
        dtControls = ControlsDAL.getControlsSettings()

        Using InsertCmd = New SQLiteCommand(con)
            Using transaction = con.BeginTransaction
                For Each row In dtControls.Rows
                    sSQL = "INSERT INTO tblControls([FieldName],[Adjustibility],[Usage])" &
                           " VALUES(@FieldName,@Adjustibility,@Usage)"

                    InsertCmd.CommandText = sSQL

                    InsertCmd.Parameters.AddWithValue("@FieldName", row("FieldName").ToString)
                    InsertCmd.Parameters.AddWithValue("@Adjustibility", row("Adjustibility").ToString)
                    InsertCmd.Parameters.AddWithValue("@Usage", row("Usage").ToString)

                    InsertCmd.ExecuteNonQuery()

                Next
                transaction.Commit()
            End Using
        End Using

        cmd.Dispose()



        con.Close()


        eExtract.RowID = 0
        eExtract.AuditUserID = eExtractInfo.AuditUserID
        eExtract.ExtractDirection = 0
        eExtract.LocationId = eExtractInfo.LocationId
        eExtract.ExtractDate = Date.Now
        eExtract.HFID = 0
        eExtract.ExtractSequence = eExtractInfoLast.ExtractSequence + 1
        eExtract.AppVersionBackend = eDefaults.AppVersionBackEnd
        eExtract.ExtractFolder = eDefaults.FTPPhoneExtractFolder
        eExtract.ExtractType = eExtractInfo.ExtractType
        eExtract.ExtractFileName = "ImisData.db3"

        If Right(eExtract.ExtractFolder, 1) = "/" Or Right(eExtract.ExtractFolder, 1) = "\" Then
            eExtractInfo.ExtractFileName = Left(eExtract.ExtractFolder, Len(eExtract.ExtractFolder) - 1) & "\" & eExtract.ExtractFileName
        Else
            eExtractInfo.ExtractFileName = eExtract.ExtractFolder & "\" & eExtract.ExtractFileName
        End If

        'insert the extract entry 
        ExtractDAL.InsertExtract(eExtract)

        eExtractInfo.ExtractStatus = 0

        'Catch ex As Exception
        '    Exit Sub
        'End Try

    End Sub




    Public Function CreateOffLineExtracts(ByRef eExtractInfo As IMIS_EN.eExtractInfo) As Boolean

        Dim Extract As New IMIS_DAL.IMISExtractsDAL
        Dim Defaults As New IMIS_BL.IMISDefaultsBL
        Dim eExtract As New IMIS_EN.tblExtracts

        Dim eDefaults As New IMIS_EN.tblIMISDefaults
        '1
        Dim dtLocations As New DataTable
        'Dim dtRegions As New DataTable
        'Dim dtDistricts As New DataTable
        'Dim dtWards As New DataTable
        'Dim dtVillages As New DataTable
        '2
        Dim dtItems As New DataTable
        Dim dtServices As New DataTable
        Dim dtPLItems As New DataTable
        Dim dtPLItemsDetails As New DataTable
        Dim dtPLServices As New DataTable
        Dim dtPLServicesDetails As New DataTable
        '3
        Dim dtICD As New DataTable
        Dim dtHF As New DataTable
        Dim dtClaimAdmin As New DataTable
        Dim dtPayer As New DataTable
        Dim dtOfficer As New DataTable
        Dim dtProduct As New DataTable
        Dim dtProductItems As New DataTable
        Dim dtProductServices As New DataTable
        Dim dtRelDistr As New DataTable
        '4
        Dim dtFamilies As New DataTable
        Dim dtInsuree As New DataTable
        Dim dtPhotos As New DataTable
        Dim dtPolicies As New DataTable
        Dim dtPremiums As New DataTable
        Dim dtInsureePolicy As New DataTable
        Dim dtOfficerVillage As New DataTable
        '5
        Dim dtExtract As New DataTable

        Dim LRV As Int64
        Dim strTemp As String
        Dim iRandom As New Random

        Dim RandomFolderName As String = Path.GetRandomFileName
        Dim ExtractFolder As String = HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/")

        'Create directory to extract data
        My.Computer.FileSystem.CreateDirectory(ExtractFolder & RandomFolderName)

        Dim strFile As String = ExtractFolder & RandomFolderName

        Try

            LRV = Extract.GetDBLastRowVersion()   'to be saved later in the extract table

            Defaults.GetDefaults(eDefaults)

            'strFile = eDefaults.FTPOffLineExtractFolder & "\WorkFolder\xDistricts.xml"
            'strFile += "/xDistricts.xml"
            'If System.IO.File.Exists(strFile) = True Then
            '    eExtractInfo.ExtractStatus = 1    'Already in process
            '    CreateOffLineExtracts = False
            '    Exit Function
            'End If

            eExtract = Extract.GetLastCreateExtractInfo(If(eExtractInfo.DistrictId = 0, eExtractInfo.RegionId, eExtractInfo.DistrictId), eExtractInfo.ExtractType, 0)

            If eExtractInfo.ExtractType = 2 Then 'FULL !!
                eExtract.RowID = 0
            End If

            Extract.GetExportOfflineExtract1(eExtract.RowID, dtLocations)
            'now create the XML encrypted files in the FTP Folder 

            EncryptData(strFile & "/xLocations.xml", "Locations", dtLocations)

            'EncryptData(strFile & "/xRegions.xml", "Regions", dtRegions)
            'EncryptData(strFile & "/xDistricts.xml", "Districts", dtDistricts)
            'EncryptData(strFile & "/xWards.xml", "Wards", dtWards)
            'EncryptData(strFile & "/xVillages.xml", "Villages", dtVillages)

            eExtractInfo.LocationsCS = dtLocations.Rows.Count
            'eExtractInfo.RegionCS = dtRegions.Rows.Count
            'eExtractInfo.DistrictsCS = dtDistricts.Rows.Count
            'eExtractInfo.WardsCS = dtWards.Rows.Count
            'eExtractInfo.VillagesCS = dtVillages.Rows.Count

            Extract.GetExportOfflineExtract2(eExtractInfo.LocationId, eExtract.RowID, dtItems, dtServices, dtPLItems, dtPLItemsDetails, dtPLServices, dtPLServicesDetails)
            'now create the XML encrypted files in the FTP Folder 
            EncryptData(strFile & "/xItems.xml", "Items", dtItems)
            EncryptData(strFile & "/xServices.xml", "Services", dtServices)
            EncryptData(strFile & "/xPLItems.xml", "PLItems", dtPLItems)
            EncryptData(strFile & "/xPLItemsDetails.xml", "PLItemsDetails", dtPLItemsDetails)
            EncryptData(strFile & "/xPLServices.xml", "PLServices", dtPLServices)
            EncryptData(strFile & "/xPLServicesDetails.xml", "PLServicesDetails", dtPLServicesDetails)
            eExtractInfo.ItemsCS = dtItems.Rows.Count
            eExtractInfo.ServicesCS = dtServices.Rows.Count
            eExtractInfo.PLItemsCS = dtPLItems.Rows.Count
            eExtractInfo.PLItemsDetailsCS = dtPLItemsDetails.Rows.Count
            eExtractInfo.PLServicesCS = dtPLServices.Rows.Count
            eExtractInfo.PLServicesDetailsCS = dtPLServicesDetails.Rows.Count

            Extract.GetExportOfflineExtract3(eExtractInfo, eExtract.RowID, dtICD, dtHF, dtPayer, dtOfficer, dtProduct, dtProductItems, dtProductServices, dtRelDistr, dtClaimAdmin, dtOfficerVillage)
            'now create the XML encrypted files in the FTP Folder 
            EncryptData(strFile & "/xICD.xml", "ICD", dtICD)
            EncryptData(strFile & "/xHF.xml", "HF", dtHF)
            EncryptData(strFile & "/xPayer.xml", "Payer", dtPayer)
            EncryptData(strFile & "/xOfficer.xml", "Officer", dtOfficer)
            EncryptData(strFile & "/xProduct.xml", "Product", dtProduct)
            EncryptData(strFile & "/xProductItems.xml", "ProductItems", dtProductItems)
            EncryptData(strFile & "/xProductServices.xml", "ProductServices", dtProductServices)
            EncryptData(strFile & "/xRelDistr.xml", "RelDistr", dtRelDistr)
            EncryptData(strFile & "/xClaimAdmin.xml", "ClaimAdmin", dtClaimAdmin)
            EncryptData(strFile & "/xOfficerVillage.xml", "OfficerVillage", dtOfficerVillage)

            eExtractInfo.RegionCS = dtLocations.Select("LocationType='R'  AND ValidityTo IS NULL").Count
            eExtractInfo.DistrictsCS = dtLocations.Select("LocationType='D'  AND ValidityTo IS NULL").Count
            eExtractInfo.WardsCS = dtLocations.Select("LocationType='W'  AND ValidityTo IS NULL").Count
            eExtractInfo.VillagesCS = dtLocations.Select("LocationType='V'").Count
            eExtractInfo.ICDCS = dtICD.Select("ValidityTo IS NULL").Count
            eExtractInfo.HFCS = dtHF.Select("ValidityTo IS NULL").Count
            eExtractInfo.PayerCS = dtPayer.Select("ValidityTo IS NULL").Count
            eExtractInfo.OfficerCS = dtOfficer.Select("ValidityTo IS NULL").Count
            eExtractInfo.ProductCS = dtProduct.Select("ValidityTo IS NULL").Count
            eExtractInfo.ProductItemsCS = dtProductItems.Select("ValidityTo IS NULL").Count
            eExtractInfo.ProductServicesCS = dtProductServices.Select("ValidityTo IS NULL").Count
            eExtractInfo.RelDistrCS = dtRelDistr.Select("ValidityTo IS NULL").Count
            eExtractInfo.ClaimAdminCS = dtClaimAdmin.Select("ValidityTo IS NULL").Count


            Extract.GetExportOfflineExtract4(eExtractInfo, eExtract.RowID, dtFamilies, 1)
            EncryptData(strFile & "/xFamilies.xml", "Families", dtFamilies)
            eExtractInfo.FamiliesCS = dtFamilies.Rows.Count
            dtFamilies = New DataTable()
            'clear memory

            Extract.GetExportOfflineExtract4(eExtractInfo, eExtract.RowID, dtInsuree, 2)
            EncryptData(strFile & "/xInsuree.xml", "Insuree", dtInsuree)
            eExtractInfo.InsureeCS = dtInsuree.Rows.Count
            dtInsuree = New DataTable()


            Extract.GetExportOfflineExtract4(eExtractInfo, eExtract.RowID, dtPhotos, 3)
            EncryptData(strFile & "/xPhotos.xml", "Photos", dtPhotos)
            eExtractInfo.PhotoCS = dtPhotos.Rows.Count
            dtPhotos = New DataTable()


            Extract.GetExportOfflineExtract4(eExtractInfo, eExtract.RowID, dtPolicies, 4)
            EncryptData(strFile & "/xPolicies.xml", "Policies", dtPolicies)
            eExtractInfo.PolicyCS = dtPolicies.Rows.Count
            dtPolicies = New DataTable()


            Extract.GetExportOfflineExtract4(eExtractInfo, eExtract.RowID, dtPremiums, 5)
            EncryptData(strFile & "/xPremiums.xml", "Premiums", dtPremiums)
            eExtractInfo.PremiumCS = dtPremiums.Rows.Count
            dtPremiums = New DataTable()

            Extract.GetExportOfflineExtract4(eExtractInfo, eExtract.RowID, dtInsureePolicy, 6)
            'now create the XML encrypted files in the FTP Folder 
            EncryptData(strFile & "/xInsureePolicy.xml", "InsureePolicy", dtInsureePolicy)
            dtInsureePolicy = New DataTable()


            eExtract.RowID = LRV
            eExtract.AuditUserID = eExtractInfo.AuditUserID
            eExtract.ExtractDirection = 0
            If eExtractInfo.DistrictId = 0 Then
                eExtract.LocationId = eExtractInfo.RegionId
            Else
                eExtract.LocationId = eExtractInfo.DistrictId
            End If
            eExtract.ExtractDate = Date.Now
            eExtract.HFID = 0
            'If eExtractInfo.ExtractType = 2 Then 'FULL !!
            '    eExtract.ExtractSequence = eExtract.ExtractSequence + 1      'The full extract has the same sequence as the detailed sequence
            'Else
            '    eExtract.ExtractSequence = eExtract.ExtractSequence + 1    'new sequence
            'End If

            eExtract.ExtractSequence = eExtractInfo.ExtractSequence

            eExtract.AppVersionBackend = eDefaults.AppVersionBackEnd
            eExtract.ExtractFolder = eDefaults.FTPOffLineExtractFolder
            eExtract.ExtractType = eExtractInfo.ExtractType

            'EDITED BY AMANI 26/09
            'ANOTHER CONDITION FOR eExtractInfo.WithInsuree = 0 ADDED
            If eExtractInfo.ExtractType = 2 Then
                strTemp = "F"
                If eExtractInfo.WithInsuree = False Then
                    strTemp = "E"
                End If
            Else
                strTemp = "D"
            End If
            'EDITED END 







            ' eExtract.ExtractFileName = "OE" & strTemp & "_" & eExtract.LocationId & "_" & FormatDateTime(Now, DateFormat.GeneralDate) & "_" & eExtract.ExtractSequence & ".RAR"
            eExtract.ExtractFileName = "OE_" & strTemp & "_" & eExtract.LocationId & "_" & eExtract.ExtractSequence & ".RAR"
            eExtractInfo.ExtractFileName = eExtract.ExtractFileName
            'zip the files 

            Extract.InsertExtract(eExtract)

            ' reader = myCommand.ExecuteReader()

            Extract.GetExportOfflineExtract5(eExtract, dtExtract)  'Check header table and include in the export 
            EncryptData(strFile & "/xExtract.xml", "Extract", dtExtract)


            Zip(HttpContext.Current.Server.MapPath("~/Extracts/Offline/"), eExtract.ExtractFileName, strFile & "/", "*.xml")

            'now create all photos into a file 

            eExtractInfo.ZippedPhotosCS = CollectPhotos(dtPhotos, "OE_" & strTemp & "_" & eExtract.LocationId & "_" & eExtract.ExtractSequence & "_Photos", strFile)
            ZipPhotos("OE_" & strTemp & "_" & eExtract.LocationId & "_" & eExtract.ExtractSequence & "_Photos", ".RAR", strFile)

            'We don't need to clear images because we will delete the temp folder
            'Call ClearJPGContents()

            eExtractInfo.ExtractStatus = 0  '=ALL OK
            Return True

        Catch ex As Exception
            Throw ex
        Finally
            DeleteWorkingFolder(strFile)
        End Try
    End Function



    Public Function NewSequenceNumber(ByVal LocationId As Integer) As Integer
        Dim IMISDAL As New IMIS_DAL.IMISExtractsDAL
        Return IMISDAL.NewSequenceNumber(LocationId)

    End Function


    Public Function ImportOffLinePhotos(ByRef eExtractInfo As IMIS_EN.eExtractInfo) As Boolean

        Dim Extract As New IMIS_DAL.IMISExtractsDAL
        Dim Defaults As New IMIS_BL.IMISDefaultsBL
        Dim eExtract As New IMIS_EN.tblExtracts

        Dim eDefaults As New IMIS_EN.tblIMISDefaults

        Dim eHF As New IMIS_EN.tblHF
        Dim HF As New IMIS_DAL.HealthFacilityDAL
        ImportOffLinePhotos = False

        Defaults.GetDefaults(eDefaults)
        eExtractInfo.HFID = eDefaults.OffLineHF

        If eDefaults.OffLineHF = 0 And eDefaults.OfflineCHF = 0 Then
            eExtractInfo.ExtractStatus = -1  'HF ID NOT set in defaults
            Exit Function
        End If

        'START ISOLATION And Rollback Mechanism
        eHF.HfID = eDefaults.OffLineHF
        HF.LoadHF(eHF)
        eExtractInfo.HFID = eDefaults.OffLineHF

        If eHF.HFName = Nothing Then
            'this must be the first time to upload a full extract
            eExtractInfo.LocationId = 0
        Else
            eExtractInfo.LocationId = eHF.tblLocations.LocationId

        End If

        'Get latest extract to verify the sequence etc...
        eExtract = Extract.GetLastCreateExtractInfo(If(eExtractInfo.DistrictId = 0, eExtractInfo.RegionId, eExtractInfo.DistrictId), 0, 1)

        'extract (unzip with password) all files to the working folder and pass them one by one to the stored procedures for upload
        '0 Step FIRST CHECK IF this Extract is valid to load 

        'Zip(eDefaults.WinRarFolder, HttpContext.Current.Server.MapPath("~/Extracts/Offline/"), eExtract.ExtractFileName, HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/"), "*.xml")
        Unzip(HttpContext.Current.Server.MapPath("~/Extracts/Offline/" & eExtractInfo.ExtractFileName), HttpContext.Current.Server.MapPath("~/Images/Updated/"))

        eExtractInfo.ExtractStatus = 0
        Return True

    End Function
    Public Sub DeleteAllLocalRecords()
        Dim Ext As New IMIS_DAL.IMISExtractsDAL
        Ext.DeleteAllLocalRecords()
    End Sub


    Public Function ImportOffLineExtracts(ByRef eExtractInfo As IMIS_EN.eExtractInfo) As Boolean

        Dim Extract As New IMIS_DAL.IMISExtractsDAL
        Dim Defaults As New IMIS_BL.IMISDefaultsBL
        Dim eExtract As New IMIS_EN.tblExtracts

        '1'
        Dim dtLocations As New DataTable
        'Dim dtRegions As New DataTable
        'Dim dtDistricts As New DataTable
        'Dim dtWards As New DataTable
        'Dim dtVillages As New DataTable
        '2'
        Dim dtItems As New DataTable
        Dim dtServices As New DataTable
        Dim dtPLItems As New DataTable
        Dim dtPLItemsDetails As New DataTable
        Dim dtPLServices As New DataTable
        Dim dtPLServicesDetails As New DataTable
        '3'
        Dim dtICD As New DataTable
        Dim dtHF As New DataTable
        Dim dtPayer As New DataTable
        Dim dtOfficer As New DataTable
        Dim dtProduct As New DataTable
        Dim dtProductItems As New DataTable
        Dim dtProductServices As New DataTable
        Dim dtRelDistr As New DataTable
        Dim dtClaimAdmin As New DataTable
        Dim dtOfficerVillage As New DataTable

        '4'
        Dim dtFamilies As New DataTable
        Dim dtInsuree As New DataTable
        Dim dtPhotos As New DataTable
        Dim dtPolicies As New DataTable
        Dim dtPremiums As New DataTable
        Dim dtInsureePolicy As New DataTable

        '5'
        Dim dtExtractHeader As New DataTable


        Dim eDefaults As New IMIS_EN.tblIMISDefaults
        Dim strFile As String

        Dim eHF As New IMIS_EN.tblHF
        Dim HF As New IMIS_DAL.HealthFacilityDAL

        Dim HighestExtractImportSequence As Integer = 0
        ImportOffLineExtracts = False

        Try
            FlushWorkFolder() 'ADDED BY AMANI 27/09
            DeleteAllLocalRecords()

            strFile = HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xLocations.xml")
            If System.IO.File.Exists(strFile) = True Then
                eExtractInfo.ExtractStatus = 1    'Already in process
                ImportOffLineExtracts = False
                Exit Function
            End If

            Defaults.GetDefaults(eDefaults)
            eExtractInfo.HFID = eDefaults.OffLineHF

            If eDefaults.OffLineHF = 0 And eDefaults.OfflineCHF = 0 Then
                eExtractInfo.ExtractStatus = -1  'HF ID NOT set in defaults
                Exit Function
            End If

            'START ISOLATION And Rollback Mechanism
            eHF.HfID = eDefaults.OffLineHF
            HF.LoadHF(eHF)
            'first check if we have a district ID via the HF.
            If eHF.HFName = Nothing Then
                'this must be the first time to upload a full extract
                eExtractInfo.LocationId = 0
            Else
                eExtractInfo.LocationId = eHF.tblLocations.LocationId

            End If

            'Get latest extract to verify the sequence etc...
            '  eExtract = Extract.GetLastCreateExtractInfo(If(eExtractInfo.DistrictId = 0, eExtractInfo.RegionId, eExtractInfo.DistrictId), 0, 1)
            'HVH changed to get last extract seq
            eExtract = Extract.GetNewSeqToImport()
            HighestExtractImportSequence = eExtract.ExtractSequence
            'extract (unzip with password) all files to the working folder and pass them one by one to the stored procedures for upload
            '0 Step FIRST CHECK IF this Extract is valid to load 

            'Zip(eDefaults.WinRarFolder, HttpContext.Current.Server.MapPath("~/Extracts/Offline/"), eExtract.ExtractFileName, HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/"), "*.xml")
            Unzip(HttpContext.Current.Server.MapPath("~/Extracts/Offline/" & eExtractInfo.ExtractFileName), HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/"))


            dtExtractHeader = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xExtract.xml"))
            If dtExtractHeader.Rows.Count = 0 Then
                eExtractInfo.ExtractStatus = -2  'extract header not read

                FlushWorkFolder()
                Exit Function    'error occured
            End If
            If eExtractInfo.LocationId <> 0 Then
                If dtExtractHeader.Rows(0)("LocationId") <> eHF.tblLocations.LocationId Then
                    eExtractInfo.ExtractStatus = -3  'extract from wrong district 
                    '
                    'FlushWorkFolder()
                    Exit Function    'error occured
                End If
            Else
                eExtractInfo.LocationId = dtExtractHeader.Rows(0)("LocationId") 'set the district to the current imported one for the first time !
            End If

            If dtExtractHeader.Rows(0)("AppVersionBackEnd") <> eDefaults.AppVersionBackEnd Then
                eExtractInfo.ExtractStatus = -4  'extract Backend version Incompatible
                FlushWorkFolder()
                Exit Function    'error occured
            End If

            If (dtExtractHeader(0)("ExtractType") <> 2) And Not Extract.isFullExtractExists Then
                eExtractInfo.ExtractStatus = -5  'extract should be full as the first extract !! 
                FlushWorkFolder()
                Exit Function    'error occured

                ''ADDED BY AMANI 27/09
                'If eExtract.RowID = 0 Then
                '    eExtractInfo.ExtractStatus = -5  'extract should be full as the first extract !! 
                '    FlushWorkFolder()
                '    Exit Function    'error occured
                'End If



            End If

            If eExtract.ExtractSequence <> 0 And (dtExtractHeader(0)("ExtractType") <> 2) Then
                'CHECK IF SEQUENCE IS CORRECT TO IMPORT
                If dtExtractHeader.Rows(0)("ExtractSequence") <= eExtract.ExtractSequence Then
                    eExtractInfo.ExtractStatus = -6  'data already uploaded 
                    FlushWorkFolder()
                    Exit Function    'error occured
                End If

                If (dtExtractHeader.Rows(0)("ExtractSequence") > eExtract.ExtractSequence + 1) And (dtExtractHeader(0)("ExtractType") = 4) Then
                    eExtractInfo.ExtractStatus = -7  'You have missed one or more sequences
                    FlushWorkFolder()
                    Exit Function    'error occured
                End If

            End If


            'Check if this import is for offline health facility
            If eHF.HfID > 0 Then
                dtHF = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xHF.xml"))
                If dtHF.Rows.Count > 0 Then
                    eExtractInfo.LocationId = dtHF.Select("HFID=" & eHF.HfID & "")(0)("LocationId")
                End If
            End If

            '1'
            dtLocations = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xLocations.xml"))
            'dtDistricts = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xDistricts.xml"))
            'dtWards = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xWards.xml"))
            'dtVillages = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xVillages.xml"))
            Extract.ImportOfflineExtract1(eExtractInfo, dtLocations)

            '2'
            dtServices = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xServices.xml"))
            dtItems = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xItems.xml"))
            dtPLItems = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPLItems.xml"))
            dtPLItemsDetails = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPLItemsDetails.xml"))
            dtPLServices = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPLServices.xml"))
            dtPLServicesDetails = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPLServicesDetails.xml"))
            Extract.ImportOfflineExtract2(eExtractInfo, dtItems, dtServices, dtPLItems, dtPLItemsDetails, dtPLServices, dtPLServicesDetails)

            '3'
            dtICD = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xICD.xml"))
            dtHF = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xHF.xml"))
            dtPayer = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPayer.xml"))
            dtOfficer = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xOfficer.xml"))
            dtProduct = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xProduct.xml"))
            dtProductItems = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xProductItems.xml"))
            dtProductServices = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xProductServices.xml"))
            dtRelDistr = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xRelDistr.xml"))
            dtClaimAdmin = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xClaimAdmin.xml"))
            dtOfficerVillage = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xOfficerVillage.xml"))

            Extract.ImportOfflineExtract3(eExtractInfo, dtICD, dtHF, dtPayer, dtOfficer, dtProduct, dtProductItems, dtProductServices, dtRelDistr, dtClaimAdmin, dtOfficerVillage)

            '4'
            dtFamilies = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xFamilies.xml"))
            dtInsuree = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xInsuree.xml"))
            dtPhotos = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPhotos.xml"))
            dtPolicies = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPolicies.xml"))
            dtPremiums = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xPremiums.xml"))
            dtInsureePolicy = DecryptData(HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/xInsureePolicy.xml"))
            Extract.ImportOfflineExtract4(eExtractInfo, dtFamilies, dtInsuree, dtPhotos, dtPolicies, dtPremiums, dtInsureePolicy)

            'NOW WRITE TO Extract Table 

            eExtract.RowID = dtExtractHeader.Rows(0)("RowID")
            eExtract.AuditUserID = eExtractInfo.AuditUserID
            eExtract.ExtractDirection = 1
            eExtract.LocationId = eExtractInfo.LocationId
            eExtract.ExtractDate = dtExtractHeader(0)("ExtractDate")
            eExtract.HFID = eExtractInfo.HFID
            eExtract.ExtractSequence = dtExtractHeader(0)("ExtractSequence")
            eExtract.AppVersionBackend = dtExtractHeader(0)("AppVersionBackend")
            eExtract.ExtractFolder = eDefaults.FTPOffLineExtractFolder
            eExtract.ExtractType = dtExtractHeader(0)("ExtractType")
            eExtract.ExtractFileName = eExtractInfo.ExtractFileName

            'determine if we are going back to an old 'FULL' extract  --> if so ... first delete all entries from extact table
            If eExtract.ExtractSequence < HighestExtractImportSequence And eExtract.ExtractType = 2 Then
                Extract.FlagExtractTableasDeleted()
            End If

            Extract.InsertExtract(eExtract)
            ''END ISOLATION AND ROLLBACK OR COMMIT

            FlushWorkFolder()
            eExtractInfo.ExtractStatus = 0

        Catch ex As Exception
            eExtractInfo.ExtractStatus = -8 'unexpected error 
            FlushWorkFolder()
        End Try


        Return True
    End Function



    'Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
    '    Dim con As New SQLite.SQLiteConnection
    '    con.ConnectionString = "Data source = " & DB_NAME
    '    con.Open()

    '    Dim cmd As New SQLiteCommand
    '    cmd = con.CreateCommand
    '    cmd.CommandText = "SELECT PHOTO from tblPolicyInquiry"
    '    Dim sqlReader As SQLiteDataReader = cmd.ExecuteReader
    '    While sqlReader.Read
    '        PictureBox1.Image = BlobToImage(sqlReader("Photo"))
    '    End While

    '    cmd.Dispose()
    '    con.Dispose()



    'End Sub

    'Private Function BlobToImage(ByVal BLOB)
    '    Dim mStream As New System.IO.MemoryStream
    '    Dim pData() As Byte = DirectCast(BLOB, Byte())
    '    mStream.Write(pData, 0, Convert.ToInt32(pData.Length))
    '    Dim bm As Bitmap = New Bitmap(mStream, False)
    '    mStream.Dispose()
    '    Return bm
    'End Function


    Private Sub ZipPhotos(ByVal FileName As String, ByVal Extension As String, WorkingFolder As String)
        Dim Defaults As New IMIS_BL.IMISDefaultsBL
        Dim eDefaults As New IMIS_EN.tblIMISDefaults

        Defaults.GetDefaults(eDefaults)

        Zip(HttpContext.Current.Server.MapPath("~/Extracts/Offline/"), FileName & Extension, WorkingFolder & "/", "*.jpg")


        'Dim DestDir As String = HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/" & FileName & "")

        'Directory.Delete(DestDir)


    End Sub


    Private Function ClearJPGContents()

        Dim strDirectory As String = HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/")
        For Each foundFile As String In My.Computer.FileSystem.GetFiles(strDirectory, FileIO.SearchOption.SearchTopLevelOnly, "*.jpg")
            My.Computer.FileSystem.DeleteFile(foundFile, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
        Next

        Return ""
    End Function

    Private Function CollectPhotos(ByVal dt As DataTable, ByVal FileName As String, WorkingFolder As String) As Integer


        Dim iPhotos As Integer

        'Dim DestDir As String = HttpContext.Current.Server.MapPath("~/Extracts/Offline/WorkFolder/")

        ''If Directory.Exists(DestDir) Then
        ''    Directory.Delete(DestDir)
        ''End If


        'Directory.CreateDirectory(DestDir)

        iPhotos = 0
        For Each row In dt.Rows
            If Not row("ValidityTo") Is DBNull.Value Then Continue For
            'check first if photo already exists in temp directory
            If System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Images/Updated/") & row("PhotoFileName")) = True Then
                If System.IO.File.Exists(WorkingFolder & "/" & row("PhotoFileName")) = False Then
                    System.IO.File.Copy(HttpContext.Current.Server.MapPath("~/Images/Updated/") & row("PhotoFileName"), WorkingFolder & "/" & row("PhotoFileName"), True)
                    ' Directory.Move(HttpContext.Current.Server.MapPath("~/Images/Updated/") & row("PhotoFileName"), DestDir & "\" & row("PhotoFileName"))
                    iPhotos = iPhotos + 1

                End If

            End If

        Next

        CollectPhotos = iPhotos

    End Function
    

    Public Sub SubmitClaimFromXML(ByVal FileName As String)
        Dim Defaults As New IMIS_EN.tblIMISDefaults
        Dim def As New IMIS_BL.IMISDefaultsBL
        def.GetDefaults(Defaults)
        Dim Extracts As New IMIS_DAL.IMISExtractsDAL


        'Amani added random folder 30/11/2017
        Dim WorkingFolder As String = HttpContext.Current.Server.MapPath("\FromPhone\Claim\")
        Dim RandomFolderName As String = Path.GetRandomFileName
        WorkingFolder = WorkingFolder & RandomFolderName
        Unzip(FileName, WorkingFolder)

        Dim XMLs As String() = Directory.GetFiles(WorkingFolder, "Claim_*.xml")
        Dim xml As String = ""

        For i As Integer = 0 To XMLs.Count - 1
            'xml = Mid(XMLs(i), XMLs(i).LastIndexOf("\") + 2, XMLs(i).Length)
            Extracts.SubmitClaimFromXML(XMLs(i))
            File.Delete(XMLs(i))
        Next

        'delete Random Folder Amani 30/11
        Directory.Delete(WorkingFolder, True)

        'delete Random Folder And Its Contents
        Directory.Delete(Path.GetDirectoryName(FileName), True)



    End Sub


    Public Sub CreateEnrolmentXML(ByVal Output As Dictionary(Of String, Integer), Optional ByVal isBackup As Boolean = False)
        'FOLLOWING LINES WILL CREATE AN XML

        Dim Ext As New IMIS_DAL.IMISExtractsDAL
        Dim dtXML As New DataTable
        Dim def As New IMIS_BL.IMISDefaultsBL
        Dim defaults As New IMIS_EN.tblIMISDefaults
        def.GetDefaults(defaults)
        Dim CHFID As Integer = defaults.OfflineCHF
        dtXML = Ext.GetEnrolmentXML(Output)
        Dim sXML As String = ""
        Dim Gen As New GeneralBL

        For Each row As DataRow In dtXML.Rows
            sXML += row(0).ToString
        Next

        Dim EnrolXML As System.Xml.XmlDocument = New System.Xml.XmlDocument
        EnrolXML.LoadXml(sXML)

        If isBackup = False Then
            If EnrolXML.ChildNodes(0).ChildNodes.Count = 0 Then
                Dim Ex As New Exception(Gen.getMessage("M_NOENROLMENTFOUND"))
                Throw Ex
                Exit Sub
            End If
        End If

        Dim FileName As String = "Enrolment_" & CHFID & "_" & Format(Now, "dd-MM-yyyy-HH-mm-ss") & ".xml"
        Dim path As String = HttpContext.Current.Server.MapPath("WorkSpace") & "\" & FileName

        EnrolXML.Save(path)

        If isBackup = False Then
            'FOLLOWING LINES WILL ZIP THE FILE
            Dim WinRarFolder As String = defaults.WinRarFolder
            Zip(HttpContext.Current.Server.MapPath("~/WorkSpace/"), Mid(FileName, 1, Len(FileName) - 4) & ".RAR", HttpContext.Current.Server.MapPath("~/WorkSpace/"), "Enrolment_*.xml")
        End If
        'FOLLOWING LINE WILL MOVE THE FILE TO Archive Folder
        MoveXMLsToArhive()

    End Sub
    Private Sub MoveXMLsToArhive()
        Dim XMLs As String()
        XMLs = Directory.GetFiles(HttpContext.Current.Server.MapPath("Workspace"), "Enrolment_*.xml")

        For i As Integer = 0 To XMLs.Length - 1
            If File.Exists(XMLs(i)) Then
                File.Move(XMLs(i), HttpContext.Current.Server.MapPath("Archive\") & Mid(XMLs(i), XMLs(i).LastIndexOf("\") + 2, XMLs(i).Length))
                'File.Delete(XMLs(i))
                'Continue For
            End If


        Next

    End Sub

    Public Function UploadEnrolments(ByVal FileName As String, ByVal Output As Dictionary(Of String, Integer)) As DataTable
        Dim Defaults As New IMIS_EN.tblIMISDefaults
        Dim def As New IMIS_BL.IMISDefaultsBL
        def.GetDefaults(Defaults)
        Dim Extracts As New IMIS_DAL.IMISExtractsDAL
        Dim WorkingFolder As String = HttpContext.Current.Server.MapPath("WorkSpace")
        Dim WorkingDirectory As String = IO.Path.GetFileNameWithoutExtension(FileName)

        Dim WorkingDirectoryPath As String = IO.Path.Combine(WorkingFolder, WorkingDirectory)

        If My.Computer.FileSystem.DirectoryExists(WorkingDirectoryPath) Then My.Computer.FileSystem.DeleteDirectory(WorkingDirectoryPath, FileIO.DeleteDirectoryOption.DeleteAllContents)

        My.Computer.FileSystem.CreateDirectory(WorkingDirectoryPath)


        'Move the file to the newly created folder
        File.Move(FileName, IO.Path.Combine(WorkingDirectoryPath, IO.Path.GetFileName(FileName)))

        'Unzip(Defaults.WinRarFolder, FileName, WorkingFolder)
        Unzip(IO.Path.Combine(WorkingDirectoryPath, IO.Path.GetFileName(FileName)), WorkingDirectoryPath)

        'Dim XMLs As String() = Directory.GetFiles(WorkingFolder, "Enrolment_*.xml")
        Dim XMLs As String() = Directory.GetFiles(WorkingDirectoryPath, "Enrolment_*.xml")
        Dim xml As String = ""

        Dim Result As New DataTable

        If XMLs.Count > 0 Then
            Result = Extracts.UploadEnrolments(XMLs(0), Output)
            File.Delete(XMLs(0))
        End If

        'File.Delete(IO.Path.Combine(WorkingDirectoryPath, IO.Path.GetFileName(FileName)))
        If My.Computer.FileSystem.DirectoryExists(WorkingDirectoryPath) Then My.Computer.FileSystem.DeleteDirectory(WorkingDirectoryPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
        Return Result

    End Function
End Class

