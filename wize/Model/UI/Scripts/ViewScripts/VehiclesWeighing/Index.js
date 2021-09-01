var items;
var current;
var yard;
var mode = '1';
var firstUserDefaultScaleLoad = true;
var websocket;
var GUID = 4;
var weight = new Weight();


$(document).ready(function () {
    current = new Object();
    Shared();
    writePermission = ReadWritePermissions(company_forms_data, GUID, 'w');
    var data = InvisibleFields(4);
    $.each(data, function (i, val) {
        $('.' + val.FormsFieldName).hide();
    });
    weight.WeightDisplay = $('#weight');

    LoadComboboxes();
    LoadScales();
    ManualChange(true);

    if (!writePermission) {
        $("#in_weight").prop('disabled', true);
        $("#items").prop('disabled', true);
        $("#out_weight").prop('disabled', true);
        $("#netto").prop('disabled', true);
        //current.ScaleID = 1;//$("#scales").val();
        $("#transports").prop('disabled', true);
      
            $("#vehicles").prop('disabled', true);
      
            $("#reference").prop('disabled', true);
            $("#remarks").prop('disabled', true);
            $("#insites").prop('disabled', true);
            $("#outsites").prop('disabled', true);
            $("#customers").prop('disabled', true);
            $("#drivers").prop('disabled', true);

            $("#items").next().children('input').prop('disabled', true);
            $("#transports").next().children('input').prop('disabled', true);

            $("#insites").next().children('input').prop('disabled', true);
            $("#outsites").next().children('input').prop('disabled', true);
            $("#customers").next().children('input').prop('disabled', true);
            $("#drivers").next().children('input').prop('disabled', true);
            $("#btnWeigh").prop('disabled', true);
            $("#btnSave").prop('disabled', true);
            $("#btnSaveAndPrint").prop('disabled', true);
            $("#btnYard").prop('disabled', true);
            $("#btnTareUpdate").prop('disabled', true);
            $("#btnSaveTare").prop('disabled', true);

    }

    $(".mode_item").first().addClass("font-weight-bold");
    UpdateMode();
    $('input[type=radio]').change(function(e) {
        if (this.name == 'modes') {
            mode = ($(this).attr("mode"));
            UpdateMode();
        }
        else if (this.name == 'weightmode') {
            UpdateManual();
        }
    });

    $('#scales_carousel').on('slid.bs.carousel', function (ev) {
        console.log($(".carousel-inner .active div").attr('id'));
        SetScale();
    });

    $(".mode_item").on('click', function () {
        $(".mode_item").removeClass("font-weight-bold");
        $(this).addClass("font-weight-bold");
    });

    $("#items").combobox();
    $("#transports").combobox();
    $("#vehicles").combobox();
    $("#insites").combobox();
    $("#outsites").combobox();
    $("#customers").combobox();
    $("#drivers").combobox();
    
    ChangesEvents();
    OnFocusEvents();
   /* $("#vehicles").next().children('input').autocomplete({
        change: function () {
            for (var i = 0; i < yard.length; i++) {
                if (yard[i].VehicleID == $("#vehicles").val()) {
                    {
                        Get(yard[i]);
                        return;
                    }
                    
                }
            }  
        }
    });*/
    
    $("#scales").change(function () {
        SetScale();
    });

    $("#out_weight").on('change', function () {
        CalcWeight();
    });

    $("#in_weight").on('change', function () {
        CalcWeight();
    });
    
    $("#btnSave").on('click', function () {
        
        GetData();
        console.log(current.InDate);
        Save(current, false);
        Yard();
    });

    $(".yard-list-group").on('click', 'a', function () {
        current = new Object();
        current.GUID = $(this).attr('val');
        Get(current);
        mode = 1;
        UpdateMode();
    });
    
    $("#btnWeigh").on('click', function () {
        var weight = $("#weight").text();
        CalcWeight(weight);
    });


    $("#btnCertificates").on('click', function () {
        window.location.href = "Certificates";
    });

    $("#btnReports").on('click', function () {
        window.location.href = "Reports";
    });

    
    $("#btnSaveAndPrint").on('click', function () {
        GetData();
        Save(current, true);
        Yard();
    });

    
    $("#btnYard").on('click', function () {
        $("#yard").modal();
    });

    
    $("#btnTareUpdate").on('click', function () {    
        var vehicle = $("#vehicles").val();
        if (vehicle == undefined || vehicle <= 0)
        {
            alert(dictionary.MissingVehicle);
        }
        else {            
            var tare = $("#vehicles option:selected").attr('tare');
            $("#v_tare").val(tare);
            $("#tare").modal();
        }
    });

    
    $("#btnSaveTare").on('click', function () {
        SaveTare();
    });
   
    
    $("#ismanual").change(function () {
        ManualChange(!$(this).is(":checked"));
        console.log("ismanual changed");
    });

    Yard();


    ////////////////////// making labels editable
    
    /////////////////////


}); 

