
var dropdown = {
    $sourceTxtbox: null,
    headers: true,
    locked: false,
    $selectedRow: null,
    rowScrollFlag: 1,
    eventType: "",
    $dropDownTable: $('<table>'),
    fn: function() { },


    unselectedRowStyle: {
        background: "none"
    },
    selectedRowStyle: {
        background: "#70A5DA"
    },

    init: function($dropDownTable, callbackFn) {
        var that = this;
        this.$dropDownTable = $dropDownTable.clone();
        this.fn = callbackFn;

        $(".txtServiceCode,.txtItemCode").unbind("keyup");
        $(".txtServiceCode,.txtItemCode").unbind("click");
        $(".txtServiceCode,.txtItemCode").unbind("keydown");
        $(".txtServiceCode,.txtItemCode").parents(":not(#sugServPanel)").unbind("click");
        //$(".txtServiceCode,.txtItemCode").attr("autocomplete", "off");

        $(".txtServiceCode").click(function(e) {

            e.stopPropagation();
            that.eventType = "click";

            if (that.checkLock())
                return;
            that.$sourceTxtbox = $(this);
            that.suggest("services");

        });

        $(".txtItemCode").click(function(e) {
            e.stopPropagation();
            that.eventType = "click";

            if (that.checkLock())
                return;

            that.$sourceTxtbox = $(this);
            that.suggest("items");

        });

        $(".txtServiceCode,.txtItemCode").keydown(function(e) {
            that.eventType = "keydown";

            if (e.which == 13) {
                that.$sourceTxtbox = $(this);
                that.writeSelectedRowToUserGridView();
                return false;
            }
        });

        $(".txtServiceCode").keyup(function(e) {
            e.stopPropagation();
            that.eventType = "keyup";

            if (e.which == 38 || e.which == 40) return; // || e.which == 13,e.which == 38 || e.which == 40
            if (that.checkLock()) {
                return;
            }

            if (e.which == 27) {
                that.removeSuggestionPanel();
                return;
            }

            that.$sourceTxtbox = $(this);
            if (e.which == 39) {
                if (that.writeSelectedRowToUserGridView()) {
                    return false;
                }
            }
            that.suggest("services");
        });

        $(".txtItemCode").keyup(function(e) {
            e.stopPropagation();
            that.eventType = "keyup";

            if (e.which == 38 || e.which == 40) return; // || e.which == 13,e.which == 38 || e.which == 40
            if (that.checkLock())
                return;

            if (e.which == 27) {
                that.removeSuggestionPanel();
                return;
            }

            that.$sourceTxtbox = $(this);
            if (e.which == 39) {
                if (that.writeSelectedRowToUserGridView())
                    return false;
            }
            that.suggest("items");
        });

        $(".txtServiceCode,.txtItemCode").parents(":not(#sugServPanel)").click(function() {
            that.removeSuggestionPanel();
            if (that.$sourceTxtbox == null || typeof (that.$sourceTxtbox) != "object") return;
            if ($.trim(that.$sourceTxtbox.parent().next().find("input[type=text]").val()) == "")
                that.clearUserKeywordInputRow();
        });

        this.bindEventsonRowSelection();
    },

    checkLock: function() {
        if (this.locked)
            return true;
        else {
            this.setLock();
            return false;
        }
    },

    setLock: function() {
        this.locked = true;
    },

    releaseLock: function() {
        this.locked = false;
    },

    bindEventsonRowSelection: function() {
        var that = this;
        $("body").unbind("keydown");

        $("body").keydown(function(e) {
            if ($("#sugServPanel").length > 0) {
                if ((e.which == 38 || e.which == 40)) { // up/down arrow pressed
                    $curSuggestionSelectedRow = that.getSelectedSuggestionRow();
                    if ($curSuggestionSelectedRow != null) {

                        if (e.which == 40) {//down arrow

                            if ($curSuggestionSelectedRow.next().index() != -1) {

                                $nextSelectedRow = $("#sugServPanel table").find("tr").eq($curSuggestionSelectedRow.next().index())
                                that.selectRow($nextSelectedRow);
                            } else {
                                if (that.headers)
                                    that.selectRow($("#sugServPanel table").find("tr:not(:first)").eq(0));
                                else
                                    that.selectRow($("#sugServPanel table").find("tr").eq(0));
                            }
                        } else if (e.which == 38) {//up arrow
                            if (that.headers) {
                                if ($curSuggestionSelectedRow.prev().index() > 0) {
                                    $nextSelectedRow = $("#sugServPanel table").find("tr").eq($curSuggestionSelectedRow.prev().index())
                                    that.selectRow($nextSelectedRow);
                                } else {
                                    that.selectRow($("#sugServPanel table").find("tr").last());
                                }

                            } else {
                                if ($curSuggestionSelectedRow.prev().index() != -1) {
                                    $nextSelectedRow = $("#sugServPanel table").find("tr").eq($curSuggestionSelectedRow.prev().index())
                                    that.selectRow($nextSelectedRow);
                                } else {
                                    that.selectRow($("#sugServPanel table").find("tr").last());
                                }
                            }
                        }
                    } else if ($curSuggestionSelectedRow == null || typeof ($curSuggestionSelectedRow) == "undefined") {

                        if (e.which == 40) {
                            if (that.headers)
                                that.selectRow($("#sugServPanel table").find("tr:not(:first)").eq(0));
                            else
                                that.selectRow($("#sugServPanel table").find("tr").eq(0));
                        } else if (e.which == 38)
                            that.selectRow($("#sugServPanel table").find("tr").last());

                    }

                    that.scrollSugPanel();
                }

            }
        });
    },

    createSuggestionPanel: function(dataTypeCode) {

        this.removeSuggestionPanel();

        $sugPanel = $('<div id="sugServPanel">');
        $sugPanel.css({
            "position": "absolute",
            "width": "270px",
            "height": "auto",
            "overflow": "auto",
            "overflow-x": "hidden",
            "z-index": 30,
            "border": "1px solid #333",
            "background": "#FFFFFF"
        });

        var $table = this.$dropDownTable.clone();
        $sugPanel.html($table);
        $table.show();
        this.$sourceTxtbox.parent().append($sugPanel);

        return $sugPanel;
    },

    regulateSugPanelHeight: function($sugPanel) {
        if ($sugPanel.innerHeight() > 250)
            $sugPanel.css("height", "250px");
    },

    finalizeSugPanel: function($sugPanel) {
        var that = this;

        if (this.headers) {
            $sugPanel.find("tr:not(:first)").hover(function() { that.selectRow($(this)); },
                                    function() {
                                        that.deselectRow($(this));
                                    });

            $sugPanel.find("tr:not(:first)").click(function() {
                that.writeSelectedRowToUserGridView();
            });
        } else {
            $sugPanel.find("tr").hover(function() { try { that.selectRow($(this)); } catch (e) { } },
                                    function() {
                                        try { that.deselectRow($(this)); } catch (e) { }
                                    });

            $sugPanel.find("tr").click(function() {
                that.writeSelectedRowToUserGridView();
            });
        }

        $sugPanel.scrollTop(0);
    },

    scrollSugPanel: function() {
        $sugPanel = $("#sugServPanel");
        $sugTable = $sugPanel.find("table")

        var Ypos = this.$selectedRow.offset().top - $sugTable.offset().top;

        //The sug table is divided into number of screens,depending on the sug panel height,
        //...each time a row is found on which screen it falls, and the sug panel scrolls up/down a number of screen to
        //....reach the screen containing the selected row...
        $sugPanel.scrollTop($sugPanel.innerHeight() * Math.floor(Ypos / $sugPanel.innerHeight()));
    },

    getSelectedSuggestionRow: function() {

        return this.$selectedRow;

    },

    selectRow: function($row) {
        if (typeof ($row) != "object" || $row == null) return;

        $curRow = this.getSelectedSuggestionRow();
        if ($curRow != null)
            this.deselectRow($curRow);

        $row.addClass("selected");
        $row.css(this.selectedRowStyle);
        this.$selectedRow = $row;
    },

    deselectRow: function($row) {
        if (typeof ($row) != "object" || $row == null) return;

        $row.removeClass("selected");
        $row.css(this.unselectedRowStyle);
        $("#sugServPanel").find("tr.selected").each(function() {
            $row = $(this);
            $row.removeClass("selected");
            $row.css(this.unselectedRowStyle);
        });
        this.$selectedRow = null;
    },

    suggest: function(dataTypeCode) {


        var $sugPanel = this.createSuggestionPanel(dataTypeCode);
        var foundRowsArray = new Array();

        if (dataTypeCode == "services") {
            foundRowsArray = this.searchKeywordOnHiddenGridView($(".pnlHiddenServiceCodes"))
        } else if (dataTypeCode == "items") {
            foundRowsArray = this.searchKeywordOnHiddenGridView($(".pnlHiddenItemCodes"));
        }

        var $sugTable = $sugPanel.find("table");
        if (foundRowsArray.length > 0) {
            for (rowIndex in foundRowsArray) {
                $sugTable.append(foundRowsArray[rowIndex]);
            }

            this.regulateSugPanelHeight($sugPanel);
            this.finalizeSugPanel($sugPanel);

        } else {
            this.removeSuggestionPanel();
            this.clearUserKeywordInputRow();
        }
        this.releaseLock();
    },

    clearUserKeywordInputRow: function() {
        if (this.$sourceTxtbox == null || typeof (this.$sourceTxtbox) != "object") return;
        $userInputRow = this.$sourceTxtbox.parent().parent();
        $userInputRow.find("td").each(function(i) {
            $(this).find("input[type=text]").val("");
        });
    },

    searchKeywordOnHiddenGridView: function($gv) {
        var foundRowsArray = new Array();
        var keyword = this.eventType == "click" ? "" : $.trim(this.$sourceTxtbox.val().toLowerCase());
        var $row = null;
        var tdVal = "";
        var keywordReg = new RegExp(keyword);

        $gv.find("tr:not(:first)").each(function(i) {
            $row = $(this);

            $row.find("td:not(:nth-child(3))").each(function(j) {
                tdVal = $.trim($(this).html().toLowerCase());

                if (keywordReg.test(tdVal)) {   ///tdVal.indexOf(keyword

                    foundRowsArray.push($row.clone(true, true));
                    return false;
                }
            });
        });
        return foundRowsArray;
    },

    removeSuggestionPanel: function() {
        if ($("#sugServPanel").length > 0) {
            $("#sugServPanel").remove();
        }
        this.$selectedRow = null;
        this.releaseLock();
    },

    writeSelectedRowToUserGridView: function() {
        $row = this.getSelectedSuggestionRow();
        if ($row == null || typeof ($row) == "undefined") {
            this.removeSuggestionPanel();
            return false;
        }

        this.$sourceTxtbox.val($row.find("td").eq(0).html() + "  " + $row.find("td").eq(1).html());
        this.$sourceTxtbox.parent().next().next().find("input[type=text]").val($row.find("td").eq(2).html());
        this.$sourceTxtbox.parent().next().find("input[type=text]").val(1);
        this.$sourceTxtbox.parent().next().find("input[type=text]").focus();

        this.removeSuggestionPanel();
        this.handleCallBack();

        return true;
    },

    handleCallBack: function() {
        this.fn();
    }
}  /* end of dropdown object */

