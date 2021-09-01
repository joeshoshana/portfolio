var items;
var current;
var filter;
var GUID = 8;

$(document).ready(function () {
    Shared();

    writePermission = ReadWritePermissions(company_tables_data, GUID, 'w');
    LoadComboboxes();
    CreateDatatable();

    CreateFilterEvents();
    AllowedRows(items.rows().data().length);
    $('#items tbody').on('click', 'tr', function (e) {
        if (!writePermission)
            return;
        current = items.row(this).data();
        if (current === undefined)
            return;
        GetModalData();
        $("#current").modal();
    });


    $('#btnNew').on('click', function (e) {
        current = new Object();
        current.Active = true;
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

function CreateDatatable() {
    items = $('#items').DataTable({
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
            "url": "/Vehicles/Get",
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
            { "data": "LicenseNumber" },
            { "data": "Tare" },
            { "data": "CustomerName" },
            { "data": "TransportName" },
            { "data": "CustomerID" },
            { "data": "TransportID" },
            { "data": "CompanyID" },
            { "data": "Active" },
            { "data": "GUID" }
        ],
        "columnDefs": [
            {
                "targets": [0, 1, 2, 3],
                "className": "dt-center",
                "orderable": true
            },
            {
                "targets": [4, 5, 6, 7, 8],
                "visible": false,
                "searchable": false
            }
        ],
    });
}

function GetModalData() {
    if (current !== undefined) {
        $("#license_number").val(current.LicenseNumber);
        $("#tare").val(current.Tare);
        $("#active").prop("checked", current.Active);
        $("#customers").val(current.CustomerID);
        $("#transports").val(current.TransportID);
        $("#weighingModes").val(current.WeighingModeID);
    }
}

function SetModalData() {
    if (current !== undefined) {
        current.LicenseNumber = $("#license_number").val();
        current.Tare = $("#tare").val();
        current.Active = $("#active").prop("checked");
        current.CustomerID = $("#customers").val();
        current.TransportID = $("#transports").val();
        current.WeighingModeID = $("#weighingModes").val();
    }
}

function LoadComboboxes() {
    LoadTransports();
    LoadCustomers();
    LoadWeighingModes();
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
                transports.append($("<option />").val(0).text(dictionary.SelectTransport));
                f_transports.empty();
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

function LoadWeighingModes() {
    $.ajax({
        type: 'POST',
        url: '/WeighingModes/Get',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                var weighingModes = $("#weighingModes");
                weighingModes.empty();
                $.each(data.data, function (i, val) {
                    weighingModes.append($("<option />").val(val.GUID).text(val.Name));
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
                customers.append($("<option />").val(0).text(dictionary.SelectCustomer));
                f_customers.empty();
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

function Save(obj) {
    $.ajax({
        type: 'POST',
        url: '/Vehicles/Save',
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

function AllowedRows(len) {
    $.ajax({
        type: 'POST',
        url: '/Vehicles/AllowedRows',
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

function Filter() {
    this.LicenseNumber = $("#f_license_number").val();
    this.Active = $("#f_active").is(":checked");
    this.CustomerID = $("#f_customers").val();
    this.TransportID = $("#f_transports").val();
    if (this.Active === undefined || !this.Active)
        this.Active = false;
}

function CreateFilterEvents() {
    $("#f_license_number").on('input', function () {
        items.draw();
    });

    $("#f_active").on('input', function () {
        items.draw();
    });

    $("#f_customers").on('change', function () {
        items.draw();
    });

    $("#f_transports").on('change', function () {
        items.draw();
    });
}