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

Public Class ProductItemsDAL
    Private Query As String = ""
    Public Function GetProductItems(ByVal ProdID As Integer, ByVal dtIType As DataTable) As DataTable
        Dim data As New ExactSQL
        Query = "SELECT tblproductItems.ProdItemId,ISNULL(LimitationType,@limit) LimitationType" & _
            ",ISNULL(PriceOrigin,@PriceOrigin) PriceOrigin,isnull(LimitAdult,@limitDefault)LimitAdult" & _
            ",isnull(LimitChild,@limitDefault) LimitChild, tblItems.ItemId,ItemCode,ItemName" & _
            ",IType.Name ItemType" & _
            ",ItemPackage,ItemPrice,WaitingPeriodAdult,WaitingPeriodChild,LimitNoAdult,LimitNoChild" & _
            ",ISNULL(LimitationTypeR,@limit) LimitationTypeR,ISNULL(LimitationTypeE,@limit) LimitationTypeE" & _
            ",ISNULL(LimitAdultR,@LimitDefault) LimitAdultR,ISNULL(LimitAdultE,@LimitDefault) LimitAdultE,ISNULL(LimitChildR,@LimitDefault) LimitChildR" & _
            ",ISNULL(LimitChildE,@LimitDefault) LimitChildE,CeilingExclusionAdult,CeilingExclusionChild" & _
            " FROM tblItems " & _
            " inner join @dtIType IType on Itype.code = tblItems.itemType" & _
            " LEFT OUTER JOIN (SELECT * FROM tblproductItems WHERE ProdID = @ProdID AND ValidityTo IS NULL) tblproductItems" & _
            " on tblItems.itemid  = tblproductItems.ItemID where tblItems.validityTo is null Order by case when tblproductItems.ProdItemId is null then 0 else 1 end  desc,ItemCode"
        data.setSQLCommand(Query, CommandType.Text)
        data.params("@ProdID", SqlDbType.Int, ProdID)
        data.params("@limit", SqlDbType.Char, 1, IMIS_EN.AppConfiguration.DefaultLimitation)
        data.params("@PriceOrigin", SqlDbType.Char, 1, IMIS_EN.AppConfiguration.DefaultPriceOrigin)
        data.params("@LimitDefault", SqlDbType.Float, If(IMIS_EN.AppConfiguration.DefaultLimitation = "C", 100, 0))
        data.params("@dtIType", dtIType, "xCareType")
        Return data.Filldata
    End Function
    Public Sub DeleteProductItems(ByVal eProductItems As IMIS_EN.tblProductItems)
        Dim data As New ExactSQL
        Query = "INSERT INTO tblProductItems ([ProdID],[ItemID],[LimitationType],[PriceOrigin],[LimitAdult]" & _
            ",[LimitChild],WaitingPeriodAdult,WaitingPeriodChild,LimitNoAdult,LimitNoChild,[ValidityTo]" & _
            ",[LegacyID],[AuditUserID]" & _
            ",LimitationTypeR,LimitationTypeE,LimitAdultR,LimitAdultE,LimitChildR,LimitChildE,CeilingExclusionAdult,CeilingExclusionChild" & _
            ") " & _
            " SELECT [ProdID],[ItemID],[LimitationType],[PriceOrigin],[LimitAdult],[LimitChild],WaitingPeriodAdult" & _
            ",WaitingPeriodChild,LimitNoAdult,LimitNoChild,getdate(),[ProdItemID],[AuditUserID]" & _
            ",LimitationTypeR,LimitationTypeE,LimitAdultR,LimitAdultE,LimitChildR,LimitChildE,CeilingExclusionAdult,CeilingExclusionChild" & _
            " FROM tblProductItems WHERE ProdItemID = @ProdItemID; " & _
            " UPDATE [tblProductItems] SET  [ValidityFrom] = GETDATE(),[ValidityTo] = GETDATE()" & _
            ",[AuditUserID] = @AuditUserID WHERE ProdItemID = @ProdItemID"
        data.setSQLCommand(Query, CommandType.Text)
        data.params("@ProdItemID", SqlDbType.Int, eProductItems.ProdItemID)
        data.params("@AuditUserID", SqlDbType.Int, eProductItems.AuditUserID)

        data.ExecuteCommand()
    End Sub
    Public Sub InsertProductItems(ByVal eProductItems As IMIS_EN.tblProductItems)
        Dim data As New ExactSQL
        Query = "INSERT INTO tblProductItems([ProdID],[ItemID],[LimitationType],[PriceOrigin],[LimitAdult]" & _
            ",[LimitChild],WaitingPeriodAdult,WaitingPeriodChild,LimitNoAdult,LimitNoChild,[AuditUserID]" & _
            ",LimitationTypeR,LimitationTypeE,LimitAdultR,LimitAdultE,LimitChildR,LimitChildE,CeilingExclusionAdult,CeilingExclusionChild" & _
            ") " & _
            " VALUES (@ProdID,@ItemID,@LimitationType,@PriceOrigin,@LimitAdult,@LimitChild,@WaitingPeriodAdult" & _
            ",@WaitingPeriodChild,@LimitNoAdult,@LimitNoChild,@AuditUserID" & _
            ",@LimitationTypeR,@LimitationTypeE,@LimitAdultR,@LimitAdultE,@LimitChildR,@LimitChildE,@CeilingExclusionAdult,@CeilingExclusionChild" & _
            ")"
        data.setSQLCommand(Query, CommandType.Text)

        data.params("@ProdID", SqlDbType.Int, eProductItems.tblProduct.ProdID)
        data.params("@ItemID", SqlDbType.Int, eProductItems.tblItems.ItemID)
        data.params("@AuditUserID", SqlDbType.Int, eProductItems.AuditUserID)
        data.params("@LimitationType", SqlDbType.Char, 1, eProductItems.LimitationType)
        data.params("@PriceOrigin", SqlDbType.Char, 1, eProductItems.PriceOrigin)
        data.params("@LimitAdult", SqlDbType.Decimal, if(eProductItems.LimitAdult Is Nothing, SqlTypes.SqlDecimal.Null, eProductItems.LimitAdult))
        data.params("@LimitChild", SqlDbType.Decimal, if(eProductItems.LimitChild Is Nothing, SqlTypes.SqlDecimal.Null, eProductItems.LimitChild))
        data.params("@WaitingPeriodAdult", SqlDbType.Int, eProductItems.WaitingPeriodAdult)
        data.params("@WaitingPeriodChild", SqlDbType.Int, eProductItems.WaitingPeriodChild)
        data.params("@LimitNoAdult", SqlDbType.Int, eProductItems.LimitNoAdult)
        data.params("@LimitNoChild", SqlDbType.Int, eProductItems.LimitNoChild)
        'Addition for Nepal >> Start
        data.params("@LimitationTypeR", SqlDbType.Char, 1, eProductItems.LimitationTypeR)
        data.params("@LimitationTypeE", SqlDbType.Char, 1, eProductItems.LimitationTypeE)
        data.params("@LimitAdultR", SqlDbType.Decimal, eProductItems.LimitAdultR)
        data.params("@LimitAdultE", SqlDbType.Decimal, eProductItems.LimitAdultE)
        data.params("@LimitChildR", SqlDbType.Decimal, eProductItems.LimitChildR)
        data.params("@LimitChildE", SqlDbType.Decimal, eProductItems.LimitChildE)
        data.params("@CeilingExclusionAdult", SqlDbType.NVarChar, 1, eProductItems.CeilingExclusionAdult)
        data.params("@CeilingExclusionChild", SqlDbType.NVarChar, 1, eProductItems.CeilingExclusionChild)
        'Addition for Nepal >> End
        data.ExecuteCommand()
    End Sub
    Public Sub UpdateProductItems(ByVal eProductItems As IMIS_EN.tblProductItems)
        Dim data As New ExactSQL
        Query = "INSERT INTO tblProductItems ([ProdID],[ItemID],[LimitationType],[PriceOrigin],[LimitAdult]" & _
            ",[LimitChild],WaitingPeriodAdult,WaitingPeriodChild,LimitNoAdult,LimitNoChild,[ValidityFrom]" & _
            ",[ValidityTo],[LegacyID],[AuditUserID]" & _
            ",LimitationTypeR,LimitationTypeE,LimitAdultR,LimitAdultE,LimitChildR,LimitChildE,CeilingExclusionAdult,CeilingExclusionChild" & _
            ") " & _
            " SELECT [ProdID],[ItemID],[LimitationType],[PriceOrigin],[LimitAdult],[LimitChild]" & _
            ",WaitingPeriodAdult,WaitingPeriodChild,LimitNoAdult,LimitNoChild,[ValidityFrom],getdate()" & _
            ",[ProdItemID],[AuditUserID]" & _
            ",LimitationTypeR,LimitationTypeE,LimitAdultR,LimitAdultE,LimitChildR,LimitChildE,CeilingExclusionAdult,CeilingExclusionChild" & _
            " FROM tblProductItems WHERE ProdItemID = @ProdItemID; " & _
            " UPDATE [tblProductItems] SET [LimitationType] = @LimitationType, [PriceOrigin] = @PriceOrigin " & _
            ", [LimitAdult] = @LimitAdult, [LimitChild] = @LimitChild,WaitingPeriodAdult=@WaitingPeriodAdult" & _
            ",WaitingPeriodChild=@WaitingPeriodChild,LimitNoAdult=@LimitNoAdult,LimitNoChild=@LimitNoChild" & _
            ",[ValidityFrom] = GETDATE(), [AuditUserID] = @AuditUserID" & _
            ",LimitationTypeR = @LimitationTypeR,LimitationTypeE = @LimitationTypeE,LimitAdultR = @LimitAdultR" & _
            ",LimitAdultE = @LimitAdultE,LimitChildR = @LimitChildR,LimitChildE = @LimitChildE,CeilingExclusionAdult = @CeilingExclusionAdult" & _
            ",CeilingExclusionChild = @CeilingExclusionChild" & _
            " WHERE ProdItemID = @ProdItemID"
        data.setSQLCommand(Query, CommandType.Text)

        data.params("@ProdItemID", SqlDbType.Int, eProductItems.ProdItemID)
        data.params("@LimitationType", SqlDbType.Char, 1, eProductItems.LimitationType)
        data.params("@PriceOrigin", SqlDbType.Char, 1, eProductItems.PriceOrigin)
        data.params("@LimitAdult", SqlDbType.Decimal, if(eProductItems.LimitAdult Is Nothing, SqlTypes.SqlDecimal.Null, eProductItems.LimitAdult))
        data.params("@LimitChild", SqlDbType.Decimal, if(eProductItems.LimitChild Is Nothing, SqlTypes.SqlDecimal.Null, eProductItems.LimitChild))
        data.params("@AuditUserID", SqlDbType.Int, eProductItems.AuditUserID)
        data.params("@WaitingPeriodAdult", SqlDbType.Int, eProductItems.WaitingPeriodAdult)
        data.params("@WaitingPeriodChild", SqlDbType.Int, eProductItems.WaitingPeriodChild)
        data.params("@LimitNoAdult", SqlDbType.Int, eProductItems.LimitNoAdult)
        data.params("@LimitNoChild", SqlDbType.Int, eProductItems.LimitNoChild)
        'Addition for Nepal >> Start
        data.params("@LimitationTypeR", SqlDbType.Char, 1, eProductItems.LimitationTypeR)
        data.params("@LimitationTypeE", SqlDbType.Char, 1, eProductItems.LimitationTypeE)
        data.params("@LimitAdultR", SqlDbType.Decimal, eProductItems.LimitAdultR)
        data.params("@LimitAdultE", SqlDbType.Decimal, eProductItems.LimitAdultE)
        data.params("@LimitChildR", SqlDbType.Decimal, eProductItems.LimitChildR)
        data.params("@LimitChildE", SqlDbType.Decimal, eProductItems.LimitChildE)
        data.params("@CeilingExclusionAdult", SqlDbType.NVarChar, 1, eProductItems.CeilingExclusionAdult)
        data.params("@CeilingExclusionChild", SqlDbType.NVarChar, 1, eProductItems.CeilingExclusionChild)
        'Addition for Nepal >> End

        data.ExecuteCommand()

    End Sub
End Class
