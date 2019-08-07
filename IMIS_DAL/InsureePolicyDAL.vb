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

Public Class InsureePolicyDAL

    Private sSQL As String = String.Empty
    Private data As New ExactSQL

    Public Sub InsertPolicyInsuree(ByVal PolicyId As Integer, Optional ByVal IsOffLine As Boolean = False)
        sSQL = " ;WITH IP AS"
        sSQL += " ("
        sSQL += " SELECT ROW_NUMBER() OVER(ORDER BY InsureeId)RNo,"
        sSQL += " Prod.MemberCount,  I.InsureeID,PL.PolicyID,PL.EnrollDate,PL.StartDate,PL.EffectiveDate,PL.ExpiryDate,PL.AuditUserID,I.isOffline"
        sSQL += " FROM tblInsuree I"
        sSQL += " INNER JOIN tblPolicy PL ON I.FamilyID = PL.FamilyID"
        sSQL += " INNER JOIN tblProduct Prod ON PL.ProdId = Prod.ProdID"
        sSQL += " WHERE(I.ValidityTo Is NULL)"
        sSQL += " AND PL.ValidityTo IS NULL"
        sSQL += " AND Prod.ValidityTo IS NULL"
        sSQL += " AND PL.PolicyID = @PolicyId"
        sSQL += " )"
        sSQL += " INSERT INTO tblInsureePolicy(InsureeId,PolicyId,EnrollmentDate,StartDate,EffectiveDate,ExpiryDate,AuditUserId,isOffline)"
        sSQL += " SELECT InsureeId, PolicyId, EnrollDate, StartDate, EffectiveDate, ExpiryDate, AuditUserId, @IsOffLine"
        sSQL += " FROM IP"
        sSQL += " WHERE RNo <= MemberCount;"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@PolicyId", SqlDbType.Int, PolicyId)
        data.params("@IsOffLine", SqlDbType.Int, IsOffLine)

        data.ExecuteCommand()

    End Sub

    Public Sub UpdatePolicyInsuree(ByVal PolicyId As Integer)
        sSQL = "UPDATE tblInsureePolicy SET EffectiveDate = PL.EffectiveDate,StartDate = PL.StartDate, ExpiryDate = PL.ExpiryDate FROM tblInsureePolicy I INNER JOIN tblPolicy PL ON I.PolicyId = PL.PolicyId WHERE I.ValidityTo IS NULL AND PL.ValidityTo IS NULL AND PL.PolicyId = @PolicyId"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@PolicyId", SqlDbType.Int, PolicyId)

        data.ExecuteCommand()

    End Sub
    Public Sub ActivateInsuree(ByVal PolicyId As Integer, ByVal EffectiveDate As Date)
        sSQL = "UPDATE tblInsureePolicy SET EffectiveDate = @EffectiveDate WHERE ValidityTo IS NULL AND EffectiveDate IS NULL AND PolicyId = @PolicyId"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@PolicyId", SqlDbType.Int, PolicyId)
        data.params("@EffectiveDate", SqlDbType.Date, EffectiveDate)

        data.ExecuteCommand()
    End Sub
    Public Sub DeletePolicyInsuree(ByVal InsureeId As Integer, ByVal PolicyId As Integer)
        sSQL = "UPDATE tblInsureePolicy SET ValidityTo = GETDATE() WHERE InsureeId = @InsureeId OR PolicyId = @PolicyId"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@InsureeId", SqlDbType.Int, InsureeId)
        data.params("@PolicyId", SqlDbType.Int, PolicyId)

        data.ExecuteCommand()
    End Sub
    Public Sub AddInsuree(ByVal InsureeId As Integer, ByVal Activate As Boolean)
        sSQL = "uspAddInsureePolicy"
        data.setSQLCommand(sSQL, CommandType.StoredProcedure)
        data.params("@InsureeId", SqlDbType.Int, InsureeId)
        data.params("@Activate", SqlDbType.Bit, Activate)
        data.ExecuteCommand()
    End Sub
End Class