/////

/////

function ManualChange(isManual)
{
    $("#in_weight").prop('disabled', isManual);
    $("#out_weight").prop('disabled', isManual);
    $("#netto").prop('disabled', isManual);
    if (isManual) {
        weight.ScaleID = $(".carousel-inner .active div").attr('id');
        weight.Start(weight);
        $("#btnWeigh").show();
    }
    else {
        weight.Stop();
        $("#btnWeigh").hide();
    }
}

function UpdateManual()
{
  /* $("#in_weight").prop('disabled', !$("#manual").is(":checked"));
    $("#out_weight").prop('disabled', !$("#manual").is(":checked"));
    $("#netto").prop('disabled', !$("#manual").is(":checked"));
    if (!$("#manual").is(":checked")) {
        
        weight.ScaleID = $(".carousel-inner .active div").attr('id');//$("#scales").val();
        SetScale();
        $("#btnWeigh").show();
        $("#weight_wrapper").show();
        $("#scales_wrapper").show();
    }
    else {
        weight.Stop();
        $("#btnWeigh").hide();
        $("#weight_wrapper").hide();
        $("#scales_wrapper").hide();
    }*/
}

function SetUserScale()
{
    $.ajax({
        type: 'POST',
        url: '/Users/DefaultScale',
        contentType: 'application/json; charset=utf-8',
        async:false,
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                if (data.scale != null && data.scale != 0) {


                    $("#scales_carousel").carousel(GetIndexByScaleID(data.scale));
                    /*if ($("#scales").val() == null)
                    {
                        if(firstUserDefaultScaleLoad == true)
                        {
                            firstUserDefaultScaleLoad = false;
                            SetUserScale();
                        }
                    }*/
                        

                   // SetScale();
                }
                else
                {
                   // document.getElementById("auto").checked = false;
                   // document.getElementById("manual").checked = true;
                   // UpdateManual();
                }
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function SetScale() {
    weight.Stop();
    weight.ScaleID = $(".carousel-inner .active div").attr('id');//$("#scales").val();
    if (weight.ScaleID == 0 || weight.ScaleID == undefined)
    {
        $("#weight").text("- - -");
        current.IsManual = true;        
    }
    else
    {
        console.log(weight.ScaleID);
        current.IsManual = false;
        //weight.Start(weight);
    }
    
    ManualChange(!current.IsManual);
    /*$("#in_weight").prop('disabled', !(current.IsManual == true));
    $("#out_weight").prop('disabled', !(current.IsManual == true));
    $("#netto").prop('disabled', !(current.IsManual == true));*/
    
}

$(window).on('load', function () {
    setTimeout(function(){ 
        SetUserScale();
        SetScale();
    }, 500);
});

$(window).on('beforeunload', function () {
    weight.Stop();
});

function CalcWeight(w)
{
    if (!current.IsManual)
    {
        if ($.isNumeric(w))
        {
            var inweight = 0;
            var outweight = 0;
            switch (mode) {
                case 1:
                case '1':
                    if (current.GUID == undefined || current.GUID == 0) {
                        $("#in_weight").val(w);
                        $("#out_weight").val("");
                    }
                    else {
                        $("#out_weight").val(w);
                    }
                    break;
                case 2:
                case '2':
                    $("#in_weight").val(w);
                    $("#out_weight").val($("#vehicles option:selected").attr('tare'));
                    if ($("#out_weight").val().length == 0)
                        $("#out_weight").val("0");
                    break;
                case 3:
                case '3':
                    $("#out_weight").val(w);
                    $("#in_weight").val($("#vehicles option:selected").attr('tare'));
                    if ($("#in_weight").val().length == 0)
                        $("#in_weight").val("0");
                    break;
                case 4:
                case '4':
                    $("#in_weight").val("0");
                    inweight = 0;
                    $("#out_weight").val(w);
                    outweight = w;
                    break;
            }
        }
        else
        {
            alert(dictionary.InvalidWeight);
            return;
        }
    }
    else
    {

        switch (mode) {
            case 1:
            case '1':
                break;
            case 2:
            case '2':
                $("#out_weight").val($("#vehicles option:selected").attr('tare'));
                if ($("#out_weight").val().length == 0)
                    $("#out_weight").val("0");
                break;
            case 3:
            case '3':
                $("#in_weight").val($("#vehicles option:selected").attr('tare'));
                if ($("#in_weight").val().length == 0)
                    $("#in_weight").val("0");
                break;
            case 4:
            case '4':
                $("#in_weight").val("0");
                break;
        }
    }

    CalcNetto();
    /*var netto = parseFloat($("#out_weight").val()) - parseFloat($("#in_weight").val());
    netto *= netto < 0 ? -1 : 1;
    $("#netto").val(netto);*/
}

function UpdateMode()
{
    $("#btnSave").text(dictionary.Save);
    switch (mode) {
        case 1:
        case '1':
            if (current.GUID == undefined || current.GUID == 0) {
                $("#btnSave").text(dictionary.ToYard);
            }
            $('.out_weight').show();
            $('.in_weight').show();
            break;
        case 2:
        case '2':
            $('.out_weight').hide();
            $('.in_weight').show();
            break;
        case 3:
        case '3':
            $('.out_weight').show();
            $('.in_weight').hide();
            break;
        case 4:
        case '4':
            $('.out_weight').show();
            $('.in_weight').hide();
            break;
    }
}

    function SaveTare() {
        var vehicle = $("#vehicles").val();
        var tare = $("#v_tare").val();

        obj = new Object();
        obj.GUID = vehicle;
        obj.Tare = tare;
        $.ajax({
            type: 'POST',
            url: '/Vehicles/UpdateTare',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ data: JSON.stringify(obj) }),
            dataType: 'json',
            success: function (data) {
                if (data.isSucceded == false) {
                    alert(data.message);
                }
                else {
                    LoadVehicles();
                    $("#tare").modal('hide');
                }
            },
            error: function (xhr, ajaxOptions, thrownError)
            { console.log(xhr.responseText); }
        });
    }

    

    function Yard() {
        $.ajax({
            type: 'POST',
            url: '/VehiclesWeighing/Yard',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data.isSucceded == false) {
                    alert(data.message);
                }
                else {
                    var yard_list = $(".yard-list-group");
                    yard = data.data;
                    yard_list.empty();
                    $.each(data.data, function (i, val) {
                        yard_list.append($('<a href="#" class="list-group-item list-group-item-action"/>').attr("val", val.GUID).text(val.LicenseNumber));
                    });
                }
            },
            error: function (xhr, ajaxOptions, thrownError)
            { console.log(xhr.responseText) }
        });
    }

    function Save(obj, toPrint)
    {
        $.ajax({
            type: 'POST',
            url: '/VehiclesWeighing/Save',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ data: JSON.stringify(obj), toPrint: toPrint }),
            dataType: 'json',
            success: function (data) {
                if (data.isSucceded == false) {
                    alert(data.message);
                }
                else {
                    if (toPrint == true)
                    {
                        obj.GUID = data.message;
                        Preview(obj);
                        //window.location = '/VehiclesWeighing/Preview?path=' + data.message;
                    }
                    current = new Object();
                    LoadComboboxes();
                    Clear();
                    UpdateMode();
                }
            },
            error: function (xhr, ajaxOptions, thrownError)
            { console.log(xhr.responseText) }
        });
    }

    function Preview(obj) {
        var file = document.getElementById("fileLink");
        file.href = '/VehiclesWeighing/Preview?guid=' + obj.GUID;
        file.click();
    }

    function Get(obj) {
        $.ajax({
            type: 'POST',
            url: '/VehiclesWeighing/Get',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ data: JSON.stringify(obj)}),
            dataType: 'json',
            success: function (data) {
                if (data.isSucceded == false) {
                    alert(data.message);
                }
                else {
                    current = data.data[0];
                    SetData();
                    $("#yard").modal('hide');
                    UpdateMode();
                }
            },
            error: function (xhr, ajaxOptions, thrownError)
            { console.log(xhr.responseText) }

        });
    }

    function LoadComboboxes()
    {
        LoadItems();
        LoadTransports();
        LoadCustomers();
        LoadVehicles();
        LoadInSites();
        LoadOutSites();
        //LoadScales();
        LoadDrivers();
    }

    function LoadDrivers() {
        $.ajax({
            type: 'POST',
            url: '/Drivers/Get',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data.isSucceded == false) {
                    alert(data.message);
                }
                else {
                    var drivers = $("#drivers");
                    drivers.empty();
                    drivers.append($("<option />").val(0).text(dictionary.SelectDriver));
                    drivers.next().children('input').val(dictionary.SelectDriver);
                    $.each(data.data, function (i, val) {
                        drivers.append($("<option />").val(val.GUID).text(val.Name));
                    });
                }
            },
            error: function (xhr, ajaxOptions, thrownError)
            { console.log(xhr.responseText) }
        });
    }

    function LoadScales() {
        $.ajax({
            type: 'POST',
            url: '/Scales/Get',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify({ data: JSON.stringify(new Scale()) }),
            success: function (data) {
                if (data.isSucceded == false) {
                    alert(data.message);
                }
                else {
             //       var scales = $("#scales");
                    var ic = $(".carousel-inner");
                    //scales.empty();
                    ic.empty();
                    var str = "";
                    str += '<div class="carousel-item">';
                    str += '<div class="d-block w-100" id="0">' + dictionary.IsManual + ' </div>';
                    str += '</div>';

                    ic.append(str);
               //     scales.append($("<option />").val(0).text(dictionary.SelectScales));
                    $.each(data.data, function (i, val) {
                 //       scales.append($("<option />").val(val.GUID).text(val.Name));
                        var str = "";
                        str += '<div class="carousel-item">';
                        str += '<div class="d-block w-100" id="' + val.GUID + '">' + val.Name + ' </div>';
                        str += '</div>';

                        ic.append(str);
                        
                            
                    });

                    $(".carousel-item").first().addClass("active");
                }
            },
            error: function (xhr, ajaxOptions, thrownError)
            { console.log(xhr.responseText) }
        });
    }


    function LoadInSites()
    {
        $.ajax({
            type: 'POST',
            url: '/Sites/Get',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data.isSucceded == false) {
                    alert(data.message);
                }
                else {
                    var InSites = $("#insites");
                    InSites.empty();
                    InSites.append($("<option />").val(0).text(dictionary.SelectSite));
                    InSites.next().children('input').val(dictionary.SelectSite);
                    $.each(data.data, function (i, val) {
                        InSites.append($("<option />").val(val.GUID).text(val.Name));
                    });
                }
            },
            error: function (xhr, ajaxOptions, thrownError)
            { console.log(xhr.responseText) }

        });
    }

    function LoadOutSites()
    {
        $.ajax({
            type: 'POST',
            url: '/Sites/Get',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data.isSucceded == false) {
                    alert(data.message);
                }
                else {
                    var outSites = $("#outsites");
                    outSites.empty();
                    outSites.append($("<option />").val(0).text(dictionary.SelectSite));
                    outSites.next().children('input').val(dictionary.SelectSite);
                    $.each(data.data, function (i, val) {
                        outSites.append($("<option />").val(val.GUID).text(val.Name));
                    });
                }
            },
            error: function (xhr, ajaxOptions, thrownError)
            { console.log(xhr.responseText) }

        });
    }

    function LoadVehicles()
    {
        $.ajax({
            type: 'POST',
            url: '/Vehicles/Get',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data.isSucceded == false) {
                    alert(data.message);
                }
                else {
                    var vehicles = $("#vehicles");
                    vehicles.empty();
                    vehicles.append($("<option />").val(0).text(dictionary.SelectVehicle));
                    vehicles.next().children('input').val(dictionary.SelectVehicle);
                    $.each(data.data, function (i, val) {
                        vehicles.append($("<option />").val(val.GUID).text(val.LicenseNumber).attr('tare', val.Tare));
                    });
                }
            },
            error: function (xhr, ajaxOptions, thrownError)
            { console.log(xhr.responseText) }

        });
    }

    function LoadItems()
    {
        $.ajax({
            type: 'POST',
            url: '/Items/Get',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data.isSucceded == false) {
                    alert(data.message);
                }
                else {
                    var items = $("#items");
                    items.empty();
                    items.append($("<option />").val(0).text(dictionary.Selectitem));
                    items.next().children('input').val(dictionary.Selectitem);
                    $.each(data.data, function (i, val) {
                        items.append($("<option />").val(val.GUID).text(val.Name));
                    });
                }
            },
            error: function (xhr, ajaxOptions, thrownError)
            { console.log(xhr.responseText) }

        });
    }

    function LoadTransports() {
        $.ajax({
            type: 'POST',
            url: '/Transports/Get',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data.isSucceded == false) {
                    alert(data.message);
                }
                else {
                    var transports = $("#transports");
                    transports.empty();
                    transports.append($("<option />").val(0).text(dictionary.SelectTransport));
                    transports.next().children('input').val(dictionary.SelectTransport);
                    $.each(data.data, function (i, val) {
                        transports.append($("<option />").val(val.GUID).text(val.Name));
                    });
                }
            },
            error: function (xhr, ajaxOptions, thrownError)
            { console.log(xhr.responseText) }
        });
    }

    function LoadCustomers() {
        $.ajax({
            type: 'POST',
            url: '/Customers/Get',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data.isSucceded == false) {
                    alert(data.message);
                }
                else {
                    var customers = $("#customers");
                    customers.empty();
                    customers.append($("<option />").val(0).text(dictionary.SelectCustomer));
                    customers.next().children('input').val(dictionary.SelectCustomer);
                    $.each(data.data, function (i, val) {
                        customers.append($("<option />").val(val.GUID).text(val.Name));
                    });
                }
            },
            error: function (xhr, ajaxOptions, thrownError)
            { console.log(xhr.responseText) }
        });
    }

    function GetData() {
        current.IsCancelled = false;
        //current.InDate = $("#license_number").val();
        current.InWeight = $("#in_weight").val();
        
        current.OutWeight = $("#out_weight").val();
        if (current.OutWeight == "")
            current.OutWeight = null;
        current.Netto = $("#netto").val();
        if (current.Netto == "")
            current.Netto = null;
        //current.OutDate = $("#license_number").val();
        
        current.ItemID = $("#items").val();
        current.ScaleID = $(".carousel-inner .active div").attr('id');
        current.TransportID = $("#transports").val();
        current.VehicleID = $("#vehicles").val();
        current.Remarks = $("#remarks").val();
        current.Reference = $("#reference").val();
        current.InSiteID = $("#insites").val();
        current.OutSiteID = $("#outsites").val();
        current.CustomerID = $("#customers").val();
        current.DriverID = $("#drivers").val();
        if (current.VehicleID == 0 && $("#vehicles").next().children('input').val() != dictionary.SelectVehicle && $("#vehicles").next().children('input').val() != "")
        {
            current.VehicleID = -1;
            current.LicenseNumber = $("#vehicles").next().children('input').val();
        }

        if (current.TransportID == 0 && $("#transports").next().children('input').val() != dictionary.SelectTransport && $("#transports").next().children('input').val() != "") {
            current.TransportID = -1;
            current.TransportName = $("#transports").next().children('input').val();
        }
        if (current.ItemID == 0 && $("#items").next().children('input').val() != dictionary.Selectitem && $("#inSites").next().children('input').val() != "") {
            current.ItemID = -1;
            current.ItemName = $("#items").next().children('input').val();
        }
        if (current.InSiteID == 0 && $("#insites").next().children('input').val() != dictionary.SelectSite && $("#inSites").next().children('input').val() != "") {
            current.InSiteID = -1;
            current.InSiteName = $("#insites").next().children('input').val();
        }
        if (current.OutSiteID == 0 && $("#outsites").next().children('input').val() != dictionary.SelectSite && $("#outSites").next().children('input').val() != "") {
            current.OutSiteID = -1;
            current.OutSiteName = $("#outsites").next().children('input').val();
        }
        if (current.CustomerID == 0 && $("#customers").next().children('input').val() != dictionary.SelectCustomer && $("#customers").next().children('input').val() != "") {
            current.CustomerID = -1;
            current.CustomerName = $("#customers").next().children('input').val();
        }
        if (current.DriverID == 0 && $("#drivers").next().children('input').val() != dictionary.SelectDriver && $("#drivers").next().children('input').val() != "") {
            current.DriverID = -1;
            current.DriverName = $("#drivers").next().children('input').val();
        }

        //current.IsManual = $("#ismanual").prop("checked");
        current.WeighingMode = mode;
    }

    function SetData() {

        console.log(current);
        $("#in_weight").val(current.InWeight);
        $("#items").val(current.ItemID);
        $("#out_weight").val(current.OutWeight);
        $("#netto").val(current.Netto);
        $("#scales_carousel").carousel(GetIndexByScaleID(current.ScaleID)); //$("#scales").val(current.ScaleID);
        $("#transports").val(current.TransportID);
        $("#vehicles").val(current.VehicleID);
        $("#remarks").val(current.Remarks);
        $("#reference").val(current.Reference);
        $("#insites").val(current.InSiteID);
        $("#outsites").val(current.OutSiteID);
        $("#customers").val(current.CustomerID);
        $("#drivers").val(current.DriverID);

        $("#items").next().children('input').val(current.ItemName);
        $("#transports").next().children('input').val(current.TransportName);
        $("#vehicles").next().children('input').val(current.LicenseNumber);
        $("#insites").next().children('input').val(current.InSiteName);
        $("#outsites").next().children('input').val(current.OutSiteName);
        $("#customers").next().children('input').val(current.CustomerName);
        $("#drivers").next().children('input').val(current.DriverName);
        //$("#ismanual").prop("checked", current.IsManual);
        mode = current.WeighingMode;
        $("#mode"+mode).prop("checked", true);
    }

    function Clear(clearVehicle)
    {
        $("#in_weight").val("");
        $("#items").val(0);
        $("#out_weight").val("");
        $("#netto").val("");
        //current.ScaleID = 1;//$("#scales").val();
        $("#transports").val(0);
        if (clearVehicle != false) {
            $("#vehicles").val(0);
            $("#vehicles").next().children('input').val(dictionary.SelectVehicle);
        }

        $("#remarks").val("");
        $("#reference").val("");
        $("#insites").val(0);
        $("#outsites").val(0);
        $("#customers").val(0);
        $("#drivers").val(0);

        $("#items").next().children('input').val(dictionary.Selectitem);
        $("#transports").next().children('input').val(dictionary.SelectTransport);
        
        $("#insites").next().children('input').val(dictionary.SelectSite);
        $("#outsites").next().children('input').val(dictionary.SelectSite);
        $("#customers").next().children('input').val(dictionary.SelectCustomer);
        $("#drivers").next().children('input').val(dictionary.SelectDriver);

    }

    function CalcNetto()
    {
        var out_w = parseFloat($("#out_weight").val());
        var in_w = parseFloat($("#in_weight").val());
        if (!isNaN(out_w) && !isNaN(in_w))
        {
            var netto = (out_w - in_w);
            if (netto < 0)
                netto = netto * -1;
            $("#netto").val(netto);
        }
    }

    function Scale()
    {
        this.ScalesTypeID = 1;
        this.Active = true;
        this.Status = true;
    }

    function GetIndexByScaleID(id)
    {
        var idx = 1;
        $(".carousel-item div").each(function (index) {
            if ($(this).attr('id') == id)
                idx = index;
        });
        return idx;
    }


    function ChangesEvents() {
        $("#vehicles").next().on('autocompletechange', function () {
            for (var i = 0; i < yard.length; i++) {
                if (yard[i].VehicleID == $("#vehicles").val()) {
                    {
                        Get(yard[i]);
                        return;
                    }

                }
            }
            Clear(false);
            if ($("#vehicles").next().children('input').val() != dictionary.SelectVehicle && $("#vehicles").next().children('input').val() != "")
                CheckForConnections();
        });

        $("#items").next().on('autocompletechange', function () {
            if ($("#items").next().children('input').val() != dictionary.Selectitem && $("#items").next().children('input').val() != "")
                CheckForConnections();

        });

        $("#insites").next().on('autocompletechange', function () {
            if ($("#insites").next().children('input').val() != dictionary.SelectSite && $("#insites").next().children('input').val() != "")
                CheckForConnections();
        });

        $("#outsites").next().on('autocompletechange', function () {
            if ($("#outsites").next().children('input').val() != dictionary.SelectSite && $("#outsites").next().children('input').val() != "")
            CheckForConnections();
        });

        $("#customers").next().on('autocompletechange', function () {
            if ($("#customers").next().children('input').val() != dictionary.SelectCustomer && $("#customers").next().children('input').val() != "")
                CheckForConnections();
        });

        $("#transports").next().on('autocompletechange', function () {
            if ($("#transports").next().children('input').val() != dictionary.SelectTransport && $("#transports").next().children('input').val() != "")
            CheckForConnections();
        });

        $("#drivers").next().on('autocompletechange', function () {
            if ($("#drivers").next().children('input').val() != dictionary.SelectDriver && $("#drivers").next().children('input').val() != "")
            CheckForConnections();
        });


    }

    function OnFocusEvents()
    {
        $(".custom-combobox-input").on("focus", function () {
            
            if ($(this).val() == $(this).parent().prev().find("option:first-child").text())
                $(this).val("");
            
        });

        $(".custom-combobox-input").on("focusout", function () {
            if ($(this).val() == "")
                $(this).val($(this).parent().prev().find("option:first-child").text());

        });
    }

    function CheckForConnections(){
        GetData();
        current.GUID = 0;
        $.ajax({
            type: 'POST',
            url: '/VehiclesWeighing/CheckForConnections',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify({ data: JSON.stringify(current)}),
            success: function (data) {
                if (data.isSucceded == false) {
                    alert(data.message);
                }
                else {
                    if(data.data.length == 1)
                    {
                        //if (confirm(dictionary.Q_ConnectionFound_Display)) {

                            $("#items").next().children('input').val(data.data[0].ItemID == null ? dictionary.Selectitem : data.data[0].ItemName);
                            $("#transports").next().children('input').val(data.data[0].TransportID == null ? dictionary.SelectTransport : data.data[0].TransportName);
                            $("#vehicles").next().children('input').val(data.data[0].VehicleID == null ? dictionary.SelectVehicle : data.data[0].LicenseNumber);
                            $("#insites").next().children('input').val(data.data[0].InSiteID == null ? dictionary.SelectSite : data.data[0].InSiteName);
                            $("#outsites").next().children('input').val(data.data[0].OutSiteID == null ? dictionary.SelectSite : data.data[0].OutSiteName);
                            $("#customers").next().children('input').val(data.data[0].CustomerID == null ? dictionary.SelectCustomer : data.data[0].CustomerName);
                            $("#drivers").next().children('input').val(data.data[0].DriverID == null ? dictionary.SelectDriver : data.data[0].DriverName);
                            
                            $("#vehicles").val(data.data[0].VehicleID == null ? 0 : data.data[0].VehicleID);
                            $("#insites").val(data.data[0].InSiteID == null ? 0 : data.data[0].InSiteID);
                            $("#outsites").val(data.data[0].OutSiteID == null ? 0 : data.data[0].OutSiteID);
                            $("#items").val(data.data[0].ItemID == null ? 0 : data.data[0].ItemID);
                            $("#customers").val(data.data[0].CustomerID == null ? 0 : data.data[0].CustomerID);
                            $("#transports").val(data.data[0].TransportID == null ? 0 : data.data[0].TransportID);
                            $("#drivers").val(data.data[0].DriverID == null ? 0 : data.data[0].DriverID);
                        //}
                    }
                }
            },
            error: function (xhr, ajaxOptions, thrownError)
            { console.log(xhr.responseText) }
        });

    }

    