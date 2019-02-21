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

Public Class ClaimItemsDAL
    Dim data As New ExactSQL
    Public Sub InsertClaimItems(ByRef eClaimItems As IMIS_EN.tblClaimItems)


        Data.setSQLCommand("Insert Into tblClaimItems ([ClaimID],[ItemID],[QtyProvided],[PriceAsked],[Explanation]," _
                           & "[ValidityFrom],[AuditUserID])" _
                           & "VALUES(@ClaimID,@ItemID,@QtyProvided,@PriceAsked,@Explanation, getdate(), @AuditUserID) ", CommandType.Text)

        Data.params("@ClaimID", SqlDbType.Int, eClaimItems.tblClaim.ClaimID)
        Data.params("@ItemID", SqlDbType.Int, eClaimItems.tblItems.ItemID)
        Data.params("@QtyProvided", SqlDbType.Decimal, eClaimItems.QtyProvided)
        Data.params("@PriceAsked", SqlDbType.Decimal, eClaimItems.PriceAsked)
        Data.params("@Explanation", SqlDbType.NText, eClaimItems.Explanation, ParameterDirection.Input)
        Data.params("@AuditUserID", SqlDbType.Int, eClaimItems.AuditUserID)

        Data.ExecuteCommand()

    End Sub

    Public Sub UpdateClaimItems(ByRef eClaimItems As IMIS_EN.tblClaimItems)

        data.setSQLCommand("Insert Into tblClaimItems ([ClaimID],[ItemID],[ProdID],[ClaimItemStatus],[Availability],[QtyProvided],[QtyApproved]," _
                           & "[PriceAsked],[PriceApproved],[Explanation],[Justification],[RejectionReason],[ValidityFrom],[ValidityTo],[LegacyID]," _
                           & "[AuditUserID],[ValidityFromReview],[ValidityToReview],[AuditUserIDReview])" _
                           & "SELECT [ClaimID],[ItemID],[ProdID],[ClaimItemStatus],[Availability],[QtyProvided],[QtyApproved],[PriceAsked],[PriceApproved]" _
                           & ",[Explanation],[Justification],[RejectionReason],[ValidityFrom],getdate(),[ClaimItemID],[AuditUserID],[ValidityFromReview]," _
                           & "[ValidityToReview],[AuditUserIDReview] FROM tblClaimItems WHERE ClaimItemID = @ClaimItemID;" _
                           & "UPDATE tblClaimItems SET [ItemID] = @ItemID, [QtyProvided] = @QtyProvided, [PriceAsked] = @PriceAsked, [Explanation] = @Explanation" _
                           & ", [ValidityFrom] = getdate(), [AuditUserID] = @AuditUserID WHERE ClaimItemID = @ClaimItemID", CommandType.Text)

        data.params("@ClaimItemID", SqlDbType.Int, eClaimItems.ClaimItemID)
        data.params("@ItemID", SqlDbType.Int, eClaimItems.tblItems.ItemID)
        data.params("@QtyProvided", SqlDbType.Decimal, eClaimItems.QtyProvided)
        data.params("@PriceAsked", SqlDbType.Decimal, eClaimItems.PriceAsked)
        data.params("@Explanation", SqlDbType.NText, eClaimItems.Explanation, ParameterDirection.Input)
        data.params("@AuditUserID", SqlDbType.Int, eClaimItems.AuditUserID)

        data.ExecuteCommand()

    End Sub
    Public Function IsItSystemRejected(ByVal eClaimReviewItem As IMIS_EN.tblClaimItems) As DataTable
        Dim str As String = "select isnull(RejectionReason,0) from tblClaimItems where ClaimItemID = @ClaimItemID and ValidityTo is null"

        data.setSQLCommand(str, CommandType.Text)

        data.params("@ClaimItemID", SqlDbType.Int, eClaimReviewItem.ClaimItemID)

        Return data.Filldata
    End Function
    
    Public Sub SaveClaimItemsforReview(ByRef eClaimReviewItem As IMIS_EN.tblClaimItems)

        Dim strSQL As String = "select top 1 claimitemid from tblClaimItems where validityFromReview is null and ClaimItemID = @ClaimItemID;" & _
                    " if @@rowcount > 0 begin" & _
                    " UPDATE tblClaimItems SET [ClaimItemStatus] = @ClaimItemStatus, [QtyApproved] = @QtyApproved, [PriceApproved] = @PriceApproved, [Justification] = @Justification " & _
                        ", [ValidityFromReview] = getdate(), [AudituserIdReview] = @AuditUserID "

        If Not eClaimReviewItem.RejectionReason Is Nothing Then
            strSQL += ", [RejectionReason] = @RejectionReason"
        End If

        strSQL += " WHERE ClaimItemID = @ClaimItemID; end else begin" & _
                        " Insert Into tblClaimItems ([ClaimID],[ItemID],[ProdID],[ClaimItemStatus],[Availability],[QtyProvided],[QtyApproved]," & _
                        " [PriceAsked],[PriceApproved],[Explanation],[Justification],[RejectionReason],[ValidityFrom],[ValidityTo],[LegacyID]," & _
                        " [AuditUserID],[ValidityFromReview],[ValidityToReview],[AuditUserIDReview])" & _
                        " SELECT [ClaimID],[ItemID],[ProdID],[ClaimItemStatus],[Availability],[QtyProvided],[QtyApproved],[PriceAsked],[PriceApproved]" & _
                        ",[Explanation],[Justification],[RejectionReason],[ValidityFrom],getdate(),[ClaimItemID],[AuditUserID],[ValidityFromReview]," & _
                        "getdate(),[AuditUserIDReview] FROM tblClaimItems WHERE ClaimItemID = @ClaimItemID;" & _
                        "UPDATE tblClaimItems SET  [ClaimItemStatus] = @ClaimItemStatus,[QtyApproved] = @QtyApproved, [PriceApproved] = @PriceApproved, [Justification] = @Justification" & _
                        ",[ValidityFromReview] = getdate(), [AudituserIdReview] = @AuditUserID "

        If Not eClaimReviewItem.RejectionReason Is Nothing Then
            strSQL += ", [RejectionReason] = @RejectionReason"
        End If
        strSQL += " WHERE ClaimItemID = @ClaimItemID; End"

        data.setSQLCommand(strSQL, CommandType.Text)


        data.params("@ClaimItemID", SqlDbType.Int, eClaimReviewItem.ClaimItemID)
        data.params("@ClaimItemStatus", SqlDbType.TinyInt, eClaimReviewItem.ClaimItemStatus)
        data.params("@QtyApproved", SqlDbType.Decimal, if(eClaimReviewItem.QtyApproved Is Nothing, DBNull.Value, eClaimReviewItem.QtyApproved))
        data.params("@PriceApproved", SqlDbType.Decimal, if(eClaimReviewItem.PriceApproved Is Nothing, DBNull.Value, eClaimReviewItem.PriceApproved))
        data.params("@Justification", SqlDbType.NText, eClaimReviewItem.Justification, ParameterDirection.Input)
        data.params("@AuditUserID", SqlDbType.Int, eClaimReviewItem.AuditUserID)

        If Not eClaimReviewItem.RejectionReason Is Nothing Then
            data.params("@RejectionReason", SqlDbType.SmallInt, eClaimReviewItem.RejectionReason)
        End If

        data.ExecuteCommand()
    End Sub

    Public Sub DeleteClaimItems(ByRef eClaimItem As IMIS_EN.tblClaimItems)

        data.setSQLCommand("Insert into tblClaimItems ([ClaimID],[ItemID],[ProdID],[ClaimItemStatus],[Availability],[QtyProvided],[QtyApproved]," _
                           & "[PriceAsked],[PriceAdjusted],[PriceApproved],[Explanation],[Justification],[RejectionReason],[ValidityFrom],[ValidityTo],[LegacyID]," _
                           & "[AuditUserID],[ValidityFromReview],[ValidityToReview],[AuditUserIDReview])" _
                           & "SELECT [ClaimID],[ItemID],[ProdID],[ClaimItemStatus],[Availability],[QtyProvided],[QtyApproved],[PriceAsked],[PriceAdjusted],[PriceApproved]" _
                           & ",[Explanation],[Justification],[RejectionReason],[ValidityFrom],getdate(),[ClaimItemID],[AuditUserID],[ValidityFromReview]," _
                           & "[ValidityToReview],[AuditUserIDReview] FROM tblClaimItems WHERE ClaimItemID = @ClaimItemID;" _
                           & "UPDATE tblClaimItems SET [ValidityFrom] = GetDate(),[ValidityTo] = getdate(),[AuditUserID] = @AuditUserID WHERE ClaimItemID = @ClaimItemID", CommandType.Text)

        data.params("@ClaimItemID", SqlDbType.Int, eClaimItem.ClaimItemID)
        data.params("@AuditUserID", SqlDbType.Int, eClaimItem.AuditUserID)

        data.ExecuteCommand()
    End Sub


End Class
