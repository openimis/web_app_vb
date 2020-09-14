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

Public Class ReportingDAL
    Private data As New ExactSQL
    Private Query As String = ""
    'Corrected
    Public Function GetPreviousMatchingFundsReportDates(ByVal UserID As Integer, ByVal LocationId As Integer, ByVal ReportingID As Integer?) As DataTable
        Query = "SELECT RP.ReportingID,RP.StartDate,RP.EndDate,CAST(RP.ReportingDate AS CHAR(20))" &
  "+ '  ' + Dis.DistrictName + '  ' + Prod.ProductCode " &
  "+ '  ' + CONVERT(NVARCHAR,RP.StartDate,103) + '  ' + CONVERT(NVARCHAR,RP.EndDate,103)" &
  "+ '  ' + ISNULL(PY.PayerName,'') Display FROM tblReporting RP" &
  " INNER JOIN  tblProduct Prod ON Prod.ProdID = RP.ProdId" &
  " INNER JOIN tblDistricts Dis ON Dis.DistrictID = RP.LocationId" &
  " LEFT OUTER JOIN tblPayer PY ON PY.PayerID = RP.PayerId" &
  " INNER JOIN tblUsersDistricts UD ON Dis.DistrictID = UD.LocationId" &
  " WHERE RP.ReportingID = CASE WHEN @ReportingID IS NULL THEN RP.ReportingID ELSE @ReportingID END" &
   " AND RP.ReportType =1" &
  " AND UD.ValidityTo IS NULL AND UD.UserID = @UserId" &
  " AND RP.LocationId = CASE WHEN @LocationId = 0 THEN RP.LocationId ELSE @LocationId END ORDER BY ReportingId DESC"
        data.setSQLCommand(Query, CommandType.Text)
        data.params("@ReportingID", SqlDbType.Int, ReportingID)
        data.params("@UserID", SqlDbType.Int, UserID)
        data.params("@LocationId", SqlDbType.Int, LocationId)
        Return data.Filldata()
    End Function
    Public Function GetPreviousOvervireOfCommissiosReportDates(ByVal UserID As Integer, ByVal LocationId As Integer, ByVal ReportingID As Integer?, Year As Integer, Month As Integer) As DataTable
        Query = "SELECT  RP.LocationId,RP.CommissionRate,R.RegionId,RP.OfficerID,RP.PayerId, RP.ProdId,Prod.ProductCode+' ' +Prod.ProductName ProductCode,RP.ReportMode,RP.CommissionRate,RP.ReportingId,RP.StartDate,RP.EndDate,CONCAT(FORMAT(RP.StartDate,'MMMM','en-US') , ' ' ,Year(RP.StartDate)) " &
  " + ' ' + CASE RP.ReportMode  WHEN 0 THEN 'Prescribed Contributions' WHEN 1 THEN 'Actually Paid Contributions' ELSE ''  END" &
  " + ' ' + CAST(RP.CommissionRate AS nvarchar)+'% ' + ' ' + ISNULL(Prod.ProductCode,'') + ' ' +ISNULL(O.LastName,'')+' '+ ISNULL(O.OtherNames,'')" &
 " + ' ' +R.RegionName+ '  ' + Dis.DistrictName " &
  "+ '  ' + CAST(RP.ReportingDate AS CHAR(20))" &
  "+ '  ' + ISNULL(PY.PayerName,'') Display FROM tblReporting RP" &
  " INNER JOIN tblDistricts Dis ON Dis.DistrictID = RP.LocationId" &
  " INNER JOIN tblRegions R ON R.RegionId = Dis.Region" &
  " LEFT JOIN tblProduct Prod ON Prod.ProdID = RP.ProdId" &
  " LEFT OUTER JOIN tblPayer PY ON PY.PayerID = RP.PayerId" &
  " INNER JOIN tblUsersDistricts UD ON Dis.DistrictID = UD.LocationId" &
   " LEFT JOIN tblOfficer O ON O.OfficerID = RP.OfficerID" &
  " WHERE RP.ReportingId = CASE WHEN @ReportingID IS NULL THEN RP.ReportingID ELSE @ReportingID END" &
  " AND RP.ReportType =2" &
   " AND Year(RP.StartDate) = @Year AND Month(RP.StartDate)  =@Month" &
   " AND UD.ValidityTo IS NULL AND UD.UserID = @UserId" &
  " AND RP.LocationId = CASE WHEN @LocationId = 0 THEN RP.LocationId ELSE @LocationId END ORDER BY ReportingId DESC"
        data.setSQLCommand(Query, CommandType.Text)
        data.params("@ReportingID", SqlDbType.Int, ReportingID)
        data.params("@UserID", SqlDbType.Int, UserID)
        data.params("@LocationId", SqlDbType.Int, LocationId)
        data.params("@Year", SqlDbType.Int, Year)
        data.params("@Month", SqlDbType.Int, Month)

        Return data.Filldata()
    End Function
End Class
