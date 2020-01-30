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

Public Class ClaimServicesDAL


    Public Sub InsertClaimServices(ByRef eClaimServices As IMIS_EN.tblClaimServices)
        Dim data As New ExactSQL

        data.setSQLCommand("Insert Into tblClaimServices ([ClaimID],[ServiceID],[QtyProvided],[PriceAsked],[Explanation],[ValidityFrom],[AuditUserID])" _
                           & "VALUES(@ClaimID,@ServiceID,@QtyProvided,@PriceAsked,@Explanation,getdate(),@AuditUserID)", CommandType.Text)

        data.params("@ClaimID", SqlDbType.Int, eClaimServices.tblClaim.ClaimID)
        data.params("@ServiceID", SqlDbType.Int, eClaimServices.tblServices.ServiceID)
        data.params("@QtyProvided", SqlDbType.Decimal, eClaimServices.QtyProvided)
        data.params("@PriceAsked", SqlDbType.Decimal, eClaimServices.PriceAsked)
        data.params("@Explanation", SqlDbType.NText, eClaimServices.Explanation, ParameterDirection.Input)
        data.params("@AuditUserID", SqlDbType.Int, eClaimServices.AuditUserID)

        data.ExecuteCommand()


    End Sub



    Public Sub UpdateClaimServices(ByRef eClaimServices As IMIS_EN.tblClaimServices)
        Dim data As New ExactSQL

        data.setSQLCommand("Insert Into tblClaimServices ([ClaimID],[ServiceID],[ProdID],[ClaimServiceStatus],[QtyProvided],[QtyApproved]," _
                           & "[PriceAsked],[PriceAdjusted],[PriceApproved],[Explanation],[Justification],[RejectionReason],[ValidityFrom],[ValidityTo],[LegacyID]," _
                           & "[AuditUserID],[ValidityFromReview],[ValidityToReview],[AuditUserIDReview])" _
                           & "SELECT [ClaimID],[ServiceID],[ProdID],[ClaimServiceStatus],[QtyProvided],[QtyApproved],[PriceAsked],[PriceAdjusted],[PriceApproved]" _
                           & ",[Explanation],[Justification],[RejectionReason],[ValidityFrom],getdate(),[ClaimServiceID],[AuditUserID],[ValidityFromReview]," _
                           & "[ValidityToReview],[AuditUserIDReview] FROM tblClaimServices WHERE ClaimServiceID = @ClaimServiceID;" _
                           & "UPDATE tblClaimServices SET [ServiceID] = @ServiceID, [QtyProvided] = @QtyProvided, [PriceAsked] = @PriceAsked, [Explanation] = @Explanation" _
                           & ", [ValidityFrom] = getdate(), [AuditUserID] = @AuditUserID WHERE ClaimServiceID = @ClaimServiceID", CommandType.Text)

        data.params("@ClaimServiceID", SqlDbType.Int, eClaimServices.ClaimServiceID)
        data.params("@ServiceID", SqlDbType.Int, eClaimServices.tblServices.ServiceID)
        data.params("@QtyProvided", SqlDbType.Decimal, eClaimServices.QtyProvided)
        data.params("@PriceAsked", SqlDbType.Decimal, eClaimServices.PriceAsked)
        data.params("@Explanation", SqlDbType.NText, eClaimServices.Explanation, ParameterDirection.Input)
        data.params("@AuditUserID", SqlDbType.Int, eClaimServices.AuditUserID)

        data.ExecuteCommand()


    End Sub
    Public Function IsItSystemRejected(ByVal eClaimReviewServices As IMIS_EN.tblClaimServices) As DataTable
        Dim data As New ExactSQL
        Dim str As String = "select isnull(RejectionReason,0) from tblClaimServices where ClaimServiceID = @ClaimServiceID and ValidityTo is null"

        data.setSQLCommand(str, CommandType.Text)

        data.params("@ClaimServiceID", SqlDbType.Int, eClaimReviewServices.ClaimServiceID)

        Return data.Filldata
    End Function
    Public Sub SaveClaimServicesforReview(ByRef eClaimReviewServices As IMIS_EN.tblClaimServices)

        Dim data As New ExactSQL
        Dim strSQL As String = "select top 1 ClaimServiceID from tblClaimServices where validityFromReview is null and ClaimServiceID = @ClaimServiceID;" & _
                    " if @@rowcount > 0 begin" & _
                    " UPDATE tblClaimServices SET  [ClaimServiceStatus] = @ClaimServiceStatus, [QtyApproved] = @QtyApproved, [PriceApproved] = @PriceApproved, [Justification] = @Justification " & _
                        ", [ValidityFromReview] = getdate(), [AudituserIdReview] = @AuditUserID "

        If Not eClaimReviewServices.RejectionReason Is Nothing Then
            strSQL += ", [RejectionReason] = @RejectionReason"
        End If
        strSQL += " WHERE ClaimServiceID = @ClaimServiceID; end else begin" & _
                        " Insert Into tblClaimServices ([ClaimID],[ServiceID],[ProdID],[ClaimServiceStatus],[QtyProvided],[QtyApproved]," & _
                        " [PriceAsked],[PriceApproved],[Explanation],[Justification],[RejectionReason],[ValidityFrom],[ValidityTo],[LegacyID]," & _
                        " [AuditUserID],[ValidityFromReview],[ValidityToReview],[AuditUserIDReview])" & _
                        " SELECT [ClaimID],[ServiceID],[ProdID],[ClaimServiceStatus],[QtyProvided],[QtyApproved],[PriceAsked],[PriceApproved]" & _
                        ",[Explanation],[Justification],[RejectionReason],[ValidityFrom],getdate(),[ClaimServiceID],[AuditUserID],[ValidityFromReview]," & _
                        "getdate(),[AuditUserIDReview] FROM tblClaimServices WHERE ClaimServiceID = @ClaimServiceID;" & _
                        "UPDATE tblClaimServices SET  [ClaimServiceStatus] = @ClaimServiceStatus,[QtyApproved] = @QtyApproved, [PriceApproved] = @PriceApproved, [Justification] = @Justification" & _
                        ",[ValidityFromReview] = getdate(), [AudituserIdReview] = @AuditUserID "

        If Not eClaimReviewServices.RejectionReason Is Nothing Then
            strSQL += ", [RejectionReason] = @RejectionReason"
        End If
        strSQL += " WHERE ClaimServiceID = @ClaimServiceID; End"

        data.setSQLCommand(strSQL, CommandType.Text)

        data.params("@ClaimServiceID", SqlDbType.Int, eClaimReviewServices.ClaimServiceID)
        data.params("@ClaimServiceStatus", SqlDbType.TinyInt, eClaimReviewServices.ClaimServiceStatus)
        data.params("@QtyApproved", SqlDbType.Decimal, if(eClaimReviewServices.QtyApproved Is Nothing, DBNull.Value, eClaimReviewServices.QtyApproved))
        data.params("@PriceApproved", SqlDbType.Decimal, if(eClaimReviewServices.PriceApproved Is Nothing, DBNull.Value, eClaimReviewServices.PriceApproved))
        data.params("@Justification", SqlDbType.NText, eClaimReviewServices.Justification, ParameterDirection.Input)
        data.params("@AuditUserID", SqlDbType.Int, eClaimReviewServices.AuditUserID)

        If Not eClaimReviewServices.RejectionReason Is Nothing Then
            data.params("@RejectionReason", SqlDbType.SmallInt, eClaimReviewServices.RejectionReason)
        End If

        data.ExecuteCommand()

    End Sub
    Public Sub DeleteClaimService(ByRef eClaimService As IMIS_EN.tblClaimServices)
        Dim data As New ExactSQL

        data.setSQLCommand("Insert Into tblClaimServices ([ClaimID],[ServiceID],[ProdID],[ClaimServiceStatus],[QtyProvided],[QtyApproved]," _
                           & "[PriceAsked],[PriceAdjusted],[PriceApproved],[Explanation],[Justification],[RejectionReason],[ValidityFrom],[ValidityTo],[LegacyID]," _
                           & "[AuditUserID],[ValidityFromReview],[ValidityToReview],[AuditUserIDReview])" _
                           & "SELECT [ClaimID],[ServiceID],[ProdID],[ClaimServiceStatus],[QtyProvided],[QtyApproved],[PriceAsked],[PriceAdjusted],[PriceApproved]" _
                           & ",[Explanation],[Justification],[RejectionReason],[ValidityFrom],getdate(),[ClaimServiceID],[AuditUserID],[ValidityFromReview]," _
                           & "[ValidityToReview],[AuditUserIDReview] FROM tblClaimServices WHERE ClaimServiceID = @ClaimServiceID;" _
                           & "UPDATE tblClaimServices SET [ValidityFrom] = GetDate(),[ValidityTo] = getdate() ,[AuditUserID] = @AuditUserID WHERE ClaimServiceID = @ClaimServiceID", CommandType.Text)

        data.params("@ClaimServiceID", SqlDbType.Int, eClaimService.ClaimServiceID)
        data.params("@AuditUserID", SqlDbType.Int, eClaimService.AuditUserID)

        data.ExecuteCommand()

    End Sub
End Class
