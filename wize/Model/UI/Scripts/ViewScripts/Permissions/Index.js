var items;
var current;
var filter;
var tablesinnerpermissions;
var formsinnerpermissions;
var settingsinnerpermissions;
var innerpermissions;
var GUID = 7;

$(document).ready(function () {
    Shared();
    writePermission = ReadWritePermissions(company_settings_data, GUID, 'w');
    CreateDatatable();
    CreateTablesDatatable();
    CreateFormsDatatable();
    CreateSettingsDatatable();
    CreateInnerPermissionsDatatable();

    CreateFilterEvents();
    AllowedRows(items.rows().data().length);
    $("#tabs").tabs();

    $('#items tbody').on('click', 'tr', function (e) {
        if (!writePermission)
            return;
        current = items.row(this).data();
        if (current === undefined)
            return;
        GetModalData();
        $("#current").modal();
    });

    $('#tablesinnerpermissions tbody').on('click', 'input', function (e) {
        var col = $(this).attr("name");
        var row = $(this).parents('tr');
        var dt = $('#tablesinnerpermissions').DataTable();
        var obj = dt.row(row).data();
        switch(col)
        {
            case '2':
                dt.row(row).data().PermissionID = $(this)[0].checked ? 1 : 0;
                $(row).find("INPUT[type='checkbox']").attr('checked', $(this)[0].checked);
                dt.row(row).data().Read = $(this)[0].checked ? 1 : 0;
                dt.row(row).data().Write = $(this)[0].checked ? 1 : 0;
                break;
            case '3':
                dt.row(row).data().Read = $(this)[0].checked ? 1 : 0;
                break;
            case '4':
                dt.row(row).data().Write = $(this)[0].checked ? 1 : 0;
                break;
        }
        
    });

    $('#formsinnerpermissions tbody').on('click', 'input', function (e) {
        var col = $(this).attr("name");
        var row = $(this).parents('tr');
        var dt = $('#formsinnerpermissions').DataTable();
        var obj = dt.row(row).data();
        dt.row(row).data().PermissionID = $(this)[0].checked ? 1 : 0;
        switch (col) {
            case '2':
                dt.row(row).data().PermissionID = $(this)[0].checked ? 1 : 0;
                $(row).find("INPUT[type='checkbox']").attr('checked', $(this)[0].checked);
                dt.row(row).data().Read = $(this)[0].checked ? 1 : 0;
                dt.row(row).data().Write = $(this)[0].checked ? 1 : 0;
                break;
            case '3':
                dt.row(row).data().Read = $(this)[0].checked ? 1 : 0;
                break;
            case '4':
                dt.row(row).data().Write = $(this)[0].checked ? 1 : 0;
                break;
        }
    });

    $('#settingsinnerpermissions tbody').on('click', 'input', function (e) {
        var col = $(this).attr("name");
        var row = $(this).parents('tr');
        var dt = $('#settingsinnerpermissions').DataTable();
        var obj = dt.row(row).data();
        dt.row(row).data().PermissionID = $(this)[0].checked ? 1 : 0;
        switch (col) {
            case '2':
                dt.row(row).data().PermissionID = $(this)[0].checked ? 1 : 0;
                $(row).find("INPUT[type='checkbox']").attr('checked', $(this)[0].checked);
                dt.row(row).data().Read = $(this)[0].checked ? 1 : 0;
                dt.row(row).data().Write = $(this)[0].checked ? 1 : 0;
                break;
            case '3':
                dt.row(row).data().Read = $(this)[0].checked ? 1 : 0;
                break;
            case '4':
                dt.row(row).data().Write = $(this)[0].checked ? 1 : 0;
                break;
        }
    });

    $('#innerpermissions tbody').on('click', 'input', function (e) {
        var col = $(this).attr("name");
        var row = $(this).parents('tr');
        var dt = $('#innerpermissions').DataTable();
        var obj = dt.row(row).data();
        dt.row(row).data().PermissionID = $(this)[0].checked ? 1 : 0;        
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
            "url": "/Permissions/Get",
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
           { "data": "CompanyID" },
           { "data": "Active" },
           { "data": "GUID" }
        ],
        "columnDefs": [
            {
                "targets": [0],
                "className": "dt-center",
                "orderable": true
            },
            {
                "targets": [1, 2, 3],
                "visible": false,
                "searchable": false
            }
        ],
    });
}

function GetModalData() {
    if (current !== undefined) {
        $("#name").val(current.Name);
        $("#active").prop("checked", current.Active);
        tablesinnerpermissions.ajax.reload();;
        formsinnerpermissions.ajax.reload();;
        settingsinnerpermissions.ajax.reload();;
    }
}

function SetModalData() {
    if (current !== undefined) {
        current.Name = $("#name").val();
        current.Active = $("#active").prop("checked");
    }
}

