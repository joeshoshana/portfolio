var items;
var current;
var filter;
var GUID = 19;
$(document).ready(function () {
    Shared();
    writePermission = ReadWritePermissions(company_tables_data, GUID, 'w');

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
            "url": "/Drivers/Get",
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
            { "data": "Code" },
            { "data": "Name" },
            { "data": "ID" },
            { "data": "CompanyID" },
            { "data": "Active" },
            { "data": "GUID" }
        ],
        "columnDefs": [
            {
                "targets": [0, 1, 2],
                "className": "dt-center",
                "orderable": true
            },
            {
                "targets": [3, 4, 5],
                "visible": false,
                "searchable": false
            }
        ],
    });
}

function GetModalData() {
    if (current !== undefined) {
        $("#code").val(current.Code);
        $("#name").val(current.Name);
        $("#id").val(current.ID);
        $("#active").prop("checked", current.Active);
    }
}

function SetModalData() {
    if (current !== undefined) {
        current.Code = $("#code").val();
        current.Name = $("#name").val();
        current.ID = $("#id").val();
        current.Active = $("#active").prop("checked");
    }
}

function AllowedRows(len) {
    $.ajax({
        type: 'POST',
        url: '/Drivers/AllowedRows',
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

function Save(obj) {
    $.ajax({
        type: 'POST',
        url: '/Drivers/Save',
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
    this.Code = $("#f_code").val();
    this.Name = $("#f_name").val();
    this.ID = $("#f_id").val();
    this.Active = $("#f_active").is(":checked");
    if (this.Active === undefined || !this.Active)
        this.Active = false;
}

function CreateFilterEvents() {
    $("#f_code").on('input', function () {
        items.draw();
    });

    $("#f_name").on('input', function () {
        items.draw();
    });

    $("#f_id").on('input', function () {
        items.draw();
    });


    $("#f_active").on('input', function () {
        items.draw();
    });
}