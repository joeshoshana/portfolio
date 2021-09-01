var items;
var current;
var filter;
var sub_items;
var sub_current;
var GUID = 3;
;
$(document).ready(function () {
    Shared();

    writePermission = ReadWritePermissions(company_tables_data, GUID, 'w');

    CreateDatatable();
    CreateSubTable();

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
        sub_items.draw();
    });

    $('#sub_items tbody').on('click', 'tr', function (e) {
        sub_current = sub_items.row(this).data();
        GetSubModelData();
        $("#sub_current").modal();
    });


    $('#btnNew').on('click', function (e) {

        current = new Object();
        current.Active = true;
        GetModalData();

        $("#current").modal();
    });

    $('#btnNewContact').on('click', function (e) {
        sub_current = new Object();
        sub_current.Active = true;
        sub_current.SupplierID = current.GUID;
        GetSubModelData();
        $("#sub_current").modal();
    });



    $('#btnSave').on('click', function (e) {
        if (current === undefined)
            return;
        SetModalData();

        Save(current);
    });

    $('#btnSaveContact').on('click', function (e) {
        if (sub_current === undefined)
            return;
        SetSubModalData();
        SaveContact(sub_current);

    });
});

function CreateSubTable() {
    sub_items = $('#sub_items').DataTable({
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
            "url": "/Suppliers/GetContacts",
            "data": function (data) {
                data.data = JSON.stringify(current);
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
            { "data": "SupplierID" },
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
            "url": "/Suppliers/Get",
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
            { "data": "Name" },
            { "data": "ID" },
            { "data": "Address" },
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

function GetSubModelData() {
    if (sub_current !== undefined) {
        $("#first_name").val(sub_current.FirstName);
        $("#last_name").val(sub_current.LastName);
        $("#mail").val(sub_current.Email);
        $("#phone").val(sub_current.Phone);
        $("#remarks").val(sub_current.Remarks);
        $("#contact_active").prop("checked", sub_current.Active);
    }
}

function GetModalData() {
    if (current !== undefined) {
        $("#name").val(current.Name);
        $("#id").val(current.ID);
        $("#address").val(current.Address);
        $("#active").prop("checked", current.Active);
    }
}

function SetSubModalData() {
    if (sub_current !== undefined) {
        sub_current.FirstName = $("#first_name").val();
        sub_current.LastName = $("#last_name").val();
        sub_current.Email = $("#mail").val();
        sub_current.Phone = $("#phone").val();
        sub_current.Remarks = $("#remarks").val();
        sub_current.Active = $("#contact_active").is(":checked");
        if (sub_current.Active === undefined || !sub_current.Active)
            sub_current.Active = false;
    }
}

function SetModalData() {
    if (current !== undefined) {
        current.Name = $("#name").val();
        current.ID = $("#id").val();
        current.Address = $("#address").val();
        current.Active = $("#active").prop("checked");
    }
}

function SaveContact(obj) {
    $.ajax({
        type: 'POST',
        url: '/Suppliers/SaveContact',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ data: JSON.stringify(obj) }),
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                $("#sub_current").modal('hide');
                sub_items.draw();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) { console.log(xhr.responseText) }

    });
}

function AllowedRows(len) {
    $.ajax({
        type: 'POST',
        url: '/Suppliers/AllowedRows',
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
        url: '/Suppliers/Save',
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
    this.Name = $("#f_name").val();
    this.ID = $("#f_id").val();
    this.Address = $("#f_address").val();
    this.Active = $("#f_active").is(":checked");
    if (this.Active === undefined || !this.Active)
        this.Active = false;
}

function CreateFilterEvents() {
    $("#f_name").on('input', function () {
        items.draw();
    });

    $("#f_id").on('input', function () {
        items.draw();
    });

    $("#f_address").on('input', function () {
        items.draw();
    });

    $("#f_active").on('input', function () {
        items.draw();
    });
}