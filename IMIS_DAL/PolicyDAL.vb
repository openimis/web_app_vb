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

Public Class PolicyDAL
    Dim data As New ExactSQL

    Public Sub UpdatePolicy(ByVal ePolicy As IMIS_EN.tblPolicy)

        Dim str As String = "INSERT INTO tblPolicy (FamilyID, EnrollDate, StartDate, EffectiveDate, ExpiryDate, ProdID, OfficerID,PolicyStage,PolicyStatus,PolicyValue,isOffline, ValidityTo, LegacyID, AuditUserID,RenewalOrder)" _
           & "SELECT FamilyID, EnrollDate, StartDate, EffectiveDate, ExpiryDate, ProdID, OfficerID,PolicyStage,PolicyStatus,PolicyValue,isOffline, GetDate(), @PolicyID, AuditUserID,RenewalOrder from tblPolicy where PolicyID = @PolicyID;"
        If Not ePolicy.StartDate = Nothing And (ePolicy.PolicyStatus Is Nothing) Then 'When on policy page
            str += " UPDATE tblPolicy set FamilyID=@FamilyID, EnrollDate=@EnrollDate, StartDate=@StartDate, EffectiveDate=@EffectiveDate, ExpiryDate=@ExpiryDate, ProdID=@ProdID, OfficerID=@OfficerID,PolicyStage = @PolicyStage "
        ElseIf ePolicy.PolicyStatus = 2 Then 'When on premium page ( enforcing policy to active on prompt/premium matches policy value
            str += " UPDATE tblPolicy set PolicyStatus=@PolicyStatus ,EffectiveDate=@EffectiveDate " ',ExpiryDate=@ExpiryDate  "
        ElseIf ePolicy.PolicyStatus = 4 Then
            str += " UPDATE tblPolicy set PolicyStatus=@PolicyStatus"
        ElseIf Not ePolicy.RenewalOrder = -1 Then
            str += " UPDATE tblPolicy set RenewalOrder = @RenewalOrder"
        ElseIf ePolicy.PolicyStatus = 16 Then
            str += " UPDATE tblPolicy set PolicyStatus=@PolicyStatus ,EffectiveDate=@EffectiveDate "
        ElseIf Not ePolicy.PolicyValue Is Nothing And (ePolicy.PolicyStatus Is Nothing Or ePolicy.PolicyStatus <> 4) Then 'When on overviewfamily page, policy value have changed
            str += " UPDATE tblPolicy set PolicyValue=@PolicyValue"
        End If

        'AMANI 06/12---update  only for ONLINE policies
        If ePolicy.isOffline = False Then
            str += ",isOffline=@isOffline"
        End If

        str += ",ValidityFrom=GetDate(), AuditUserID = @AuditUserID where PolicyID=@PolicyID"

        data.setSQLCommand(str, CommandType.Text)
        data.params("@PolicyID", SqlDbType.Int, ePolicy.PolicyID)
        data.params("@RenewalOrder", SqlDbType.Int, ePolicy.RenewalOrder)
        If Not ePolicy.StartDate = Nothing And ePolicy.PolicyStatus Is Nothing Then
            data.params("@FamilyId", SqlDbType.Int, ePolicy.tblFamilies.FamilyID)
            data.params("@EnrollDate", SqlDbType.Date, ePolicy.EnrollDate)
            data.params("@StartDate", SqlDbType.Date, ePolicy.StartDate)
            data.params("@OfficerID", SqlDbType.Int, ePolicy.tblOfficer.OfficerID)
            data.params("@EffectiveDate", SqlDbType.Date, ePolicy.EffectiveDate)
            data.params("@ExpiryDate", SqlDbType.Date, ePolicy.ExpiryDate)
            data.params("@ProdID", SqlDbType.Int, ePolicy.tblProduct.ProdID)
            data.params("@PolicyStage", SqlDbType.Char, 1, ePolicy.PolicyStage)
        ElseIf ePolicy.PolicyStatus = 2 Then
            data.params("@EffectiveDate", SqlDbType.Date, ePolicy.EffectiveDate)
            'data.params("@ExpiryDate", SqlDbType.Date, ePolicy.ExpiryDate)
            data.params("@PolicyStatus", SqlDbType.Int, ePolicy.PolicyStatus)
        ElseIf Not ePolicy.PolicyValue Is Nothing And ePolicy.PolicyStatus Is Nothing Then
            data.params("@PolicyValue", SqlDbType.Decimal, ePolicy.PolicyValue)
            'data.params("@PolicyStatus", SqlDbType.Int, ePolicy.PolicyStatus)
        ElseIf ePolicy.PolicyStatus = 4 Then  'When suspending policy from premium page
            data.params("@PolicyStatus", SqlDbType.Int, ePolicy.PolicyStatus)
        ElseIf ePolicy.PolicyStatus = 16 Then
            data.params("@EffectiveDate", SqlDbType.Date, ePolicy.EffectiveDate)
            data.params("@PolicyStatus", SqlDbType.Int, ePolicy.PolicyStatus)
        End If
        data.params("@isOffline", SqlDbType.Bit, ePolicy.isOffline)


        data.params("@AuditUserID", SqlDbType.Int, ePolicy.AuditUserID)

        data.ExecuteCommand()
    End Sub

    Public Sub LoadPolicy(ByRef ePolicy As IMIS_EN.tblPolicy, ByRef PremiumPaid As Decimal)
        Dim eProd As New IMIS_EN.tblProduct
        Dim eOfficer As New IMIS_EN.tblOfficer
        Dim eFamily As New IMIS_EN.tblFamilies


        data.setSQLCommand("select pd.InsurancePeriod,FamilyID,EnrollDate,StartDate,ExpiryDate,EffectiveDate,PolicyStatus,isnull(PolicyValue,0) PolicyValue,tblPolicy.ProdID,OfficerID,PolicyStage,isnull(PremiumPaid,0) PremiumPaid," &
                           "tblPolicy.isOffline,tblPolicy.ValidityTo from tblPolicy left join (select MAX(policyID) policyID, SUM(Amount) PremiumPaid " &
                            "from tblPremium where PolicyID=@PolicyID and ValidityTo is null and isPhotoFee = 0) Premium on Premium.policyID = tblPolicy.PolicyID " &
                            " INNER JOIN tblProduct pd ON pd.ProdID = tblPolicy.ProdID" &
                            " where tblpolicy.PolicyID = @PolicyID", CommandType.Text)
        data.params("@PolicyID", SqlDbType.Int, ePolicy.PolicyID)
        Dim dr As DataRow = data.Filldata()(0)
        If Not dr Is Nothing Then
            eFamily.FamilyID = dr("FamilyID")
            ePolicy.EnrollDate = dr("EnrollDate")
            ePolicy.StartDate = dr("StartDate")
            ePolicy.PolicyStatus = dr("PolicyStatus")
            ePolicy.PolicyValue = dr("PolicyValue")
            eProd.ProdID = dr("ProdID")
            eOfficer.OfficerID = If(dr("OfficerID") Is DBNull.Value, 0, dr("OfficerID"))
            ePolicy.PolicyStage = dr("PolicyStage")
            PremiumPaid = dr("PremiumPaid")
            If Not dr("EffectiveDate") Is DBNull.Value Then
                ePolicy.EffectiveDate = dr("EffectiveDate")
            End If

            If Not dr("ExpiryDate") Is DBNull.Value Then
                ePolicy.ExpiryDate = dr("ExpiryDate")
            End If

            If Not dr("ValidityTo") Is DBNull.Value Then
                ePolicy.ValidityTo = dr("ValidityTo").ToString
            End If


            If Not dr("InsurancePeriod") Is DBNull.Value Then
                eProd.InsurancePeriod = dr("InsurancePeriod").ToString
            End If
            If dr("isOffline") IsNot DBNull.Value Then
                ePolicy.isOffline = dr("isOffline")
            End If
            ePolicy.tblOfficer = eOfficer
            ePolicy.tblFamilies = eFamily
            ePolicy.tblProduct = eProd
        End If
    End Sub
    Public Sub getPolicyValue(ByRef ePolicy As IMIS_EN.tblPolicy, PreviousPolicyId As Integer)
        data.setSQLCommand("uspPolicyValue", CommandType.StoredProcedure)
        data.params("@FamilyId", SqlDbType.Int, ePolicy.tblFamilies.FamilyID)
        data.params("@ProdId", SqlDbType.Int, ePolicy.tblProduct.ProdID)
        data.params("@PolicyId", SqlDbType.Int, ePolicy.PolicyID)
        data.params("@PolicyStage", SqlDbType.Char, 1, ePolicy.PolicyStage)
        data.params("@Enrolldate", SqlDbType.Date, ePolicy.EnrollDate)
        data.params("@PreviousPolicyId", SqlDbType.Int, PreviousPolicyId)
        data.params("@ErrorCode", SqlDbType.Int, 0, ParameterDirection.Output)
        ePolicy.PolicyValue = CDec(data.Filldata.Rows(0)(0))
    End Sub

    Public Function getPolicyValue(ByVal FamilyId As Integer, ByVal ProdId As Integer, ByVal PolicyId As Integer, ByVal PolicyStage As String, ByVal EnrollDate As String, ByVal PreviousPolicyId As Integer) As Double
        Dim sSQL As String = "uspPolicyValue"
        data.setSQLCommand(sSQL, CommandType.StoredProcedure)
        data.params("@FamilyId", FamilyId)
        data.params("@ProdId", ProdId)
        data.params("@PolicyId", PolicyId)
        data.params("@PolicyStage", PolicyStage)
        data.params("@EnrollDate", Date.ParseExact(EnrollDate, "dd/MM/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo))
        data.params("@PreviousPolicyId", PreviousPolicyId)
        Dim dt As DataTable = data.Filldata
        Return dt.Rows(0)("PolicyValue")
    End Function
    Public Sub InsertPolicy(ByVal ePolicy As IMIS_EN.tblPolicy)
        Dim data As New ExactSQL
        Dim sSQL As String = "INSERT INTO tblPolicy(FamilyID, EnrollDate, StartDate, EffectiveDate, ExpiryDate, ProdID, OfficerID,PolicyStage,isOffline, AuditUserID,RenewalOrder)" &
            " VALUES (@FamilyID, @EnrollDate, @StartDate, @EffectiveDate, @ExpiryDate, @ProdID, @OfficerID,@PolicyStage,@isOffline, @AuditUserID, @RenewalOrder); select @PolicyID = scope_identity()"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@PolicyID", SqlDbType.Int, ePolicy.PolicyID, ParameterDirection.Output)
        data.params("@FamilyId", SqlDbType.Int, ePolicy.tblFamilies.FamilyID)
        data.params("@EnrollDate", SqlDbType.SmallDateTime, ePolicy.EnrollDate)
        data.params("@StartDate", SqlDbType.SmallDateTime, ePolicy.StartDate)
        data.params("@EffectiveDate", SqlDbType.SmallDateTime, Nothing, ParameterDirection.Input) ' if(ePolicy.EffectiveDate Is Nothing, SqlTypes.SqlDateTime.Null, ePolicy.EffectiveDate))
        data.params("@ExpiryDate", SqlDbType.SmallDateTime, if(ePolicy.ExpiryDate Is Nothing, SqlTypes.SqlDateTime.Null, ePolicy.ExpiryDate))
        data.params("@ProdID", SqlDbType.Int, ePolicy.tblProduct.ProdID)
        data.params("@OfficerID", SqlDbType.Int, ePolicy.tblOfficer.OfficerID)
        data.params("@PolicyStage", SqlDbType.Char, 1, ePolicy.PolicyStage)
        data.params("@isOffline", SqlDbType.Bit, ePolicy.isOffline)
        data.params("@AuditUserID", SqlDbType.Int, ePolicy.AuditUserID)
        data.params("@RenewalOrder", SqlDbType.Int, ePolicy.RenewalOrder)
        data.ExecuteCommand()
        ePolicy.PolicyID = data.sqlParameters("@PolicyID")
        'getPolicyValue(ePolicy)
        InsertPolicyValue(ePolicy)
    End Sub
    Private Sub InsertPolicyValue(ByVal ePolicy As IMIS_EN.tblPolicy)
        Dim data As New ExactSQL
        Dim sSQL As String = "update tblPolicy set PolicyValue = @PolicyValue where policyid = @policyId"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@PolicyID", SqlDbType.Int, ePolicy.PolicyID)
        data.params("@PolicyValue", SqlDbType.Decimal, ePolicy.PolicyValue)
        data.ExecuteCommand()
    End Sub
    Public Function GetPolicybyFamily(ByVal FamilyId As Integer, ByVal status As DataTable) As DataTable


        data.setSQLCommand("SELECT PolicyId,PolicyUUID,EnrollDate,EffectiveDate,StartDate,ExpiryDate,Status.name As PolicyStatus ,PolicyValue,tblPolicy.isOffline,tblPolicy.ValidityFrom,tblPolicy.ValidityTo,tblPolicy.PolicyStage,ProductCode,LastName + ' ' + OtherNames OfficerName, tblPolicy.ProdID,tblPolicy.FamilyId,F.FamilyUUID, tblProduct.ProdUUID, tblPolicy.PolicyStatus as PolicyStatusID" &
                           " FROM tblPolicy INNER JOIN tblProduct ON tblPolicy.ProdID = tblProduct.ProdId" &
                           " INNER JOIN tblFamilies F On F.FamilyID = tblPolicy.FamilyID " &
                           " LEFT OUTER JOIN tblOfficer ON tblPolicy.OfficerId = tblOfficer.OfficerID" &
                            " INNER JOIN @Status Status ON Status.id = tblPolicy.PolicyStatus" &
                           " WHERE(tblPolicy.FamilyId = @FamilyId) AND tblPolicy.ValidityTo IS NULL ", CommandType.Text)
        data.params("@FamilyId", SqlDbType.Int, FamilyId)
        data.params("@Status", status, "xAttribute")
        Return data.Filldata
    End Function
    Public Function GetOfficers() As DataTable

        Dim sSQL As String = "SELECT OfficerID,LastName + ' ' + OtherNames OfficerName FROM tblOfficer ORDER BY LastName"
        data.setSQLCommand(sSQL, CommandType.Text)
        Return data.Filldata
    End Function
    Public Function GetProducts() As DataTable

        Dim sSQL As String = "SELECT ProdID, ProdUUID, ProductCode FROM tblProduct"
        data.setSQLCommand(sSQL, CommandType.Text)
        Return data.Filldata
    End Function
    Public Function FindInsureeByCHFIDGrid(ByVal CHFID As String) As DataTable
        Dim sSQL As String = "uspPolicyInquiry"
        data.setSQLCommand(sSQL, CommandType.StoredProcedure)
        data.params("@CHFID", SqlDbType.NVarChar, 12, CHFID)
        Return data.Filldata
    End Function
    Public Function GetPolicy(ByVal ePolicy As IMIS_EN.tblPolicy, ByVal All As Boolean, ByVal Status As DataTable, Optional ByVal DeactivatedPolicies As Boolean = False) As DataTable
        'sSQL = "SELECT TOP 20 tblPolicy.isOffline,tblPolicy.PolicyId, tblPolicy.FamilyId,EnrollDate,tblInsuree.LastName + ' ' + tblInsuree.OtherNames FamilyName,EffectiveDate,StartDate,ExpiryDate, status.name as PolicyStatus,tblPolicy.PolicyStage,PolicyValue,PolicyValue - isnull(PaidAmount,0) as Balance,ProductCode,tblOfficer.LastName + ' ' + tblOfficer.OtherNames OfficerName,tblPolicy.ValidityFrom,tblPolicy.ValidityTo FROM tblPolicy INNER JOIN tblInsuree ON tblPolicy.FamilyID = tblInsuree.FamilyID and ishead = 1 and tblInsuree.validityto is null INNER JOIN tblFamilies ON tblPolicy.FamilyID = tblFamilies.FamilyID inner join tblUsersDistricts UD on UD.DistrictID = tblFamilies.districtid and UD.userid = @userid and UD.ValidityTo is null INNER JOIN tblProduct ON tblPolicy.ProdID = tblProduct.ProdID  INNER JOIN tblOfficer ON tblPolicy.OfficerID = tblOfficer.OfficerID  left join (select policyid, sum(Amount) as PaidAmount from tblpremium where ValidityTo is null and isPhotoFee = 0 group by policyid) Premiums on tblPolicy.PolicyID = premiums.policyid  Inner join @Status status on status.ID = tblpolicy.Policystatus "
        Dim sSQL As String

        sSQL = " SELECT PL.isOffline, PL.PolicyId, PL.PolicyUUID, F.FamilyId, F.FamilyUUID,  PL.EnrollDate, I.LastName + ' ' + I.OtherNames FamilyName, PL.EffectiveDate,"
        sSQL += " PL.StartDate, PL.ExpiryDate,PS.Name PolicyStatus, PL.PolicyStage, PL.PolicyValue, PL.PolicyValue - ISNULL(SUM(PR.Amount), 0) Balance,"
        sSQL += " Prod.ProductCode, O.Lastname + ' ' + O.OtherNames OfficerName, PL.ValidityFrom, PL.ValidityTo"
        sSQL += " FROM tblPolicy  PL"
        sSQL += " INNER JOIN tblFamilies F ON PL.FamilyId = F.FamilyId"
        sSQL += " INNER JOIN tblInsuree I ON F.InsureeId = I.InsureeId"
        sSQL += " INNER JOIN tblProduct Prod ON Prod.ProdId = PL.ProdId"
        sSQL += " INNER JOIN tblOfficer O ON O.OfficerId = PL.OfficerId"
        sSQL += " INNER JOIN tblVillages V ON V.VillageId = F.LocationId"
        sSQL += " INNER JOIN tblWards W ON W.WardId = V.WardId"
        sSQL += " INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId"
        sSQL += " INNER JOIN tblRegions R  ON R.RegionId = D.Region"
        sSQL += " INNER JOIN tblUsersDistricts UD ON UD.LocationId = D.DistrictId AND UD.UserId = @UserId AND UD.ValidityTo IS NULL"
        sSQL += " LEFT OUTER JOIN tblPremium PR ON PR.PolicyId = PL.PolicyId AND PR.isPhotoFee = 0"
        sSQL += " INNER JOIN @Status PS ON PS.Id = PL.PolicyStatus"
       

        If DeactivatedPolicies = True Then
            sSQL += "Select Case PL.PolicyID"
            sSQL += "From tblInsuree I"
            sSQL += "INNER Join tblPolicy PL ON I.FamilyId = PL.FamilyID"
            sSQL += "Left OUTER JOIN tblInsureePolicy IP ON PL.PolicyId = IP.PolicyId And I.InsureeId = IP.InsureeId"
            sSQL += "WHERE PL.ValidityTo Is NULL"
            sSQL += " And I.ValidityTo Is NULL"
            sSQL += "And IP.ValidityTo Is NULL"
            sSQL += " And PL.PolicyStatus > 1"
            sSQL += "And IP.EffectiveDate Is NULL"
            sSQL += "Group BY PL.PolicyId"

            sSQL += ")InActivePolicies ON PL.PolicyID = InActivePolicies.PolicyId"
        End If

        sSQL += " WHERE F.ValidityTo IS NULL"
        sSQL += " AND PR.ValidityTo IS NULL"
        sSQL += " AND (D.DistrictId = @DistrictId OR @DistrictId = 0)"
        sSQL += " AND (PS.ID = @PolicyStatus OR @PolicyStatus = 0)"
        sSQL += " AND (O.OfficerId = @OfficerId  OR @OfficerId = 0)"
        sSQL += " AND (Prod.ProdId = @ProdId OR @ProdId = 0)"
      
        'sSQL += " WHERE case when @ProdId = 0 then @ProdId else tblPolicy.ProdId end = @ProdId "
        If ePolicy.StartDateFrom IsNot Nothing Then
            sSQL += " AND PL.StartDate >= @StartDateFrom   "
        End If
        If ePolicy.StartDateTo IsNot Nothing Then
            sSQL += " AND PL.StartDate <= @StartDateTo   "
        End If

        If ePolicy.ExpiryDateFrom IsNot Nothing Then
            sSQL += " AND  PL.ExpiryDate  >= @ExpiryDateFrom "
        End If
        If ePolicy.ExpiryDateTo IsNot Nothing Then
            sSQL += " AND  PL.ExpiryDate  <= @ExpiryDateTo "
        End If

        If ePolicy.EnrollDateFrom IsNot Nothing Then
            sSQL += " AND PL.EnrollDate >= @EnrollDateFrom "
        End If
        If ePolicy.EnrollDateTo IsNot Nothing Then
            sSQL += " AND PL.EnrollDate <= @EnrollDateTo "
        End If

        If ePolicy.EffectiveDateFrom IsNot Nothing Then
            sSQL += " AND PL.EffectiveDate >= @EffectiveDateFrom  "
        End If
        If ePolicy.EffectiveDateTo IsNot Nothing Then
            sSQL += " AND PL.EffectiveDate <= @EffectiveDateTo  "
        End If

        'sSQL += " AND case when @officerId = 0 then @officerId else tblPolicy.officerId end = @officerId"

        If ePolicy.PolicyStage IsNot Nothing Then
            sSQL += " AND PL.PolicyStage = @PolicyStage "
        End If
        If Not ePolicy.tblFamilies.RegionId = 0 Then
            sSQL += " AND R.RegionId = @RegionId"
        End If
        If Not ePolicy.tblFamilies.DistrictID = 0 Then
            sSQL += " and D.DistrictID = @DistrictID"
        End If
        If Not ePolicy.PolicyStatus = 0 Then
            sSQL = sSQL & " and PL.PolicyStatus = @PolicyStatus"
        End If
        If All = False Then
            sSQL = sSQL & " and PL.ValidityTo is null"
        End If
        If ePolicy.isOffline IsNot Nothing Then
            If ePolicy.isOffline Then
                sSQL += " and PL.isOffline = 1"
            End If
        End If


        sSQL += " GROUP BY PL.isOffline, PL.PolicyId, F.FamilyId, PL.EnrollDate, I.LastName, I.OtherNames, PL.EffectiveDate,"
        sSQL += " PL.StartDate, PL.ExpiryDate, PL.PolicyStage, PL.PolicyValue, Prod.ProductCode, O.OtherNames, O.LastName,"
        sSQL += " PL.ValidityFrom, PL.ValidityTo,PS.Name, PL.PolicyUUID, F.FamilyUUID"
        If Not ePolicy.PolicyValue Is Nothing Then
            If ePolicy.PolicyValue = 0 Then
                'sSQL += " and (PolicyValue - isnull(PaidAmount,0)) > @PolicyValue"
                sSQL += " HAVING  PL.PolicyValue - ISNULL(SUM(PR.Amount), 0) > @PolicyValue"
            Else
                ' sSQL += " and (PolicyValue - isnull(PaidAmount,0)) >= @PolicyValue"
                sSQL += " HAVING  PL.PolicyValue - ISNULL(SUM(PR.Amount), 0) >= @PolicyValue"
            End If

        End If

       
        sSQL += " order by EnrollDate desc, F.familyid ,PL.ValidityFrom desc ,PL.ValidityTo desc"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@UserId", SqlDbType.Int, ePolicy.AuditUserID)
        data.params("@RegionId", SqlDbType.Int, ePolicy.tblFamilies.RegionId)
        data.params("@DistrictID", SqlDbType.Int, ePolicy.tblFamilies.DistrictID)

        data.params("@EnrollDateFrom", SqlDbType.Date, ePolicy.EnrollDateFrom)
        data.params("@ExpiryDateFrom", SqlDbType.Date, ePolicy.ExpiryDateFrom)
        data.params("@StartDateFrom", SqlDbType.Date, ePolicy.StartDateFrom)
        data.params("@EffectiveDateFrom", SqlDbType.Date, ePolicy.EffectiveDateFrom)

        data.params("@EnrollDateTo", SqlDbType.Date, ePolicy.EnrollDateTo)
        data.params("@ExpiryDateTo", SqlDbType.Date, ePolicy.ExpiryDateTo)
        data.params("@StartDateTo", SqlDbType.Date, ePolicy.StartDateTo)
        data.params("@EffectiveDateTo", SqlDbType.Date, ePolicy.EffectiveDateTo)

        data.params("@PolicyStatus", SqlDbType.TinyInt, ePolicy.PolicyStatus)
        data.params("@PolicyValue", SqlDbType.Decimal, ePolicy.PolicyValue)
        data.params("@OfficerId", SqlDbType.Int, ePolicy.tblOfficer.OfficerID)
        data.params("@ProdId", SqlDbType.Int, ePolicy.tblProduct.ProdID)
        data.params("@Status", Status, "xAttribute")
        If ePolicy.PolicyStage IsNot Nothing Then
            data.params("@PolicyStage", SqlDbType.Char, 1, ePolicy.PolicyStage)
        End If
        Return data.Filldata

    End Function
    Public Function CheckIfPolicyIDExists(ByVal PolicyID As Integer) As DataTable

        Dim data As New ExactSQL
        Dim strSQL As String = "Select * from tblPolicy where PolicyID = @PolicyID AND LegacyID IS NULL AND ValidityTo IS NULL"

        data.setSQLCommand(strSQL, CommandType.Text)

        data.params("@PolicyID", SqlDbType.Int, PolicyID)

        Return data.Filldata
    End Function
    Public Function CheckCanBeDeleted(ByVal PolicyID As Integer) As DataTable
        Dim str As String = "SELECT po.* FROM tblPolicy po INNER JOIN tblPremium pr ON po.PolicyID=pr.PolicyID WHERE po.PolicyID=@PolicyID AND pr.ValidityTo IS NULL AND pr.LegacyID IS NULL"

        data.setSQLCommand(str, CommandType.Text)
        data.params("@PolicyID", SqlDbType.Int, PolicyID)
        Return data.Filldata()
    End Function
    Public Function DeletePolicy(ByVal ePolicy As IMIS_EN.tblPolicy) As Boolean
        Dim str As String = "INSERT INTO tblPolicy (FamilyID, EnrollDate, StartDate, EffectiveDate, ExpiryDate, ProdID, OfficerID,PolicyStatus,PolicyValue,isOffline, ValidityTo, LegacyID, AuditUserID)" _
                            & " SELECT FamilyID, EnrollDate, StartDate, EffectiveDate, ExpiryDate, ProdID, OfficerID,PolicyStatus,PolicyValue,isOffline, GetDate(), @PolicyID, AuditUserID from tblPolicy where PolicyID = @PolicyID AND ValidityTo IS NULL;" _
                            & " UPDATE tblPolicy set ValidityFrom=GetDate(), ValidityTo=GetDate(), AuditUserID = @AuditUserID where PolicyID=@PolicyID AND ValidityTo IS NULL"


        data.setSQLCommand(str, CommandType.Text)
        data.params("@PolicyID", SqlDbType.Int, ePolicy.PolicyID)
        data.params("@AuditUserID", SqlDbType.Int, ePolicy.AuditUserID)
        data.ExecuteCommand()
        Return True
    End Function
    Public Function GetPolicyOfflineValue(ByVal PolicyID As Integer) As Boolean
        Dim Query As String = "SELECT isnull(isOffline,0) isOffline FROM tblPOlicy where policyid=@policyid"
        data.setSQLCommand(Query, CommandType.Text)
        data.params("@policyid", SqlDbType.Int, PolicyID)
        Return CBool(data.Filldata().Rows(0)("isOffline"))
    End Function
    Public Function isExceededAdherents(ByVal ProdId As Integer, ByVal FamilyId As Integer) As Boolean
        Dim sSQL As String = ""
        sSQL = "IF EXISTS(SELECT 1 FROM (SELECT MemberCount FROM tblProduct Prod WHERE Prod.ValidityTo IS NULL AND Prod.ProdID = @ProdId)MemberCount, (SELECT COUNT(InsureeId)TotalMembers FROM tblInsuree WHERE FamilyID = @FamilyId AND ValidityTo IS NULL)TotalMembers WHERE MemberCount < TotalMembers AND MemberCount <> 0) SELECT 1 isExceeded ELSE SELECT 0 isExceeded"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@ProdId", SqlDbType.Int, ProdId)
        data.params("@FamilyId", SqlDbType.Int, FamilyId)

        If data.Filldata().Rows(0)(0) = 1 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetLastRenewalDate(ByVal PolicyID As Integer) As Nullable(Of Date)
        Dim Query As String = "SELECT DATEADD(MONTH,Prod.GracePeriodRenewal,DATEADD(DAY,1,PL.ExpiryDate))LastRenewalDate" & _
                " FROM tblPolicy PL INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID WHERE PL.PolicyID = @PolicyId"
        data.setSQLCommand(Query, CommandType.Text)
        data.params("@PolicyId", SqlDbType.Int, PolicyID)
        Dim dt As DataTable = data.Filldata
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("LastRenewalDate") IsNot DBNull.Value Then Return dt.Rows(0)("LastRenewalDate")
        End If
        Return Nothing
    End Function

    Public Function GetPolicyIdByUUID(ByVal uuid As Guid) As DataTable
        Dim sSQL As String = ""
        Dim data As New ExactSQL

        sSQL = "select PolicyID from tblPolicy where PolicyUUID = @PolicyUUID"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@PolicyUUID", SqlDbType.UniqueIdentifier, uuid)

        Return data.Filldata
    End Function
    Public Function GetPolicyUUIDByID(ByVal id As Integer) As DataTable
        Dim sSQL As String = ""
        Dim data As New ExactSQL

        sSQL = "select PolicyUUID from tblPolicy where PolicyId = @PolicyId"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@PolicyId", SqlDbType.Int, id)

        Return data.Filldata
    End Function
    Public Function GetRenewalCount(ByVal ProdID As Integer, ByVal FamilyID As Integer) As Integer
        Dim sSQL As String = ""
        Dim data As New ExactSQL
        sSQL = "SELECT COUNT(PolicyStage) AS RenewalCount FROM tblPolicy WHERE FamilyID =@FamilyID AND ValidityTo IS NULL AND PolicyStage = 'R' AND ProdID = @ProductId"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@ProductId", ProdID)
        data.params("@FamilyID", FamilyID)
        Dim dt As DataTable = data.Filldata()
        Dim RenewalCount As Integer = 0
        If dt.Rows.Count > 0 Then
            If Not dt(0)("RenewalCount") Is DBNull.Value Then
                RenewalCount = dt(0)("RenewalCount")
            Else
                RenewalCount = 0
            End If
        End If
        Return RenewalCount
    End Function
End Class
