var certificates;
var current;
var filter;
var contacts;
var contact_way
var GUID = 4;

$(document).ready(function () {
    Shared();
    writePermission = ReadWritePermissions(company_forms_data, GUID, 'w');
    LoadComboboxes();

    //$("#f_items").combobox();
    //$("#f_transports").combobox();
    //$("#f_vehicles").combobox();
    //$("#f_insites").combobox();
    //$("#f_outsites").combobox();
    //$("#f_users").combobox();
    //$("#f_customers").combobox();
    //$("#f_drivers").combobox();
    //
    //$("#items").combobox();
    //$("#transports").combobox();
    //$("#vehicles").combobox();
    //$("#insites").combobox();
    //$("#outsites").combobox();
    //$("#users").combobox();
    //$("#customers").combobox();
    //$("#drivers").combobox();

    $("#f_from_out_date").datepicker({
        dateFormat: "dd/mm/yy",
        todayBtn: "linked",
        clearBtn: true,
        language: 'he',
        autoclose: true
    }).datepicker("setDate", 'now').change();

    $("#f_to_out_date").datepicker({
        dateFormat: "dd/mm/yy",
        todayBtn: "linked",
        clearBtn: true,
        language: 'he',
        autoclose: true
    });

    $("#out_date").datepicker({
        dateFormat: "dd/mm/yy",
        todayBtn: "linked",
        clearBtn: true,
        language: 'he',
        autoclose: true
    });

    $("#in_date").datepicker({
        dateFormat: "dd/mm/yy",
        todayBtn: "linked",
        clearBtn: true,
        language: 'he',
        autoclose: true
    });

    CreateDatatable();
    var data = InvisibleFields(4);
    $.each(data, function (i, val) {
        UpdateVisibilityDataTableColumns(val.FormsFieldName);
    });
    
    CreateContactsTable();
    CreateFilterEvents();

    

    $('#certificates tbody').on('click', 'tr', function (e) {
        if (!writePermission)
            return;
        current = certificates.row(this).data();
        if (current === undefined)
            return;
        GetModalData();
        $("#current").modal();
    });

    $('#contacts tbody').on('click', 'tr', function (e) {
        var obj = contacts.row(this).data();
        if (obj === undefined)
            return;
        if (contact_way == 1 && (obj.Phone == undefined || obj.Phone == ""))
            alert(dictionary.MissingPhone);
        else if (contact_way == 2 && (obj.Email == undefined || obj.Email == ""))
            alert(dictionary.MissingEmail);
        else
        {
            if(contact_way == 1)
            {
                if (confirm(dictionary.SendTo + obj.FirstName + ' SMS?')) {
                    $.ajax({
                        type: 'POST',
                        url: '/Customers/SMSContact',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ data: JSON.stringify(obj), type:1,weighingVehicleID : current.GUID }),
                        dataType: 'json',
                        success: function (data) {
                            if (data.isSucceded == false) {
                                alert(data.message);
                            }
                            else {
                                alert("SMS" + dictionary.Sent);
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError)
                        { console.log(xhr.responseText) }

                    });
                }
            }

            if (contact_way == 2) {
                if (confirm(dictionary.SendTo + obj.FirstName + ' מייל?')) {
                    $.ajax({
                        type: 'POST',
                        url: '/Customers/MailContact',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ data: JSON.stringify(obj),type:1, weighingVehicleID: current.GUID }),
                        dataType: 'json',
                        success: function (data) {
                            if (data.isSucceded == false) {
                                alert(data.message);
                            }
                            else {
                                alert(dictionary.Sent + " " + dictionary.Email);
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError)
                        { console.log(xhr.responseText) }

                    });
                }
            }
        }
    });

    $('#certificates tbody').on('click', '.btnCertificate', function (e) {
        if (!writePermission)
            return;
        e.stopPropagation();
        var row = $(this).parents('tr');
        var dt = $('#certificates').DataTable();
        var obj = dt.row(row).data();
        Preview(obj);
    });

    $('#certificates tbody').on('click', '.btnSMS', function (e) {
        if (!writePermission)
            return;
        e.stopPropagation();
        contact_way = 1;
        var row = $(this).parents('tr');
        var dt = $('#certificates').DataTable();
        var obj = dt.row(row).data();
        current = obj;
        ContactSelection(obj);
    });

    $('#certificates tbody').on('click', '.btnMail', function (e) {
        if (!writePermission)
            return;
        e.stopPropagation();
        contact_way = 2;
        var row = $(this).parents('tr');
        var dt = $('#certificates').DataTable();
        var obj = dt.row(row).data();
        current = obj;
        ContactSelection(obj);
    });

    $('#btnSave').on('click', function (e) {
        if (!writePermission)
            return;
        if (current === undefined)
            return;
        SetModalData();

        Save(current);
    });

    $('#btnBack').on('click', function (e) {
        window.location.href = "Index";
    });

    $('#f_weightType').on('change', function(){
        console.log(certificates.rows().data())
        const selectedValue = $('#f_weightType').val();
        if(selectedValue !== -1){

        }
    })
    

});




