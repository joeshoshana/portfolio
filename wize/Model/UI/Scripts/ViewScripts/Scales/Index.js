var items;
var current;
var filter;
var GUID = 5;
$(document).ready(function () {

    Shared();
    writePermission = ReadWritePermissions(company_settings_data, GUID, 'w');
    LoadScaleTypes();

    CreateDatatable();
   
    CreateFilterEvents();
    AllowedRows(items.rows().data().length);
    LoadCompanies();
    LoadUnits();

    if (is_owner == false && is_super == false) {
        
        items.columns([10, 11]).visible(false, false);
    }
    else {
        
        items.columns([10, 11]).visible(true, false);
    }


    $('#items tbody').on('click', 'tr', function (e) {
        if (!writePermission && (!is_owner && !is_super))
            return;
        current = items.row(this).data();
        if (current === undefined)
            return;
        GetModalData();
        
        $("#current").modal();
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

    $('#items tbody').on('click', 'button', function (e) {
        e.stopPropagation();
       var row = $(this).parents('tr');
       var dt = $('#items').DataTable();
       var obj = dt.row(row).data();
       $.ajax({
           type: 'POST',
           url: '/Scales/ChangeConnectionState',
           data: JSON.stringify({ data: JSON.stringify(obj) }),
           contentType: 'application/json; charset=utf-8',
           dataType: 'json',
           success: function (data) {
               if (data.isSucceded == false) {
                   alert(data.message);
               }
               else {
                   items.draw();
               }
           },
           error: function (xhr, ajaxOptions, thrownError)
           { console.log(xhr.responseText) }

       });
   });

});

function LoadUnits() {
    var d_units = $("#units");
    d_units.empty();
    d_units.append($("<option />").val(0).text(dictionary.SelectUnit));
    $.each(units, function (i, val) {
        d_units.append($("<option />").val(val.GUID).text(val.Name));
    });
}

function CreateDatatable()
{
    items = $('#items').DataTable({
        "orderClasses": false,
        "autoWidth": false,
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
            "url": "/Scales/Get",
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
           { "data": "MAC" },
           { "data": "Status" },
           { "data": "ScalesTypeName" },
           { "data": "Check" },
           { "data": "UnitName" },
           { "data": "GUID" },
           { "data": "CompanyID" },
           { "data": "Weight" },
           { "data": "WeightDate" },
           { "data": "Active" },
           { "data": "CompanyName" }
        ],
        "columnDefs": [
            {
                "targets": [0, 1, 2, 3, 4,5,6,7,8,9,10,11],
                "className": "dt-center",
                "orderable": true
            },
            {
                "targets": [6,7,8,9,10,11],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [4],
                "data": null,
                "defaultContent": "<button>" + dictionary.Check + "</button>"
            }
        ],
        "createdRow": function (row, data, dataIndex) {
            if (data.Status == true) {
                $(row).addClass("green");
                //startWorker();
            }
            if (data.Status == false) {
                $(row).addClass("red");
                //stopWorker();
            }
        }
    });
}

function GetModalData() {
    if (current !== undefined) {
        $("#name").val(current.Name);
        $("#mac").val(current.MAC);
        $("#status").val(current.Status);
        if (current.ScalesTypeID === undefined)
            current.ScalesTypeID = 1;
        $("#scale_type").val(current.ScalesTypeID);
        $("#units").val(current.UnitID);
        $("#companies").val(current.CompanyID);
        $("#active").prop("checked", current.Active);
        $("#isdemo").prop("checked", current.IsDemo);
        
    }
}

function SetModalData() {
    if (current !== undefined) {
        current.Name = $("#name").val();
        current.MAC = $("#mac").val();
        current.CompanyID = $("#companies").val();
        current.Active = $("#active").is(":checked");
        if (current.Active === undefined || !current.Active)
            current.Active = null;
        current.IsDemo = $("#isdemo").is(":checked");
        /*current.Status = $("#status").val();*/
        current.ScalesTypeID = $("#scale_type").val();
        current.UnitID = $("#units").val();
    }
}

function Save( obj)
{
    $.ajax({
        type: 'POST',
        url: '/Scales/Save',
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
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function LoadScaleTypes()
{
    $.ajax({
        type: 'POST',
        url: '/Scales/GetScalesTypes',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                var scale_type = $("#scale_type");
                var f_scale_type = $("#f_scale_type");
                scale_type.empty();
                f_scale_type.empty();
                f_scale_type.append($("<option />").val(0).text("כולם"));
                $.each(data.data, function (i, val) {
                    scale_type.append($("<option />").val(val.GUID).text(val.Name));
                    f_scale_type.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function Filter() {
    this.Name = $("#f_name").val();
    this.MAC = $("#f_mac").val();
    this.Status = $("#f_status").is(":checked");
    if (this.Status === undefined || !this.Status)
        this.Status = null;
    this.ScalesTypeID = $("#f_scale_type").val();
}

function CreateFilterEvents() {
    $("#f_name").on('input', function () {
        items.draw();
    });

    $("#f_mac").on('input', function () {
        items.draw();
    });

    $("#f_status").on('input', function () {
        items.draw();
    });

    $("#f_scale_type").on('input', function () {
        items.draw();
    });
}

function LoadCompanies() {
    $.ajax({
        type: 'POST',
        url: '/Companies/Get',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify({ data: JSON.stringify(new Company()) }),
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                var companies = $("#companies");
                var f_companies = $("#f_companies");
                companies.empty();
                f_companies.empty();
                companies.append($("<option />").val(0).text(dictionary.SelectCompany));
                f_companies.append($("<option />").val(0).text(dictionary.SelectCompany));
                $.each(data.data, function (i, val) {
                    companies.append($("<option />").val(val.GUID).text(val.Name));
                    f_companies.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }
    });
}
function AllowedRows(len) {
    $.ajax({
        type: 'POST',
        url: '/Scales/AllowedRows',
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

function Company()
{
    this.Active = true;
}