function Save(obj) {
    var t = tablesinnerpermissions.rows().data().toArray();
    var f = formsinnerpermissions.rows().data().toArray();
    var s = settingsinnerpermissions.rows().data().toArray();

    $.ajax({
        type: 'POST',
        url: '/Permissions/Save',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ data: JSON.stringify(obj), tablesPermissions: JSON.stringify(t), formsPermissions: JSON.stringify(f), settingsPermissions: JSON.stringify(s) }),
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
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function Filter() {
    this.Name = $("#f_name").val();
    this.Active = $("#f_active").is(":checked");
    if (this.Active === undefined || !this.Active)
        this.Active = false;
}

function CreateFilterEvents() {
    $("#f_name").on('input', function () {
        items.draw();
    });

    $("#f_active").on('input', function () {
        items.draw();
    });
}

function CreateTablesDatatable() {
    tablesinnerpermissions = $('#tablesinnerpermissions').DataTable({
        "orderClasses": false,
        "processing": true,
        "serverSide": false,
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
            "url": "/Permissions/GetTablesPermissions",
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
            { "data": "TablesInnerPermissionName" },
           { "data": "TableName" },
           { "data": "PermissionID" },
           { "data": "Read" },
           { "data": "Write" },
           { "data": "TablesInnerPermissionID" },
           { "data": "GUID" }
        ],
        "columnDefs": [
            {
                "targets": [0, 1, 2,3,4],
                "className": "dt-center",
                "orderable": true
            },
            {
                "targets": [2,3,4],
                "render": function (data, type, row, meta) {
                    if (data != 0 && data != null) {
                        return '<input type=\"checkbox\" name="' + meta.col + '" value="' + data + '" checked >';
                    } else {
                        return '<input type=\"checkbox\" name="' + meta.col + '" value="' + data + '">';
                    }
                }
            },
            {
                "targets": [5, 6],
                "visible": false,
                "searchable": false
            }
        ],
    });
}

function CreateFormsDatatable() {
    formsinnerpermissions = $('#formsinnerpermissions').DataTable({
        "orderClasses": false,
        "processing": true,
        "serverSide": false,
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
            "url": "/Permissions/GetFormsPermissions",
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
            { "data": "FormsInnerPermissionName" },
           { "data": "FormName" },
           { "data": "PermissionID" },
           { "data": "Read" },
           { "data": "Write" },
           { "data": "FormsInnerPermissionID" },
           { "data": "GUID" }
        ],
        "columnDefs": [
            {
                "targets": [0, 1, 2,3,4],
                "className": "dt-center",
                "orderable": true
            },
            {
                "targets": [2,3,4],
                "render": function (data, type, row, meta) {
                    if (data != 0 && data != null) {
                        return '<input type=\"checkbox\" name="' + meta.col + '" value="' + data + '" checked >';
                    } else {
                        return '<input type=\"checkbox\" name="' + meta.col + '" value="' + data + '">';
                    }
                }
            },
            {
                "targets": [5,6],
                "visible": false,
                "searchable": false
            }
        ],
    });
}

function CreateSettingsDatatable() {
    settingsinnerpermissions = $('#settingsinnerpermissions').DataTable({
        "orderClasses": false,
        "processing": true,
        "serverSide": false,
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
            "url": "/Permissions/GetSettingsPermissions",
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
            { "data": "SettingsInnerPermissionName" },
           { "data": "SettingName" },
           { "data": "PermissionID" },
           { "data": "Read" },
           { "data": "Write" },
           { "data": "SettingsInnerPermissionID" },
           { "data": "GUID" }
        ],
        "columnDefs": [
            {
                "targets": [0, 1, 2,3,4],
                "className": "dt-center",
                "orderable": true
            },
            {
                "targets": [2,3,4],
                "render": function (data, type, row, meta) {
                    if (data != 0 &&  data != null) {
                        return '<input type=\"checkbox\" name="' + meta.col + '" value="' + data + '" checked >';
                    } else {
                        return '<input type=\"checkbox\" name="' + meta.col + '" value="' + data + '">';
                    }
                }
            },
            {
                "targets": [5,6],
                "visible": false,
                "searchable": false
            }
        ],
    });
}

function CreateInnerPermissionsDatatable() {
    innerpermissions = $('#innerpermissions ').DataTable({
        "orderClasses": false,
        "processing": true,
        "serverSide": false,
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
            "url": "/Permissions/GetInnerPermissions",
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
            { "data": "SettingsInnerPermissionName" },
           { "data": "SettingName" },
           { "data": "PermissionID" },
           { "data": "Read" },
           { "data": "Write" },
           { "data": "SettingsInnerPermissionID" },
           { "data": "GUID" }
        ],
        "columnDefs": [
            {
                "targets": [0, 1, 2, 3, 4],
                "className": "dt-center",
                "orderable": true
            },
            {
                "targets": [2, 3, 4],
                "render": function (data, type, row, meta) {
                    if (data != 0 && data != null) {
                        return '<input type=\"checkbox\" name="' + meta.col + '" value="' + data + '" checked >';
                    } else {
                        return '<input type=\"checkbox\" name="' + meta.col + '" value="' + data + '">';
                    }
                }
            },
            {
                "targets": [5, 6],
                "visible": false,
                "searchable": false
            }
        ],
    });
}

function AllowedRows(len) {
    $.ajax({
        type: 'POST',
        url: '/Permissions/AllowedRows',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ data: len }),
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                if (data.data == false || !writePermission)
                    $("#btnNew").hide();
                else
                    $("#btnNew").show();
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}