/** Advance Search Sug >> START **/
var advSearch = {
               $ajax : { url: window.location, dataType: "json",type:"GET" },
               $sugDiv : null,
               $sug : null,
               $selectedRow: null,
               searchItem : function($txtbx){
               
               },
               searchService : function($txtbx){
               
               },
               init : function(){
                  var me = this;
                   $(".txtItemCode").keyup(function(e) {
                       try {
                            e.stopPropagation();
                            if (e.which === 38 || e.which === 40 || e.which === 13) return;
                           //if ($.trim($(this).val()) === "") {
                              //$(this).next().find(".AdvSearchSug").remove();
                             // return;
                            //}
                           var $txtItem = $(this);
                           me.$ajax.data = {RequestFor:"SuggestItemCodes","ItemCode":$(this).val()}
                           me.$ajax.done = function(data, textStatus, jqxhr) {
                             try{
                                   if( $("#divAdvSearchItemSug").length === 0 )
                                      me.$sugDiv = $("<div>",{"id":"divAdvSearchItemSug","class":"AdvSearchSug"});
                                   else
                                      me.$sugDiv = $("#divAdvSearchItemSug");
                                   me.$sugDiv.html("");
                                   $.each(data.Items, function(i, item) {
                                      me.$sug = $("<span>",{"id":"item-"+item.ItemID,"class":"sugItem"});
                                      me.$sug.html( item.ItemCode + " - " + item.ItemName );
                                      me.$sug.appendTo( me.$sugDiv );
                                   });
                                    if( me.$sugDiv.find("span").length == 0 )
                                      me.$sugDiv.html("<span style=\"padding:3px 2px;\">no suggested item code</span>");
                                    if( $txtItem.parent().find( me.$sugDiv ).length === 0 )
                                       $txtItem.next().append( me.$sugDiv );
                                       
                                  me.OnSuggestionSelect($("#divAdvSearchItemSug"),$txtItem);
                                  me.bindEventsonRowSelection($("#divAdvSearchItemSug"));
                                } catch (ex) { popup.alert(ex.message); }
                            }
                           sendAjax(me.$ajax);
                       } catch (ex) { popup.alert(ex.message); }
                   }).focus(function(){  $(".AdvSearchSug").remove(); }).click(function(){ $(this).trigger("keyup"); });
                   $(".txtServiceCode").keyup(function(e) {
                     try {
                           e.stopPropagation();
                           if (e.which === 38 || e.which === 40 || e.which === 13) return;
                           //if ($.trim($(this).val()) === ""){
                                //$(this).next().find(".AdvSearchSug").remove();
                                //return;
                            //}
                           var $txtService = $(this);
                           me.$ajax.data = {RequestFor:"SuggestServiceCodes","ServiceCode":$(this).val()}
                           me.$ajax.done = function(data, textStatus, jqxhr) {
                             try{
                                  if( $("#divAdvSearchServiceSug").length === 0 )
                                     me.$sugDiv = $("<div>",{"id":"divAdvSearchServiceSug","class":"AdvSearchSug"});
                                   else
                                     me.$sugDiv = $("#divAdvSearchServiceSug");
                                   me.$sugDiv.html("");
                                   $.each(data.Services, function(i, service) {
                                      me.$sug = $("<span>",{"id":"service-"+service.ServiceId,"class":"sugService"});
                                      me.$sug.html( service.ServCode + " - " + service.ServName );
                                      me.$sug.appendTo( me.$sugDiv );
                                   });
                                   if( me.$sugDiv.find("span").length == 0 )
                                      me.$sugDiv.html("<span style=\"padding:3px 2px;\">no suggested service code</span>");
                                    if( $txtService.parent().find( me.$sugDiv ).length === 0 )
                                        $txtService.next().append( me.$sugDiv );
                                        
                                 me.OnSuggestionSelect($("#divAdvSearchServiceSug"),$txtService);
                                 me.bindEventsonRowSelection($("#divAdvSearchServiceSug"));
                               } catch (ex) { popup.alert(ex.message); }
                            }
                            sendAjax(me.$ajax);
                       } catch (ex) { popup.alert(ex.message); }
                  }).focus(function(){  $(".AdvSearchSug").remove(); }).click(function(){ $(this).trigger("keyup"); });
            },
            getSelectedCode:function(){
               if( ! this.$selectedRow ) return "";
               var txt = this.$selectedRow.html();
               var codeParts = txt.split("-");
               if( codeParts[0] ) return $.trim( codeParts[0] );
               return "";
            },
            OnSuggestionSelect:function($curSugPanel,$txtbox){
                var me = this;
                $curSugPanel.find(".sugItem,.sugService").unbind("click").click(function(e){e.stopPropagation();});
                $curSugPanel.unbind("click").click(function(e){e.stopPropagation();});
                $curSugPanel.find(".sugItem,.sugService").unbind("click").click(function(){
                   try{
                        $txtbox.val( me.getSelectedCode() );
                        $(this).parent().remove();
                    } catch (ex) { popup.alert(ex.message); }
                });
                $curSugPanel.find(".sugItem,.sugService").unbind("keydown").keydown(function(e){
                   try{
                         if( e.which !== 13 ) return;
                         $txtbox.val( me.getSelectedCode() );
                         $(this).parent().remove();
                    } catch (ex) { popup.alert(ex.message); }
                });
            },
            bindEventsonRowSelection: function( $curSugPanel) {
                var me = this;
                $("body").unbind("keydown").keydown(function(e) {
                   try{
                   if(  $curSugPanel.find(".sugItem,.sugService").length == 0 ) return;
                     if ($curSugPanel.length > 0) {
                        if ((e.which == 38 || e.which == 40)) { // up/down arrow pressed
                            $curSuggestionSelectedRow = me.getSelectedSuggestionRow();
                            if ($curSuggestionSelectedRow != null) {
                                if (e.which == 40) {//down arrow
                                    if ($curSuggestionSelectedRow.next().index() != -1) {
                                        $nextSelectedRow = $curSugPanel.find(".sugItem,.sugService").eq($curSuggestionSelectedRow.next().index())
                                        me.selectRow($curSugPanel,$nextSelectedRow);
                                    } else {
                                        me.selectRow($curSugPanel,$curSugPanel.find(".sugItem,.sugService").eq(0));
                                    }
                                } else if (e.which == 38) {//up arrow
                                    if ($curSuggestionSelectedRow.prev().index() != -1) {
                                        $nextSelectedRow = $curSugPanel.find(".sugItem,.sugService").eq($curSuggestionSelectedRow.prev().index())
                                        me.selectRow($curSugPanel,$nextSelectedRow);
                                    } else {
                                        me.selectRow($curSugPanel,$curSugPanel.find(".sugItem,.sugService").last());
                                    }
                                }
                            } else if ($curSuggestionSelectedRow == null || typeof ($curSuggestionSelectedRow) == "undefined") {
                                if (e.which == 40) {
                                    me.selectRow($curSugPanel,$curSugPanel.find(".sugItem,.sugService").eq(0));
                                } else if (e.which == 38)
                                    me.selectRow($curSugPanel,$curSugPanel.find(".sugItem,.sugService").last());
                            }
                            me.scrollSugPanel($curSugPanel);
                        }

                     }
                   } catch (ex) { popup.alert(ex.message); }
                });
                $curSugPanel.find(".sugItem,.sugService").hover(function(e){
                    e.stopPropagation();
                    me.selectRow($curSugPanel,$(this));                    
                 },function(e){
                    e.stopPropagation();
                    me.deselectRow($curSugPanel,$(this));
                 });
          },
          scrollSugPanel: function($curSugPanel) {
                $sugPanel = $curSugPanel;
                var Ypos = this.$selectedRow.offset().top;
                //The sug table is divided into number of screens,depending on the sug panel height,
                //...each time a row is found on which screen it falls, and the sug panel scrolls up/down a number of screen to
                //....reach the screen containing the selected row...
                 $sugPanel.scrollTop($sugPanel.innerHeight() * Math.floor(Ypos / $sugPanel.innerHeight()));
           },
           getSelectedSuggestionRow: function() {
              return this.$selectedRow;
           },
           selectRow: function($curSugPanel,$row) {
                if (typeof ($row) != "object" || $row == null) return;

                $curRow = this.getSelectedSuggestionRow();
                if ($curRow != null)
                    this.deselectRow($curSugPanel,$curRow);

                $row.addClass("selected");
                this.$selectedRow = $row;
            },
            deselectRow: function($curSugPanel,$row) {
                if (typeof ($row) != "object" || $row == null) return;

                $row.removeClass("selected");
                $curSugPanel.find("span.selected").each(function() {
                    $row = $(this);
                    $row.removeClass("selected");
                });
                this.$selectedRow = null;
            }
          
   }
/** Advance Search Sug >> END **/