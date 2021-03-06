<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BusinessAdmin.Master"
    AutoEventWireup="true" CodeBehind="ConfigureTpk.aspx.cs" Inherits="e_Travel.BusinessAdmin.ConfigureTpk"
    ValidateRequest="false" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jQuery/jcarousellite/jquery.carouFredSel-6.0.4-packed.js"
        type="text/javascript"></script>
    <link href="../Scripts/jQuery/jcarousellite/jcarousellite.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/JqueyUI/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/JqueyUI/jquery-ui.js" type="text/javascript"></script>
    <link href="../Scripts/FancyBox/jquery.fancybox-1.3.4.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/FancyBox/jquery.fancybox.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FancyBox/jquery.fancybox.js" type="text/javascript"></script>
    <script src="../Scripts/FancyBox/jquery.fancybox.pack.js" type="text/javascript"></script>
    <script src="../Scripts/FancyBox/jquery.mousewheel-3.0.4.pack.js" type="text/javascript"></script>
    <script src="../Scripts/FancyBox/jquery.mousewheel-3.0.6.pack.js" type="text/javascript"></script>
    <link href="../Scripts/FancyBox/jquery.fancybox-buttons.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FancyBox/jquery.fancybox-buttons.js" type="text/javascript"></script>
    <script src="../Scripts/FancyBox/jquery.fancybox-media.js" type="text/javascript"></script>
    <link href="../Scripts/FancyBox/jquery.fancybox-thumbs.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FancyBox/jquery.fancybox-thumbs.js" type="text/javascript"></script>

    <link href="../Scripts/JCal/border-radius.css" rel="stylesheet" type="text/css" />
   
    <link href="../Scripts/JCal/jscal2.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/JCal/jscal2.js" type="text/javascript"></script>
    <link href="../Scripts/JCal/style.css" rel="stylesheet" type="text/css" />
     <script src="../Scripts/JCal/en.js" type="text/javascript"></script>


    <style type="text/css">
        .whiteFont
        {
            color: #ffffff;
        }
        .gridVehicleTypeSelect
        {
            background-image: url('Images/IconImage/dropdownicon.jpg');
            background-repeat: no-repeat;
            background-attachment: fixed;
            background-position: right;
            margin-left: 7px;
        }
        .gridAdvanceVehicleTypeSelect
        {
            margin-left: 7px;
        }
        .sendEmailGrid
        {
            margin-top: 10px;
            background-color: #ffffff;
            border: 1px solid #808080;
        }
        .gridTextBox, .gridAdvanceTextBox
        {
            background-image: url("../Images/IconImage/dropdownicon.jpg");
            background-position: right center;
            background-repeat: no-repeat;
            cursor: pointer;
        }
        .optionalPanel
        {
           display:none;
        }
        .captionLabel ,.CountPerson
        {
            display:none;
        }
        .addToPackage
        {
            margin-left:20px;
            display:inline-block;
        }
        
        
        
       /* .imageButton
        {
           
            background-image:url("../Images/IconImage/AddIcon.jpg");
            height:10px;
            width:10px;
        }*/
    </style>
    <script type="text/javascript">
        //Configuring Calendar Date Picker---------------------------------------------------------------------------

        function returnFormattedDate(dateToFormat) {
            return setTwoFigureDate(dateToFormat.getDate()) + '/' + setTwoFigureDate(dateToFormat.getMonth() + 1) + '/' + dateToFormat.getFullYear();
        }

        function setTwoFigureDate(value) {
            if (value.toString().length == 1) {
                return '0' + value;
            }
            else {
                return value;
            }
        }
        //------------------------------------------------------------------------------------------------

    
    </script>
    <script type="text/javascript">
        // Script Used For PDF Conversion
        function SetValues() {
            $('#SendMailPackageDetails').append($('.rightCol').clone());
            $('#SendMailPackageDetails fieldset').css('border', 'none');
        }
        function SetMailValues() {
                $('[id$=ucGridUpdateProgress]').css('display', 'block');
                $('[id$=MailAddressHiddenField]').val($('[id$=UserEmailTextBox]').val());
                $('[id$=MessageHiddenField]').val($('[id$=MessageTextBox]').val());
                $('[id$=CustomerNameHiddenField]').val($('[id$=CustNameTextBox]').val());
                $('[id$=CustomerNumberHiddenField]').val($('[id$=CustNumTextBox]').val());
                $('[id$=AdditonalCostHiddenField]').val($('[id$=AdditionalCostLabel]').text());
                $('[id$=GrandTotalHiddenField]').val($('[id$=GrandTotalAmountLabel]').text());
        }
    </script>
    <script type="text/javascript">

        $(document).ready(function () {
            $("#PackageDetail").fancybox({
                'titleShow': true,
                'transitionIn': 'elastic',
                'transitionOut': 'elastic'
            });
            $("#MailThis").fancybox({
                'titleShow': true,
                'transitionIn': 'elastic',
                'transitionOut': 'elastic'
            });
            $(function () {
                $(document).tooltip();
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $('#carousel ul').carouFredSel({
                prev: '#prev',
                next: '#next',
                pagination: "#pager",
                scroll: 1000,
                width: 560,
                items: 4
            });
        });
    </script>
    <script type="text/javascript">
        function pageLoad(sender, args) {

            // Calender 
            $(function () {

                var CheckinCalstartDate = new Date();
                var CheckinCalendDate = new Date();
                CheckinCalendDate.setDate(CheckinCalstartDate.getDate() + 365);
                CheckinCalstartDate.setDate(CheckinCalstartDate.getDate());

                if ($('[id$=ArrivalDateTextBox]').val() == '') {
                    var initStartDate = new Date();
                    initStartDate.setDate(initStartDate.getDate());
                    $('[id$=ArrivalDateTextBox]').val(returnFormattedDate(initStartDate));
                }
                if ($('[id$=AdvanceArrivalDateTextBox]').val() == '') {
                    var initStartDate = new Date();
                    initStartDate.setDate(initStartDate.getDate());
                    $('[id$=AdvanceArrivalDateTextBox]').val(returnFormattedDate(initStartDate));
                }
                var CheckinCalText = Calendar.setup({
                    inputField: $('[id$=ArrivalDateTextBox]').attr('id'),
                    dateFormat: "%d/%m/%Y",
                    trigger: $('[id$=ArrivalDateTextBox]').attr('id'),
                    bottomBar: false,
                    onSelect: function () {
                        var date = Calendar.intToDate(this.selection.get());
                    },
                    min: CheckinCalstartDate,
                    max: CheckinCalendDate
                });
                var AdvanceCheckinCalText = Calendar.setup({
                    inputField: $('[id$=AdvanceArrivalDateTextBox]').attr('id'),
                    dateFormat: "%d/%m/%Y",
                    trigger: $('[id$=AdvanceArrivalDateTextBox]').attr('id'),
                    bottomBar: false,
                    onSelect: function () {
                        var date = Calendar.intToDate(this.selection.get());
                    },
                    min: CheckinCalstartDate,
                    max: CheckinCalendDate
                });
            });


       //Add Additional Cost 
            $('.imageButton').click(function (event) {
                if ($(this).val() == 'Close') {
                    $(this).parent().parent().parent().find('.optionalPanel').css('display', 'none');
                    $(this).val('Add');
                }
                else {
                    var panel = $(this).parent().parent().find('.optionalPanel').css('display', 'block');
                    $(this).val('Close');
                    if ($(panel).find('.perPax').text() == 'True') {
                        $(panel).find('.captionLabel').css('display', 'inline-block');
                        $(panel).find('.CountPerson').css('display', 'inline-block');
                    }
                }

            });

            // Calculate Additonal Cost

            $('.addToPackage').click(function (event) {
                $('[id$=AdditionalCostPanel]').css('display', 'block');
                var cost;
                if ($(this).parent().parent().parent().find('.perPax').text() == 'True') {
                    cost = $(this).parent().parent().parent().find('.sellingPrice').text() * $(this).parent().find('.CountPerson').val();
                }
                else {

                    cost = $(this).parent().parent().parent().find('.sellingPrice').text() * $('[id$=VehicleCountHiddenField]').val();
                }
                $('[id$=AdditionalCostLabel]').text(parseFloat($('[id$=AdditionalCostLabel]').text()) + cost);
                $.Zebra_Dialog('This Point Is Added To Package');
                
                var panel = $(this).parent().parent().parent();
              
                $(panel).css('display', 'none');
                $(panel).parent().find('.imageButton').val("Add");
                $('[id$=GrandTotalAmountLabel]').text(parseFloat($('[id$=GrandTotalAmountLabel]').text()) + parseFloat(cost));
                $('[id$=SelectedSupplementHiddenField]').val($('[id$=SelectedSupplementHiddenField]').val() + "," + $(panel).parent().find('.idLabel').text());
            });

            $(".tab_content").hide();
            $("ul.tabs li:first").addClass("active").show();
            $(".tab_content:first").show();
            $("ul.tabs li").click(function () {
                $("ul.tabs li").removeClass("active");
                $(this).addClass("active");
                $('[id$=SelectedTabHiddenField]').val('.' + $(this).text());
                $(".tab_content").hide();
                var activeTab = $(this).find("a").attr("href");
                $(activeTab).fadeIn(500);
                return false;
            });

            $(".popuptab_content").hide();
            $("ul.popuptabs li:first").addClass("active").show();
            $(".popuptab_content:first").show();
            $("ul.popuptabs li").click(function (e) {
                $("ul.popuptabs li").removeClass("active");
                e.preventDefault();
                $(this).addClass("active");
                $(".popuptab_content").hide();
                var activeTab = $(this).find("a").attr("href");
                $(activeTab).fadeIn(500);
                return false;
            });
            if ($('[id$=ResultTabOpenHiddenField]').val() == "Open") {
                $('#PackageDetail').trigger('click');
            }
            if ($('[id$=SelectedTabHiddenField]').val() != "") {
                $("ul.tabs li").removeClass("active");
                $(".tab_content").hide();
                $($('[id$=SelectedTabHiddenField]').val()).addClass("active");
                var activeTab = $($('[id$=SelectedTabHiddenField]').val()).find("a").attr("href");
                $(activeTab).fadeIn(500);
            }
            $('[id$=MessageTextBox]').keypress(function (event) {
                var keycode = (event.keyCode ? event.keyCode : event.which);
                if (keycode == '13') {
                    $('[id$=SendMailButton]').trigger("click");
                }
            });

            //--------Loading Default Search Value On PostBack

            if ($('[id$=RoomHeaderQueryHiddenField]').val() != "") {
                $('#RoomHeader').children().remove();
                $('#RoomList').children().remove();
                $('#VehicleHeader').children().remove();
                $('#VehicleList').children().remove();
                $('#RoomHeader').append(localStorage.RoomHeader);
                $('#RoomList').append(localStorage.RoomDetail);
                $('#VehicleHeader').append(localStorage.VehicleHeader);
                $('#VehicleList').append(localStorage.VehicleDetail);
                suggestVehicleType();

                $('.gridSelect').each(function (index, value) {
                    $(value).val($('[id$=SelectedRoomListAdultHiddenField]').val().split(',')[index]);
                });
                $('.gridRoomSelect').each(function (index, value) {
                    $(value).val($('[id$=SelectedRoomListChildHiddenField]').val().split(',')[index]);
                });
                $('.gridTextBox').each(function (index, value) {
                    $(value).val($('[id$=VehicleDetailQueryHiddenField]').val().split(',')[index]);
                    createPax(value, $('[id$=VehiclePaxListHiddenField]').val().split(',')[index], '.gridVehicleTypeSelect');
                 });
                 $('.gridVehicleTypeSelect').each(function (index, value) {
                     $(value).val($('[id$=PaxListForVehicleHiddenField]').val().split(',')[index]);

                    
                 });

            }

            //-------Loading Advance Serach Details After PostBack 


            if ($('[id$=AdvanceRoomHeaderQueryHiddenField]').val() != "") {
                $('#AdvanceRoomHeader').children().remove();
                $('#AdvanceRoomList').children().remove();
                $('#AdvanceVehicleHeader').children().remove();
                $('#AdvanceVehicleList').children().remove();
                $('#AdvanceRoomHeader').append(localStorage.AdvanceRoomHeader);
                $('#AdvanceRoomList').append(localStorage.AdvanceRoomDetail);
                $('#AdvanceVehicleHeader').append(localStorage.AdvanceVehicleHeader);
                $('#AdvanceVehicleList').append(localStorage.AdvanceVehicleDetail);
                suggestAdvanceVehicleType();
                $('.gridAdvanceSelect').each(function (index, value) {
                    $(value).val($('[id$=AdvanceSelectedRoomListAdultHiddenField]').val().split(',')[index]);
                });
                $('[id$=AdvanceSelectedRoomListAdultHiddenField]').val("");
                $('.gridAdvanceRoomSelect').each(function (index, value) {
                    $(value).val($('[id$=AdvanceSelectedRoomListChildHiddenField]').val().split(',')[index]);
                });
                $('[id$=AdvanceSelectedRoomListChildHiddenField]').val("");
                $('.gridAdvanceTextBox').each(function (index, value) {
                    $(value).val($('[id$=AdvanceVehicleDetailQueryHiddenField]').val().split(',')[index]);
                     createPax(value, $('[id$=VehiclePaxListHiddenField]').val().split(',')[index] , '.gridAdvanceVehicleTypeSelect');
                });
                 $('.gridAdvanceVehicleTypeSelect').each(function (index, value) {
                    $(value).val($('[id$=PaxListForVehicleHiddenField]').val().split(',')[index]);
                    
                });
            }
            //------------------------------------------------------------------------------------
            //Default RoomNumber Change Event


            $('[id$=RoomNumberDropDownList]').change(function () {
                $('[id$=SelectedRoomListAdultHiddenField]').val("");
                $('[id$=SelectedRoomListChildHiddenField]').val("");
                $('#RoomHeader').children().remove();

                $('#RoomList').children().remove();
                localStorage.RoomDetail = "";
                var i;
                var loopUpto = $('[id$=RoomNumberDropDownList]').val();
                if (loopUpto > 0) {
                    $('[id$=RoomHeaderQueryHiddenField]').val("Yes");
                    localStorage.RoomHeader = '<hr style = "width: 246px; float: left; margin: 10px 0 5px" /><div style="width: 100%; padding: 0; margin: 0"><div style="width: 70px; display: inline-block; line-height: 20px;"></div><div style="width: 70px; display: inline-block; padding-left: 10px; line-height: 20px;">Adult</div><div style="width: 70px; display: inline-block; padding-left: 10px; line-height: 20px;">Child</div><div style="clear: both;"></div></div>';
                    $('#RoomHeader').append(localStorage.RoomHeader);
                }
                for (i = 1; i <= loopUpto; i++) {
                    // $('[id$=RoomDetailsQueryHiddenField]').val('<div style="width: 100%; padding: 0; margin: 0 0 8px"><div style="width: 70px; display: inline-block; line-height: 20px;">Room ' + i + '</div><div style="width: 70px; display: inline-block; padding-left: 10px; line-height: 20px;"><select class="gridSelect" style="width:70px;"><option value="0">0</option><option value="1">1</option><option value="2">2</option><option value="3">3</option></select></div><div style="width: 70px; display: inline-block; padding-left: 10px; line-height: 20px;"><select class="gridRoomSelect" style="width:83px;"><option value="0">0</option><option value="1">1</option><option value="2">2</option><option value=3">3</option></select></div><div style="clear: both;"></div></div>');
                    $('[id$=RoomHeaderQueryHiddenField]').val("Yes");
                    localStorage.RoomDetail = localStorage.RoomDetail + '<div style="width: 100%; padding: 0; margin: 0 0 8px"><div style="width: 70px; display: inline-block; line-height: 20px;">Room ' + i + '</div><div style="width: 70px; display: inline-block; padding-left: 10px; line-height: 20px;"><select class="gridSelect" style="width:70px;"><option value="0">0</option><option value="1">1</option><option value="2">2</option><option value="3">3</option></select></div><div style="width: 70px; display: inline-block; padding-left: 10px; line-height: 20px;"><select class="gridRoomSelect" style="width:83px;"><option value="0">0</option><option value="1">1</option><option value="2">2</option><option value=3">3</option></select></div><div style="clear: both;"></div></div>';
                }
                $('#RoomList').append(localStorage.RoomDetail);
            });

            //------------------------------------------------------------------------------------------
            // Advance RoomNumber Change Event
            $('[id$=AdvanceRoomNumberDropDownList]').change(function () {
                $('#AdvanceRoomHeader').children().remove();
                $('#AdvanceRoomList').children().remove();
                localStorage.AdvanceRoomDetail = "";
                var i;
                var loopUpto = $('[id$=AdvanceRoomNumberDropDownList]').val();
                if (loopUpto > 0) {
                    $('[id$=AdvanceRoomHeaderQueryHiddenField]').val("Yes");
                    localStorage.AdvanceRoomHeader = '<hr style = "width: 246px; float: left; margin: 10px 0 5px" /><div style="width: 100%; padding: 0; margin: 0"><div style="width: 70px; display: inline-block; line-height: 20px;"></div><div style="width: 70px; display: inline-block; padding-left: 10px; line-height: 20px;">Adult</div><div style="width: 70px; display: inline-block; padding-left: 10px; line-height: 20px;">Child</div><div style="clear: both;"></div></div>';
                    $('#AdvanceRoomHeader').append(localStorage.AdvanceRoomHeader);
                }
                for (i = 1; i <= loopUpto; i++) {

                    // $('[id$=RoomDetailsQueryHiddenField]').val('<div style="width: 100%; padding: 0; margin: 0 0 8px"><div style="width: 70px; display: inline-block; line-height: 20px;">Room ' + i + '</div><div style="width: 70px; display: inline-block; padding-left: 10px; line-height: 20px;"><select class="gridSelect" style="width:70px;"><option value="0">0</option><option value="1">1</option><option value="2">2</option><option value="3">3</option></select></div><div style="width: 70px; display: inline-block; padding-left: 10px; line-height: 20px;"><select class="gridRoomSelect" style="width:83px;"><option value="0">0</option><option value="1">1</option><option value="2">2</option><option value=3">3</option></select></div><div style="clear: both;"></div></div>');
                    $('[id$=AdvanceRoomHeaderQueryHiddenField]').val("Yes");
                    localStorage.AdvanceRoomDetail = localStorage.AdvanceRoomDetail + '<div style="width: 100%; padding: 0; margin: 0 0 8px"><div style="width: 70px; display: inline-block; line-height: 20px;">Room ' + i + '</div><div style="width: 70px; display: inline-block; padding-left: 10px; line-height: 20px;"><select class="gridAdvanceSelect" style="width:70px;"><option value="0">0</option><option value="1">1</option><option value="2">2</option><option value="3">3</option></select></div><div style="width: 70px; display: inline-block; padding-left: 10px; line-height: 20px;"><select class="gridAdvanceRoomSelect" style="width:83px;"><option value="0">0</option><option value="1">1</option><option value="2">2</option><option value=3">3</option></select></div><div style="clear: both;"></div></div>';
                }
                $('#AdvanceRoomList').append(localStorage.AdvanceRoomDetail);
            });

            //--------------------------------------------------------------------------------------------------------------------
            //   Default  Vehicle List DropDown Change Event

            $('[id$=NoOfVehicleDropDownList]').change(function () {

                //Check Whether No Of Vehicle Should Be Less Than Number Of Adult
                if ($('[id$=NoOfVehicleDropDownList]').val() > $('[id$=NoOfAdultDropDownList]').val()) {
                    $('[id$=NoOfVehicleDropDownList]').val(0);
                    $.Zebra_Dialog("Number Of Vehicle Could Not Be More Than Number Of Adult");
                    return false;
                }

                $('#VehicleHeader').children().remove();
                $('#VehicleList').children().remove();
                localStorage.VehicleDetail = "";
                var i;
                var loopUpto = $('[id$=NoOfVehicleDropDownList]').val();
                if (loopUpto > 0) {

                    $('[id$=RoomHeaderQueryHiddenField]').val("Yes");
                    localStorage.VehicleHeader = '<hr/><p style="margin-left:100px;display:inline-block;"> Vehicle Type</p>  <p style="margin-left:40px;display:inline-block;">  Pax</p>';
                    $('#VehicleHeader').append(localStorage.VehicleHeader);
                }
                for (i = 1; i <= loopUpto; i++) {
                    localStorage.VehicleDetail = localStorage.VehicleDetail + '<p>Vehicle ' + i + '<input class="gridTextBox" type="text"/><select class="gridVehicleTypeSelect" style="width:50px;"></select></p><div style="clear:both;height:2px;"></div>';
                   
                    $('[id$=RoomHeaderQueryHiddenField]').val("Yes");
                }
                $('#VehicleList').append(localStorage.VehicleDetail);
                $('[id$=SelectedVehicleTypeHiddenField]').val("");
                suggestVehicleType();
            });
            //----------------------------------------------------------------------------------------------------------
            //    Advance Vehicle List DropDown Change Event

            $('[id$=AdvanceNoOfVehicleDropDownList]').change(function () {

                if ($('[id$=AdvanceNoOfVehicleDropDownList]').val() > $('[id$=AdvanceNoOfAdultDropDownList]').val()) {
                    $('[id$=AdvanceNoOfVehicleDropDownList]').val(0)
                    $.Zebra_Dialog("Number Of Vehicle Could Not Be More Than Number Adult");
                    return false;
                }

                $('#AdvanceVehicleHeader').children().remove();
                $('#AdvanceVehicleList').children().remove();
                localStorage.AdvanceVehicleDetail = "";

                var i;
                var loopUpto = $('[id$=AdvanceNoOfVehicleDropDownList]').val();
                if (loopUpto > 0) {

                    $('[id$=AdvanceRoomHeaderQueryHiddenField]').val("Yes");
                    localStorage.AdvanceVehicleHeader = '<hr/><p style="margin-left:100px;display:inline-block;"> Vehicle Type</p>  <p style="margin-left:40px;display:inline-block;">  Pax</p>';
                    $('#AdvanceVehicleHeader').append(localStorage.AdvanceVehicleHeader);
                }
                for (i = 1; i <= loopUpto; i++) {
                    localStorage.AdvanceVehicleDetail = localStorage.AdvanceVehicleDetail + '<p>Vehicle ' + i + '<input class="gridAdvanceTextBox" type="text"/><select class="gridAdvanceVehicleTypeSelect" style="width:50px;"><option value="0">0</option><option value="1">1</option><option value="2">2</option><option value="3">3</option><option value="4">4</option><option value="5">5</option><option value="6">6</option><option value="7">7</option><option value="8">8</option></select></p><div style="clear:both;height:2px;"></div>';
                    $('[id$=AdvanceRoomHeaderQueryHiddenField]').val("Yes");
                }
                $('#AdvanceVehicleList').append(localStorage.AdvanceVehicleDetail);
                $('[id$=AdvanceSelectedVehicleListHiddenField]').val("");
                suggestAdvanceVehicleType();
            });
        }
        //---------------------------------------------------------------------------------------------
        // autosuggest VehicleType  for Default Search

        function suggestVehicleType() {
            $('.gridTextBox').each(function (index, value) {

                var projects = $.parseJSON($('[id$=VehicleTypeListHiddenField]').val());

                $(value).autocomplete({
                    minLength: 0,
                    source: projects,
                    focus: function (event, ui) {
                        $(value).val(ui.item.VehicleTypeName);
                        return false;
                    },
                    select: function (event, ui) {
                        $(value).val(ui.item.VehicleTypeName);
                        $('[id$=SelectedVehicleTypeHiddenField]').val($('[id$=SelectedVehicleTypeHiddenField]').val() + ui.item.VehicleTypeID + ',');
                        $('[id$=VehiclePaxListHiddenField]').val($('[id$=VehiclePaxListHiddenField]').val() + ui.item.VehicleCapacity + ',');
                        createPax(value, ui.item.VehicleCapacity, '.gridVehicleTypeSelect')
                        return false;
                    }
                })
            .data("ui-autocomplete")._renderItem = function (ul, item) {
                return $("<li>")
            .append("<a>" + item.VehicleTypeName + "<br/>" + "Max Pax :" + item.VehicleCapacity + "</a>")
            .appendTo(ul);
            };
            });
            $('.gridTextBox').each(function (index, value) {

                $(value).focus(function () {
                    if (index == 0) {
                        $('[id$=SelectedVehicleTypeHiddenField]').val("");
                    }
                    $(value).autocomplete("search", "");
                });
            });
        }
        //------------------------------------------------------------------------------------------------------------
        //----- AutoSuggest for VehicleType for Advance Search 

        function suggestAdvanceVehicleType() {
            $('.gridAdvanceTextBox').each(function (index, value) {

                var projects = $.parseJSON($('[id$=VehicleTypeListHiddenField]').val());
                $(value).autocomplete({
                    minLength: 0,
                    source: projects,
                    focus: function (event, ui) {
                        $(value).val(ui.item.VehicleTypeName);
                        return false;
                    },
                    select: function (event, ui) {
                        $(value).val(ui.item.VehicleTypeName);
                        $('[id$=SelectedVehicleTypeHiddenField]').val($('[id$=SelectedVehicleTypeHiddenField]').val() + ui.item.VehicleTypeID + ',');
                        $('[id$=VehiclePaxListHiddenField]').val($('[id$=VehiclePaxListHiddenField]').val() + ui.item.VehicleCapacity + ',');
                        createPax(value, ui.item.VehicleCapacity, '.gridAdvanceVehicleTypeSelect');
                        return false;
                    }
                })
            .data("ui-autocomplete")._renderItem = function (ul, item) {
                return $("<li>")
            .append("<a>" + item.VehicleTypeName + "<br/>" + "Max Pax :" + item.VehicleCapacity + "</a>")
            .appendTo(ul);
            };
            });
            $('.gridAdvanceTextBox').each(function (index, value) {
                $(value).focus(function () {
                    if (index == 0) {
                        $('[id$=SelectedVehicleTypeHiddenField]').val("");
                    }
                    $(value).autocomplete("search", "");

                });
            });
        }
        //-------------------------------------------------------------------------------------
        // Create paxdropdown list  for Selected Vehicle 
        function createPax(element, loopUpto, findClass) {
            $(element).parent().find(findClass).children().remove();
            var loopUpto = loopUpto;
            var element = $(element).parent().find(findClass);
            for (i = 1; i <= loopUpto; i++) {
                element.append('<option value=' + i + '>' + i + '</option>');
            }
        }

        //-----------------------------------------------------------------------------        

        // Validate Data for Default Search and Then Storint The Value In HiddenField
        function ValidateBasicData() {
            var Adult = $('[id$=NoOfAdultDropDownList]').val()
            var Child = $('[id$=NoOfChildDropDownList]').val()
            var AdultCount = 0;
            var ChildCount = 0;
            var TotalCount = parseInt(Adult) + parseInt(Child);
            var PaxCount = 0;
            var AdultRoomList = new Array();
            var ChildRoomList = new Array();
            var TotalRoom = $('[id$=RoomNumberDropDownList]').val();
            if (Child > 0) {
                //do nothing
            }
            else {
                Child = 0;
            }
            $('#RoomList select.gridSelect').each(function (index, value) {
                var t = $(value).val();
                t = parseInt(t);
                AdultRoomList[index] = t;
                AdultCount = AdultCount + t;

            });

            $('#RoomList select.gridRoomSelect').each(function (index, value) {
                var t = $(value).val();
                t = parseInt(t);
                ChildRoomList[index] = t;
                ChildCount = ChildCount + t;

            });
            $('.gridVehicleTypeSelect').each(function (index, value) {
                var t = $(value).val();
                t = parseInt(t);
                PaxCount = PaxCount + t;
            });

            for (i = 0; i < TotalRoom; i++) {
                var TotalAdult = AdultRoomList[i];
                var TotalChild = ChildRoomList[i];
                var TotalPerRoom = TotalAdult + TotalChild;
                if (TotalPerRoom > 4) {
                    $.Zebra_Dialog('Only 4 Person Per Room');
                    return false;
                }
                if (TotalAdult == 0) {
                    $.Zebra_Dialog('1 Adult Must Be Exist In Each Room');
                    return false;
                }
            }
            $('[id$=SelectedRoomListAdultHiddenField]').val("");
            $('[id$=SelectedRoomListChildHiddenField]').val("");
            $('[id$=SelectedRoomListHiddenField]').val("");
            $('[id$=PaxListForVehicleHiddenField]').val("");
            if ($('[id$=ArrivalDateTextBox]').val() == "") {
                $.Zebra_Dialog('Arrival Date Is Required');
                return false;
            }
            else if ($('[id$=AccommodationTypeDropDownList]').val() == "0") {
                $.Zebra_Dialog('Accomadation Type Is Required');
                return false;
            }
            else if ($('[id$=RoomPlanDropDownList]').val() == "0") {
                $.Zebra_Dialog('You Must Select Room Plan');
                return false;
            }
            else if ($('[id$=RoomNumberDropDownList]').val() == "0") {
                $.Zebra_Dialog('Provide Number Of Room  Required');
                return false;
            }
            else if ($('[id$=NoOfAdultDropDownList]').val() == "0") {
                $.Zebra_Dialog('Number Of Adult Cannot Be 0');
                return false;
            }

            else
                if (AdultCount > Adult) {
                    $.Zebra_Dialog('Total Adult(s) in Room is Greater than total adult Arriving');
                    return false;
                }
                else if (AdultCount != Adult) {
                    $.Zebra_Dialog('Total Adult(s) In Room Did Not Match With Total Arrival Adult(s)');
                    return false;
                }
                else if (ChildCount != Child) {
                    $.Zebra_Dialog('Total Childern(s) In Room Did Not Match With Arrival Childern(s)');
                    return false;
                }
                else if (ChildCount > Child) {
                    $.Zebra_Dialog('Total Childs in Room is Greater than total Child');
                    return false;
                }
                else if (TotalCount != PaxCount) {
                    $.Zebra_Dialog('Total Count And Pax Count Should Be Equal');
                    return false;
                }
                else {
                    $('#RoomList select.gridSelect').each(function (index, value) {
                        $('[id$=SelectedRoomListAdultHiddenField]').val($('[id$=SelectedRoomListAdultHiddenField]').val() + $(value).val() + ',');
                    });
                    $('#RoomList select.gridRoomSelect').each(function (index, value) {
                        $('[id$=SelectedRoomListChildHiddenField]').val($('[id$=SelectedRoomListChildHiddenField]').val() + $(value).val() + ',');
                    });
                    $('#VehicleList select').each(function (index, value) {
                        $('[id$=PaxListForVehicleHiddenField]').val($('[id$=PaxListForVehicleHiddenField]').val() + $(value).val() + ',');

                    });
                    $('.gridTextBox').each(function (index, value) {
                        $('[id$=VehicleDetailQueryHiddenField]').val($('[id$=VehicleDetailQueryHiddenField]').val() + $(value).val() + ',');
                    });
                    return true;
                }
        }
        //-----------------------------------------------------------------------------------------------
        // end of Default Search Here 

        //----------------------------------------------------------------------------

        //-- Validate Advance Search here
        function ValidateAdvanceData() {
            var Adult = $('[id$=AdvanceNoOfAdultDropDownList]').val()
            var Child = $('[id$=AdvanceNoOfChildDropDownList]').val()
            var AdultCount = 0;
            var ChildCount = 0;
            var TotalCount = parseInt(Adult) + parseInt(Child);
            var PaxCount = 0;
            var AdultRoomList = new Array();
            var ChildRoomList = new Array();
            var TotalRoom = $('[id$=AdvanceRoomNumberDropDownList]').val();

            if (Child > 0) {
                //do nothing
            }
            else {
                Child = 0;
            }
            $('#AdvanceRoomList select.gridAdvanceSelect').each(function (index, value) {
                var t = $(value).val();
                t = parseInt(t);
                AdultRoomList[index] = t;
                AdultCount = AdultCount + t;
            });

            $('#AdvanceRoomList select.gridAdvanceRoomSelect').each(function (index, value) {
                var t = $(value).val();
                t = parseInt(t);
                ChildRoomList[index] = t;
                ChildCount = ChildCount + t;

            });
            $('.gridAdvanceVehicleTypeSelect').each(function (index, value) {
                var t = $(value).val();
                t = parseInt(t);
                PaxCount = PaxCount + t;
            });
            for (i = 0; i < TotalRoom; i++) {
                var TotalAdult = AdultRoomList[i];
                var TotalChild = ChildRoomList[i];
                var TotalPerRoom = TotalAdult + TotalChild;
                if (TotalPerRoom > 4) {
                    $.Zebra_Dialog('Only 4 Person Per Room');
                    return false;
                }
                if (TotalAdult == 0) {
                    $.Zebra_Dialog('1 Adult Must Be Exist In Each Room');
                    return false;
                }
            }
            $('[id$=SelectedRoomListHiddenField]').val("");
            $('[id$=PaxListForVehicleHiddenField]').val("");
            $('[id$=DayCountHiddenField]').val("");
            $('[id$=SelectAccomadationListHiddenField]').val("");
            $('[id$=AdvanceSelectedRoomListAdultHiddenField]').val("");
            $('[id$=AdvanceSelectedRoomListChildHiddenField]').val("");

            if ($('[id$=AdvanceArrivalDateTextBox]').val() == "") {
                $.Zebra_Dialog('Arrival Date Is Required');
                return false;
            }

            else if ($('[id$=AdvanceRoomPlanDropDownList]').val() == "0") {
                $.Zebra_Dialog('You Must Select Room Plan');
                return false;
            }
            else if ($('[id$=AdvanceRoomNumberDropDownList]').val() == "0") {
                $.Zebra_Dialog('Provide Number Of Room  Required');
                return false;
            }
            else if ($('[id$=AdvanceNoOfAdultDropDownList]').val() == "0") {
                $.Zebra_Dialog('Number Of Adult Cannot Be 0');
                return false;
            }
            else if (AdultCount > Adult) {
                $.Zebra_Dialog('Total Adult(s) in Room is Greater than total adult Arriving');
                return false;
            }
            else if (AdultCount != Adult) {
                $.Zebra_Dialog('Total Adult(s) In Room Did Not Match With Total Arrival Adult(s)');
                return false;
            }
            else if (ChildCount != Child) {
                $.Zebra_Dialog('Total Childern(s) In Room Did Not Match With Arrival Childern(s)');
                return false;
            }
            else if (ChildCount > Child) {
                $.Zebra_Dialog('Total Childs in Room is Greater than total Child');
                return false;
            }
            else if (TotalCount != PaxCount) {
                $.Zebra_Dialog('Pax Number Should Be Equal To Total Person');
                return false;
            }

            else {
                $('#AdvanceRoomList select.gridAdvanceSelect').each(function (index, value) {
                    $('[id$=AdvanceSelectedRoomListAdultHiddenField]').val($('[id$=AdvanceSelectedRoomListAdultHiddenField]').val() + $(value).val() + ',');
                });
                $('#AdvanceRoomList select.gridAdvanceRoomSelect').each(function (index, value) {
                    $('[id$=AdvanceSelectedRoomListChildHiddenField]').val($('[id$=AdvanceSelectedRoomListChildHiddenField]').val() + $(value).val() + ',');
                });
                $('#AdvanceVehicleList select').each(function (index, value) {
                    $('[id$=PaxListForVehicleHiddenField]').val($('[id$=PaxListForVehicleHiddenField]').val() + $(value).val() + ',');
                });
                $('.gridAdvanceTextBox').each(function (index, value) {
                    $('[id$=AdvanceVehicleDetailQueryHiddenField]').val($('[id$=AdvanceVehicleDetailQueryHiddenField]').val() + $(value).val() + ',');
                });
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel ID="PageUpdatePanel" runat="server">
<ContentTemplate>
    <div class="FromBody">
        <div class="BoxHeader">
            Search Packages</div>
        <div class="BoxBody">
            <div class="BoxContent">
                <div class="leftCol">
                    <div class="tabscontainer" id="fix">
                        <ul class="tabs">
                            <li class="Default"><a href="#tab1">Default</a></li>
                            <li class="Advance"><a href="#tab2">Advance</a></li>
                        </ul>
                        <div class="tab_container">
                            <div style="display: none;" id="tab1" class="tab_content">
                                <div class="searchformTourPackage">
                                    <input type="hidden" value="INT" id="hdsearchFor" name="ctl00$ContentPlaceHolder1$ucHotelSearchControl1$hdsearchFor" />
                                    <h3>
                                        Configure Package (Basic)</h3>
                                    <div class="searchform_innerTourPackage">
                                        <div style="padding: 5px; float: left; width: 246px;">
                                            <div style="width: 100%; padding: 0; margin: 0; float: left; height: 24px;">
                                                <label for="ArrivalDateTextBox" style="width: 246px;">
                                                    Arrival Date</label>
                                            </div>
                                            <div style="margin-bottom: 10px; width: 100%; height: 24px; float: left;">
                                        
                                                <asp:TextBox ID="ArrivalDateTextBox" runat="server" CssClass="arrivaldate" name="ArrivalDateTextBox"></asp:TextBox>
                                              
                                                
                                            </div>
                                            <div style="width: 100%; padding: 0; margin: 0">
                                                <div style="width: 70px; display: inline-block; line-height: 20px;">
                                                    Adult
                                                </div>
                                                <div style="width: 70px; display: inline-block; padding-left: 10px; line-height: 20px;">
                                                    Child
                                                </div>
                                                <div style="width: 70px; display: inline-block; padding-left: 10px; line-height: 20px;">
                                                    Infant
                                                </div>
                                                <div style="clear: both;">
                                                </div>
                                            </div>
                                            <div style="margin-bottom: 10px; width: 100%;">
                                                <div style="width: 70px; display: inline-block;">
                                                    <asp:DropDownList ID="NoOfAdultDropDownList" runat="server" Width="70px">
                                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div style="width: 70px; display: inline-block; padding-left: 10px;">
                                                    <asp:DropDownList ID="NoOfChildDropDownList" runat="server" Width="70px">
                                                        <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div style="width: 70px; display: inline-block; padding-left: 10px;">
                                                    <asp:DropDownList ID="NoOfInfantDropDownList" runat="server" Width="76px">
                                                        <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div style="clear: both;">
                                                </div>
                                            </div>
                                            <div style="width: 100%; padding: 0; margin: 0; float: left; height: 24px;">
                                                <label for="AccommodationTypeDropDownList" style="width: 246px;">
                                                    Accomadation Type</label>
                                            </div>
                                            <div style="margin-bottom: 10px; width: 100%; height: 24px; float: left;">
                                                <asp:DropDownList ID="AccommodationTypeDropDownList" runat="server" AppendDataBoundItems="true"
                                                    Width="246px">
                                                </asp:DropDownList>
                                            </div>
                                            <div style="width: 100%; padding: 0; margin: 0">
                                                <div style="width: 160px; display: inline-block; line-height: 20px;">
                                                    Room Plan
                                                </div>
                                                <div style="width: 66px; display: inline-block; padding-left: 10px; line-height: 20px;">
                                                    Room(s)
                                                </div>
                                                <div style="clear: both;">
                                                </div>
                                            </div>
                                            <div style="width: 100%; padding: 0; margin: 0">
                                                <div style="width: 160px; display: inline-block; line-height: 20px;">
                                                    <asp:DropDownList ID="RoomPlanDropDownList" runat="server" AppendDataBoundItems="true"
                                                        Width="160px">
                                                    </asp:DropDownList>
                                                </div>
                                                <div style="width: 66px; display: inline-block; padding-left: 10px; line-height: 20px;">
                                                    <asp:DropDownList ID="RoomNumberDropDownList" runat="server" Width="68px">
                                                        <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                        <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                        <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                        <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div style="clear: both;">
                                                </div>
                                            </div>
                                            <div style="clear: both;">
                                            </div>
                                            <div id="RoomHeader">
                                            </div>
                                            <div id="RoomList">
                                            </div>
                                            <hr style="width: 246px; float: left; margin: 10px 0 0px" />
                                            <div style="width: 100%; padding: 0; margin: 0; float: left; height: 24px;">
                                                <label for="NoOfVehicleDropDownList" style="width: 246px;">
                                                    Number of Vehicle Required</label>
                                            </div>
                                            <div style="margin-bottom: 10px; width: 100%; height: 24px; float: left;">
                                                <asp:DropDownList ID="NoOfVehicleDropDownList" runat="server" AppendDataBoundItems="true"
                                                    Width="160px">
                                                    <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                    <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div style="clear: both;">
                                            </div>
                                            <div id="VehicleHeader">
                                            </div>
                                            <div id="VehicleList">
                                            </div>
                                            <hr style="width: 246px; float: left; margin: 10px 0 0px" />
                                            <div style="margin: 10px 0px 0px 0px; width: 100%; float: left; text-align: right;">
                                                <asp:Button ID="BacisPackageConfireButton" runat="server" Text=" Calculate Package Cost "
                                                    CssClass="FromBodyButton" ValidationGroup="ValidateData" OnClientClick="return ValidateBasicData();"
                                                    OnClick="BacisPackageConfireButton_Click" />
                                            </div>
                                        </div>
                                        <div style="clear: both">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div style="display: none;" id="tab2" class="tab_content">
                                <div class="searchformTourPackage">
                                    <input type="hidden" value="INT" id="Hidden1" name="ctl00$ContentPlaceHolder1$ucHotelSearchControl1$hdsearchFor" />
                                    <h3>
                                        Configure Package (Advance)</h3>
                                    <div class="searchform_innerTourPackage">
                                        <div style="display: block; min-height: 214px; padding: 1px;" class="IntDomContent"
                                            id="Div4">
                                            <div style="width: 100%; padding: 0; margin: 0; float: left; height: 24px;">
                                                <label for="AdvanceArrivalDateTextBox" style="width: 246px;">
                                                    Arrival Date</label>
                                            </div>
                                            <div style="margin-bottom: 10px; width: 100%; height: 24px; float: left;">
                                                <asp:TextBox ID="AdvanceArrivalDateTextBox" runat="server"></asp:TextBox>
                                                
                                            </div>
                                            <div style="width: 100%; padding: 0; margin: 0">
                                                <div style="width: 70px; display: inline-block; line-height: 20px;">
                                                    Adult
                                                </div>
                                                <div style="width: 70px; display: inline-block; padding-left: 10px; line-height: 20px;">
                                                    Child
                                                </div>
                                                <div style="width: 70px; display: inline-block; padding-left: 10px; line-height: 20px;">
                                                    Infant
                                                </div>
                                                <div style="clear: both;">
                                                </div>
                                            </div>
                                            <div style="margin-bottom: 10px; width: 100%;">
                                                <div style="width: 70px; display: inline-block;">
                                                    <asp:DropDownList ID="AdvanceNoOfAdultDropDownList" runat="server" Width="70px">
                                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div style="width: 70px; display: inline-block; padding-left: 10px;">
                                                    <asp:DropDownList ID="AdvanceNoOfChildDropDownList" runat="server" Width="70px">
                                                        <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div style="width: 70px; display: inline-block; padding-left: 10px;">
                                                    <asp:DropDownList ID="AdvanceNoOfInfantDropDownList" runat="server" Width="76px">
                                                        <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div style="clear: both;">
                                                </div>
                                            </div>
                                           
                                            <div class="row" style="margin-bottom: 10px;">
                                                <asp:GridView ID="DestinationWiseAccomodationGridView" runat="server" AutoGenerateColumns="false"
                                                    BorderWidth="0px" GridLines="None" CellPadding="0" CssClass="GridStyle">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                            <fieldset>
                                                            <legend>
                                                            Day <asp:Label ID="DayNoLabel" runat="server" Text='<%#Eval("DayNo") %>'></asp:Label>
                                                            (<asp:Label ID="DestinationNameLabel" runat="server" Text='<%#Eval("DestinationName") %>'></asp:Label>)
                                                            </legend>
                                                            <div style="display:none;">
                                                            <asp:Label ID="DayDestinationLabel" runat="server" Text='<%#Eval("DestinationID") %>'></asp:Label>
                                                            </div>
                                                            Accomadation Type
                                                             <div style="clear:both;height:5px;"></div>
                                                            <asp:DropDownList ID="DayAccTypeDropDownList" runat="server" AppendDataBoundItems="true" OnSelectedIndexChanged="DayAccTypeDropDownList_SelectedIndexChanged" AutoPostBack="true" Width="220px">
                                                            
                                                            </asp:DropDownList>
                                                            <div style="clear:both; height:5px;"></div>
                                                            Accomodation Name
                                                             <div style="clear:both;height:5px;"></div>
                                                            <asp:UpdatePanel ID="AccUpdatePanel" runat="server" >
                                                            <ContentTemplate>
                                                             <asp:DropDownList ID="AccDropDownList" runat="server" AppendDataBoundItems="true" Width="220px" CssClass="advanceDestination"></asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DayAccTypeDropDownList" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                            </asp:UpdatePanel>
                                                          

                                                            </fieldset>
                                                               
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                            <div style="width: 100%; padding: 0; margin: 0">
                                                <div style="width: 160px; display: inline-block; line-height: 20px;">
                                                    Room Plan
                                                </div>
                                                <div style="width: 66px; display: inline-block; padding-left: 10px; line-height: 20px;">
                                                    Room(s)
                                                </div>
                                                <div style="clear: both;">
                                                </div>
                                            </div>
                                            <div style="width: 100%; padding: 0; margin: 0">
                                                <div style="width: 160px; display: inline-block; line-height: 20px;">
                                                    <asp:DropDownList ID="AdvanceRoomPlanDropDownList" runat="server" AppendDataBoundItems="true"
                                                        Width="160px">
                                                    </asp:DropDownList>
                                                </div>
                                                <div style="width: 66px; display: inline-block; padding-left: 10px; line-height: 20px;">
                                                    <asp:DropDownList ID="AdvanceRoomNumberDropDownList" runat="server" Width="68px">
                                                        <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                        <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                        <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                        <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div style="clear: both;">
                                            </div>
                                            <div style="clear: both;">
                                            </div>
                                            <div id="AdvanceRoomHeader">
                                            </div>
                                            <div id="AdvanceRoomList">
                                            </div>
                                            <hr style="width: 246px; float: left; margin: 10px 0 0px" />
                                            <div style="width: 100%; padding: 0; margin: 0; float: left; height: 24px;">
                                                <label for="AdvanceNoOfVehicleDropDownList" style="width: 246px;">
                                                    Number of Vehicle Required</label>
                                            </div>
                                            <div style="margin-bottom: 10px; width: 100%; height: 24px; float: left;">
                                                <asp:DropDownList ID="AdvanceNoOfVehicleDropDownList" runat="server" AppendDataBoundItems="true"
                                                    Width="160px">
                                                    <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                    <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div style="clear: both;">
                                            </div>
                                            <div id="AdvanceVehicleHeader">
                                            </div>
                                            <div id="AdvanceVehicleList">
                                            </div>
                                            <hr style="width: 246px; float: left; margin: 10px 0 0px" />
                                            <div style="margin: 10px 0px 0px 0px; width: 100%; float: left; text-align: right;">
                                                <asp:Button ID="AdvancePackageConfireButton" runat="server" Text=" Calculate Package Cost "
                                                    CssClass="FromBodyButton" ValidationGroup="ValidateData" OnClientClick="return ValidateAdvanceData();"
                                                    OnClick="AdvancePackageConfireButton_Click" />
                                            </div>
                                            <div style="clear: both;">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="rightCol">
                    <div style="float: left; border: 1px solid #808080; width: 655px; background-color: #ffffff">
                        <div style="background-color: #5C5C5C; height: 40px; padding-left: 10px; font-size: 17px;
                            font-weight: bold; width: 625px; line-height: 36px; margin: 10px;">
                            <div style="height: 26px; margin: 5px 0 9px; width: 26px; float: left;">
                                <img src="../App_Themes/AdminThemeDefault/Images/ThumbUp.png" alt="Thumb Up" />
                            </div>
                            <div style="height: 40px; width: 599px; float: left; line-height: 36px;">
                                &nbsp;&nbsp;<asp:Label ID="PackageNameLabel" runat="server" ForeColor="#28a1c4"></asp:Label>
                                <asp:Label ID="PackageDurationLabel" runat="server" ForeColor="#FFFFFF"></asp:Label>
                            </div>
                        </div>
                        <div style="width: 595px; margin: 10px 5px 10px 45px;">
                            <div id="wrapper">
                                <div id="carousel">
                                    <div id="gallery" style="text-align: center; width: 100%; margin-top: 20px;">
                                        <asp:Literal ID="TpPhotoLiteral" runat="server"></asp:Literal>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                    <a id="prev" class="prev" href="#"></a><a id="next" class="next" href="#"></a>
                                    <div id="pager" class="pager">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="test3" style="width: 645px; margin: 10px;">
                            <asp:FormView ID="PackageDetailsFormView" runat="server" Width="100%">
                                <ItemTemplate>
                                    <fieldset>
                                        <table cellpadding="5px" cellspacing="0">
                                            <tr>
                                                <td colspan="3" style="width: 590px; margin: 0px 10px 20px 15px; font-weight: bold;
                                                    font-size: 14pt; color: #1b96ba; padding-bottom: 15px;">
                                                    Tour Package Details
                                                </td>
                                            </tr>
                                            <tr style="background-color: #F6F2F3;">
                                                <td width="45px" style="border-bottom: 1px Solid #afadad; border-top: 1px Solid #afadad;">
                                                    <asp:Image ID="Image7" runat="server" ImageUrl="~/Images/IconImage/pkgDesc.jpg" alt="Package Description Image" />
                                                </td>
                                                <td width="100px" style="border-bottom: 1px Solid #afadad; border-top: 1px Solid #afadad;">
                                                    Package Description
                                                </td>
                                                <td style="width: 460px; text-align: justify; padding: 10px 5px 10px 5px; border-bottom: 1px Solid #afadad;
                                                    border-top: 1px Solid #afadad;">
                                                    <%# Eval("TpkDesc")%>
                                                </td>
                                            </tr>
                                            <tr style="height: 40px; margin: 5px 0 5px;">
                                                <td width="45px" style="border-bottom: 1px Solid #afadad">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/IconImage/DestCovered.jpg"
                                                        alt="Destination Covered Image" />
                                                </td>
                                                <td width="100px" style="border-bottom: 1px Solid #afadad">
                                                    Destinations Covered
                                                </td>
                                                <td style="float: left; text-align: left; padding: 5px 0 5px 10px; width: 460px;
                                                    overflow: hidden; line-height: 30px; border-bottom: 1px Solid #afadad;">
                                                    <%# Eval("TpkDestCovered")%>
                                                </td>
                                            </tr>
                                            <tr style="height: 40px; margin: 5px 0 5px; background-color: #F6F2F3;">
                                                <td width="45px" style="border-bottom: 1px Solid #afadad">
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/IconImage/PickupPoint.jpg"
                                                        alt="Pickup Point Image" />
                                                </td>
                                                <td width="100px" style="border-bottom: 1px Solid #afadad">
                                                    Pickup Point
                                                </td>
                                                <td style="float: left; text-align: left; padding: 5px 0 5px 10px; width: 460px;
                                                    overflow: hidden; line-height: 30px; border-bottom: 1px Solid #afadad;">
                                                    <%# Eval("TpkPickUpPoint")%>
                                                </td>
                                            </tr>
                                            <tr style="height: 35px; margin: 5px 0 0px;">
                                                <td width="45px">
                                                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/IconImage/DropPoint.jpg"
                                                        alt="Drop Point Image" />
                                                </td>
                                                <td width="100px">
                                                    Drop Point
                                                </td>
                                                <td style="float: left; text-align: left; margin: 5px 0 0 10px; width: 460px; overflow: hidden;
                                                    line-height: 30px;">
                                                    <%# Eval("TpkDropPoint")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </ItemTemplate>
                            </asp:FormView>
                        </div>
                        <div id="PackageItineary" style="width: 645px; margin: 5px;">
                            <fieldset>
                                <div style="width: 590px; margin: 0px 10px 10px 5px; font-weight: bold; font-size: 14pt;
                                    color: #1b96ba; height: 25px;">
                                    Tour Package Itinerary
                                </div>
                                <asp:GridView ID="ItineraryDetailsFGridView" runat="server" AutoGenerateColumns="false"
                                    ShowHeader="false" PageSize="20">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <table style="width: 615px;" cellpadding="0" cellspacing="0">
                                                    <tr style="height: 35px; line-height: 35px;">
                                                        <td style="width: 80px; background-color: #5cb2ea; text-align: center; font-size: 10pt;
                                                            font-weight: bold;" class="whiteFont">
                                                            Day &nbsp;<asp:Label ID="DayLabel" runat="server" Text='<%#Eval("DayNo") %>'></asp:Label>
                                                        </td>
                                                        <td style="width: 533px; margin-left: 2px; padding-left: 10px; background-color: #5C5C5C;
                                                            line-height: 35px; text-align: left; font-weight: bold;" class="whiteFont">
                                                            <%#Eval("ItineraryHeading") %>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="color: #656464; width: 70px; font-weight: bold; text-align: left; background-color: #F6F2F3;
                                                            padding-left: 10px; font-size: 9pt; padding-top: 10px; padding-bottom: 10px;
                                                            border-bottom: 1px Solid #afadad">
                                                            Activity
                                                        </td>
                                                        <td style="width: 525px; padding: 10px; background-color: #F6F2F3; text-align: justify;
                                                            border-bottom: 1px Solid #afadad">
                                                            <%#Eval("ItineraryDetail")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="color: #656464; width: 80px; font-weight: bold; text-align: center; font-size: 10pt;
                                                            padding-top: 10px; padding-bottom: 10px;">
                                                            Overnight
                                                        </td>
                                                        <td style="width: 525px; padding: 10px; text-align: justify;">
                                                            <%#Eval("DestinationName") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </fieldset>
                        </div>
                    </div>
                </div>
                <div style="clear: both">
                </div>
            </div>
        </div>
    </div>

    <div style="display: none">
        <a href="#CompleteDetailsDiv" id="PackageDetail" onclick="SetValues();">Click Here</a>
    </div>
    <div style="display: none">
        <div id="CompleteDetailsDiv">
            <div id="SendMailPackageDetails">
            </div>
            <div style="clear: both; height: 10px;">
            </div>
            <div id="SemdMailCostDescription">
                <div class="popuptabscontainer" id="Div1">
                    <ul class="popuptabs">
                        <li class="NetCost"><a href="#NetCost">Cost Details</a></li>
                        <li class="Inclusion"><a href="#Inclusion">Inclusion/Exclusion</a></li>
                        <li class="OptionalSightSeen"><a href="#SightSeen">Optional SightSeen</a></li>
                        <li class="Accomadation"><a href="#Accomadation">Accomodation </a></li>
                        <li class="Transport"><a href="#Transport">Transport</a></li>
                        
                    </ul>
                    <div class="popuptab_container" style="min-height:250px;">
                     <%-- Cost Tab   --%>
                    <div style="display: none;" id="NetCost" class="popuptab_content">
                            <div class="searchformTourPackage" style="width: 640px;min-height:200px;">
                                <h3>
                                    Net Cost Details
                                </h3>

                                <div class="searchform_innerTourPackage" style="width: 98%;">
                                    <div class="flR clearFix right_part">
                                        <!-- Heading -->
                                       <div style="float: right;padding-bottom: 5px;color: #CB3904;font-size:16px;">
                                        <asp:Label ID="ArrivalDateLabel" runat="server"></asp:Label>
                                        </div>
                                        <div style="clear:both;height:10px;"></div>
                                        <asp:Label ID="AdultNumberLabel" runat="server" Text="" Width="200px" ForeColor="#CB3904"></asp:Label>
                                        <asp:Label ID="ChildNumberLabel" runat="server" Text="" Width="200px" ForeColor="#CB3904"></asp:Label>
                                        <asp:Label ID="InfantNumberLabel" runat="server" Text="" Width="200px" ForeColor="#CB3904"></asp:Label>
                                        <div style="clear:both;height:15px;"></div>
                                        <hr />
                                        <div class="travellerDetails rowHead append_bottomHalf clearFix flL">
                                            <table>
                                            <tr>
                                            <td>
                                            
                                            <p style="padding: 5px; margin: 0; text-align: center; line-height: 20px; float: left;
                                                width: 450px; display: inline-block;">
                                                Particulars </p>
                                                </td>
                                                <td>
                                            <p style="padding: 5px; margin: 0; text-align: center; line-height: 20px; float: left;
                                                width: 150px; display: none;">
                                                Total Cost</p>
                                                </td>
                                                <td>
                                            <p style="padding: 5px; margin: 0; text-align: center; line-height: 20px; float: left;
                                                width: 150px; display: none;">
                                                Discount</p>
                                                </td>
                                                <td>
                                            <p style="padding: 5px; margin: 0; text-align: center; line-height: 20px; float: left;
                                                width: 150px; display: inline-block;">
                                               Total Cost</p>
                                                </td>
                                            </tr>
                                            </table>
                                            <hr style="clear: both; width: 600px; color: #E6E6E6;" />
                                            <asp:GridView ID="CostDetailsGridView" runat="server" AutoGenerateColumns="false" ShowHeader="false"
                                                GridLines="None" EmptyDataText="No Other Cost Applicable" Style="float: left"  >
                                                <Columns>
                                                    <asp:TemplateField>
                                                   
                                                        <ItemTemplate>
                                                            <p style="padding: 5px; margin: 0; line-height: 20px; float: left;
                                                                width: 450px; color: #878787 !important;">
                                                                <asp:Label ID="TypeLabel" runat="server" Text='<%#Eval("Type") %>'></asp:Label>
                                                            </p>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                  
                                                        <ItemTemplate>
                                                            <p style="padding: 5px; margin: 0; line-height: 20px; float: left;
                                                                width: 150px; color: #878787 !important;">
                                                                <asp:Label ID="TotalCostLabel" runat="server" Text='<%#Eval("TotalAmount") %>'></asp:Label>
                                                            </p>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField  Visible="false">
                                                    
                                                        <ItemTemplate>
                                                            <p style="padding: 5px; margin: 0; line-height: 20px; float: left;
                                                                width: 150px; color: #878787 !important;">
                                                                <asp:Label ID="DiscountAmountLabel" runat="server" Text='<%#Eval("Discount") %>'></asp:Label></p>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                    <HeaderTemplate>
                                                     Cost 
                                                    </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <p style="padding: 5px; margin: 0; line-height: 20px; float: left;
                                                                width: 150px; color: #878787 !important; text-align:center;">
                                                                <asp:Label ID="AmountAfterDiscountLabel" runat="server" Text='<%#Eval("AmountAfterDiscount") %>'></asp:Label></p>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <asp:Panel ID="RapCostPanel" runat="server">
                                        <div style="clear: both;">
                                        </div>
                                         <p style="padding: 5px; margin: 0; line-height: 20px; float: left; width:450px;color: #878787 !important;display:inline-block;">
                                             Extra /RAP Cost
                                            </p>
                                        <p style="padding: 5px; margin: 0; text-align: center; line-height: 20px; float: right;
                                            color: #878787 !important;display:inline-block; width:150px;">
                                         <%--   <span style="margin: 0px 80px 0 100px">--%>
                                                <asp:Label ID="RapCostLabel" runat="server" Text="Rs.0.00"></asp:Label>
                                           <%-- </span>--%>
                                        </p>
                                        </asp:Panel>
                                        <asp:Panel ID="AdditionalCostPanel" runat="server" Visible="true" style="display:none" >
                                        <div style="clear:both;"></div>
                                        <p style="padding: 5px; margin: 0;  line-height: 20px; float: left; width:450px;color: #878787 !important;display:inline-block;">
                                              Supplement Cost For Selected Supplement Points
                                            </p>
                                        <p style="padding: 5px; margin: 0; text-align: center; line-height: 20px; float: right;
                                            color: #878787 !important;display:inline-block; width:150px;">
                                           <%-- <span style="margin: 0px 80px 0 100px">--%>
                                                <asp:Label ID="AdditionalCostLabel" runat="server" Text="0.00"></asp:Label>
                                           <%-- </span>--%>
                                        </p>
                                        </asp:Panel>
                                        <asp:Panel ID="PackageDiscountPanel" runat="server">                                        <div style="clear:both;"></div>
                                         <p style="padding: 5px; margin: 0;  line-height: 20px; float: left; width:450px;color: #878787 !important;display:inline-block;">
                                              Package Discount
                                            </p>
                                         <p style="padding: 5px; margin: 0; text-align: center; line-height: 20px; float: right;
                                            color: #878787 !important;display:inline-block; width:150px;">
                                          <%-- <span style="margin: 0px 80px 0 100px">--%>
                                                <asp:Label ID="PackageDiscountLabel" runat="server" Text="0.00"></asp:Label>
                                           <%-- </span>--%>
                                        </p>
                                        </asp:Panel>
                                        <asp:Panel ID="PackageAdditionalCostPanel" runat="server">                                        <div style="clear:both;"></div>
                                         <p style="padding: 5px; margin: 0;  line-height: 20px; float: left; width:450px;color: #878787 !important;display:inline-block;">
                                              
                                              <asp:Label ID="PackageAdditonialRemarksLabel" runat="server" Text="" ></asp:Label>
                                            </p>
                                         <p style="padding: 5px; margin: 0; text-align: center; line-height: 20px; float: right;
                                            color: #878787 !important;display:inline-block; width:150px;">
                                          <%-- <span style="margin: 0px 80px 0 100px">--%>
                                                <asp:Label ID="PackageAdditionlCostLabel" runat="server" Text="0.00"></asp:Label>
                                           <%-- </span>--%>
                                        </p>
                                        </asp:Panel>
                                        <div style="clear: both;">
                                        </div>
                                        <div class="netCost">
                                            <div style="clear: both;">
                                            </div>
                                            <hr />
                                           
                                            <p style="color: #CB3904; font-size: 0px; float: right; display:none;">
                                                <font style="float: left; text-align: left; margin-right: 20px;"> Total</font><span
                                                    style="float: right; text-align: right;"><span class="INR"> Rs. &nbsp;</span><span
                                                        id="Span1"><asp:Label ID="TotalAmountLabel" runat="server" Text="" ></asp:Label></span></span>
                                            </p>
                                            <div style="clear:both;"></div>
                                           
                                           
                                              <div style="clear:both;"></div>
                                              <p style="color: #CB3904; font-size: 20px; float: right;margin:10px 30px;">
                                                <font style="float: left; text-align: left; margin-right: 20px;">Grant Total</font><span
                                                    style="float: right; text-align: right;"><span class="INR"> Rs. &nbsp;</span><span
                                                        id="Span3"><asp:Label ID="GrandTotalAmountLabel" runat="server" Text="0.00"></asp:Label></span></span>
                                            </p>
                                            <%--<p style="color: #CB3904; font-size: 20px; float: right;margin:10px 30px;">
                                                <font style="float: left; text-align: left; margin-right: 20px;"> In Words:</font><span
                                                    style="float: right; text-align: right;"><span class="INR">  &nbsp;</span><span
                                                        id="Span2"><asp:Label ID="AmountInWordsLabel" runat="server" Text="0.00"></asp:Label></span></span>
                                            </p>--%>
                                              <div style="clear:both;"></div>
                                              <%--User Email Dtls  --%>
                                              <div style="width:100%;display:inline-block;">
                                            <hr />
                                            
                                            <div style="vertical-align:top;margin-top:20px;float:left;">Customer Name </div>
                                            <asp:TextBox ID="CustNameTextBox" runat="server" Text="" Width="400px" Style="margin-top: 20px;float:right;"></asp:TextBox>
                                            <div style="clear:both;"></div>
                                            <div style="vertical-align:top;margin-top:20px;float:left;"> User Email</div>
                                            <asp:TextBox ID="UserEmailTextBox" runat="server" Text="" Width="400px" Style="margin-top: 20px;float:right;"></asp:TextBox>
                                            <div style="clear:both;"></div>
                                          
                                             <div style="vertical-align:top;margin-top:20px;float:left;">Customer Number </div>
                                            <asp:TextBox ID="CustNumTextBox" runat="server" Text="" Width="400px" Style="margin-top: 20px;float:right;"></asp:TextBox>
                                            <div style="clear:both;"></div>
                                            <div style="vertical-align:top;margin-top:20px;float:left;">Type Message To Your Client</div> 
                                            <asp:TextBox ID="MessageTextBox" runat="server" Width="400px" TextMode="MultiLine" Height="150px" Style="margin-top: 20px;float:right;"   ></asp:TextBox>
                                            <div style="clear:both;height:20px;""></div>
                                            <asp:Button runat="server" Text="Send Mail" ID="SendMailButton" OnClick="SendMailButton_Click"
                                            UseSubmitBehavior="false"    OnClientClick="SetMailValues();" style="float:right;" />
                                           
                                            </div>
                                        </div>
                                    </div>
                                    <div style="clear: both;">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%-- Inclusion And Exclusion  Tab   --%>
                        <div style="display: none;" id="Inclusion" class="popuptab_content">
                            <div class="searchformTourPackage" style="width: 640px;min-height:200px;">
                                <h3>
                                    Inclusion And Exclusion
                                </h3>
                                <div class="searchform_innerTourPackage" style="width: 98%;">
                                   Incluision
                                   <hr />
                                    <asp:GridView ID="InclusionGridView" runat="server" AutoGenerateColumns="false" GridLines="None" EmptyDataText="No Inclusion Available">
                                    <Columns>
                                    <asp:TemplateField>
                                    <ItemTemplate>
                                        <img src="../Images/IconImage/bulletIcon.png" style="border:none;margin-top:-4px;" />   <asp:Label ID="ItemNameLabel" runat="server" Text='<%#Eval("ItemName") %>'  ToolTip='<%#Eval("ItemDesc") %>'></asp:Label>
                                    
                                    </ItemTemplate>
                                    
                                    </asp:TemplateField>
                                    </Columns>
                                    
                                    </asp:GridView>
                                    <div style="clear:both; height:20px;" ></div>
                                     Exclusion
                                     <hr />
                                    <asp:GridView ID="ExclusionGridView" runat="server" AutoGenerateColumns="false" GridLines="None" EmptyDataText="No Exclusion Availbale">
                                    <Columns>
                                    <asp:TemplateField>
                                    <ItemTemplate>
                                   <img src="../Images/IconImage/bulletIcon.png" style="border:none;margin-top:-4px;" />  <asp:Label ID="ItemNameLabel" runat="server" Text='<%#Eval("ItemName") %>' ToolTip='<%#Eval("ItemDesc") %>'></asp:Label>
                                    
                                    </ItemTemplate>
                                    
                                    </asp:TemplateField>
                                    </Columns>
                                    
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <%-- Supplement Sight Seen Tab   --%>
                        <div style="display: none;" id="SightSeen" class="popuptab_content">
                            <div class="searchformTourPackage" style="width: 640px;min-height:200px;">
                                <h3>
                                    Optional SightSeen
                                </h3>
                                <div class="searchform_innerTourPackage" style="width: 98%">
                                  
                                   <asp:GridView ID="OptionalGridView" runat="server" AutoGenerateColumns="false" GridLines="None" EmptyDataText="No Optional Points Available For Selected Package In This Time Period" Width="100%">
                                   <Columns>
                                   <asp:TemplateField>
                                   <HeaderTemplate>
                                   List Of All Optional SightSeens Available For This Package
                                   <hr  style="margin-top:10px;margin-bottom:10px"/>
                                   </HeaderTemplate>
                                   
                                   <ItemTemplate>
                                   <div style="width:96%; border-radius:10px;padding:10px;border:solid 1px #E6E6E6;margin-bottom:10px">
                                   <img src="../Images/IconImage/bulletIcon.png"  style="border:none;"/> 
                                   <asp:Label ID="SuppPointsNameLabel" runat="server" Text='<%# Eval("SupplementName") %>' Font-Size="16px"></asp:Label>
                                   <div style="clear:both;"></div> 

                                   <asp:Label ID="SuppPointsDescLabel" runat="server" Text='<%#Eval("SupplementDesc") %>' style="padding-left:40px;word-break:break-all;"></asp:Label>
                                   <div style="float:right">
                                   <asp:Button ID="AddButton" runat="server" Text="Add"   CssClass="imageButton" />
                                
                                   </div>
                                   <div style="clear:both;"></div>
                                   <asp:Panel ID="SupplementDtlPanel" runat="server" Visible="true" CssClass="optionalPanel" >
                                   <div style="display:none;">
                                   <asp:Label ID="SupplementIDLabel" runat="server" Text='<%#Eval("SupplementID") %>' CssClass="idLabel"></asp:Label>
                                   Is Per Pax   <asp:Label ID="IsPerPaxLabel" runat="server" Text='<%#Eval("IsPerPax") %>' CssClass="perPax"></asp:Label>
                                   Selling Price <asp:Label ID="SellingPriceLabel" runat="server" Text='<%#Eval("SellingPrice") %>' CssClass="sellingPrice"></asp:Label>  
                                 </div>
                                 <div>
                                 <div style="width:100%;display:inline-block;">
                                 <asp:Label ID="CaptionLabel" runat="server" Text="People Doing Optional SightSeen" CssClass="captionLabel" ></asp:Label>
                                 <asp:TextBox ID="CountPersonTextBox" runat="server" CssClass="CountPerson" MaxLength="2"></asp:TextBox>
                                 <ajaxToolkit:FilteredTextBoxExtender ID="FilterTextBox" runat="server" TargetControlID="CountPersonTextBox" FilterType="Numbers" FilterMode="ValidChars"></ajaxToolkit:FilteredTextBoxExtender>
                                 <asp:Button ID="AddToPackageButton" runat="server" Text="Add To Package"  CssClass="addToPackage" />
                                  </div>
                                  
                                   </asp:Panel>
                                   </div>

                                   <div>
                                   
                                   
                                   
                                                                  
                                   </div>
                                   </ItemTemplate>
                                   
                                   </asp:TemplateField>
                                   </Columns>
                                  
                                   </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <%-- Accomadation Tab   --%>
                        <div style="display: none;" id="Accomadation" class="popuptab_content">
                            <div class="searchformTourPackage" style="width: 640px;min-height:200px;">
                                <h3>
                                    Hotels provided in our packages
                                </h3>
                                <div class="searchform_innerTourPackage" style="width: 98%;">
                                    <asp:Panel ID="BasicAccPanel" runat="server">
                                        <asp:GridView ID="AccomadationGridView" runat="server" AutoGenerateColumns="false"
                                            ShowHeader="false" GridLines="None" Width="100%">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <div style="display: none;">
                                                            <asp:Label ID="DestinationIDLabel" runat="server" Text='<%#Eval("DestinationID") %>'></asp:Label>
                                                        </div>
                                                        <asp:Label ID="DestinationNameLabel" runat="server" Text='<%#Eval("DestinationName") %>'
                                                            Font-Size="20px"></asp:Label>
                                                           
                                                          
                                                          ( <asp:Label ID="AccTypeNameLabel" runat="server" Text='<%#Eval("AccTypeName") %>' Font-Size="16px"></asp:Label>, <asp:Label ID="RoomPlanLabel" runat="server" Text='<%#Eval("RoomPlanName") %>' Font-Size="16px"></asp:Label>)
                                                           
                                                            <hr />
                                                        <div style="float: right; width: 98%;">
                                                        <asp:DataList runat="server" ID="AccListDataList" RepeatColumns="3">
                                                        <ItemTemplate>
                                                        <div style="border-radius: 5px; border: solid 1px #E6E6E6; width: 150px;  padding: 10px; margin: 10px; text-align:center;">
                                                                                <asp:Image ID="AccomadationImage" runat="server" Height="80px" ImageUrl='<%# String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String((byte[])Eval("ThumbImage"))) %>' style="float:none;margin:0px;" />
                                                                                <br />
                                                                                <asp:Label ID="AccNameLabel" runat="server" Text='<%#Eval("AccName") %>'></asp:Label>
                                                                                <br />
                                                                                <asp:Label ID="AccDescLabel" runat="server" Text='<%#Eval("AccAddress") %>'></asp:Label>
                                                                            </div>

                                                       
                                                        </ItemTemplate>
                                                        
                                                        </asp:DataList>
                                                            
                                                        </div>
                                                        <div style="clear:both;height:5px;"></div>
                                                        <asp:Label ID="NoteLabel" runat="server" Text ="Note :" Visible='<%# !String.IsNullOrEmpty(Convert.ToString(Eval("Remarks"))) %>'></asp:Label>
                                                         <asp:Label ID="AccRemarksLabel" runat="server" Text='<%#Eval("Remarks") %>'></asp:Label>
                                                          <div style="clear:both;height:5px;"></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                    <asp:Panel ID="AdvanceAccPanel" runat="server">
                                  
                                        <asp:GridView ID="DayAccDtlsGridView" runat="server" AutoGenerateColumns="false"
                                            GridLines="None" Width="100%">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <div>
                                                            Day
                                                            <asp:Label ID="AccDayNoLabel" runat="server" Text='<%#Eval("DayNo")%>' Font-Size="20px"></asp:Label>
                                                            
                                                           (<asp:Label ID="AccTypeNameLabel" runat="server" Text='<%#Eval("AccTypeName") %>'  Font-Size="16px"></asp:Label>,
                                                            
                                                            <asp:Label ID="PlanNameLabel" runat="server" Text='<%#Eval("RoomPlanName") %>' Font-Size="16px"></asp:Label>
                                                            )
                                                            <hr />
                                                            <div style="display:none;">
                                                            <asp:Label ID="AccImageLabel" runat="server" Text='<%#Convert.ToBase64String((byte[])Eval("ThumbImage")) %>'></asp:Label>
                                                            </div>
                                                            <div style="border-radius: 5px; border: solid 1px #E6E6E6; height: 90px; width: 400px;
                                                                padding: 10px; margin: 10px; float:right;">
                                                                <asp:Image ID="AccomadationImage" runat="server" Height="80px" ImageUrl='<%# String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String((byte[])Eval("ThumbImage"))) %>' />
                                                                <asp:Label ID="AccNameLabel" runat="server" Text='<%#Eval("AccName") %>'></asp:Label>
                                                                <br />
                                                                <asp:Label ID="AccDescLabel" runat="server" Text='<%#Eval("AccAddress") %>'></asp:Label>
                                                                
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                        <%-- Vehicle  Tab   --%>
                        <div style="display: none;" id="Transport" class="popuptab_content">
                            <div class="searchformTourPackage" style="width: 640px;min-height:200px;">
                                <h3>
                                    Vehicles Provided In Our packages
                                </h3>
                                <div class="searchform_innerTourPackage" style="width: 98%">
                                    <asp:GridView ID="VehicleGridView" runat="server" AutoGenerateColumns="false" GridLines="None"
                                        ShowHeader="false" Width="100%">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    Day
                                                    <asp:Label ID="DayNoLabel" runat="server" Text='<%#Eval("DayNo") %>' Font-Size="20px"></asp:Label>
                                                   <hr />
                                                    <asp:GridView ID="VehicleDtlsGridView" runat="server" AutoGenerateColumns="false"
                                                        GridLines="None" ShowHeader="false" Width="100%">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <div style="display: none;">
                                                                        <asp:Label ID="OrgItineraryTypeLabel" runat="server" Text='<%#Eval("ItineraryType") %>'> </asp:Label>
                                                                    </div>
                                                                    <asp:Label ID="ItineraryTypeLabel" runat="server" Text='<%#this.ItineraryTypeText(Convert.ToString(Eval("ItineraryType")))%>'
                                                                        Visible='<%#this.ItineraryTypeVisible(Convert.ToString(Eval("ItineraryType")))%>'
                                                                        Font-Size="16px"></asp:Label>
                                                                    <div style="border-radius: 5px; border: solid 1px #E6E6E6; min-height: 130px; width: 450px;
                                                                        padding: 10px; margin: 10px; float: right;">
                                                                        <asp:GridView ID="VehicleFinalDtlsGridView" runat="server" AutoGenerateColumns="false"
                                                                            GridLines="None">
                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                  
                                                                                        <div style="width:400px;">
                                                                                           
                                                                                            <asp:Image ID="VehicleImage" runat="server" ImageUrl='<%# String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String((byte[])Eval("VehicleImgThumb"))) %>' />
                                                                                            <asp:Label ID="VehicleNameLabel" runat="server" Text='<%#Eval("VehicleName") %>'></asp:Label>
                                                                                            <br />
                                                                                            <asp:Label ID="VehicleRemarksLabel" runat="server" Text='<%#Eval("Remarks") %>'></asp:Label>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                       
                        
                    </div>
                </div>
            </div>
        </div>
        
        
    </div>
    <asp:HiddenField ID="ResultTabOpenHiddenField" runat="server" />
    <asp:HiddenField ID="PaxListForVehicleHiddenField" runat="server" />
    <asp:HiddenField ID="PaxListForVehicleHiddenField2" runat="server" />
    <asp:HiddenField ID="SelectedRoomListAdultHiddenField" runat="server" />
    <asp:HiddenField ID="SelectedRoomListChildHiddenField" runat="server" />
    <asp:HiddenField ID="VehicleTypeListHiddenField" runat="server" />
    <asp:HiddenField ID="SelectedVehicleTypeHiddenField" runat="server" />
    <asp:HiddenField ID="SelectedDetailVehicleTypeHiddenField" runat="server" />
    <asp:HiddenField ID="SelectAccomadationListHiddenField" runat="server" />
    <asp:HiddenField ID="DayCountHiddenField" runat="server" />
    <asp:HiddenField ID="RoomHeaderQueryHiddenField" runat="server" />
    <asp:HiddenField ID="RoomDetailsQueryHiddenField" runat="server" />
    <asp:HiddenField ID="VehicleHeaderQueryHiddenField" runat="server" />
    <asp:HiddenField ID="VehicleDetailQueryHiddenField" runat="server" />
    <asp:HiddenField ID="AdvanceRoomHeaderQueryHiddenField" runat="server" />
    <asp:HiddenField ID="AdvanceSelectedVehicleListHiddenField" runat="server" />
    <asp:HiddenField ID="AdvanceSelectedRoomListAdultHiddenField" runat="server" />
    <asp:HiddenField ID="AdvanceSelectedRoomListChildHiddenField" runat="server" />
    <asp:HiddenField ID="AdvanceVehicleDetailQueryHiddenField" runat="server" />
    <asp:HiddenField ID="SelectedTabHiddenField" runat="server" />
    <asp:HiddenField ID="MailAddressHiddenField" runat="server" />
    <asp:HiddenField ID="MailQueryHiddenField" runat="server" />
    <asp:HiddenField ID="PackageItinearyMailQueryHiddenField" runat="server" />
    <asp:HiddenField ID="SendFinalCostMailQueryHiddenField" runat="server" />
    <asp:HiddenField ID="MessageHiddenField" runat="server" />
    <asp:HiddenField ID="VehicleCountHiddenField" runat="server" />
    <asp:HiddenField ID="SelectedSupplementHiddenField" runat="server" />
   <asp:HiddenField ID="AdditonalCostHiddenField" runat="server" />
   <asp:HiddenField ID="GrandTotalHiddenField" runat="server" />
   <asp:HiddenField ID="VehiclePaxListHiddenField" runat="server" />
   <asp:HiddenField ID="CustomerNameHiddenField" runat="server" />
   <asp:HiddenField ID="CustomerNumberHiddenField" runat="server" />
   </ContentTemplate>

</asp:UpdatePanel>
    <asp:UpdateProgress ID="ucGridUpdateProgress" runat="server">
        <ProgressTemplate>
            <div style="z-index: 10000; position: absolute; height: 42px; width: 220px; top: 10%;
                left: 40%; background-color: White; border: 1px solid #000000; -moz-border-radius: 7px;
                -webkit-border-radius: 7px; -khtml-border-radius: 7px; border-radius: 7px;">
                <img style="padding: 5px;" alt="Loading, Please Wait..." src="<%= ResolveClientUrl("~/Images/loading.gif")%>" />
                <img style="padding: 5px;" alt="Loading, Please Wait..." src="<%= ResolveClientUrl("~/Images/loadingText.gif")%>" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
                                            
</asp:Content>
