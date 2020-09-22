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

Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration.WebConfigurationManager


Public Class ExactSQL

    Private _sqladapter As New SqlClient.SqlDataAdapter
    Private _dtbl As New DataTable
    Private _ds As DataSet = Nothing
    Private _IdentityKey As String = ""
    Private _TableName As String = ""
    Private _SQLCommand As New SqlClient.SqlCommand


    Public ReadOnly Property sqlParameters(Optional ByVal Paramname As String = "") As Object
        Get
            Return _SQLCommand.Parameters(Paramname).Value
        End Get
    End Property
    Public Sub params(ByVal param As String, ByVal type As SqlDbType, ByVal paramvalue As Object, Optional ByVal direction As ParameterDirection = ParameterDirection.Input)
        _SQLCommand.Parameters.Add(param, type).Value = if(paramvalue Is Nothing, DBNull.Value, paramvalue)
        _SQLCommand.Parameters(param).Direction = direction
    End Sub
    Public Sub params(ByVal param As String, ByVal paramvalue As Object, Optional ByVal UserType As String = Nothing)
        _SQLCommand.Parameters.AddWithValue(param, paramvalue)
        If Not UserType = Nothing Then
            _SQLCommand.Parameters(param).TypeName = UserType
        End If
    End Sub

   
    Public Sub params(ByVal param As String, ByVal type As SqlDbType, ByVal Size As Integer, ByVal paramvalue As String, Optional ByVal direction As ParameterDirection = ParameterDirection.Input)
        _SQLCommand.Parameters.Add(param, type, Size).Value = if(paramvalue Is Nothing, DBNull.Value, paramvalue)
        _SQLCommand.Parameters(param).Direction = direction
    End Sub
    Public Sub setSQLCommand(ByVal cmd As String, ByVal cmdtype As CommandType, Optional ByVal ConString As String = "IMISConnectionString", Optional ByVal timeout As Integer = 300)
        _SQLCommand = New SqlClient.SqlCommand
        'If ConString = "" Then
        '    ConString = Web.Configuration.WebConfigurationManager.ConnectionStrings("IMISConnectionString").ConnectionString
        'Else
        ConString = Web.Configuration.WebConfigurationManager.ConnectionStrings(ConString).ConnectionString
        'End If
        Dim con As New SqlConnection(ConString)
        Dim commandTimeout As Integer = IMIS_EN.AppConfiguration.CommandTimeout
        If commandTimeout <= 0 Then
            _SQLCommand.CommandTimeout = timeout
        Else
            _SQLCommand.CommandTimeout = commandTimeout
        End If
        _SQLCommand.CommandText = cmd
        _SQLCommand.CommandType = cmdtype
        _SQLCommand.Connection = con 'Exact.Data.sql.SQLConn
    End Sub

    Public Sub setSQLCommand(ByVal SQLCommand As SqlCommand, Optional ByVal ConString As String = "IMISConnectionString", Optional ByVal timeout As Integer = 60)
        _SQLCommand = SQLCommand
        'If ConString = "" Then
        '    ConString = Web.Configuration.WebConfigurationManager.ConnectionStrings("IMISConnectionString").ConnectionString
        'Else
        ConString = Web.Configuration.WebConfigurationManager.ConnectionStrings(ConString).ConnectionString
        'End If
        Dim con As New SqlConnection(ConString)
        _SQLCommand.CommandTimeout = timeout
        _SQLCommand.Connection = con 'Exact.Data.sql.SQLConn

    End Sub

    Public Function Filldata(Optional ByVal IdentityColumn As String = "", Optional ByVal TableName As String = "", Optional ByVal timeout As Integer = 120) As DataTable
        Try
            _TableName = TableName
            _IdentityKey = IdentityColumn
            _sqladapter = New SqlClient.SqlDataAdapter
            Dim commandTimeout As Integer = IMIS_EN.AppConfiguration.CommandTimeout
            If commandTimeout <= 0 Then
                _SQLCommand.CommandTimeout = timeout
            Else
                _SQLCommand.CommandTimeout = commandTimeout
            End If
            _sqladapter.SelectCommand = _SQLCommand
            _dtbl = New DataTable
            _sqladapter.Fill(_dtbl)
            Return _dtbl
        Catch ex1 As System.Data.SqlClient.SqlException
            Throw New Exception(ex1.Message, ex1)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    Public Function ExecuteScalar() As Boolean
        Try
            Dim count As Integer
            If _SQLCommand.Connection.State = 0 Then _SQLCommand.Connection.Open()

            Dim res As Object = _SQLCommand.ExecuteScalar()
            count = Integer.Parse(if(res Is Nothing, 0, res))


            _SQLCommand.Connection.Close()
            If count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function
    Public Function ExecuteCommand() As Boolean
        Try
            If _SQLCommand.Connection.State = 0 Then _SQLCommand.Connection.Open()
            _SQLCommand.ExecuteNonQuery()
            _SQLCommand.Connection.Close()
            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    Private Sub getUpdateCommand(Optional ByVal conflictType As System.Data.ConflictOption = 1)
        '1 = compare,'2 = use timestamp,3= overwrite
        Try
            Dim cmdUpdate As New SqlClient.SqlCommandBuilder(_sqladapter)
            cmdUpdate.ConflictOption = conflictType
            _sqladapter.UpdateCommand = cmdUpdate.GetUpdateCommand


        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub
    Public Sub savedata(Optional ByVal conflictType As System.Data.ConflictOption = 1)
        Try
            If _sqladapter.UpdateCommand Is Nothing Then
                AddHandler _sqladapter.RowUpdated, New SqlClient.SqlRowUpdatedEventHandler(AddressOf OnRowUpdated)
            End If
            getUpdateCommand(conflictType)
            _sqladapter.Update(_dtbl)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub
    Public Function FilldataSet() As DataSet
        Try

            _sqladapter = New SqlClient.SqlDataAdapter
            _sqladapter.SelectCommand = _SQLCommand
            _ds = New DataSet
            _sqladapter.Fill(_ds)
            Return _ds
        Catch ex1 As System.Data.SqlClient.SqlException
            Throw New Exception(ex1.Message, ex1)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    'Private Sub ConcurrencyError()
    '    Try
    '        If Windows.Forms.MessageBox.Show("Another user has recently altered the record!" & vbNewLine & vbNewLine & "Would you like to overwrite these changes?", "Concurrency Error", Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIcon.Question) = MsgBoxResult.Yes Then
    '            getUpdateCommand(ConflictOption.OverwriteChanges)
    '            savedata()
    '        Else
    '            _dtbl.RejectChanges()
    '        End If
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    End Try

    'End Sub
    Private Sub OnRowUpdated(ByVal sender As Object, ByVal args As SqlClient.SqlRowUpdatedEventArgs)
        Try

            If args.RecordsAffected = 0 Then
                If args.Errors.GetType().Name = "DBConcurrencyException" Then
                    args.Status = UpdateStatus.Continue
                    'ConcurrencyError()
                Else
                    Throw New Exception(args.Errors.Message)
                End If
            Else
                If args.StatementType = 1 Then
                    If _IdentityKey <> "" Then
                        args.Row(_IdentityKey) = GetIdentity(args.Command.Connection)
                    End If
                End If
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub
    Private Function GetIdentity(ByRef cnn As SqlClient.SqlConnection) As Integer
        Try

            Dim oCmd As New SqlClient.SqlCommand("SELECT ident_current('" & _TableName & "')", cnn)
            Dim x As Object = oCmd.ExecuteScalar()
            Return CInt(x)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function
End Class


