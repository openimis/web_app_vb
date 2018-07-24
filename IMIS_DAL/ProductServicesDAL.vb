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

Public Class ProductServicesDAL
    Private Query As String = ""
    Public Function GetProductServices(ByVal ProdID As Integer, dtSType As DataTable, dtServType As DataTable) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""

        sSQL = "SELECT tblproductServices.ProdServiceId,ISNULL(LimitationType,@limit) LimitationType,ISNULL(PriceOrigin,@PriceOrigin) PriceOrigin,"
        sSQL += " isnull(LimitAdult,@LimitDefault)LimitAdult,ISNULL(LimitChild,@LimitDefault) LimitChild, S.ServiceID,ServCode,ServName,"
        sSQL += " dt.Name ServType, Sl.Name ServLevel,"
        sSQL += " ServPrice,LimitNoAdult,WaitingPeriodAdult,LimitNoChild,WaitingPeriodChild,ISNULL(LimitationTypeR,@limit) LimitationTypeR,"
        sSQL += " ISNULL(LimitationTypeE,@limit) LimitationTypeE,ISNULL(LimitAdultR,@LimitDefault) LimitAdultR,ISNULL(LimitAdultE,@LimitDefault) LimitAdultE,"
        sSQL += " ISNULL(LimitChildR,@LimitDefault) LimitChildR,ISNULL(LimitChildE,@LimitDefault) LimitChildE,CeilingExclusionAdult,CeilingExclusionChild"
        sSQL += " FROM tblServices S"
        sSQL += " LEFT OUTER JOIN @dtIType dt ON dt.Code = S.ServType"
        sSQL += " LEFT OUTER JOIN @dtServLevel Sl ON Sl.Code = S.ServLevel"
        sSQL += " LEFT OUTER JOIN"
        sSQL += " (SELECT * FROM tblproductServices"
        sSQL += " WHERE ProdID = @ProdId and ValidityTo is null) tblproductServices on S.Serviceid  = tblproductServices.ServiceID"
        sSQL += " where S.Validityto is null"
        sSQL += " Order by case when tblproductServices.ProdServiceId IS NULL THEN 0 ELSE 1 END DESC,ServCode"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@ProdID", SqlDbType.Int, ProdID)
        data.params("@limit", SqlDbType.Char, 1, IMIS_EN.AppConfiguration.DefaultLimitation)
        data.params("@PriceOrigin", SqlDbType.Char, 1, IMIS_EN.AppConfiguration.DefaultPriceOrigin)
        data.params("@LimitDefault", SqlDbType.Float, If(IMIS_EN.AppConfiguration.DefaultLimitation = "C", 100, 0))
        data.params("@dtIType", dtSType, "xCareType")
        data.params("@dtServLevel", dtServType, "xCareType")

        Return data.Filldata
    End Function
    Public Function DeleteProductServices(ByVal eProductServices As IMIS_EN.tblProductServices) As Boolean
        Dim data As New ExactSQL
        Query = "INSERT INTO tblProductServices ([ProdID],[ServiceID],[LimitationType],[PriceOrigin],[LimitAdult],[LimitChild],LimitNoAdult,WaitingPeriodAdult,LimitNoChild,WaitingPeriodChild,[ValidityTo],[LegacyID],[AuditUserID],LimitationTypeR,LimitationTypeE,LimitAdultR,LimitAdultE,LimitChildR,LimitChildE,CeilingExclusionAdult,CeilingExclusionChild)" & _
         " SELECT [ProdID],[ServiceID],[LimitationType],[PriceOrigin],[LimitAdult],[LimitChild],LimitNoAdult,WaitingPeriodAdult,LimitNoChild,WaitingPeriodChild,GETDATE(),[ProdServiceID],[AuditUserID],LimitationTypeR,LimitationTypeE,LimitAdultR,LimitAdultE,LimitChildR,LimitChildE,CeilingExclusionAdult,CeilingExclusionChild" & _
         " FROM tblProductServices WHERE ProdServiceID = @ProdServiceID;" & _
         " UPDATE [tblProductServices] SET [ValidityFrom] = GETDATE(),[ValidityTo] = GETDATE(), [AuditUserID] = @AuditUserID WHERE ProdServiceID = @ProdServiceID"
        data.setSQLCommand(Query, CommandType.Text)
        data.params("@ProdServiceID", SqlDbType.Int, eProductServices.ProdServiceID)
        data.params("@AuditUserID", SqlDbType.Int, eProductServices.AuditUserID)
        data.ExecuteCommand()
        Return True
    End Function
    Public Function InsertProductServices(ByVal eProductServices As IMIS_EN.tblProductServices) As Boolean
        Dim data As New ExactSQL
        Query = "INSERT INTO tblProductServices([ProdID],[ServiceID],[LimitationType],[PriceOrigin]" & _
            ",[LimitAdult],[LimitChild],LimitNoAdult,WaitingPeriodAdult,LimitNoChild,WaitingPeriodChild" & _
            ",[AuditUserID]" & _
            ",LimitationTypeR,LimitationTypeE,LimitAdultR,LimitAdultE,LimitChildR,LimitChildE,CeilingExclusionAdult,CeilingExclusionChild" & _
            ")" & _
             " VALUES (@ProdID,@ServiceID,@LimitationType,@PriceOrigin,@LimitAdult,@LimitChild,@LimitNoAdult" & _
             ",@WaitingPeriodAdult,@LimitNoChild,@WaitingPeriodChild,@AuditUserID" & _
             ",@LimitationTypeR,@LimitationTypeE,@LimitAdultR,@LimitAdultE,@LimitChildR,@LimitChildE,@CeilingExclusionAdult,@CeilingExclusionChild" & _
             ")"
        data.setSQLCommand(Query, CommandType.Text)

        data.params("@ProdID", SqlDbType.Int, eProductServices.tblProduct.ProdID)
        data.params("@ServiceID", SqlDbType.Int, eProductServices.tblServices.ServiceID)
        data.params("@AuditUserID", SqlDbType.Int, eProductServices.AuditUserID)
        data.params("@LimitationType", SqlDbType.Char, 1, eProductServices.LimitationType)
        data.params("@PriceOrigin", SqlDbType.Char, 1, eProductServices.PriceOrigin)
        data.params("@LimitAdult", SqlDbType.Decimal, if(eProductServices.LimitAdult Is Nothing, SqlTypes.SqlDecimal.Null, eProductServices.LimitAdult))
        data.params("@LimitChild", SqlDbType.Decimal, if(eProductServices.LimitChild Is Nothing, SqlTypes.SqlDecimal.Null, eProductServices.LimitChild))
        data.params("@LimitNoAdult", SqlDbType.Int, eProductServices.LimitNoAdult)
        data.params("@WaitingPeriodAdult", SqlDbType.Int, eProductServices.WaitingPeriodAdult)
        data.params("@LimitNoChild", SqlDbType.Int, eProductServices.LimitNoChild)
        data.params("@WaitingPeriodChild", SqlDbType.Int, eProductServices.WaitingPeriodChild)
        'Addition for Nepal >> Start
        data.params("@LimitationTypeR", SqlDbType.Char, 1, eProductServices.LimitationTypeR)
        data.params("@LimitationTypeE", SqlDbType.Char, 1, eProductServices.LimitationTypeE)
        data.params("@LimitAdultR", SqlDbType.Decimal, eProductServices.LimitAdultR)
        data.params("@LimitAdultE", SqlDbType.Decimal, eProductServices.LimitAdultE)
        data.params("@LimitChildR", SqlDbType.Decimal, eProductServices.LimitChildR)
        data.params("@LimitChildE", SqlDbType.Decimal, eProductServices.LimitChildE)
        data.params("@CeilingExclusionAdult", SqlDbType.NVarChar, 1, eProductServices.CeilingExclusionAdult)
        data.params("@CeilingExclusionChild", SqlDbType.NVarChar, 1, eProductServices.CeilingExclusionChild)
        'Addition for Nepal >> End
        data.ExecuteCommand()
        Return True
    End Function
    Public Sub UpdateProductServices(ByVal eProductServices As IMIS_EN.tblProductServices)
        Dim data As New ExactSQL
        Query = "INSERT INTO tblProductServices ([ProdID],[ServiceID],[LimitationType],[PriceOrigin]" & _
            ",[LimitAdult],[LimitChild],LimitNoAdult,WaitingPeriodAdult,LimitNoChild,WaitingPeriodChild,[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID]" & _
            ",LimitationTypeR,LimitationTypeE,LimitAdultR,LimitAdultE,LimitChildR,LimitChildE,CeilingExclusionAdult,CeilingExclusionChild" & _
            ")" & _
            " SELECT [ProdID],[ServiceID],[LimitationType],[PriceOrigin],[LimitAdult],[LimitChild]" & _
            ",LimitNoAdult,WaitingPeriodAdult,LimitNoChild,WaitingPeriodChild,[ValidityFrom],GETDATE()" & _
            ",[ProdServiceID],[AuditUserID]" & _
            ",LimitationTypeR,LimitationTypeE,LimitAdultR,LimitAdultE,LimitChildR,LimitChildE,CeilingExclusionAdult,CeilingExclusionChild" & _
            " FROM tblProductServices WHERE ProdServiceID = @ProdServiceID;" & _
            " UPDATE [tblProductServices] SET  [LimitationType] = @LimitationType, [PriceOrigin] = @PriceOrigin" & _
            ", [LimitAdult] = @LimitAdult, [LimitChild] = @LimitChild,LimitNoAdult=@LimitNoAdult" & _
            ",WaitingPeriodAdult=@WaitingPeriodAdult,LimitNoChild=@LimitNoChild,WaitingPeriodChild=@WaitingPeriodChild" & _
            ",[ValidityFrom] = getdate(), [AuditUserID] = @AuditUserID" & _
            ",LimitationTypeR = @LimitationTypeR,LimitationTypeE = @LimitationTypeE,LimitAdultR = @LimitAdultR" & _
            ",LimitAdultE = @LimitAdultE,LimitChildR = @LimitChildR,LimitChildE = @LimitChildE,CeilingExclusionAdult = @CeilingExclusionAdult" & _
            ",CeilingExclusionChild = @CeilingExclusionChild" & _
            " WHERE ProdServiceID = @ProdServiceID"
        data.setSQLCommand(Query, CommandType.Text)
        data.params("@ProdServiceID", SqlDbType.Int, eProductServices.ProdServiceID)
        data.params("@LimitationType", SqlDbType.Char, 1, eProductServices.LimitationType)
        data.params("@PriceOrigin", SqlDbType.Char, 1, eProductServices.PriceOrigin)
        data.params("@LimitAdult", SqlDbType.Decimal, if(eProductServices.LimitAdult Is Nothing, SqlTypes.SqlDecimal.Null, eProductServices.LimitAdult))
        data.params("@LimitChild", SqlDbType.Decimal, if(eProductServices.LimitChild Is Nothing, SqlTypes.SqlDecimal.Null, eProductServices.LimitChild))
        data.params("@AuditUserID", SqlDbType.Int, eProductServices.AuditUserID)
        data.params("@LimitNoAdult", SqlDbType.Int, eProductServices.LimitNoAdult)
        data.params("@WaitingPeriodAdult", SqlDbType.Int, eProductServices.WaitingPeriodAdult)
        data.params("@LimitNoChild", SqlDbType.Int, eProductServices.LimitNoChild)
        data.params("@WaitingPeriodChild", SqlDbType.Int, eProductServices.WaitingPeriodChild)
        'Addition for Nepal >> Start
        data.params("@LimitationTypeR", SqlDbType.Char, 1, eProductServices.LimitationTypeR)
        data.params("@LimitationTypeE", SqlDbType.Char, 1, eProductServices.LimitationTypeE)
        data.params("@LimitAdultR", SqlDbType.Decimal, eProductServices.LimitAdultR)
        data.params("@LimitAdultE", SqlDbType.Decimal, eProductServices.LimitAdultE)
        data.params("@LimitChildR", SqlDbType.Decimal, eProductServices.LimitChildR)
        data.params("@LimitChildE", SqlDbType.Decimal, eProductServices.LimitChildE)
        data.params("@CeilingExclusionAdult", SqlDbType.NVarChar, 1, eProductServices.CeilingExclusionAdult)
        data.params("@CeilingExclusionChild", SqlDbType.NVarChar, 1, eProductServices.CeilingExclusionChild)
        'Addition for Nepal >> End
        data.ExecuteCommand()
    End Sub
End Class
