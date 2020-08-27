Public Class Role
    Inherits System.Web.UI.Page
    Private BI As New IMIS_BI.RoleRightBI
    Private eRole As New IMIS_EN.tblRole
    Private imisGen As New IMIS_Gen
    Private dtRights As DataTable = Nothing


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RunPageSecurity()

        If HttpContext.Current.Request.QueryString("r") IsNot Nothing Then
            eRole.RoleUUID = Guid.Parse(HttpContext.Current.Request.QueryString("r"))
            eRole.RoleID = BI.GetRoleIdByUUID(eRole.RoleUUID)
        End If

        If Not IsPostBack Then

            If eRole.RoleID > 0 Then
                Populate()
            End If

        End If
        If Request.QueryString("action") = "duplicate" Then
            If Not IsPostBack Then
                eRole.RoleID = 0
                txtRoles.Text = ""
                txtAltLanguage.Text = ""
                eRole.IsSystem = 0
                chkIsSystem.Checked = False
            Else
                eRole.RoleID = 0
            End If

        End If

        tvRoleRights.ExpandAll()
        tvRoleRights2.ExpandAll()
        tvRoleRights3.ExpandAll()
        tvRoleRights4.ExpandAll()
        If eRole.IsSystem Or eRole.ValidityTo IsNot Nothing Then
            txtRoles.Enabled = False
            txtAltLanguage.Enabled = False
            chkIsSystem.Enabled = False
            If eRole.ValidityTo IsNot Nothing Then
                chkIsBlocked.Enabled = False
                B_SAVE.Visible = False
            Else
                B_SAVE.Visible = True
            End If

            tvRoleRights.Enabled = False
            tvRoleRights2.Enabled = False
            tvRoleRights3.Enabled = False
            tvRoleRights4.Enabled = False
        Else
            tvRoleRights.Attributes.Add("onclick", "fireCheckChanged()")
            tvRoleRights2.Attributes.Add("onclick", "fireCheckChanged()")
            tvRoleRights3.Attributes.Add("onclick", "fireCheckChanged()")
            tvRoleRights4.Attributes.Add("onclick", "fireCheckChanged()")
        End If
    End Sub
    Private Sub RunPageSecurity()
        Dim RefUrl = Request.Headers("Referer")
        'Dim RoleID As Integer = imisGen.getRoleId(Session("User"))
        Dim UserID As Integer = imisGen.getUserId(Session("User"))
        If BI.RunPageSecurity(IMIS_EN.Enums.Pages.Role, Page) Then
            Dim Add As Boolean = BI.checkRights(IMIS_EN.Enums.Rights.AddUserProfile, UserID)
            Dim Edit As Boolean = BI.checkRights(IMIS_EN.Enums.Rights.EditUserProfile, UserID)

            If Not Add And Not Edit Then
                B_SAVE.Visible = False

            End If
        Else
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.User.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub

    Private Sub Populate()

        Dim ds As DataSet = BI.GetRoleRights(eRole.RoleID)
        Dim dtRole As DataTable = ds.Tables(0)
        Dim dtCol1 As DataTable = ds.Tables(1)
        Dim dtCol2 As DataTable = ds.Tables(2)
        Dim dtCol3 As DataTable = ds.Tables(3)
        Dim dtCol4 As DataTable = ds.Tables(4)


        eRole.RoleName = dtRole.Rows(0)("RoleName")
        eRole.IsSystem = dtRole.Rows(0)("IsSystem")
        hfSystemCode.Value = eRole.IsSystem
        eRole.IsBlocked = dtRole.Rows(0)("IsBlocked")
        eRole.AltLanguage = dtRole.Rows(0)("AltLanguage").ToString
        If dtRole.Rows(0)("ValidityTo") IsNot DBNull.Value Then
            eRole.ValidityTo = dtRole.Rows(0)("ValidityTo")
        End If
    
        txtRoles.Text = eRole.RoleName
        txtAltLanguage.Text = eRole.AltLanguage
        chkIsBlocked.Checked = eRole.IsBlocked
        chkIsSystem.Checked = eRole.IsSystem

        For Each row As DataRow In dtCol1.Rows
            Dim tnode As New TreeNode
            Dim childnode As New TreeNode
            'Nodes are represented by a value greater then 100000, This allows us to use up to 3 level
            'such that the first level is the first 2 digits, then second level is the 2nd two digits and the 3rd level is the lAR 2 digits
            tnode = tvRoleRights.FindNode(Left(row("RightID"), 2) & "0000")
            If tnode IsNot Nothing Then
                tnode.Checked = True
                Searchnode(row("RightID"), tnode.ChildNodes)
            End If
        Next
        For Each row As DataRow In dtCol2.Rows
            Dim tnode As New TreeNode
            Dim childnode As New TreeNode
            'Nodes are represented by a value greater then 100000, This allows us to use up to 3 level
            'such that the first level is the first 2 digits, then second level is the 2nd two digits and the 3rd level is the lAR 2 digits
            tnode = tvRoleRights2.FindNode(Left(row("RightID"), 2) & "0000")
            If tnode IsNot Nothing Then
                tnode.Checked = True
                Searchnode(row("RightID"), tnode.ChildNodes)
            End If
        Next
        For Each row As DataRow In dtCol3.Rows
            Dim tnode As New TreeNode
            Dim childnode As New TreeNode
            'Nodes are represented by a value greater then 100000, This allows us to use up to 3 level
            'such that the first level is the first 2 digits, then second level is the 2nd two digits and the 3rd level is the lAR 2 digits
            tnode = tvRoleRights3.FindNode(Left(row("RightID"), 2) & "0000")
            If tnode IsNot Nothing Then
                tnode.Checked = True
                Searchnode(row("RightID"), tnode.ChildNodes)
            End If
        Next
        For Each row As DataRow In dtCol4.Rows
            Dim tnode As New TreeNode
            Dim childnode As New TreeNode
            'Nodes are represented by a value greater then 100000, This allows us to use up to 3 level
            'such that the first level is the first 2 digits, then second level is the 2nd two digits and the 3rd level is the lAR 2 digits
            tnode = tvRoleRights4.FindNode(Left(row("RightID"), 2) & "0000")
            If tnode IsNot Nothing Then
                tnode.Checked = True
                Searchnode(row("RightID"), tnode.ChildNodes)
            End If
        Next

    End Sub
    Private Sub Searchnode(ByVal nodetext As String, ByVal tn As TreeNodeCollection)

        For Each node As TreeNode In tn
            If node.ChildNodes.Count > 0 Then
                If Left(node.Value, 4) = Left(nodetext, 4) Then
                    node.Checked = True
                    Searchnode(nodetext, node.ChildNodes)
                End If
            Else
                If node.Value = nodetext Then
                    node.Checked = True
                    Exit For
                End If
            End If

        Next

    End Sub
    Private Function SaveRights()
        dtRights = New DataTable

        Dim dr As DataRow = Nothing
        dtRights.Columns.Add("RightID")
        dtRights.Columns.Add("Description")

        eRole.RoleName = txtRoles.Text
        eRole.AltLanguage = txtAltLanguage.Text
        eRole.IsSystem = chkIsSystem.Checked
        eRole.IsBlocked = chkIsBlocked.Checked



        For Each tnLevel1 As TreeNode In tvRoleRights.Nodes
            LoadNodes(tnLevel1.ChildNodes)
        Next
        For Each tnLevel1 As TreeNode In tvRoleRights2.Nodes
            LoadNodes(tnLevel1.ChildNodes)
        Next
        For Each tnLevel1 As TreeNode In tvRoleRights3.Nodes
            LoadNodes(tnLevel1.ChildNodes)
        Next
        For Each tnLevel1 As TreeNode In tvRoleRights4.Nodes
            LoadNodes(tnLevel1.ChildNodes)
        Next
        eRole.AuditUserID = imisGen.getUserId(Session("User"))
        If dtRights.Rows.Count = 0 Then
            Dim msg As String = imisGen.getMessage("M_NORIGHTSSELECTED")
            imisGen.Alert(msg, pnlButtons, alertPopupTitle:="IMIS")
            Return False

        End If


        BI.SaveRights(dtRights, eRole)
        Return True
    End Function

    Private Sub LoadNodes(ByVal tnc As TreeNodeCollection)
        For Each tn As TreeNode In tnc
            If tn.ChildNodes.Count > 0 Then
                LoadNodes(tn.ChildNodes)
            Else
                If tn.Checked Then
                    Dim dr As DataRow = dtRights.NewRow
                    dr("rightid") = tn.Value
                    dr("Description") = tn.Text
                    dtRights.Rows.Add(dr)
                End If

            End If
        Next
    End Sub

    Private Sub B_SAVE_Click(sender As Object, e As EventArgs) Handles B_SAVE.Click
        If eRole.RoleID = 0 And BI.IsRoleNameUnique(txtRoles.Text) Then
            imisGen.Alert(imisGen.getMessage("M_UNIQUEROLENAME"), pnlHeader, alertPopupTitle:="IMIS")
            Return
        End If

        Try
            If SaveRights() = False Then Exit Sub
            Session("msg") = imisGen.getMessage("M_SAVED")
        Catch ex As Exception
            imisGen.Alert(imisGen.getMessage("M_ERRORMESSAGE"), pnlHeader, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try

        Response.Redirect("FindProfile.aspx?r=" & eRole.RoleUUID.ToString())

    End Sub

    Protected Sub B_CANCEL_Click(sender As Object, e As EventArgs) Handles B_CANCEL.Click
        Response.Redirect("FindProfile.aspx?r=" & eRole.RoleUUID.ToString())
    End Sub
End Class