function UpdateVisibilityDataTableColumns(fieldName)
{
    $('.' + fieldName).hide();
    switch(fieldName)
    {
        case 'LicenseNumber':
            certificates.column(2).visible(false);
            break;
        case 'TransportName':
            certificates.column(1).visible(false);
            break;
        case 'ItemName':
            certificates.column(8).visible(false);
            break;
        case 'SourceSite':
            certificates.column(9).visible(false);
            break;
        case 'DestinationSite':
            certificates.column(10).visible(false);
            break;
        case 'CustomerName':
            certificates.column(11).visible(false);
            break;
        case 'DriverName':
            certificates.column(12).visible(false);
            break;
        case 'Remarks':
            certificates.column(16).visible(false);
            break;
    }
}

function LoadComboboxes() {
    LoadItems();
    LoadTransports();
    LoadDrivers();
    LoadVehicles();
    LoadInSites();
    LoadOutSites();
    LoadUsers();
    LoadCustomers();
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
                var f_customers = $("#f_customers");
                var customers = $("#customers");
                f_customers.empty();
                f_customers.append($("<option />").val(0).text("בחר לקוח"));
                customers.empty();
                customers.append($("<option />").val(0).text("בחר לקוח"));
                $.each(data.data, function (i, val) {
                    customers.append($("<option />").val(val.GUID).text(val.Name));
                    f_customers.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }
    });
}

