var items;
var current;
var filter;
var sub_items;
var sub_current;
var GUID = 20;

$(document).ready(function () {
    Shared();
    writePermission = ReadWritePermissions(company_tables_data, GUID, 'w');
    CreateDatatable();

    LoadComboboxes();

    CreateFilterEvents();
    AllowedRows(items.rows().data().length);
    $('#connections tbody').on('click', 'tr', function (e) {
        if (!writePermission)
            return;
        current = items.row(this).data();
        if (current === undefined)
            return;
        GetModalData();
        $("#current").modal();
    });

    var data = InvisibleFields(4);
    $.each(data, function (i, val) {
        UpdateVisibilityDataTableColumns(val.FormsFieldName);
    });


    $('#connections tbody').on('click', '.btnDuplicate', function (e) {
        if (!writePermission)
            return;
        e.stopPropagation();
        var row = $(this).parents('tr');
        var dt = $('#connections').DataTable();
        var obj = dt.row(row).data();
        current = obj;
        current.GUID = 0;
        GetModalData();
        $("#current").modal();
    });

    $('#connections tbody').on('click', '.btnDelete', function (e) {
        if (!writePermission)
            return;
        e.stopPropagation();
        var row = $(this).parents('tr');
        var dt = $('#connections').DataTable();
        var obj = dt.row(row).data();

        if (confirm(dictionary.Delete + "?")) {
            Delete(obj);
            items.draw();
        }

    });

    $('#btnNew').on('click', function (e) {

        current = new Object();
        GetModalData();
        $("#current").modal();
    });

    $('#btnSave').on('click', function (e) {
        if (current === undefined)
            return;
        SetModalData();

        Save(current);
    });

});

function AllowedRows(len) {
    $.ajax({
        type: 'POST',
        url: '/Connections/AllowedRows',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ data: len }),
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                if (data.data == false || !writePermission)
                    // $("#btnNew").hide();
                    $("#btnNew").remove();
                else
                    $("#btnNew").show();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) { console.log(xhr.responseText) }

    });
}

function CreateDatatable() {
    items = $('#connections').DataTable({
        "orderClasses": false,
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
            "url": "/Connections/Get",
            "async": false,
            "data": function (data) {
                data.data = JSON.stringify(new Filter());
            },
            "type": "POST",
            "dataType": "json",
            error: function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
            }
        },
        "columns": [
            { "data": "Tag" },
            { "data": "LicenseNumber" },
            { "data": "TransportName" },
            { "data": "ItemName" },
            { "data": "InSiteName" },
            { "data": "OutSiteName" },
            { "data": "CustomerName" },
            { "data": "DriverName" },
            { "data": "Duplicate" },
            { "data": "Delete" },
            { "data": "VehicleID" },
            { "data": "ItemID" },
            { "data": "TransportID" },
            { "data": "DriverID" },
            { "data": "InSiteID" },
            { "data": "OutSiteID" },
            { "data": "CustomerID" },
            { "data": "CompanyID" },
            { "data": "GUID" }
        ],
        "columnDefs": [
            {
                "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18],
                "className": "dt-center",
                "orderable": true
            },
            {
                "targets": [10, 11, 12, 13, 14, 15, 16, 17, 18],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [8],
                "data": null,
                "defaultContent": "<button class='btnDuplicate dt_button'>" + dictionary.Duplicate + "</button>"
            },
            {
                "targets": [9],
                "data": null,
                "defaultContent": "<button class='btnDelete dt_button'>" + dictionary.Delete + "</button>"
            }
        ],
    });
}


function GetModalData() {
    if (current !== undefined) {
        $("#tag").val(current.Tag);
        $("#vehicles").val(current.VehicleID);
        $("#items").val(current.ItemID);
        $("#transports").val(current.TransportID);
        $("#drivers").val(current.DriverID);
        $("#insites").val(current.InSiteID);
        $("#outsites").val(current.OutSiteID);
        $("#customers").val(current.CustomerID);
    }
}

function SetModalData() {
    if (current !== undefined) {
        current.Tag = $("#tag").val();
        current.VehicleID = $("#vehicles").val();
        current.ItemID = $("#items").val();
        current.TransportID = $("#transports").val();
        current.DriverID = $("#drivers").val();
        current.InSiteID = $("#insites").val();
        current.OutSiteID = $("#outsites").val();
        current.CustomerID = $("#customers").val();
    }
}

function Save(obj) {
    $.ajax({
        type: 'POST',
        url: '/Connections/Save',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ data: JSON.stringify(obj) }),
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                $("#current").modal('hide');
                items.draw();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) { console.log(xhr.responseText) }

    });
}

function Delete(obj) {
    $.ajax({
        type: 'POST',
        url: '/Connections/Delete',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ data: JSON.stringify(obj) }),
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                $("#current").modal('hide');
                items.draw();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) { console.log(xhr.responseText) }

    });
}

