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

Public Class PolicyRenewalDAL

    Public Sub UpdatePolicyRenewal(RemindingInterval As Integer?, RegionId As Integer?, DistrictId As Integer?, WardId As Integer?, VillageId As Integer?, OfficerId As Integer?, DateFrom As Date?, DateTo As Date?)
        Dim sSQL As String = "uspPolicyRenewalInserts"
        Dim data As New ExactSQL

        data.setSQLCommand(sSQL, CommandType.StoredProcedure)

        data.params("@RemindingInterval", SqlDbType.Int, RemindingInterval)
        data.params("@RegionId", SqlDbType.Int, RegionId)
        data.params("@DistrictId", SqlDbType.Int, DistrictId)
        data.params("@WardId", SqlDbType.Int, WardId)
        data.params("@VillageId", SqlDbType.Int, VillageId)
        data.params("@OfficerId", SqlDbType.Int, OfficerId)
        data.params("@DateFrom", SqlDbType.Date, DateFrom)
        data.params("@DateTo", SqlDbType.Date, DateTo)

        data.ExecuteCommand()

    End Sub

    Public Function GetPolicyStatus(ByVal RangeFrom As DateTime, ByVal RangeTo As DateTime, ByVal OfficerID As Integer, ByVal RegionId As Integer, ByVal District As Integer, ByVal Village As Integer, ByVal Ward As Integer, ByVal PolicyStatus As Integer) As DataTable

        Dim data As New ExactSQL
        Dim sSQL As String = "uspSSRSPolicyStatus"

        data.setSQLCommand(sSQL, CommandType.StoredProcedure)

        data.params("@RangeFrom", SqlDbType.DateTime, RangeFrom)
        data.params("@RangeTo", SqlDbType.DateTime, RangeTo)
        data.params("@OfficerID", OfficerID)
        data.params("@RegionId", RegionId)
        data.params("@DistrictID", District)
        data.params("@VillageID", Village)
        data.params("@WardID", Ward)
        data.params("@PolicyStatus", PolicyStatus)

        Dim ds As DataSet = data.FilldataSet

        '        Return data.Filldata

        Return ds.Tables(ds.Tables.Count - 1)
    End Function


    Public Function GetPolicyPromptJournal(ByVal RangeFrom As DateTime, ByVal RangeTo As DateTime, ByVal OfficerID As Integer, ByVal District As Integer, ByVal Village As Integer, ByVal Ward As Integer, ByVal SMSStatus As Integer, ByVal IntervalType As Integer) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = "uspSSRSPolicyRenewalPromptJournal"

        data.setSQLCommand(sSQL, CommandType.StoredProcedure)

        data.params("@RangeFrom", SqlDbType.DateTime, RangeFrom)
        data.params("@RangeTo", SqlDbType.DateTime, RangeTo)
        data.params("@OfficerID", OfficerID)
        data.params("@LocationId", District)
        data.params("@VillageID", Village)
        data.params("@WardID", Ward)
        data.params("@SMSStatus", SMSStatus)
        data.params("@IntervalType", IntervalType)

        Return data.Filldata

    End Function
    Public Function getPolicyRenewalSMSData(ByVal DateFrom As Date, ByVal DateTo As Date) As DataTable
        Dim data As New ExactSQL
        Dim strSQL As String = "uspPolicyRenewalSMS"
        data.setSQLCommand(strSQL, CommandType.StoredProcedure)
        data.params("@RangeFrom", SqlDbType.DateTime, DateFrom)
        data.params("@RangeTo", SqlDbType.DateTime, DateTo)
        Return Data.Filldata
    End Function

End Class