function LoadUsers() {
    $.ajax({
        type: 'POST',
        url: '/Users/Get',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                var f_Users = $("#f_users");
                var Users = $("#users");
                f_Users.empty();
                f_Users.append($("<option />").val(0).text("בחר שוקל"));
                Users.empty();
                Users.append($("<option />").val(0).text("בחר שוקל"));
                $.each(data.data, function (i, val) {
                    Users.append($("<option />").val(val.GUID).text(val.FirstName));
                    f_Users.append($("<option />").val(val.GUID).text(val.FirstName));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function LoadInSites() {
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
                var f_InSites = $("#f_insites");
                var InSites = $("#insites");
                f_InSites.empty();
                f_InSites.append($("<option />").val(0).text(dictionary.SelectSite));
                InSites.empty();
                InSites.append($("<option />").val(0).text(dictionary.SelectSite));
                $.each(data.data, function (i, val) {
                    InSites.append($("<option />").val(val.GUID).text(val.Name));
                    f_InSites.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function LoadOutSites() {
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
                var f_outSites = $("#f_outsites");
                var outSites = $("#outsites");
                f_outSites.empty();
                f_outSites.append($("<option />").val(0).text(dictionary.SelectSite));
                outSites.empty();
                outSites.append($("<option />").val(0).text(dictionary.SelectSite));
                $.each(data.data, function (i, val) {
                    outSites.append($("<option />").val(val.GUID).text(val.Name));
                    f_outSites.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function LoadVehicles() {
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
                var f_vehicles = $("#f_vehicles");
                var vehicles = $("#vehicles");
                vehicles.empty();
                vehicles.append($("<option />").val(0).text(dictionary.SelectVehicle));
                f_vehicles.empty();
                f_vehicles.append($("<option />").val(0).text(dictionary.SelectVehicle));
                $.each(data.data, function (i, val) {
                    vehicles.append($("<option />").val(val.GUID).text(val.LicenseNumber).attr('tare', val.Tare));
                    f_vehicles.append($("<option />").val(val.GUID).text(val.LicenseNumber).attr('tare', val.Tare));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function LoadItems() {
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
                var f_items = $("#f_items");
                var items = $("#items");
                items.empty();
                items.append($("<option />").val(0).text(dictionary.Selectitem));
                f_items.empty();
                f_items.append($("<option />").val(0).text(dictionary.Selectitem));
                $.each(data.data, function (i, val) {
                    items.append($("<option />").val(val.GUID).text(val.Name));
                    f_items.append($("<option />").val(val.GUID).text(val.Name));
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
                var f_transports = $("#f_transports");
                var transports = $("#transports");
                transports.empty();
                transports.append($("<option />").val(0).text(dictionary.SelectTransport));
                f_transports.empty();
                f_transports.append($("<option />").val(0).text(dictionary.SelectTransport));
                $.each(data.data, function (i, val) {
                    transports.append($("<option />").val(val.GUID).text(val.Name));
                    f_transports.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }
    });
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
                var f_drivers = $("#f_drivers");
                var drivers = $("#drivers");
                drivers.empty();
                drivers.append($("<option />").val(0).text(dictionary.SelectDriver));
                f_drivers.empty();
                f_drivers.append($("<option />").val(0).text(dictionary.SelectDriver));
                $.each(data.data, function (i, val) {
                    drivers.append($("<option />").val(val.GUID).text(val.Name));
                    f_drivers.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }
    });
}


function CreateDatatable() {
    certificates = $('#certificates').DataTable({
        "orderClasses": false,
        "processing": true,
        "serverSide": true,
        "pagingType": "full_numbers",
        "searching": false,
        "colReorder": true,
        "responsive": true,
        "ajax": {
            "url": "/VehiclesWeighing/GetCertificates",
            "data": function (data) {
                const filter = new Filter();
                filter.WeighingMode = $('#f_weightType').val();
                data.data = JSON.stringify(filter);
                // data.data = JSON.stringify(new Filter());
                console.log(data);
            },
            "type": "POST",
            "dataType": "json",
            error: function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
            }
        },
        "dom": 'Bfrtip',
        "buttons": [
            {
                extend: 'csv',
                text: 'CSV',
                className: "btn btn_modern col-2 margin-5"
            },
            {
                extend: 'excel',
                text: 'XLS',
                className: "btn btn_modern col-2 margin-5"
            },
            {
                extend: 'print',
                text: dictionary.Print,
                className: "btn btn_modern col-2 margin-5"
            }
        ],

        "language": {
            "decimal": "",
            "emptyTable": dictionary.EmptyTable,
            "info": dictionary.Info,
            "infoEmpty": dictionary.InfoEmpty,
            "infoFiltered": dictionary.InfoFiltered,
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": dictionary.LengthMenu,
            "loadingRecords": dictionary.LoadingRecords,
            "processing": dictionary.Processing,
            "search": dictionary.Search,
            "zeroRecords": dictionary.ZeroRecords,
            "paginate": {
                "first": dictionary.First,
                "last": dictionary.Last,
                "next": dictionary.Next,
                "previous": dictionary.Previous
            }
        },
        "columns": [
           { "data": "CertificateID" },
           { "data": "TransportName" },
           { "data": "LicenseNumber" },
           { "data": "InDate" },
           { "data": "OutDate" },
           { "data": "InWeight" },
           { "data": "OutWeight" },
           { "data": "Netto" },
           { "data": "ItemName" },
           { "data": "InSiteName" },
           { "data": "OutSiteName" },
           { "data": "CustomerName" },
           { "data": "DriverName" },
           { "data": "UserFirstName" },
           { "data": "IsManual" },
           { "data": "IsCancelled" },
           { "data": "Remarks" },
           { "data": "Certificate" },
           { "data": "Mail" },
           { "data": "SMS" },
           { "data": "ItemID" },
           { "data": "ScaleID" },
           { "data": "TransportID" },
           { "data": "VehicleID" },
           { "data": "InSiteID" },
           { "data": "OutSiteID" },
           { "data": "CompanyID" },
           { "data": "UserID" },
           { "data": "CustomerID" },
           { "data": "GUID" }
        ],
        "columnDefs": [
            {
                "targets": [3,4],
                "render": function (data, type, row, meta) {
                    return moment(data).format('DD/MM/YYYY HH:mm');
                }
            },
            {
                "targets": [14, 15],
                "render": function (data, type, row, meta) {
                    if (data == true) {
                        return '<input type=\"checkbox\" checked value="' + data + '">';
                    } else {
                        return '<input type=\"checkbox\" value="' + data + '">';
                    }
                }
            },
            {
                "targets": [0],
                "render": function (data, type, row, meta) {
                    return String('000000' + data).slice(-6);
                }
            },
            {
                "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,17],
                "className": "dt-center",
                "orderable": true
            },
            {
                "targets": [ 20, 21, 22, 23, 24, 25, 26, 27, 28, 29],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [17],
                "data": null,
                "defaultContent": "<button  class='btnCertificate dt_button'>תעודה</button>"
            }
            ,
            {
                "targets": [18],
                "data": null,
                "defaultContent": "<button class='btnMail dt_button'>שליחה במייל</button>"
            }
            ,
            {
                "targets": [19],
                "data": null,
                "defaultContent": "<button class='btnSMS dt_button'>שליחה בSMS</button>"
            }
        ],
      /*  "footerCallback": function (row, data, start, end, display) {
            var api = this.api();
            
            api.columns(7, {
                page: 'current'
            }).every(function () {
                var sum = this
                  .data()
                  .reduce(function (a, b) {
                      var x = parseFloat(a) || 0;
                      var y = parseFloat(b) || 0;
                      return x + y;
                  }, 0);
                console.log(sum); //alert(sum);
                $(this.footer()).html(sum);
            });
        }*/
    });
}

function GetModalData() {
    if (current !== undefined) {        
        $("#certificate_id").text(current.CertificateID);
        $("#in_weight").val(current.InWeight);
        $("#items").val(current.ItemID);
        $("#out_weight").val(current.OutWeight);
        $("#netto").val(current.Netto);
        $("#transports").val(current.TransportID);
        $("#vehicles").val(current.VehicleID);
        $("#remarks").val(current.Remarks);
        $("#reference").val(current.Reference);
        $("#insites").val(current.InSiteID);
        $("#outsites").val(current.OutSiteID);
        $("#customers").val(current.CustomerID);
        $("#drivers").val(current.DriverID);
        $("#users").val(current.UserID);
        $("#in_date").val(moment(current.InDate).format('DD/MM/YYYY HH:mm'));
        $("#out_date").val(moment(current.OutDate).format('DD/MM/YYYY HH:mm'));
       // $("#items").next().children('input').val(current.ItemName);
       // $("#transports").next().children('input').val(current.TransportName);
       // $("#vehicles").next().children('input').val(current.LicenseNumber);
       // $("#insites").next().children('input').val(current.InSiteName);
       // $("#users").next().children('input').val(current.UserFirstName);
       // $("#customers").next().children('input').val(current.CustomerName);
       // $("#drivers").next().children('input').val(current.DriverName);
       // $("#outsites").next().children('input').val(current.OutSiteName);
        $("#is_cancelled").prop("checked", current.IsCancelled);
    }
}

function SetModalData() {
    if (current !== undefined) {
        
        current.InDate = ToServerDate($("#in_date").val(), 'DD/MM/YYYY');
        current.InWeight = $("#in_weight").val();
        current.ItemID = $("#items").val();
        current.OutWeight = $("#out_weight").val();
        if (current.OutWeight == "")
            current.OutWeight = 0;
        current.Netto = current.OutWeight - current.InWeight;
        if (current.Netto < 0)
            current.Netto = current.Netto * -1;
        current.OutDate = ToServerDate($("#out_date").val(), 'DD/MM/YYYY');

        current.UserID = $("#users").val();
        current.CustomerID = $("#customers").val();
        current.DriverID = $("#drivers").val();
        current.TransportID = $("#transports").val();
        current.VehicleID = $("#vehicles").val();
        current.Remarks = $("#remarks").val();
        current.Reference = $("#reference").val();
        current.InSiteID = $("#insites").val();
        current.OutSiteID = $("#outsites").val();
        current.IsCancelled = $("#is_cancelled").prop("checked");
    }
}

function Save(obj) {
    $.ajax({
        type: 'POST',
        url: '/VehiclesWeighing/Save',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ data: JSON.stringify(obj), toPrint: false }),
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                $("#current").modal('hide');
                certificates.draw();
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function Filter() {

    this.CertificateID = $("#f_certificate_id").val();
    if (this.CertificateID == null || this.CertificateID == undefined || this.CertificateID.length == 0)
        this.CertificateID = 0;
    this.ItemID = $("#f_items").val();
    this.VehicleID = $("#f_vehicles").val();
    this.TransportID = $("#f_transports").val();
    this.InSiteID = $("#f_insites").val();
    this.OutSiteID = $("#f_outsites").val();
    this.UserID = $("#f_users").val();
    this.CustomerID = $("#f_customers").val();
    this.DriverID = $("#f_drivers").val();
    this.FromOutDate = ToServerDate($("#f_from_out_date").val(), 'DD/MM/YYYY');
    this.ToOutDate = ToServerDate($("#f_to_out_date").val(), 'DD/MM/YYYY');
    this.IsCancelled = $("#f_is_cancelled").is(":checked");
    if (this.IsCancelled === undefined || !this.IsCancelled)
        this.IsCancelled = false;
    this.IsManual = $("#f_is_manual").is(":checked");
    if (this.IsManual === undefined || !this.IsManual)
        this.IsManual = false;

}

function CreateFilterEvents() {
    $("#f_certificate_id").on('input', function () {
        certificates.draw();
    });

    $("#f_items").on('change',function(){
            certificates.draw();
    });

    $("#f_users").on('change',function(){
            certificates.draw();
    });

    $("#f_customers").on('change',function(){
            certificates.draw();
    });

    $("#f_drivers").on('change',function(){
            certificates.draw();
    });
    
    $("#f_vehicles").on('change',function(){
            certificates.draw();
    });

    $("#f_transports").on('change',function(){
            certificates.draw();
    });

    $("#f_insites").on('change',function(){
            certificates.draw();
    });

    $("#f_outsites").on('change',function(){
            certificates.draw();
    });

    $("#f_from_out_date").on('change', function () {
        certificates.draw();
    });

    $("#f_to_out_date").on('change', function () {
        certificates.draw();
    });

    $("#f_is_cancelled").on('input', function () {
        certificates.draw();
    });

    $("#f_is_manual").on('input', function () {
        certificates.draw();
    });
    
    $("#f_weightType").on('input', function () {
        certificates.draw();
    });
   
}

function Preview(obj) {
    var file = document.getElementById("fileLink");
    file.href = '/VehiclesWeighing/Preview?guid=' + obj.GUID;
    file.click();
}

function ContactSelection(obj) {
    if (obj.CustomerID == undefined || obj.CustomerID == 0)
    {
        alert(dictionary.CustomerNotExist);
    }
    else
    {
        $("#sub_items").modal();
        contacts.draw();
    }  
}

function CreateContactsTable() {
    contacts = $('#contacts').DataTable({
        "processing": true,
        "serverSide": true,
        "pagingType": "full_numbers",
        "searching": false,
        "colReorder": true,
        "responsive": true,
        "language": {
            "decimal": "",
            "emptyTable": dictionary.EmptyTable,
            "info": dictionary.Info,
            "infoEmpty": dictionary.InfoEmpty,
            "infoFiltered": dictionary.InfoFiltered,
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": dictionary.LengthMenu,
            "loadingRecords": dictionary.LoadingRecords,
            "processing": dictionary.Processing,
            "search": dictionary.Search,
            "zeroRecords": dictionary.ZeroRecords,
            "paginate": {
                "first": dictionary.First,
                "last": dictionary.Last,
                "next": dictionary.Next,
                "previous": dictionary.Previous
            }
        },
        "ajax": {
            "url": "/Customers/GetContacts",
            "data": function (data) {
                    data.data = JSON.stringify(new Contact() );
            },
            "type": "POST",
            "dataType": "json",
            error: function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
            }
        },
        "columns": [
           { "data": "FirstName" },
           { "data": "LastName" },
           { "data": "Email" },
           { "data": "Phone" },
           { "data": "Remarks" },
           { "data": "CustomerID" },
           { "data": "Active" },
           { "data": "GUID" }
        ],
        "columnDefs": [
            {
                "targets": [0, 1, 2, 3, 4],
                "className": "dt-center",
                "orderable": true
            },
            {
                "targets": [5, 6, 7],
                "visible": false,
                "searchable": false
            }
        ],
    });
}

function Contact()
{
    if (current != null)
    {
        this.GUID = current.CustomerID;
    }
}