function Filter() {
    this.Tag = $("#tag").val();
    this.VehicleID = $("#f_vehicles").val();
    this.ItemID = $("#f_items").val();
    this.TransportID = $("#f_transports").val();
    this.DriverID = $("#f_drivers").val();
    this.InSiteID = $("#f_insites").val();
    this.OutSiteID = $("#f_outsites").val();
    this.CustomerID = $("#f_customers").val();
}

function CreateFilterEvents() {
    $("#f_tag").on('input', function () {
        items.draw();
    });

    $("#f_items").on('change', function () {
        items.draw();
    });

    $("#f_users").on('change', function () {
        items.draw();
    });

    $("#f_customers").on('change', function () {
        items.draw();
    });

    $("#f_drivers").on('change', function () {
        items.draw();
    });

    $("#f_vehicles").on('change', function () {
        items.draw();
    });

    $("#f_transports").on('change', function () {
        items.draw();
    });

    $("#f_insites").on('change', function () {
        items.draw();
    });

    $("#f_outsites").on('change', function () {
        items.draw();
    });
}


function LoadComboboxes() {
    LoadItems();
    LoadTransports();
    LoadCustomers();
    LoadVehicles();
    LoadInSites();
    LoadOutSites();
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
                var f_drivers = $("#f_drivers");
                drivers.empty();
                f_drivers.empty();
                drivers.append($("<option />").val(0).text(dictionary.SelectDriver));
                f_drivers.append($("<option />").val(0).text(dictionary.SelectDriver));
                $.each(data.data, function (i, val) {
                    drivers.append($("<option />").val(val.GUID).text(val.Name));
                    f_drivers.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError) { console.log(xhr.responseText) }
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
                var InSites = $("#insites");
                var f_InSites = $("#f_insites");
                InSites.empty();
                f_InSites.empty();
                InSites.append($("<option />").val(0).text(dictionary.SelectSite));
                f_InSites.append($("<option />").val(0).text(dictionary.SelectSite));
                $.each(data.data, function (i, val) {
                    InSites.append($("<option />").val(val.GUID).text(val.Name));
                    f_InSites.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError) { console.log(xhr.responseText) }

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
                var outSites = $("#outsites");
                var f_outSites = $("#f_outsites");
                outSites.empty();
                f_outSites.empty();
                outSites.append($("<option />").val(0).text(dictionary.SelectSite));
                f_outSites.append($("<option />").val(0).text(dictionary.SelectSite));
                $.each(data.data, function (i, val) {
                    outSites.append($("<option />").val(val.GUID).text(val.Name));
                    f_outSites.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError) { console.log(xhr.responseText) }

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
                var vehicles = $("#vehicles");
                var f_vehicles = $("#f_vehicles");
                vehicles.empty();
                f_vehicles.empty();
                vehicles.append($("<option />").val(0).text(dictionary.SelectVehicle));
                f_vehicles.append($("<option />").val(0).text(dictionary.SelectVehicle));
                $.each(data.data, function (i, val) {
                    vehicles.append($("<option />").val(val.GUID).text(val.LicenseNumber).attr('tare', val.Tare));
                    f_vehicles.append($("<option />").val(val.GUID).text(val.LicenseNumber).attr('tare', val.Tare));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError) { console.log(xhr.responseText) }

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
                var items = $("#items");
                var f_items = $("#f_items");
                items.empty();
                f_items.empty();
                items.append($("<option />").val(0).text(dictionary.Selectitem));
                f_items.append($("<option />").val(0).text(dictionary.Selectitem));
                $.each(data.data, function (i, val) {
                    items.append($("<option />").val(val.GUID).text(val.Name));
                    f_items.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError) { console.log(xhr.responseText) }

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
                var f_transports = $("#f_transports");
                transports.empty();
                f_transports.empty();
                transports.append($("<option />").val(0).text(dictionary.SelectTransport));
                f_transports.append($("<option />").val(0).text(dictionary.SelectTransport));
                $.each(data.data, function (i, val) {
                    transports.append($("<option />").val(val.GUID).text(val.Name));
                    f_transports.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError) { console.log(xhr.responseText) }
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
                var f_customers = $("#f_customers");
                customers.empty();
                f_customers.empty();
                customers.append($("<option />").val(0).text(dictionary.SelectCustomer));
                f_customers.append($("<option />").val(0).text(dictionary.SelectCustomer));
                $.each(data.data, function (i, val) {
                    customers.append($("<option />").val(val.GUID).text(val.Name));
                    f_customers.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError) { console.log(xhr.responseText) }
    });
}

function UpdateVisibilityDataTableColumns(fieldName) {
    $('.' + fieldName).hide();
    switch (fieldName) {
        case 'LicenseNumber':
            items.column(1).visible(false);
            break;
        case 'TransportName':
            items.column(2).visible(false);
            break;
        case 'ItemName':
            items.column(3).visible(false);
            break;
        case 'SourceSite':
            items.column(4).visible(false);
            break;
        case 'DestinationSite':
            items.column(5).visible(false);
            break;
        case 'CustomerName':
            items.column(6).visible(false);
            break;
        case 'DriverName':
            items.column(7).visible(false);
            break;
    }
}